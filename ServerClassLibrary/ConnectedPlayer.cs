using GameClassLibrary;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerClassLibrary
{
	//an instance of a connected player, provides game state information about a player
	public class ConnectedPlayer : Player, IDisposable
	{
		//lock should be used any time data is queried from this player
		private readonly object locker = new object();

		//connection-specific stuff
		private Client connection;
		public Socket Sock { get { lock (locker) return connection.GetSocket(); } }
		public bool Connected { get { lock (locker) return connection.Connected; } }
		public IPEndPoint EndPoint { get { lock (locker) return (IPEndPoint)connection.GetSocket().RemoteEndPoint; } }

		public class UpdateArgs : EventArgs
		{
			private string m_msg, m_caption;
			public UpdateArgs(string _dlgMessage, string _caption = "Error logging in!")
			{
				m_msg = _dlgMessage;
				m_caption = _caption;
			}

			public string Message { get { return m_msg; } }
			public string Caption { get { return m_caption; } }
		}
		public EventHandler<UpdateArgs> UpdateErrorMessage = null;
		public EventHandler BacteriaCollect;
		public EventHandler OnSocketError = null;

		//recv buffer: used by server as a storage location for data received on this player's socket
		//any time data is received, call Socket.EndReceive to get the number of bytes then
		//	call PushReceivedData with the number of bytes to encapsulate that data within a PacketStream 
		//	object and store it in the player's internal received data queue
		private byte[] buf = new byte[Client.MAX_RECV];
		public byte[] recvBuf
		{
			get { lock (locker) return buf; }
			set { lock (locker) buf = value; }
		}

		//used to signal a response to data that was sent with SendToServer
		ManualResetEvent response = new ManualResetEvent(true);
		bool LastAction = false;

		//game state stuff : private members and public get-only accessors
		private bool loggedIn;
		public bool LoggedIn
		{
			get
			{
				lock (locker) return loggedIn;
			}
		}

		bool pendingResponse;
		public bool PendingResponse
		{
			get { lock (locker) return pendingResponse; }
			set { lock (locker) pendingResponse = value; }
		}

		public Action<ChatMessage> AddChatMessage { get; set; }

		public ConnectedPlayer(string host, int port) //constructed from client side
			: base("", -1, -1, -1, 0)
		{
			connection = new Client(host, port);
			connection.ReceiveFromServer(this);
			loggedIn = false;
		}

		public ConnectedPlayer(Socket s) //constructed from accept loop server side
			: base("", -1, -1, -1, 0)
		{
			connection = new Client(s); //receive called on this socket from accept loop server-side
			loggedIn = false;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				Close();

			response.Close();
			response.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Close()
		{
			lock (locker)
			{
				loggedIn = false;
				connection.Close();
			}
		}

		//called by client in order to send to server
		public bool SendToServer(ServerAction action, PacketData data = null)
		{
			if(action != ServerAction.ClientCollectItem)
				PendingResponse = true;

			//constructs PPacketBuilder and calls this.SendData
			Packet pkt = new Packet(action);
			if (data != null)
				pkt.AddData<byte[]>(data.ConvertToByteArray());

			bool responseExpected = false;
			switch (action)
			{
				case ServerAction.ClientCreateAcc:
				case ServerAction.ClientLogin:
				case ServerAction.TestLargeSend:
					responseExpected = true;
					break;
			}

			lock (locker) connection.Send(pkt);

			//some things expect a response from the server: wait for it and return true if successful
			response.Reset();
			if (responseExpected)
			{
				if (!response.WaitOne(5000))//wait for 5 seconds for a response
				{
					response.Set();
					return false;
				}
				return LastAction; //set in receive handling
			}
			return true;
		}

		//called from server when sending to client
		public void SendData(Packet pkt)
		{
			lock (locker) connection.Send(pkt);
		}

		//called by server: pushes data from buffer into received queue and clears buffer
		public void PushReceivedData(int bytesReceived)
		{
			lock (locker)
			{
				Array.Resize(ref buf, bytesReceived);
				connection.PushRecv(new PacketStream(buf));
				buf = new byte[Client.MAX_RECV]; //reset buffer
			}
		}
		//called by server.HandleReceivedData: returns next packet in queue
		public PacketStream PopReceivedData()
		{
			lock (locker) return connection.PopRecv();
		}

		//callback for handling data received from the server (client-side)
		public void HandleReceivedData(object state)
		{

			PacketStream pkt = PopReceivedData();
			pkt.Seek(1, System.IO.SeekOrigin.Begin);

			//these have to be out of scope of the switch block
			string uName;
			string msg;
			int dist, dir, newMapX, newMapY;
			int mapX, mapY, mapNum, bactCount, admin;

			switch (pkt.Action)
			{
				case ServerAction.ServerMessage:
					if (AddChatMessage != null)
					{
						msg = "";
						if (!pkt.GetString(ref msg))
							break;

						AddChatMessage(new ChatMessage { Message = msg, Username = "Server", UsernameColor = System.Drawing.Color.Yellow });
					}
					break;
				case ServerAction.ServerKick:
					//match packet formatting. two bytes to prevent erroneous kicks 
					if ((ServerAction)pkt.Peek() == ServerAction.ServerKick)
					{
						World.GameState = GameState.PregameMenu;
						//hacky workaround so that the game state changes on the other thread before the dialog is construted
						//	allows for the dialog to pop up once the game state changes properly and all the components are happy
						Thread.Sleep(100);

						Close(); //close first: we exit the game in UpdateErrorMessage if the client isn't connected

						if (UpdateErrorMessage != null)
							UpdateErrorMessage(this, new UpdateArgs("Kicked from server by server.", "You were kicked!"));
					}
					break;
				case ServerAction.ServerShutdown:

					World.GameState = GameState.Login;
					//hacky workaround so that the game state changes on the other thread before the dialog is construted
					//	allows for the dialog to pop up once the game state changes properly and all the components are happy
					Thread.Sleep(100);

					Close(); //close first: we exit the game in UpdateErrorMessage if the client isn't connected.

					if (UpdateErrorMessage != null)
						UpdateErrorMessage(this, new UpdateArgs("The game server has been shut down.", "Server shut down"));

					break;
				case ServerAction.ServerResponse:
					if ((ServerErrorResponse)pkt.Peek() == ServerErrorResponse.Success)
					{
						if (World.GameState == GameState.Registering)
							World.GameState = GameState.Login;
						LastAction = true;
					}
					else
					{
						if (World.GameState == GameState.Registering)
							World.GameState = GameState.Register;
						else if (World.GameState == GameState.LoggingIn)
							World.GameState = GameState.Login;
						else
							World.GameState = GameState.PregameMenu;

						switch ((ServerErrorResponse)pkt.Peek())
						{
							//handle the errors
							case ServerErrorResponse.AccountAlreadyExists:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("The specified account already exists.", "Error creating account!"));

								break;
							case ServerErrorResponse.AccountCredentialMismatch:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("Mismatched username or password."));

								break;
							case ServerErrorResponse.AccountPasswordStrength:
								pkt.Seek(1);
								msg = "Password does not match requirement";
								string custMsg = "";
								if (!pkt.GetString(ref custMsg))
									msg += ".";
								else
									msg += "s " + custMsg;

								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs(msg, "Error creating account!"));

								break;
							case ServerErrorResponse.DataFormatError:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("Data was formatted improperly.", "Server error"));

								break;
							case ServerErrorResponse.InvalidAccessError:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("Access denied."));

								break;
							case ServerErrorResponse.ServerFatalError:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("Fatal server-side error.", "Server error"));

								break;
							case ServerErrorResponse.ServerTooManyClients:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("The server has reached the connection limit. Try again later."));

								break;
							case ServerErrorResponse.SQLQueryError:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("There was an error querying the database.", "Server error"));

								break;
							default:
								if (UpdateErrorMessage != null)
									UpdateErrorMessage(this, new UpdateArgs("The server returned an error.", "Server error"));

								break;
						}
						LastAction = false;
					}
					response.Set();

					break;
				case ServerAction.ClientCollectItem:
					uName = "";
					int clientX = 0, clientY = 0;
					if (!pkt.GetString(ref uName) || !pkt.GetInt(ref clientX) || !pkt.GetInt(ref clientY))
					{
						Console.WriteLine("Error getting data from server (collect)!");

						break;
					}

					Tile iTile = World.CurrentMap.GetTile(clientX, clientY, LAYERS.Item);
					if (iTile != null)
					{
						ItemTile itemTile = iTile as ItemTile;
						if (itemTile.Type == ItemTileSpec.BACTERIA)
						{
							if (uName == UserName)
								BacteriaCount += itemTile.Quantity;

							World.CurrentMap.AddTile(
								new Microsoft.Xna.Framework.Vector2(clientX, clientY),
								LAYERS.Item,
								new ItemTile(ItemTileSpec.NONE, clientX, clientY));

							if (BacteriaCollect != null && uName == UserName)
								BacteriaCollect(null, null);
						}
					}

					break;
				case ServerAction.Content:
					byte type = 0;
					int values = 0;

					if (!pkt.GetByte(ref type) || !pkt.GetInt(ref values))
					{
						Console.WriteLine("Error getting data from server (content)!");
						break;
					}

					ContentType contentType = (ContentType)type;

					if (contentType == ContentType.Items)
					{
						for (int i = 0; i < values; i++)
						{
							int x = 0, y = 0, quantity = 0;

							if (!pkt.GetByte(ref type) || !pkt.GetInt(ref x) || !pkt.GetInt(ref y) || !pkt.GetInt(ref quantity))
							{
								Console.WriteLine("Error getting data from server (content)!");
								break;
							}

							ItemTile tile = new ItemTile(ItemTileSpec.BACTERIA, x, y, quantity);
							World.CurrentMap.AddTile(new Microsoft.Xna.Framework.Vector2(x, y), LAYERS.Item, tile);
						}
					}
					else if (contentType == ContentType.Players)
					{
						for (int i = 0; i < values; i++)
						{
							mapNum = mapX = mapY = admin = 0;
							uName = "";
							if (!pkt.GetInt(ref mapNum) || !pkt.GetInt(ref mapX) || !pkt.GetInt(ref mapY) || !pkt.GetString(ref uName) || !pkt.GetInt(ref admin))
							{
								Console.WriteLine("Error getting data from server (content)!");
								break;
							}

							if (World.Players.Where(j => j.UserName == uName).Count() == 0)
							{
								Player newPlayer = new Player(uName, mapNum, mapX, mapY, admin);
								newPlayer.FrontGraphicIndex = World.MainPlayer.FrontGraphicIndex;
								newPlayer.BackGraphicIndex = World.MainPlayer.BackGraphicIndex;
								newPlayer.LeftGraphicIndex = World.MainPlayer.LeftGraphicIndex;
								newPlayer.RightGraphicIndex = World.MainPlayer.RightGraphicIndex;
								newPlayer.CurrentGraphicIndex = newPlayer.FrontGraphicIndex;
								World.Players.Add(newPlayer);
							}
						}
					}
					break;

				case ServerAction.Walk:
					uName = "";
					dist = dir = newMapX = newMapY = 0;
					int prevX = 0, prevY = 0;
					if (!pkt.GetString(ref uName) || !pkt.GetInt(ref dir) || !pkt.GetInt(ref dist) || !pkt.GetInt(ref newMapX) || !pkt.GetInt(ref newMapY) || !pkt.GetInt(ref prevX) || !pkt.GetInt(ref prevY))
					{
						Console.WriteLine("Error getting data from server (walk)!");
						break;
					}

					Player playerToMove;
					string directionString = "";
					if (uName != UserName)
					{
						int otherPlayerIndex;
						if ((otherPlayerIndex = World.Players.FindIndex((Player other) => { return other.UserName == uName; })) < 0)
						{
							//add the other player, manually set x/y
							Player add = new Player(uName, CurrentMap, prevX, prevY, 0);

							// Establish the graphic indices. For now, just set them to the same as the main player
							add.FrontGraphicIndex = World.MainPlayer.FrontGraphicIndex;
							add.BackGraphicIndex = World.MainPlayer.BackGraphicIndex;
							add.LeftGraphicIndex = World.MainPlayer.LeftGraphicIndex;
							add.RightGraphicIndex = World.MainPlayer.RightGraphicIndex;
							add.CurrentGraphicIndex = add.FrontGraphicIndex;

							World.Players.Add(add);
							playerToMove = add;
							directionString = ((Direction)dir).ToString();
						}
						else
						{
							//move the other player based on direction/dist if it exists in the collection
							Direction direction = (Direction)dir;
							playerToMove = World.Players[otherPlayerIndex];
							directionString = direction.ToString();
						}

						if (Math.Abs(newMapX - playerToMove.X) > 1 || Math.Abs(newMapY - playerToMove.Y) > 1)
						{
							// Don't bother animating if we are too far away
							playerToMove.X = newMapX;
							playerToMove.Y = newMapY;

							switch ((Direction)dir)
							{
								case Direction.Up:
									playerToMove.CurrentGraphicIndex = playerToMove.BackGraphicIndex;
									break;
								case Direction.Down:
									playerToMove.CurrentGraphicIndex = playerToMove.FrontGraphicIndex;
									break;
								case Direction.Left:
									playerToMove.CurrentGraphicIndex = playerToMove.LeftGraphicIndex;
									break;
								case Direction.Right:
									playerToMove.CurrentGraphicIndex = playerToMove.RightGraphicIndex;
									break;
							}
						}
						else
							playerToMove.NextMoves.Enqueue((Keys)Enum.Parse(typeof(Keys), directionString));
					}
					else
					{
						Direction direction = (Direction)dir;
						int correctedDistance = direction == Direction.Left || direction == Direction.Up ? dist * -1 : dist;
						int newPosition = direction == Direction.Left || direction == Direction.Right ? X + correctedDistance : Y + correctedDistance;
						if (CheckPosition(direction, newPosition))
							StartMoving(direction, dist);
					}

					Console.Write("Player ");
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write(uName);
					Console.ResetColor();
					Console.WriteLine(" walked to [{0},{1}]", newMapX, newMapY);
					break;
				case ServerAction.WalkResponse:
					Console.WriteLine("Player {0} unable to walk because of obstacle on server.", UserName);
					break;
				case ServerAction.ClientLogin:
					uName = "";
					newMapX = newMapY = admin = 0;
					if (!pkt.GetString(ref uName) || !pkt.GetInt(ref newMapX) || !pkt.GetInt(ref newMapY) || !pkt.GetInt(ref admin))
					{
						World.GameState = GameState.PregameMenu;

						if (UpdateErrorMessage != null)
							UpdateErrorMessage(this, new UpdateArgs("Error getting data from server (login)."));

						break;
					}

					if (uName != World.MainPlayer.UserName)
					{
						int otherPlayerIndex;
						if ((otherPlayerIndex = World.Players.FindIndex((Player other) => { return other.UserName == uName; })) < 0)
						{
							//add the other player, manually set x/y
							Player add = new Player(uName, CurrentMap, newMapX, newMapY, admin);

							// Establish the graphic indices. For now, just set them to the same as the main player
							add.FrontGraphicIndex = World.MainPlayer.FrontGraphicIndex;
							add.BackGraphicIndex = World.MainPlayer.BackGraphicIndex;
							add.LeftGraphicIndex = World.MainPlayer.LeftGraphicIndex;
							add.RightGraphicIndex = World.MainPlayer.RightGraphicIndex;
							add.CurrentGraphicIndex = add.FrontGraphicIndex;

							World.Players.Add(add);
							break;
						}
					}

					World.GameState = GameState.PregameMenu;

					if (UpdateErrorMessage != null)
						UpdateErrorMessage(this, new UpdateArgs("Unexpected login packet received."));

					break;
				case ServerAction.ClientLoginResponse:
					uName = "";
					mapX = mapY = mapNum = bactCount = admin = 0;

					if (LoggedIn)
					{
						if (UpdateErrorMessage != null)
							UpdateErrorMessage(this, new UpdateArgs("Erroneous login response packet received!", "Unsupported game state!"));

						break;
					}

					if (!pkt.GetString(ref uName) || !pkt.GetInt(ref mapNum) || !pkt.GetInt(ref mapX) || !pkt.GetInt(ref mapY) || !pkt.GetInt(ref bactCount)
						|| !pkt.GetInt(ref admin))
					{
						World.GameState = GameState.PregameMenu;

						if (UpdateErrorMessage != null)
							UpdateErrorMessage(this, new UpdateArgs("Error getting data from server (LoginResponse)"));

						break;
					}

					Login(uName, mapNum, mapX, mapY, bactCount, admin);
					LastAction = true;
					response.Set();

					if (World.GameState == GameState.LoggingIn)
						World.GameState = GameState.InGame;

					break;
				case ServerAction.ClientLogout:
					uName = "";

					if (!pkt.GetString(ref uName))
					{
						World.GameState = GameState.PregameMenu;

						if (UpdateErrorMessage != null)
							UpdateErrorMessage(this, new UpdateArgs("Error getting data from server (logout)."));

						Logout();

						break;
					}

					if (uName == UserName)
					{
						World.GameState = GameState.PregameMenu;
						Logout();
					}
					else
					{
						World.Players.RemoveAll((Player x) => { return x.UserName == uName; });
					}

					break;
				case ServerAction.ClientSay:
					uName = msg = "";
					if (!pkt.GetString(ref uName) || !pkt.GetString(ref msg))
					{
						Console.WriteLine("Error getting data from server (say)!");
						break;
					}

					bool isAdmin = World.Players.FindIndex((Player adminSearch) => { return adminSearch.UserName == uName && adminSearch.AdminLevel > 0; }) >= 0;
					System.Drawing.Color uNameColor = uName == UserName ? System.Drawing.Color.Blue : System.Drawing.Color.Cyan;
					uNameColor = isAdmin ? System.Drawing.Color.Purple : uNameColor;

					AddChatMessage(new ChatMessage
						{
							Message = msg,
							MessageColor = System.Drawing.Color.White,
							Username = uName,
							UsernameColor = uNameColor
						});
					break;
				case ServerAction.Warp:
					uName = "";
					mapNum = mapX = mapY = 0;
					Player playa = null;
					if (!pkt.GetString(ref uName) || !pkt.GetInt(ref mapNum) || !pkt.GetInt(ref mapX) || !pkt.GetInt(ref mapY))
					{
						Console.WriteLine("Error getting data from server (Warp)!");
						break;
					}

					if (uName != UserName)
					{
						int otherPlayerIndex;
						if ((otherPlayerIndex = World.Players.FindIndex((Player other) => { return other.UserName == uName; })) < 0)
						{
							//add the other player, manually set x/y
							Player add = new Player(uName, CurrentMap, mapX, mapY, 1);

							// Establish the graphic indices. For now, just set them to the same as the main player
							add.FrontGraphicIndex = World.MainPlayer.FrontGraphicIndex;
							add.BackGraphicIndex = World.MainPlayer.BackGraphicIndex;
							add.LeftGraphicIndex = World.MainPlayer.LeftGraphicIndex;
							add.RightGraphicIndex = World.MainPlayer.RightGraphicIndex;
							add.CurrentGraphicIndex = add.FrontGraphicIndex;

							World.Players.Add(add);
							break;
						}
					}

					playa = World.Players.Find((Player p) => { return uName == p.UserName; });
					playa.CurrentMap = mapNum;
					playa.X = mapX;
					playa.Y = mapY;
					break;
				default:

					World.GameState = GameState.PregameMenu;

					if (UpdateErrorMessage != null)
						UpdateErrorMessage(this, new UpdateArgs("Unhandled data received from server."));

					break;
			}
			pendingResponse = false;
		}

		public bool CheckPosition(Direction direction, int position)
		{
			lock (locker)
				return !(position < 0 || ((direction == Direction.Left || direction == Direction.Right) ? position > World.CurrentMap.Width - 1 : position > World.CurrentMap.Height - 1));
		}

		public void Login(string userName, int mapNum, int x, int y, int bactCount, int admin)
		{
			if (loggedIn)
				return;

			lock (locker)
			{
				UserName = userName;
				CurrentMap = mapNum;
				X = x;
				Y = y;
				loggedIn = true;
				BacteriaCount = bactCount;
				AdminLevel = admin;
			}
		}

		public void Logout()
		{
			if (!loggedIn)
				return;

			lock(locker)
			{
				UserName = "";
				CurrentMap = -1;
				X = -1;
				Y = -1;
				loggedIn = false;
				BacteriaCount = -1;
			}
		}

		public void Walk(Direction dir, int dist)
		{
			lock (locker)
				switch (dir)
				{
					case Direction.Up: Y -= dist; break;
					case Direction.Down: Y += dist; break;
					case Direction.Left: X -= dist; break;
					case Direction.Right: X += dist; break;
				}
		}
	}
}
