using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
namespace SimpleLogs4Net
{
	public class Log
	{
		private static string _Dir;
		public static bool _FileOutputEnabled;
		public static bool _ConsoleOutputEnabled;
		private static string _LastFile;
		public static string _Prefix = "LOG";
        private static int _Index = 0;
        public static EType _DefaultType = EType.Normal;
		private static void InitStream(OutputStream stream)
		{
			switch (stream)
			{
				case OutputStream.None:
					_FileOutputEnabled = false;
					_ConsoleOutputEnabled = false;
					break;
				case OutputStream.Console:
					_FileOutputEnabled = false;
					_ConsoleOutputEnabled = true;
					break;
				case OutputStream.File:
					_FileOutputEnabled = true;
					_ConsoleOutputEnabled = false;
					break;
				case OutputStream.Both:
					_FileOutputEnabled = true;
					_ConsoleOutputEnabled = true;
					break;
				default:
					break;
			}
		}
        public Log(string dir, OutputStream stream, string prefix)
		{
			if (!Directory.Exists(dir))
			{
				try
				{
					Directory.CreateDirectory(dir);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", dir);
				}
			}
			_Dir = dir;
			InitStream(stream);
			_Prefix = prefix;
		}
		public Log(string dir, OutputStream stream)
		{
			if (!Directory.Exists(dir))
			{
				try
				{
					Directory.CreateDirectory(dir);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", dir);
				}
			}
			_Dir = dir;
			InitStream(stream);
		}
		public Log(string dir,string prefix)
		{
			if (!Directory.Exists(dir))
			{
				try
				{
					Directory.CreateDirectory(dir);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", dir);
				}
				
			}
			_Dir = dir;
			_FileOutputEnabled = true;
			_Prefix = prefix;
		}
		public Log(string dir)
		{
			if (!Directory.Exists(dir))
			{
				try
				{
					Directory.CreateDirectory(dir);
				}
				catch
				{
					throw new FileNotFoundException("Directory Not Found", dir);
				}
			}
			_Dir = dir;
			_FileOutputEnabled = true;
		}
        public static void Initialize(string path)
        {
            new Log(path);
        }
        public static void Initialize(string path, string prefix)
        {
            new Log(path, prefix);
        }
        public static void Initialize(string path, OutputStream stream)
        {
            new Log(path, stream);
        }
        public static void Initialize(string path, OutputStream stream, string prefix)
        {
            new Log(path, stream, prefix);
        }
		public static void NextLog() 
		{
			_LastFile = null;
			_Index = 0;
		}
        public static void ChangeDefaultType(EType type)
        {
            _DefaultType = type;
        }
        public static void ChangeOutputStream(OutputStream stream)
		{
			InitStream(stream);
		}
        public static void Write(string text)
        {
            AddEvent(new Event(text,_DefaultType));
        }
		public static void Write(string text, EType type)
		{
			AddEvent(new Event(text, type));
		}
		public static void Write(string[] text)
		{
			AddEvent(new Event(text, _DefaultType));
		}
		public static void Write(string[] text, EType type)
		{
			AddEvent(new Event(text, type));
		}
		public static void AddEvent(Event logEvent)
		{
			if (!_FileOutputEnabled && !_ConsoleOutputEnabled)
				return;
            #region Console Output
            if (_ConsoleOutputEnabled)
			{
				Console.ResetColor();
				Console.Write("[");
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write(logEvent._DateTime.ToString("dd.MM.yyyy"));
				Console.ResetColor();
				Console.Write("-");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write(logEvent._DateTime.ToString("HH:mm:ss"));
				Console.ResetColor();
				Console.Write("][");
				switch (logEvent._Type)
				{
					default:
					case EType.Normal:
						Console.Write("NORMAL");
						break;
					case EType.Informtion:
						Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("INFO");
                        break;
					case EType.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("WARNING");
                        break;
					case EType.Error:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("ERROR");
                        break;
					case EType.Critical_Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("CRITICAL_ERROR");
                        break;
				}
				Console.ResetColor();
				Console.Write("]");
				if (logEvent._IsMultiLine)
				{
					Console.WriteLine("[MULTILINE]");
					Console.WriteLine("{");
					foreach (string item in logEvent._MultiineText)
					{
						Console.WriteLine(item);
					}
					Console.WriteLine("}");
                }
				else
				{
					Console.WriteLine(logEvent._Text);
				}
				Console.ResetColor();
			}
#endregion
            if (!_FileOutputEnabled)
				return;
            #region File Output
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
						_LastFile = _Dir + _Prefix + i.ToString() + ".log";
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
					_LastFile = _Dir + _Prefix + i.ToString() + ".log";
					i++;
				} while (File.Exists(@_LastFile));
				StreamWriter writer = new StreamWriter(@_LastFile);
				writer.WriteLine(EventToString(logEvent));
				writer.Close();
			}
            _Index = File.ReadAllLines(@_LastFile).Length;
            #endregion
        }
        public static string EventToString(Event logEvent)
		{
			string s = logEvent._DateTime.ToString("[dd.MM.yyyy-HH:mm:ss]");
			switch (logEvent._Type)
			{
				case EType.Normal:
					s += "[NORMAL]";
					break;
				case EType.Informtion:
					s += "[INFO]";
					break;
				case EType.Warning:
					s += "[WARNING]";
					break;
				case EType.Error:
					s += "[ERROR]";
					break;
				case EType.Critical_Error:
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
				case EType.Normal:
					s += "[NORMAL]";
					break;
				case EType.Informtion:
					s += "[INFO]";
					break;
				case EType.Warning:
					s += "[WARNING]";
					break;
				case EType.Error:
					s += "[ERROR]";
					break;
				case EType.Critical_Error:
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
            foreach (string item in Directory.GetFiles(_Dir))
            {
                if (item.EndsWith(".log"))
                {
					File.Delete(item);
                }
            }
		}
	}
}
