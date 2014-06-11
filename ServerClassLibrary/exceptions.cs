using System;

namespace ServerClassLibrary
{
	[Serializable()]
	public class ServerInitException : Exception
	{
		public ServerInitException() : base("Error initializing the server.") { }
		public ServerInitException(string msg) : base(msg) { }
		public ServerInitException(string msg, Exception innerException) : base(msg, innerException) { }
	}

	[Serializable()]
	public class ServerStartException : Exception
	{
		public ServerStartException() : base("Error starting the server.") { }
		public ServerStartException(string msg) : base(msg) { }
		public ServerStartException(string msg, Exception innerException) : base(msg, innerException) { }
	}

	[Serializable()]
	public class ServerShutdownException : Exception
	{
		public ServerShutdownException() : base("Error shutting down the server.") { }
		public ServerShutdownException(string msg) : base(msg) { }
		public ServerShutdownException(string msg, Exception innerException) : base(msg, innerException) { }
	}

	[Serializable()]
	public class ServerClosedException : Exception
	{
		public ServerClosedException() : base("The connection to the server has been closed.") { }
		public ServerClosedException(string msg) : base(msg) { }
		public ServerClosedException(string msg, Exception innerException) : base(msg, innerException) { }
	}

	[Serializable()]
	public class ClientConnectException : Exception
	{
		public ClientConnectException() : base("Could not connect to the server.") { }
		public ClientConnectException(string msg) : base(msg) { }
		public ClientConnectException(string msg, Exception innerException) : base(msg, innerException) { }
	}

	[Serializable()]
	public class ClientCreateException : Exception
	{
		public ClientCreateException() : base("Could not create client object.") { }
		public ClientCreateException(string msg) : base(msg) { }
		public ClientCreateException(string msg, Exception innerException) : base(msg, innerException) { }
	}
}