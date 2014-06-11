using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SWF = System.Windows.Forms;

namespace ServerClassLibrary
{	
	public delegate void AcceptAction(object param);

	public sealed partial class Server : IDisposable
	{
		public static string CONFIG_NAME = ".\\config\\server.ini";
		private DB m_database;
		private Config m_config;
		private Log m_log;

		private Socket m_sock;
		private IPEndPoint m_endPoint;
		private bool m_started;
		private bool m_termSig;

		private List<ConnectedPlayer> m_players;
		private readonly object _PLAYERLOCK_ = new object();

		private AcceptAction m_acceptAction;

		private Timer m_playerMaintainTimer; //60 sec timer that removes disconnected players and such
		private Timer m_bacteriaMaintainTimer; //bacteria maintenance timer that adds bacteria every second
		private Timer m_playerSaveTimer; //saves player info for all logged in players back to the database every 5 minutes
		
		public bool Started { get { return this.m_started; } }
		public string ListenIP { get { return this.m_endPoint.Address.ToString(); } }
		public string ListenPort { get { return this.m_endPoint.Port.ToString(); } }
		public Config ServerConfiguration { get { return this.m_config; } }

		public Server()
		{
			try
			{
				m_log = new Log();
				m_config = new Config(CONFIG_NAME);
				if (!m_config.Load())
				{
					m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
						"Unable to load server configuration file {0}", m_config.Filename);
					throw new ServerInitException("Unable to load configuration file");
				}

				string bindAddr;
				int port;
				m_config.GetValue("server", "bindaddress", out bindAddr);
				m_config.GetValue("server", "port", out port);

				m_endPoint = new IPEndPoint(IPAddress.Parse(bindAddr), port);
				m_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				m_started = false;
				m_termSig = false;
				m_players = new List<ConnectedPlayer>();
				
				m_database = new DB();
				string dbu, dbp, dbh;
				m_config.GetValue("database", "dbuser", out dbu);
				m_config.GetValue("database", "dbpass", out dbp);
				m_config.GetValue("database", "dbaddr", out dbh);
				if (!m_database.Connect(dbu, dbp, dbh)) //Use http://dev.mysql.com/downloads/file.php?id=450594 for connector
				{
					m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
						"Unable to connect to database. Check the server config and make sure ODBC driver is installed.");
					throw new ServerInitException("Unable to connect to database");
				}
			}
			catch(Exception ex)
			{
				m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Unable to create server.");
				throw new ServerInitException("Error initializing the server: " + ex.Message, ex);
			}
		}

		~Server()
		{
			Close();
		}

		public bool Start(AcceptAction action)
		{
			if (m_started)
			{
				m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Attempted to start server a second time.");
				throw new ServerStartException("This server instance has already been started!");
			}

			if(action == null)
			{
				string err = "Attempted to start server with invalid AcceptAction";
				m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name, err);
				throw new ServerStartException(err);
			}
			
			try
			{
				m_acceptAction = action;
				//get server ready to listen for and accept connections
				m_sock.Bind(m_endPoint);
				m_sock.Listen(10);

				if (!m_database.LogoutAll())
				{
					throw new Exception("Unable to log clients out of database!");
				}

				m_playerMaintainTimer = new Timer(new TimerCallback((object param) => 
				{
					if (m_players.Count > 0)
						RemoveDisconnectedPlayers();
				}), null, 0, 60000);
				m_bacteriaMaintainTimer = new Timer(World.AddBacteria, this, 0, 1000);
				m_playerSaveTimer = new Timer(SavePlayerInfo, null, 0, 60000 * 5);

				//begin the accept loop
				m_sock.BeginAccept(_accept, null);
				m_started = true;
			}
			catch (Exception ex)
			{
				m_started = false;
				m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Unable to start the server: " + ex.Message);
				throw new ServerStartException("Unable to start the server!", ex);
			}

			return m_started;
		}

		public void Dispose()
		{
			Close();
		}

		public void Close()
		{
			if (!Started)
				return;
			m_started = false;

			m_playerMaintainTimer.Change(0, Timeout.Infinite);
			m_playerMaintainTimer.Dispose();
			m_playerMaintainTimer = null;

			m_bacteriaMaintainTimer.Change(0, Timeout.Infinite);
			m_bacteriaMaintainTimer.Dispose();
			m_bacteriaMaintainTimer = null;

			m_playerSaveTimer.Change(0, Timeout.Infinite);
			m_playerSaveTimer.Dispose();
			m_playerSaveTimer = null;

			SavePlayerInfo();

			lock (_PLAYERLOCK_)
			{
				foreach (ConnectedPlayer client in m_players)
					if (client.Connected)
						client.SendData(new Packet(ServerAction.ServerShutdown));
			}
			this.m_database.LogoutAll();

			this.m_termSig = true; //set termination signal
			try
			{
				//terminate the 'accept' thread by connecting to it and unblocking the call
				using (Socket killer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
				{
					killer.Connect("127.0.0.1", this.m_endPoint.Port);
					killer.Shutdown(SocketShutdown.Both);
				}

				m_sock.Close();
				m_sock.Dispose();

				m_log.Close();
				m_database.Dispose();
			}
			catch (Exception ex)
			{
				throw new ServerShutdownException("There was an error shutting down the server: " + ex.Message);
			}
		}

		private void RemoveDisconnectedPlayers()
		{
			//remove old invalid/closed clients from the list that shouldn't be in there anymore
			List<ConnectedPlayer> unconnected = m_players.FindAll((ConnectedPlayer p) => { return !p.Connected; }); //lambda solution
			if (unconnected.Count > 0)
			{
				lock (_PLAYERLOCK_)
				{
					foreach (ConnectedPlayer p in unconnected)
					{
						m_players.Remove(p);
						m_database.Logout(p.UserName); //log out players that are disconnected

						//send a packet saying this client has logged out to each player in the list
						Packet pkt = new Packet(ServerAction.ClientLogout);
						pkt.AddData<string>(p.UserName);
						m_players.ForEach((ConnectedPlayer connected) => { connected.SendData(pkt); });
					}
				}
			}
		}

		private void SavePlayerInfo(object param = null)
		{
			RemoveDisconnectedPlayers();
			if(m_players.Count > 0)
			{
				lock(_PLAYERLOCK_)
				{
					foreach(ConnectedPlayer p in m_players)
					{
						if(!m_database.UpdateAccountInfo(p.UserName, p.CurrentMap, p.X, p.Y, p.BacteriaCount))
						{
							m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
								"Unable to save player info in database! Player: " + p.UserName);
						}
					}
				}
			}
		}

		private void _accept(IAsyncResult res)
		{
			if (m_termSig)
				return;

			Socket s;
			ConnectedPlayer newPlayer = new ConnectedPlayer((s = m_sock.EndAccept(res)));
			s.BeginReceive(newPlayer.recvBuf, 0, Client.MAX_RECV, SocketFlags.None, ReceiveFromClient, newPlayer);

			this.m_acceptAction(s.RemoteEndPoint);

			lock (_PLAYERLOCK_) m_players.Add(newPlayer);
			m_sock.BeginAccept(_accept, null); //accept loop
			RemoveDisconnectedPlayers();
		}

		public void ReceiveFromClient(IAsyncResult res)
		{
			try
			{
				ConnectedPlayer p = (ConnectedPlayer)res.AsyncState;
				if (p.Sock == null || !p.Connected)
					return;

				int bytes = p.Sock.EndReceive(res);
				p.PushReceivedData(bytes);
				p.Sock.BeginReceive(p.recvBuf, 0, Client.MAX_RECV, SocketFlags.None, ReceiveFromClient, p);
				
				if (!p.Connected)
					return;
				ThreadPool.QueueUserWorkItem(new WaitCallback(HandleReceivedData), p);
			}
			catch (Exception ex)
			{
				this.m_log.LogWarning("ClientReceiveCallback", "Caught exception: " + ex.Message);
			}
		}

		public bool SendToClient(ServerAction sa, PacketData pd)
		{
			lock (_PLAYERLOCK_)
			{
				if (m_players.Count == 0)
					return false;
			}
			
			Packet pkt = new Packet(sa);
			switch (sa)
			{
				case ServerAction.ServerMessage:
					if (!(pd is TalkData))
						return false;
					if (!pkt.AddData<string>((pd as TalkData).Message))
						return false;
					lock (_PLAYERLOCK_)
					{
						foreach (ConnectedPlayer p in m_players)
							p.SendData(pkt);
					}
					break;
				case ServerAction.ServerKick:
					pkt.AddData<byte>((byte)sa); //two bytes of serveraction.kick for kicking a client

					if (!(pd is KickData))
						return false;
					KickData data = (KickData)pd;
					if (data.IndexToKick == -1 && string.IsNullOrEmpty(data.UserToKick))
						return false;
					
					int toKick;
					if (data.IndexToKick == -1)
					{
						lock (_PLAYERLOCK_)
						{
							toKick = this.m_players.FindIndex((ConnectedPlayer other) =>
							{
								return other.UserName.ToLower() == data.UserToKick.ToLower();
							});
						}
					}
					else
						toKick = data.IndexToKick;

					if (toKick < 0 || toKick >= m_players.Count)
						return false;

					lock (_PLAYERLOCK_)
					{
						if (!m_players[toKick].Connected)
							return false;

						ConnectedPlayer playerToKick = m_players[toKick];
						m_players.RemoveAt(toKick);
						playerToKick.SendData(pkt);
						m_database.Logout(playerToKick.UserName);

						Packet logoutPacket = new Packet(ServerAction.ClientLogout);
						logoutPacket.AddData(playerToKick.UserName);
						foreach (ConnectedPlayer player in m_players)
						{
							if (player != playerToKick)
								player.SendData(logoutPacket);
						}
					}
					break;
				case ServerAction.Content:
					if (!(pd is ItemData))
						return false;

					pkt.AddData((byte)ContentType.Items);
					pkt.AddData(1);
					pkt.AddData<byte[]>(pd.ConvertToByteArray());

					lock (_PLAYERLOCK_)
					{
						foreach (ConnectedPlayer p in m_players)
							p.SendData(pkt);
					}
					break;
				default:
					return false;
			}
			return true;
		}

		public List<KeyValuePair<IPEndPoint, string>> GetClientInfo()
		{
			RemoveDisconnectedPlayers();

			List<KeyValuePair<IPEndPoint, string>> ret = 
				new List<KeyValuePair<IPEndPoint, string>>();
			lock (_PLAYERLOCK_)
			{
				foreach (ConnectedPlayer p in m_players)//only need to get the usernames and endpoints for this method
				{
					ret.Add(new KeyValuePair<IPEndPoint, string>(p.EndPoint, p.UserName));
				}
			}
			return ret;
		}

		/// <summary>
		/// Pretty much just a passthrough method that allows external classes to log errors to the server's log file.
		/// </summary>
		public void Log(string method, string fmt, params object[] args)
		{
			this.m_log.LogError(method, fmt, args);
		}

		public bool PromoteUser(string acc)
		{
			SavePlayerInfo();
			return this.m_database.MakeAdmin(acc);
		}
	}

}