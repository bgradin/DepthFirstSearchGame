using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;

namespace GameClassLibrary
{
	public sealed class MySqlConnector : IDisposable
	{
		public string Username { get; private set; }
		public string Database { get; private set; }
		public string Server { get; private set; }
		public bool IsOpen { get; private set; }

		public MySqlConnector()
		{
			Username = m_password = Database = Server = "";
			IsOpen = false;
		}
		~MySqlConnector()
		{
			Dispose();
		}

		public void Dispose()
		{
			Close();
		}

		public MySqlConnector(string connectionString)
		{
			string[] fields = connectionString.Split(';');

			foreach (string str in fields)
			{
				string[] splitValue = str.Split('=');
				if (splitValue.Length != 2)
					throw new Exception("Invalid connection string");

				switch (splitValue[0].ToLower())
				{
					case "userid":
					case "uid":
					case "user":
						Username = splitValue[1];
						break;

					case "server":
						Server = splitValue[1];
						break;

					case "db":
					case "database":
						Database = splitValue[1];
						break;

					case "pwd":
					case "password":
						m_password = splitValue[1];
						break;
				}
			}
		}

		public MySqlConnector(string username, string password, string database, string server)
		{
			Username = username;
			m_password = password;
			Database = database;
			Server = server;
			IsOpen = false;
		}

		public bool OpenConnection()
		{
			if (Username == "" || Server == "" || Database == "" || IsOpen)
				return false;

			string connectionString = @"server=" + Server + ";userid=" + Username + ";pwd=" + m_password + "database=" + Database;
			connection = new MySqlConnection(connectionString);
			connection.Open();
			return true;
		}

		public void Close()
		{
			if (connection != null && IsOpen)
			{
				connection.Close();
				connection.Dispose();
			}
		}

		public DataTable RunCommand(string command)
		{
			if (command == "")
				throw new Exception("A valid command must be provided");

			MySqlCommand cmd = new MySqlCommand(command);

			using (MySqlDataReader reader = cmd.ExecuteReader())
			{
				DataTable table = new DataTable();
				table.Load(reader);

				return table;
			}
		}

		MySqlConnection connection;
		string m_password;
	}
}
