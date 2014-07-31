using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ServerClassLibrary
{
	public static class ConfigConst
	{
		public static string CONF_GEN = "general";
		public static string CONF_GEN_PASSWORDLEN = "passwordlen";
		public static string CONF_GEN_PASSWORDENC = "passwordenc";
		public static string CONF_GEN_LOGGEDINUSERS = "loggedinusers";

		public static string CONF_DB = "database";
		public static string CONF_DB_USER = "dbuser";
		public static string CONF_DB_PASS = "dbpass";
		public static string CONF_DB_ADDR = "dbaddr";

		public static string CONF_GAME = "game";
		public static string CONF_GAME_START_MAP = "startmap";
		public static string CONF_GAME_START_X = "startx";
		public static string CONF_GAME_START_Y = "starty";

		public static string CONF_SERVER = "server";
		public static string CONF_SERVER_BIND_ADDR = "bindaddress";
		public static string CONF_SERVER_PORT = "port";
		public static string CONF_SERVER_DUAL = "dualmode";
	}

	public class Config
	{
		private SortedList<string, SortedList<string, object>> sections = new SortedList<string, SortedList<string, object>>();
		public string Filename { get; set; }

		public Config() { Filename = string.Empty; }
		public Config(string filename) { Filename = filename; }

		public bool Load()
		{
			try
			{
				StreamReader str = new StreamReader(Filename);
				string nextLine;
				string header = "";
				while (!str.EndOfStream)
				{
					nextLine = str.ReadLine();
					if (string.IsNullOrEmpty(nextLine) || nextLine[0] == '\'' || nextLine[0] == '#')
					{
						continue;
					}
					else if (nextLine.Contains('#')) //remove any comment from the header
					{
						nextLine = nextLine.Remove(nextLine.IndexOf('#'));
					}
					else if (nextLine.Contains('\''))
					{
						nextLine = nextLine.Remove(nextLine.IndexOf('\''));
					}
					nextLine = nextLine.Trim();

					if (nextLine[0] == '[' && nextLine[nextLine.Length - 1] == ']')
					{
						header = nextLine.Remove(0, 1);
						header = header.Remove(header.Length - 1);
						sections.Add(header, new SortedList<string, object>());
					}
					else
					{
						//        0    5    10   15  19
						//format: identifier(datatype)=value #or identifier ( datatype   ) =    value
						//get type string
						int dTypeStart = nextLine.IndexOf('(') + 1;
						int dTypeLen = nextLine.IndexOf(')') - dTypeStart;
						string typeString;
						if (dTypeStart < 0 || dTypeLen < 0)
						{
							typeString = "string";
						}
						else if (dTypeStart < 1 || dTypeLen < 2 || dTypeStart >= nextLine.Length || dTypeLen >= nextLine.Length)
						{
							continue;
						}
						else
						{
							typeString = nextLine.Substring(dTypeStart, dTypeLen);
							nextLine = nextLine.Remove(dTypeStart - 1, dTypeLen + 2);
						}
						//get the pair of identifier/value
						string[] pair = nextLine.Split(new char[] { '=' });
						if (pair.Length != 2)
							continue;
						//add to the database
						switch (typeString.ToLower())
						{
							case "int":
								int i_res;
								if (!int.TryParse(pair[1], out i_res))
									continue;
								sections[header].Add(pair[0], i_res);
								break;
							case "bool":
								bool b_res;
								if (!bool.TryParse(pair[1], out b_res))
									continue;
								sections[header].Add(pair[0], b_res);
								break;
							case "string":
							default://default to string
								sections[header].Add(pair[0], pair[1]);
								break;
						}
					}
				}

				str.Close();
			}
			catch
			{
				if (!Directory.Exists(Directory.GetParent(Filename).FullName))
				{
					Directory.CreateDirectory(Directory.GetParent(Filename).FullName);
				}
				if (!File.Exists(Filename))
				{
					File.Create(Filename).Close();
				}
				return false;
			}

			return true;
		}

		public bool GetValue(string section, string identifier, out object value)
		{
			value = null;
			if (!sections.ContainsKey(section))
				return false;
			if (!sections[section].ContainsKey(identifier))
				return false;

			value = sections[section][identifier];
			return true;
		}
		public bool GetValue(string identifier, out object value)
		{
			return GetValue("", identifier, out value);
		}

		public bool GetValue(string section, string identifier, out string value)
		{
			value = null;
			if (!sections.ContainsKey(section))
				return false;
			if (!sections[section].ContainsKey(identifier))
				return false;

			value = sections[section][identifier] as string;

			return true;
		}
		public bool GetValue(string identifier, out string value)
		{
			return GetValue("", identifier, out value);
		}

		public bool GetValue(string section, string identifier, out int value)
		{
			value = int.MaxValue;
			if (!sections.ContainsKey(section))
				return false;
			if (!sections[section].ContainsKey(identifier))
				return false;

			value = Convert.ToInt32(sections[section][identifier]);

			return true;
		}
		public bool GetValue(string identifier, out int value)
		{
			return GetValue("general", identifier, out value);
		}

		public bool GetValue(string section, string identifier, out bool value)
		{
			value = false;
			if (!sections.ContainsKey(section))
				return false;
			if (!sections[section].ContainsKey(identifier))
				return false;

			value = Convert.ToBoolean((sections[section][identifier]));

			return true;
		}
		public bool GetValue(string identifier, out bool value)
		{
			return GetValue("general", identifier, out value);
		}

		public void Add(string section, string ident, object value)
		{
			if(sections.ContainsKey(section))
			{
				if (sections[section].ContainsKey(ident))
					sections[section][ident] = value;
				else
					sections[section].Add(ident, value);
			}
			else
			{
				sections.Add(section, new SortedList<string, object>());
				sections[section].Add(ident, value);
			}
		}

		public bool SaveChanges()
		{
			try
			{
				if (!Directory.Exists(Directory.GetParent(Filename).FullName))
				{
					Directory.CreateDirectory(Directory.GetParent(Filename).FullName);
				}
				if (!File.Exists(Filename))
				{
					File.Create(Filename).Close();
				}
				StreamWriter sw = new StreamWriter(Filename);
				foreach(KeyValuePair<string, SortedList<string, object>> section in sections)
				{
					sw.WriteLine("[" + section.Key + "]");
					foreach(KeyValuePair<string, object> dataItem in section.Value)
					{
						Type typeOf = dataItem.Value.GetType();
						string type;
						if (typeOf == typeof(Int32))
							type = "int";
						else if (typeOf == typeof(bool))
							type = "bool";
						else
							type = "string";
						sw.WriteLine(dataItem.Key + "(" + type + ")=" + dataItem.Value);
					}
				}
				sw.Close();
			}
			catch
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Writes a default configuration file with no values
		/// </summary>
		/// <param name="fName">File to write to</param>
		/// <returns>True on success, false on error</returns>
		public static bool WriteEmptyServerConfig(string fName)
		{
			try
			{
				if (!Directory.Exists(Directory.GetParent(fName).FullName))
				{
					Directory.CreateDirectory(Directory.GetParent(fName).FullName);
				}
				if (!File.Exists(fName))
				{
					File.Create(fName).Close();
				}
				StreamWriter sw = new StreamWriter(fName);
				sw.WriteLine("[general]");
				sw.WriteLine("passwordlen(int)=");
				sw.WriteLine("passwordenc(bool)=");
				sw.WriteLine("loggedinusers(int)=\n");
				sw.WriteLine("[database]");
				sw.WriteLine("dbaddr(string)=");
				sw.WriteLine("dbuser(string)=");
				sw.WriteLine("dbpass(string)=");
				sw.WriteLine("[game]");
				sw.WriteLine("startmap(int)=");
				sw.WriteLine("startx(int)=");
				sw.WriteLine("starty(int)=");
				sw.WriteLine("[{0}]", ConfigConst.CONF_SERVER);
				sw.WriteLine(ConfigConst.CONF_SERVER_BIND_ADDR + "(string)=");
				sw.WriteLine(ConfigConst.CONF_SERVER_PORT + "(int)=");
				sw.WriteLine(ConfigConst.CONF_SERVER_DUAL + "(bool)=");
				sw.Close();
			}
			catch
			{
				return false;
			}

			return true;
		}
	}

	public class Log
	{
		private StreamWriter stream;

		public Log()
		{
			this._construct("");
		}

		public Log(string fname)
		{
			this._construct(fname);
		}

		private void _construct(string fname)
		{
			if (string.IsNullOrEmpty(fname))
				fname = ".\\logs\\server.log";

			if (!File.Exists(fname))
			{
				if (!Directory.Exists(".\\logs"))
					Directory.CreateDirectory(".\\logs");
				stream = File.CreateText(fname);
			}
			else
				stream = File.AppendText(fname);

			stream.AutoFlush = true;
		}

		~Log() { Close(); }

		public void Close()
		{
			try
			{
				if (stream != null)
				{
					stream.Close();
					stream.Dispose();
					stream = null;
				}
			}
			catch(ObjectDisposedException)
			{
				return;
			}
		}

		public void LogError(string callingMethod, string format, params object[] args)
		{
			string currTime = DateTime.Now.ToString("MM/dd/yy HH:mm:ss");
			stream.WriteLine("{0,-7}{1,-30}{2,-20}{3}",
				"[ERR]",
				"[" + callingMethod + "]",
				"[" + currTime + "]",
				string.Format(format, args));
		}

		public void LogWarning(string callingMethod, string format, params object[] args)
		{
			string currTime = DateTime.Now.ToString("MM/dd/yy HH:mm:ss");
			stream.WriteLine("{0,-7}{1,-30}{2,-20}{3}",
				"[WRN]",
				"[" + callingMethod + "]",
				"[" + currTime + "]",
				string.Format(format, args));
		}
	}
}
