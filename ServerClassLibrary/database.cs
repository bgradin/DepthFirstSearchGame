using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading;

namespace ServerClassLibrary
{
	//use http://dev.mysql.com/downloads/file.php?id=450594 for connector
	public class DB : IDisposable
	{
		private MySqlConnection dbConn;
		private bool connected = false;
		private readonly object _LOCK_ = new object();

		public DB()
		{
			dbConn = new MySqlConnection();
		}

		public DB(string uName, string pass, string server)
		{
			dbConn = new MySqlConnection("server=" + server + ";userid=" + uName + ";pwd=" + pass + ";database=test");
		}

		public bool Connect()
		{
			if (connected)
				return true;

			bool ret = true;

			try
			{
				dbConn.Open();
				connected = true;
			}
			catch
			{
				ret = false;
			}

			return ret;
		}

		public bool Connect(string uName, string pass, string server)
		{
			string newConnStr = "server=" + server + ";userid=" + uName + ";pwd=" + pass + ";database=test";
			if (connected && dbConn.ConnectionString == newConnStr)
				return true;

			bool ret = true;
			if (connected)
			{
				dbConn.Close();
				connected = false;
			}
			dbConn.ConnectionString = newConnStr;

			dbConn.Open();
			
			connected = true;

			return ret;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			dbConn.Close();
			dbConn.Dispose();
			dbConn = null;
		}

		//private helper functions
		private int QueryModify(MySqlCommand cmd)
		{
			int ret;
			lock (_LOCK_)
			{
				try
				{
					ret = cmd.ExecuteNonQuery();
				}
				catch
				{
					ret = -1;
				}
			}
			return ret;
		}
		private MySqlDataReader QueryLookup(MySqlCommand cmd)
		{
			MySqlDataReader ret;
			lock (_LOCK_)
			{
				try
				{
					ret = cmd.ExecuteReader();
				}
				catch
				{
					ret = null;
				}
			}
			return ret;
		}
		private object[] QueryLookupFirst(MySqlCommand cmd)
		{
			object[] ret;
			lock (_LOCK_)
			{
				try
				{
					using (MySqlDataReader res = cmd.ExecuteReader(CommandBehavior.SingleResult))
					{
						if (res.HasRows)
						{
							ret = new object[res.FieldCount];
							if (res.Read())
								res.GetValues(ret);
							else
								ret = null;
						}
						else ret = null;
					}
				}
				catch
				{
					ret = null;
				}
			}
			return ret;
		}

		public bool CreateAccount(string user, string pass, string ip, int map, int x, int y)
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "INSERT INTO accounts(username,password,lastip,loc_map,loc_x,loc_y) " + "VALUES (?User,?Pass,?Ip,?map,?x,?y)";
					cmd.Prepare();

					cmd.Parameters.AddWithValue("?User", user);
					cmd.Parameters.AddWithValue("?Pass", pass);
					cmd.Parameters.AddWithValue("?Ip", ip);
					cmd.Parameters.AddWithValue("?map", map);
					cmd.Parameters.AddWithValue("?x", x);
					cmd.Parameters.AddWithValue("?y", y);

					return QueryModify(cmd) == 1;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool FindAccount(string name)
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "SELECT username FROM accounts WHERE username=?Name";
					cmd.Prepare();

					cmd.Parameters.AddWithValue("?Name", name);

					return QueryLookupFirst(cmd) != null;
				}
			}
			catch
			{
				return false;
			}
		}

		public object[] GetAccount(string name)
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "SELECT * FROM accounts WHERE username=?Name";
					cmd.Prepare();

					cmd.Parameters.AddWithValue("?Name", name);

					return QueryLookupFirst(cmd);
				}
			}
			catch
			{
				return null;
			}
		}

		public bool UpdateAccountInfo(string acc, int mapNum, int mapX, int mapY, int bactCount)
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "UPDATE accounts SET bacteria_count=?cnt, loc_map=?map, loc_x=?x, loc_y=?y WHERE username=?acc";
					cmd.Prepare();

					cmd.Parameters.AddWithValue("?cnt", bactCount);
					cmd.Parameters.AddWithValue("?map", mapNum);
					cmd.Parameters.AddWithValue("?x", mapX);
					cmd.Parameters.AddWithValue("?y", mapY);
					cmd.Parameters.AddWithValue("?acc", acc);

					return QueryModify(cmd) >= 0; //query returns 0 when there is no change in data, even if successful. -1 indicates an exception of some kind.
				}
			}
			catch
			{
				return false;
			}
		}

		public bool Login(string acc, string newIP)
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "UPDATE accounts SET lastip=?NewIP,loggedin=1 WHERE username=?Acc";
					cmd.Prepare();

					cmd.Parameters.AddWithValue("?NewIP", newIP);
					cmd.Parameters.AddWithValue("?Acc", acc);

					return QueryModify(cmd) != 0;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool Logout(string acc)
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "UPDATE accounts SET loggedin=0 WHERE username=?Acc";
					cmd.Prepare();

					cmd.Parameters.AddWithValue("?Acc", acc);

					return QueryModify(cmd) != 0;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool LogoutAll()
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "UPDATE accounts SET loggedin=0"; //do it for all accounts

					return QueryModify(cmd) != 0;
				}
			}
			catch
			{
				return false;
			}
		}

		public bool MakeAdmin(string userName)
		{
			try
			{
				using (MySqlCommand cmd = dbConn.CreateCommand())
				{
					cmd.CommandText = "UPDATE accounts SET admin_level=1 WHERE username=?Acc";
					cmd.Prepare();

					cmd.Parameters.AddWithValue("?Acc", userName);

					return QueryModify(cmd) != 0;
				}
			}
			catch
			{
				return false;
			}
		}
	}
}