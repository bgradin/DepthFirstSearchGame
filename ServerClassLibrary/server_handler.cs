using GameClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;

namespace ServerClassLibrary
{
	public partial class Server
	{
		public void HandleReceivedData(object ThreadContext)
		{
			ConnectedPlayer client = (ConnectedPlayer)ThreadContext;

			PacketStream pkt = client.PopReceivedData();

			bool ret = false;

			pkt.Seek(1, System.IO.SeekOrigin.Begin); //skip the packet type, otherwise errors for days
			switch (pkt.Action)
			{
				//locking handled within different cases: HandleClientXXXX();
				case ServerAction.ClientSay:
					ret = HandleClientSay(pkt, client);
					break;
				case ServerAction.ClientDisconnect:
					ret = HandleClientDisconnect(pkt, client);
					break;
				case ServerAction.ClientCreateAcc:
					ret = HandleClientCreateAcc(pkt, client);
					break;
				case ServerAction.ClientLogin:
					ret = HandleClientLogin(pkt, client);
					break;
				case ServerAction.ClientLogout:
					ret = HandleClientLogout(pkt, client);
					break;
				case ServerAction.ClientCreateChar:
					this.m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
						"ClientCreateChar is not implemented yet.");
					break;
				case ServerAction.TestLargeSend:
					ret = HandleTestLarge(pkt, client);
					break;
				case ServerAction.Walk:
					ret = HandleClientWalk(pkt, client);
					break;
				case ServerAction.ClientCollectItem:
					ret = HandleClientCollectItem(pkt, client);
					break;
				case ServerAction.Warp:
					ret = HandleClientWarpToStart(pkt, client);
					break; 
				default:
					this.m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
						"Unhandled data received from server: " + pkt.Action);
					break;
			}

			if (!ret) //since this is a callback, we can't return ret anymore...
				m_log.LogWarning("RecvThread", "Unable to handle client data!");
		}

		private bool HandleClientSay(PacketStream pkt, ConnectedPlayer client)
		{
			string msg = "";
			if (!pkt.GetString(ref msg))
			{
				this.m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Unable to get message from client");
				SendResponse(client, ServerErrorResponse.DataFormatError);
				return false;
			}

			if (!client.LoggedIn)
			{
				this.m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Client tried to talk without logging in");
				SendResponse(client, ServerErrorResponse.InvalidAccessError);
				return false;
			}

			if (msg.Length > 0)
			{
				if (msg[0] == '$')
				{
					string cmd = msg.Substring(1);
					string[] parts = cmd.Split(new char[] { ' ' });

					Packet errorResponse = new Packet(ServerAction.ServerMessage); //show this as a server message on the client side
					Packet response;

					if (client.AdminLevel == 0)
					{
						errorResponse.AddData("You must be a server administrator in order to use client-side commands.");
						client.SendData(errorResponse);
						return true;
					}

					int mapNum, mapX, mapY;
					switch (parts[0].ToLower())
					{
						case "loc":
							errorResponse.AddData("Location = " + client.CurrentMap + ", " + client.X + ", " + client.Y);
							client.SendData(errorResponse);
							break;
						case "warp":
							if (parts.Length != 4 || !int.TryParse(parts[1], out mapNum) || !int.TryParse(parts[2], out mapX) || !int.TryParse(parts[3], out mapY))
							{
								errorResponse.AddData("Usage: warp [mapnum] [x] [y]");
								client.SendData(errorResponse);
								break;
							}

							client.CurrentMap = mapNum;
							client.X = mapX;
							client.Y = mapY;

							response = new Packet(ServerAction.Warp);
							response.AddData(client.UserName);
							response.AddData(client.CurrentMap);
							response.AddData(client.X);
							response.AddData(client.Y);

							foreach (ConnectedPlayer player in m_players)
							{
								player.SendData(response);
							}

							break;
						case "warpmeto":
							if(parts.Length != 2)
							{
								errorResponse.AddData("Usage: warpmeto [playername]");
								client.SendData(errorResponse);
								break;
							}

							Player other = null;
							lock (_PLAYERLOCK_) other = m_players.Find((ConnectedPlayer x) => { return x.UserName == parts[1]; });

							if (other == null)
								break;

							client.CurrentMap = other.CurrentMap;
							client.X = other.X;
							client.Y = other.Y;

							response = new Packet(ServerAction.Warp);
							response.AddData(client.UserName);
							response.AddData(client.CurrentMap);
							response.AddData(client.X);
							response.AddData(client.Y);

							foreach (ConnectedPlayer player in m_players)
							{
								player.SendData(response);
							}

							break;
						default:
							errorResponse.AddData("Invalid command entered");
							client.SendData(errorResponse);
							break;
					}
				}
				else
				{
					Packet response = new Packet(ServerAction.ClientSay);
					response.AddData<string>(client.UserName);
					response.AddData<string>(msg);
					SendResponse(client, response);
					client.SendData(response);
				}
			}
			return true;
		}

		private bool HandleClientDisconnect(PacketStream pkt, ConnectedPlayer client)
		{
			//possibly grab/handle data from pkt such as reason for disconnect?
			if (!client.LoggedIn)
			{
				Console.WriteLine("Client disconnected.");
			}
			else
			{
				Console.WriteLine(client.UserName + " disconnected.");
				this.m_database.Logout(client.UserName);
			}
			client.Close();
			Console.Write("\n> ");
			return true;
		}

		private bool HandleClientCreateAcc(PacketStream pkt, ConnectedPlayer client)
		{
			string uName = "", pass = "";
			int passlen, start_map, start_x, start_y;
			bool encrypt;

			if (!pkt.GetString(ref uName) || !pkt.GetString(ref pass))
			{
				this.m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Unable to get username/password from account creation packet");
				SendResponse(client, ServerErrorResponse.DataFormatError);
				return false;
			}

			if (!this.m_config.GetValue("passwordlen", out passlen) || !this.m_config.GetValue("passwordenc", out encrypt)
				|| !this.m_config.GetValue(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_MAP, out start_map)
				|| !this.m_config.GetValue(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_X, out start_x)
				|| !this.m_config.GetValue(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_Y, out start_y))
			{
				this.m_log.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Unable to get passwordlen/passwordenc/startMap/startX/startY from log file");
				passlen = 8; 
				encrypt = true;
				start_map = 0;
				start_x = 29;
				start_y = 79;
			}

			if (this.m_database.FindAccount(uName))
			{
				SendResponse(client, ServerErrorResponse.AccountAlreadyExists);
				return false;
			}

			if (pass.Length < passlen)
			{
				SendResponse(client, ServerErrorResponse.AccountPasswordStrength, new string[] { "passwordlength: " + passlen });
				return false;
			}

			if (encrypt)
				pass = Encryption.SHA256(pass);

			if (!this.m_database.CreateAccount(uName, pass, client.EndPoint.Address.ToString(), start_map, start_x, start_y))
			{
				SendResponse(client, ServerErrorResponse.SQLQueryError);
				return false;
			}

			SendResponse(client, ServerErrorResponse.Success);
			Console.WriteLine("Client " + client.EndPoint.Address.ToString() + " created account: " + uName);
			Console.Write("\n> ");

			return true;
		}

		public bool HandleClientLogin(PacketStream pkt, ConnectedPlayer client)
		{
			string uName = "", pass = "";
			int x = -1, y = -1, mapNum = 1, bacteriaCount = 0, admin = 0;

			if (!pkt.GetString(ref uName) || !pkt.GetString(ref pass))
			{
				this.m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Unable to get username and password from login request");
				SendResponse(client, ServerErrorResponse.DataFormatError);
				return false;
			}

			int maxClients;
			bool enc;
			if (!this.m_config.GetValue("loggedinusers", out maxClients) || !this.m_config.GetValue("passwordenc", out enc))
			{
				this.m_log.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Unable to get passwordlen and passwordenc from log file");
				this.m_config.Add("general", "loggedinusers", 50);
				maxClients = 50;
				enc = true;
			}

			if (enc)
				pass = Encryption.SHA256(pass);

			object[] accData = this.m_database.GetAccount(uName);
			if (accData == null || pass != accData[(int)DBAccountIndex.Password].ToString())
			{
				SendResponse(client, ServerErrorResponse.AccountCredentialMismatch);
				return false;
			}

			if ((int)accData[(int)DBAccountIndex.LoggedIn] == 1)
			{
				this.m_log.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"User " + uName + " tried to log in twice");
				SendResponse(client, ServerErrorResponse.InvalidAccessError);
				return false;
			}

			if (!this.m_database.Login(uName, client.EndPoint.Address.ToString()))
			{
				SendResponse(client, ServerErrorResponse.SQLQueryError);
				return false;
			}

			if (this.m_players.Count >= maxClients)
			{
				SendResponse(client, ServerErrorResponse.ServerTooManyClients);
				this.m_database.Logout(uName);
				return false;
			}

			x = (int)accData[(int)DBAccountIndex.LocX];
			y = (int)accData[(int)DBAccountIndex.LocY];
			mapNum = (int)accData[(int)DBAccountIndex.LocMap];
			bacteriaCount = (int)accData[(int)DBAccountIndex.BactCount];
			admin = (int)accData[(int)DBAccountIndex.AdminLevel];

			client.Login(uName, mapNum, x, y, bacteriaCount, admin);

			//send response with: map num, x, y, bacteria count
			Packet loginResponse = new Packet(ServerAction.ClientLoginResponse);
			loginResponse.AddData<string>(uName);
			loginResponse.AddData<int>(mapNum);
			loginResponse.AddData<int>(x);
			loginResponse.AddData<int>(y);
			loginResponse.AddData<int>(bacteriaCount);
			loginResponse.AddData<int>(admin);
			client.SendData(loginResponse);

			// Log the new player into everyone else's window
			Packet packet = new Packet(ServerAction.ClientLogin);
			packet.AddData<string>(client.UserName);
			packet.AddData<int>(client.X);
			packet.AddData<int>(client.Y);
			packet.AddData<int>(admin);
			SendResponse(client, packet);

			SortedList<string, Tile> items = World.Maps[client.CurrentMap].Layers[(int)LAYERS.Item];
			if (items != null && items.Count > 0)
			{
				Packet itemsPacket = new Packet(ServerAction.Content);
				itemsPacket.AddData((byte) ContentType.Items);
				itemsPacket.AddData(items.Count);
				foreach (var pair in items)
				{
					if (!(pair.Value is ItemTile))
						continue;

					ItemTile iTile = pair.Value as ItemTile;

					itemsPacket.AddData((byte)iTile.Type);
					itemsPacket.AddData(iTile.X);
					itemsPacket.AddData(iTile.Y);
					itemsPacket.AddData(iTile.Quantity);
				}
				client.SendData(itemsPacket);
			}

			if (m_players != null)
			{
				Packet playersPacket = new Packet(ServerAction.Content);
				playersPacket.AddData((byte) ContentType.Players);
				playersPacket.AddData(m_players.Where(i => i.CurrentMap == client.CurrentMap).Count());
				foreach (ConnectedPlayer player in m_players)
				{
					if (player.CurrentMap == client.CurrentMap)
					{
						playersPacket.AddData(player.CurrentMap);
						playersPacket.AddData(player.X);
						playersPacket.AddData(player.Y);
						playersPacket.AddData(player.UserName);
						playersPacket.AddData(player.AdminLevel);
					}
				}
				client.SendData(playersPacket);
			}

			Console.WriteLine(uName + " logged in from " + client.EndPoint.Address.ToString());
			Console.Write("\n> ");

			return true;
		}

		public bool HandleClientLogout(PacketStream pkt, ConnectedPlayer client)
		{
			if (!client.LoggedIn)
			{
				Console.WriteLine("Client tried to log out without logging in first!");
				return false;
			}

			m_database.UpdateAccountInfo(client.UserName, client.CurrentMap, client.X, client.Y, client.BacteriaCount);
			this.m_database.Logout(client.UserName);
			Console.WriteLine(client.UserName + " logged out.");
			Console.Write("\n> ");


			Packet logoutPacket = new Packet(ServerAction.ClientLogout);
			logoutPacket.AddData(client.UserName);

			foreach (ConnectedPlayer player in m_players)
				player.SendData(logoutPacket);

			client.SendData(logoutPacket);

			client.Logout();

			SendResponse(client, logoutPacket);
			return true;
		}

		public bool HandleTestLarge(PacketStream pkt, ConnectedPlayer client)
		{
			Console.WriteLine("Received large packet of size {0} bytes", pkt.DataLength - 1);
			Console.Write("\n> ");
			SendResponse(client, ServerErrorResponse.Success);
			return true;
		}

		public bool HandleClientWalk(PacketStream pkt, ConnectedPlayer client)
		{
			byte dir;
			int dist;
			dist = dir = 0;
			if (!pkt.GetByte(ref dir) || !pkt.GetInt(ref dist))
			{
				SendResponse(client, ServerErrorResponse.DataFormatError);
				return false;
			}

			// Save previous location
			int previousX = client.X;
			int previousY = client.Y;

			//check the next space before walking
			int checkX = client.X + ((Direction)dir == Direction.Left ? -1 : 0);
			checkX += (Direction)dir == Direction.Right ? 1 : 0;
			int checkY = client.Y + ((Direction)dir == Direction.Down ? 1 : 0);
			checkY += (Direction)dir == Direction.Up ? -1 : 0;
			if(m_players.Where(player => player.X == checkX && player.Y == checkY).Count() > 0)
			{
				//don't walk: player in square
				Packet dontWalk = new Packet(ServerAction.WalkResponse);
				client.SendData(dontWalk);
				return true;
			}
			
			//move the client server-side
			client.Walk((Direction)dir, dist);

			//walk packet update for clients in range
			Packet walkUpdate = new Packet(ServerAction.Walk);
			walkUpdate.AddData<string>(client.UserName);
			walkUpdate.AddData<Direction>((Direction)dir);
			walkUpdate.AddData<int>(dist);
			walkUpdate.AddData<int>(client.X);
			walkUpdate.AddData<int>(client.Y);
			walkUpdate.AddData<int>(previousX);
			walkUpdate.AddData<int>(previousY);
			client.SendData(walkUpdate);

			Packet collectPacket = null;
			Tile currentTile = World.CurrentMap.GetTile(client.X, client.Y, LAYERS.Item);
			if (currentTile is ItemTile)
			{
				if ((currentTile as ItemTile).Type == ItemTileSpec.BACTERIA)
				{
					//update count for the player server-side
					client.BacteriaCount += (currentTile as ItemTile).Quantity;

					// Send collection packet
					collectPacket = new Packet(ServerAction.ClientCollectItem);
					collectPacket.AddData(client.UserName);
					collectPacket.AddData(client.X);
					collectPacket.AddData(client.Y);
					client.SendData(collectPacket);

					// Erase item
					World.CurrentMap.EraseTile(client.X, client.Y, LAYERS.Item);
				}
			}

			foreach (ConnectedPlayer player in m_players)
			{
				if (player != client && player.CurrentMap == client.CurrentMap)
				{
					player.SendData(walkUpdate);

					if (collectPacket != null)
						player.SendData(collectPacket);
				}
			}

			return true;
		}

		public bool HandleClientCollectItem(PacketStream pkt, ConnectedPlayer client)
		{
			int id, qty = id = 0;

			if(!pkt.GetInt(ref id) || !pkt.GetInt(ref qty))
			{
				this.m_log.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"User " + client.UserName + " sent invalid ItemCollectData packet");
				return false;
			}

			//at this point the only item is the bacteria count
			client.BacteriaCount += qty;

			return true;
		}

		public bool HandleClientWarpToStart(PacketStream pkt, ConnectedPlayer client)
		{
			if (pkt.Peek() != (byte)ServerAction.Warp)
			{
				this.m_log.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name,
					"Packet is improperly formatted! You did an awful job at winning!");
				return false;
			}

			int newClientX = 0, newClientY = 0, newClientMap = 0;

			if(!m_config.GetValue(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_MAP, out newClientMap))
			{
				m_config.Add(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_MAP, 0);
				m_config.SaveChanges();
			}

			if(!m_config.GetValue(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_X, out newClientX))
			{
				m_config.Add(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_X, 29);
				m_config.SaveChanges();
			}

			if(!m_config.GetValue(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_Y, out newClientY))
			{
				m_config.Add(ConfigConst.CONF_GAME, ConfigConst.CONF_GAME_START_Y, 79);
				m_config.SaveChanges();
			}

			client.CurrentMap = newClientMap;
			client.X = newClientX;
			client.Y = newClientY; 

			Packet warpPacket = new Packet(ServerAction.Warp);
			warpPacket.AddData(client.UserName);
			warpPacket.AddData(client.CurrentMap);
			warpPacket.AddData(client.X);
			warpPacket.AddData(client.Y);

			foreach (ConnectedPlayer player in m_players)
			{
				//if (player.UserName != client.UserName)
					player.SendData(warpPacket);
			}
			
			return true; 
		}

		//This is just for sending an error message back after something failed server-side
		public void SendResponse(ConnectedPlayer client, ServerErrorResponse err, params string[] extraData)
		{
			Packet response = new Packet(ServerAction.ServerResponse);
			response.AddData<byte>((byte)err);
			switch (err)
			{
				case ServerErrorResponse.AccountPasswordStrength:
					for (int i = 0; i < extraData.Length; ++i)
						response.AddData<string>(extraData[i]);
					break;
			}
			client.SendData(response);
		}

		//This is for sending to all clients within a certain distance from 'client'
		//Packet is pre-build by handler method
		public void SendResponse(ConnectedPlayer client, Packet pkt)
		{
			//find all clients that are logged in, not of the userName, and within 10 units
			List<ConnectedPlayer> updateClients = m_players.FindAll((ConnectedPlayer p) =>
			{
				bool pass1 = p.LoggedIn && p.UserName != client.UserName;
				//bool pass2 = Math.Abs(p.X - client.X) < 10 && Math.Abs(p.Y - client.Y) < 10; //change threshold to be dynamically updated by the game client
				bool pass2 = true; //send to all clients now
				bool pass3 = p.CurrentMap == client.CurrentMap;
				return pass1 && pass2 && pass3;
			});

			//send the packet to each client
			foreach (ConnectedPlayer sc in updateClients)
				sc.SendData(pkt);
		}
	}

}
