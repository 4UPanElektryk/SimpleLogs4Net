using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SimpleLogs4Net
{
	public class Log
	{
		private static string _CurrentFile;
		private static List<Event> _EventQue = new List<Event>();
		#region Event Writting
		public static void DebugMsg(string text)
		{
			DebugMsg(text, LogConfiguration._DefaultType);
		}
		public static void DebugMsg(string text, EType type)
		{
			Console.Write("[");
			ConsoleColor color;
			if (type == EType.Normal)
			{
				color = ConsoleColor.Green;
			}
			else if (type == EType.Informtion)
			{
				color = ConsoleColor.Blue;
			}
			else if (type == EType.Warning)
			{
				color = ConsoleColor.Yellow;
			}
			else
			{
				color = ConsoleColor.Red;
			}
			Console.ForegroundColor = color;
			Console.Write("*");
			Console.ResetColor();
			Console.WriteLine("] "+ text);
			Event t = new Event(text, type);
			t._Trace = "Debug";
			if (LogConfiguration._FileOutputEnabled)
			{
				AddEvent(t,true);
			}
			else
			{
				_EventQue.Add(t);
			}
		}
		public static void Write(string text)
		{
			Event t = new Event(text, LogConfiguration._DefaultType);
			t._Trace = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
			AddEvent(t);
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
		public static void AddEvent(Event logEvent, bool DisableConsoleOutput = false)
		{
			if (!LogConfiguration._FileOutputEnabled && !LogConfiguration._ConsoleOutputEnabled)
				return;
			#region Console Output
			if (!DisableConsoleOutput)
			{
				if (LogConfiguration._ConsoleOutputEnabled && LogConfiguration._LogFormatting == "[$date-$time][$type]$trace: $msg")
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
				}
				Console.ResetColor();
				Console.Write("]");
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write(logEvent._Trace);
				Console.ResetColor();
				Console.Write(": ");
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
			}
			#endregion
			if (!LogConfiguration._FileOutputEnabled)
				return;
			#region File Output
			string output = "";
			foreach (Event item in _EventQue)
			{
				output += EventToString(item);
			}
			_EventQue.Clear();
			if (_CurrentFile != null)
			{
				if (File.Exists(_CurrentFile))
				{
					File.WriteAllText(_CurrentFile,File.ReadAllText(_CurrentFile) + EventToString(logEvent));
				}
				else
				{
					FindNewFile();
					File.WriteAllText(_CurrentFile, output + EventToString(logEvent));
				}
			}
			else
			{
				FindNewFile();
				File.WriteAllText(_CurrentFile, output + EventToString(logEvent));
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
		internal static string EventToString(Event logEvent)
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
			}
			string output = LogConfiguration._LogFormatting;
			output = output.Replace("$date", date);
			output = output.Replace("$time", time);
			output = output.Replace("$type", type);
			output = output.Replace("$trace", logEvent._Trace);
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
	}
}
