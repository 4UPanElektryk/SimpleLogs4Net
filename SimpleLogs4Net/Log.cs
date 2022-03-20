using System.Collections.Generic;
using System.IO;
namespace SimpleLogs4Net
{
	public class Log
	{
		private static string _Path;
		private static bool _Enabled;
		private static string _LastFile;
		private static string _Prefix = "LOG";
		public Log(string path, bool enabled, string prefix)
		{
			if (!Directory.Exists(path))
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", path);
				}
			}
			_Path = path;
			_Enabled = enabled;
			_Prefix = prefix;
		}
		public Log(string path, bool enabled)
		{
			if (!Directory.Exists(path))
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", path);
				}
			}
			_Path = path;
			_Enabled = enabled;
		}
		public Log(string path,string prefix)
		{
			if (!Directory.Exists(path))
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", path);
				}
				
			}
			_Path = path;
			_Enabled = true;
			_Prefix = prefix;
		}
		public Log(string path)
		{
			if (!Directory.Exists(path))
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", path);
				}
			}
			_Path = path;
			_Enabled = true;
		}
		public static void ChangeEnable(bool enabled)
		{
			_Enabled = enabled;
		}
		public static void AddEvent(Event logEvent)
		{
			if (!_Enabled)
				return;
			if (@_LastFile != null)
			{
				if (File.Exists(@_LastFile))
				{
					string[] pl = File.ReadAllLines(@_LastFile);
					int u = pl.Length + 1;
					string[] plu = new string[u];
					int i = 0;
					foreach (string item in pl)
					{
						plu[i] = item;
						i++;
					}
					plu[u - 1] = EventToString(logEvent);
					StreamWriter writer = new StreamWriter(@_LastFile);
					foreach (string item in plu)
					{
						writer.WriteLine(item);
					}
					writer.Close();
				}
				else
				{
					int i = 1;
					do
					{
						_LastFile = _Path + _Prefix + i.ToString() + ".log";
						i++;
					} while (File.Exists(@_LastFile));
					StreamWriter writer = new StreamWriter(@_LastFile);
					writer.WriteLine(EventToString(logEvent));
					writer.Close();
				}
			}
			else
			{
				int i = 1;
				do
				{
					_LastFile = _Path + _Prefix + i.ToString() + ".log";
					i++;
				} while (File.Exists(@_LastFile));
				StreamWriter writer = new StreamWriter(@_LastFile);
				writer.WriteLine(EventToString(logEvent));
				writer.Close();
			}
		}
		public static string EventToString(Event logEvent)
		{
			string s = "[";
			s = s + logEvent._DateTime.Day.ToString() + "."
				+ logEvent._DateTime.Month.ToString() + "."
				+ logEvent._DateTime.Year.ToString() + "-"
				+ logEvent._DateTime.Hour.ToString() + ":"
				+ logEvent._DateTime.Minute.ToString() + ":"
				+ logEvent._DateTime.Second.ToString() + "]";
			switch (logEvent._Type)
			{
				case Event.Type.Normal:
					s += "[NORMAL]";
					break;
				case Event.Type.Informtion:
					s += "[INFO]";
					break;
				case Event.Type.Warrning:
					s += "[WARRNING]";
					break;
				case Event.Type.Error:
					s += "[ERROR]";
					break;
				case Event.Type.Critical_Error:
					s += "[CRITICAL_ERROR]";
					break;
			}
			s = s + logEvent._Text;
			return s;
		}
		public static void ClearLogs()
		{
			List<string> LogFiles = new List<string>();
			string x;
			int i = 1;
			do
			{
				x = _Path + _Prefix + i.ToString() + ".log";
				if (File.Exists(@x))
				{
					LogFiles.Add(x);
				}
				i++;
			} while (File.Exists(@x));
			foreach (string item in LogFiles)
			{
				File.Delete(item);
			}
		}
	}
}
