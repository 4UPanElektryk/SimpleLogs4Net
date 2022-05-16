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
        private static int _Index = 0;
        private static Event.Type _DefaultType = Event.Type.Normal;
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
        public static void Initialize(string path)
        {
            new Log(path);
        }
        public static void Initialize(string path, string prefix)
        {
            new Log(path, prefix);
        }
        public static void Initialize(string path, bool enabled)
        {
            new Log(path, enabled);
        }
        public static void Initialize(string path, bool enabled, string prefix)
        {
            new Log(path, enabled, prefix);
        }
        public static void ChangeDefaultType(Event.Type type)
        {
            _DefaultType = type;
        }
        public static void ChangeEnable(bool enabled)
		{
			_Enabled = enabled;
		}
        public static void Write(string text)
        {
            AddEvent(new Event(text,_DefaultType));
        }
		public static void Write(string text, Event.Type type)
		{
			AddEvent(new Event(text, type));
		}
		public static void Write(string[] text)
		{
			AddEvent(new Event(text, _DefaultType));
		}
		public static void Write(string[] text, Event.Type type)
		{
			AddEvent(new Event(text, type));
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
					plu[u - 1] = EventToString(logEvent,_Index);
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
					writer.WriteLine(EventToString(logEvent,_Index));
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
            _Index = File.ReadAllLines(@_LastFile).Length;
        }
		public static string EventToString(Event logEvent)
		{
			string s = logEvent._DateTime.ToString("[dd.MM.yyyy-HH:mm:ss]");
			switch (logEvent._Type)
			{
				case Event.Type.Normal:
					s += "[NORMAL]";
					break;
				case Event.Type.Informtion:
					s += "[INFO]";
					break;
				case Event.Type.Warning:
					s += "[WARNING]";
					break;
				case Event.Type.Error:
					s += "[ERROR]";
					break;
				case Event.Type.Critical_Error:
					s += "[CRITICAL_ERROR]";
					break;
			}
            if (logEvent._IsMultiLine)
            {
				s = s + "[MULTILINE]\r\n";
				s = s + "{\r\n";
                foreach (string item in logEvent._MultiineText)
                {
                    s = s + item + "\r\n";
                }
                s = s + "}";
                s = s + logEvent._Text;
			}
            else
            {
				s = s + logEvent._Text;
			}
			return s;
		}
		public static string EventToString(Event logEvent, int startindex)
		{
			string s = logEvent._DateTime.ToString("[dd.MM.yyyy-HH:mm:ss]");
			switch (logEvent._Type)
			{
				case Event.Type.Normal:
					s += "[NORMAL]";
					break;
				case Event.Type.Informtion:
					s += "[INFO]";
					break;
				case Event.Type.Warning:
					s += "[WARNING]";
					break;
				case Event.Type.Error:
					s += "[ERROR]";
					break;
				case Event.Type.Critical_Error:
					s += "[CRITICAL_ERROR]";
					break;
			}
			if (logEvent._IsMultiLine)
			{
				s = s + "[MULTILINE]["+ (_Index + 3) +"]["+ (_Index + 2 + logEvent._MultiineText.Length) + "]\r\n";
				s = s + "{\r\n";
				foreach (string item in logEvent._MultiineText)
				{
					s = s + item + "\r\n";
				}
				s = s + "}";
				s = s + logEvent._Text;
			}
			else
			{
				s = s + logEvent._Text;
			}
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
