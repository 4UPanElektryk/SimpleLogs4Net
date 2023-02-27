using System;
using System.Diagnostics;
using System.IO;

namespace SimpleLogs4Net
{
	public class Log
	{
		private static string _CurrentFile;
        #region Event Writting
        public static void Write(string text)
        {
            AddEvent(new Event(text,LogConfiguration._DefaultType));
        }
		public static void Write(string text, EType type)
		{
			AddEvent(new Event(text, type));
		}
		public static void Write(string[] text)
		{
			AddEvent(new Event(text, LogConfiguration._DefaultType));
		}
		public static void Write(string[] text, EType type)
		{
			AddEvent(new Event(text, type));
		}
        public static void Write(string text, object sender)
        {
            AddEvent(new Event(text, LogConfiguration._DefaultType), sender);
        }
        public static void Write(string text, EType type, object sender)
        {
            AddEvent(new Event(text, type), sender);
        }
        public static void Write(string[] text, object sender)
        {
            AddEvent(new Event(text, LogConfiguration._DefaultType), sender);
        }
        public static void Write(string[] text, EType type, object sender)
        {
            AddEvent(new Event(text, type),sender);
        }
        public static void AddEvent(Event logEvent)
		{
			if (!LogConfiguration._FileOutputEnabled && !LogConfiguration._ConsoleOutputEnabled)
				return;
            #region Console Output
            if (LogConfiguration._ConsoleOutputEnabled && LogConfiguration._LogFormatting == "[$date-$time][$type]$msg")
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
            else if (LogConfiguration._ConsoleOutputEnabled)
            {
                Console.Write(EventToString(logEvent));
            }
            #endregion
            if (!LogConfiguration._FileOutputEnabled)
				return;
            #region File Output
            if (_CurrentFile != null)
			{
				if (File.Exists(_CurrentFile))
				{
					File.WriteAllText(_CurrentFile,File.ReadAllText(_CurrentFile) + EventToString(logEvent));
				}
				else
				{
					FindNewFile();
                    File.WriteAllText(_CurrentFile, EventToString(logEvent));
				}
			}
			else
			{
                FindNewFile();
                File.WriteAllText(_CurrentFile, EventToString(logEvent));
            }
            #endregion
        }
        public static void AddEvent(Event logEvent, object sender)
        {
            if (!LogConfiguration._FileOutputEnabled && !LogConfiguration._ConsoleOutputEnabled)
                return;
            #region Console Output
            if (LogConfiguration._ConsoleOutputEnabled && LogConfiguration._LogFormatting == "[$date-$time][$type]$msg")
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
            else if (LogConfiguration._ConsoleOutputEnabled)
            {
                Console.Write(EventToString(logEvent, sender));
            }
            #endregion
            if (!LogConfiguration._FileOutputEnabled)
                return;
            #region File Output
            if (_CurrentFile != null)
            {
                if (File.Exists(_CurrentFile))
                {
                    File.WriteAllText(_CurrentFile, File.ReadAllText(_CurrentFile) + EventToString(logEvent, sender));
                }
                else
                {
                    FindNewFile();
                    File.WriteAllText(_CurrentFile, EventToString(logEvent, sender));
                }
            }
            else
            {
                FindNewFile();
                File.WriteAllText(_CurrentFile, EventToString(logEvent, sender));
            }
            #endregion
        }
        #endregion
        #region File Manipulation
        public static void NextLog() 
		{
			_CurrentFile = null;
		}
        internal static void FindNewFile()
		{
            int i = 1;
            do
            {
                _CurrentFile = LogConfiguration._Dir + LogConfiguration._Prefix + i.ToString() + ".log";
                i++;
            } while (File.Exists(_CurrentFile));
        }
		public static void ClearLogs()
		{
            foreach (string item in Directory.GetFiles(LogConfiguration._Dir))
            {
                if (item.EndsWith(".log"))
                {
					File.Delete(item);
                }
            }
		}
#endregion
        internal static string EventToString(Event logEvent, object sender)
        {
            string date = logEvent._DateTime.ToString("dd.MM.yyyy");
            string time = logEvent._DateTime.ToString("HH:mm:ss");
            string type = "";
            //string trace = Environment.StackTrace;
            switch (logEvent._Type)
            {
                case EType.Normal:
                    type = "NORMAL";
                    break;
                case EType.Informtion:
                    type = "INFO";
                    break;
                case EType.Warning:
                    type = "WARNING";
                    break;
                case EType.Error:
                    type = "ERROR";
                    break;
                case EType.Critical_Error:
                    type = "CRITICAL_ERROR";
                    break;
            }
            string output = LogConfiguration._LogFormatting;
            output = output.Replace("$date", date);
            output = output.Replace("$time", time);
            output = output.Replace("$type", type);
            output = output.Replace("$sender", sender.GetType().Name);
            if (logEvent._IsMultiLine)
            {
                string s = "[MULTILINE]\r\n";
                s += "{\r\n";
                foreach (string item in logEvent._MultiineText)
                {
                    s += item + "\r\n";
                }
                s += "}\r\n";
                output = output.Replace("$msg", s);
            }
            else
            {
                output = output.Replace("$msg", logEvent._Text);
            }
            return output + "\r\n";
        }
        public static string EventToString(Event logEvent)
		{
			string date = logEvent._DateTime.ToString("dd.MM.yyyy");
			string time = logEvent._DateTime.ToString("HH:mm:ss");
			string type = "";
			switch (logEvent._Type)
			{
				case EType.Normal:
					type = "NORMAL";
					break;
				case EType.Informtion:
					type = "INFO";
					break;
				case EType.Warning:
					type = "WARNING";
					break;
				case EType.Error:
					type = "ERROR";
					break;
				case EType.Critical_Error:
					type = "CRITICAL_ERROR";
					break;
			}
			string output = LogConfiguration._LogFormatting;
			output = output.Replace("$date", date);
			output = output.Replace("$time", time);
			output = output.Replace("$type", type);
            if (logEvent._IsMultiLine)
            {
				string s = "[MULTILINE]\r\n";
				s += "{\r\n";
                foreach (string item in logEvent._MultiineText)
                {
                    s += item + "\r\n";
                }
                s += "}\r\n";
                output = output.Replace("$msg", s);
            }
            else
            {
				output = output.Replace("$msg",logEvent._Text);
			}
            return output + "\r\n";
		}
	}
}
