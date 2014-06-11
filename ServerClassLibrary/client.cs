using GameClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerClassLibrary
{
	//provides functions for interfacing with the network component of a connected player
	public class Client : IDisposable
	{
		private static readonly object locker = new object(); //lock around the receive callback so the buffer isn't filled twice. fixes errors!
		public const int MAX_RECV = short.MaxValue;
		private Socket m_sock;
		private Queue<PacketStream> recvQueue = new Queue<PacketStream>();

		private bool sigTerm = false;
		//poll the socket && see if data is available
		//return false if the poll fails or no data is available
		public bool Connected
		{
			get
			{
				if (m_sock == null)
					return false;

				try
				{
					if (sigTerm) return false; //auto-disconnect

					return !(m_sock.Poll(1000, SelectMode.SelectRead) && m_sock.Available == 0);
				}
				catch
				{
					//return false if there is an error accessing the socket
					return false;
				}
			}
		}

		//m_sock should not be a member and should not really be accessed by anything
		//abstraction should be provided through methods
		public Socket GetSocket() { return this.m_sock; }

		//construct from host/port
		public Client(string host, int port)
		{
			IPAddress ip;
			if (!IPAddress.TryParse(host, out ip))
			{
				throw new ClientCreateException("[in Client.Constructor] Unable to create client");
			}

			m_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			m_sock.Connect(host, port);
			if (!m_sock.Connected)
				throw new ClientConnectException("[in Client.Constructor] Unable to connect to PokeServer!");
		}

		//construct from an already created socket
		public Client(Socket s)
		{
			m_sock = s;
		}

		public void Close()
		{
			if (sigTerm) return;
			sigTerm = true;
			m_sock.Shutdown(SocketShutdown.Both);
			m_sock.Close();
			m_sock.Dispose();
			m_sock = null;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				Close();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		//Called from the client side to receive from the server
		public void ReceiveFromServer(ConnectedPlayer p)
		{
			if (sigTerm)
				return;
			try
			{
				m_sock.BeginReceive(p.recvBuf, 0, MAX_RECV, SocketFlags.None, _recvCB, p);
			}
			catch (ObjectDisposedException)
			{
				Console.WriteLine("ReceiveFromServer terminated due to disposed socket or other object.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("ReceiveFromServer terminated due to other error: " + ex.Message);
			}
		}

		//callback for the ReceiveFromServer function. Only used client-side
		private void _recvCB(IAsyncResult res)
		{
			lock (locker)
			{
				if (sigTerm)
				{
					try
					{
						m_sock.EndReceive(res);
					}
					catch { }
					return;
				}
				ConnectedPlayer p = (ConnectedPlayer)res.AsyncState;

				int bytes;
				try
				{
					bytes = m_sock.EndReceive(res);
				}
				catch
				{
					if (p.OnSocketError != null)
					{
						p.OnSocketError(p, null);
					}
					return;
				}

				if (bytes > 0) //ignore empty packets
				{
					p.PushReceivedData(bytes);
					ThreadPool.QueueUserWorkItem(new WaitCallback(p.HandleReceivedData));
				}

				//this logic was added by Ethan to prevent a race condition on the sigTerm variable
				if (sigTerm) return;
				try
				{
					m_sock.BeginReceive(p.recvBuf, 0, MAX_RECV, SocketFlags.None, _recvCB, p); //receive loop client-side
				}
				catch
				{
					return;
				}
			}
		}

		//Push/PopRecv use queue to order packets
		public void PushRecv(PacketStream recvData)
		{
			this.recvQueue.Enqueue(recvData);
		}
		public PacketStream PopRecv()
		{
			return recvQueue.Dequeue();
		}

		//send just fires a packet off immediately, we don't care about queuing as much as sending asap
		public void Send(Packet pkt)
		{
			if (sigTerm)
				return;
			try
			{
				m_sock.BeginSend(pkt.Data, 0, pkt.Data.Length, SocketFlags.None, _sendCB, pkt.Data.Length);
			}
			catch (ObjectDisposedException)
			{
				Console.WriteLine("Send terminated due to disposed socket or other object.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Send terminated due to other error: " + ex.Message);
			}
		}

		private void _sendCB(IAsyncResult res)
		{
			if (sigTerm)
			{
				try
				{
					m_sock.EndSend(res);
				}
				catch (ObjectDisposedException) { }
				catch (NullReferenceException) { }
				return;
			}

			int len = (int)res.AsyncState;
			if (len != m_sock.EndSend(res))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("error when sending: packet size did not match bytes sent");
				Console.ResetColor();
			}
		}
	}
}
