using System;
using System.Diagnostics;
using System.IO;

namespace SimpleLogs4Net
{
	public class Log
	{
		internal static string _CurrentFile;
		
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
                WriterThread.AddEvent(t,true);
			}
		}
		public static void Write(string text)
		{
			Event t = new Event(text, LogConfiguration._DefaultType);
			t._Trace = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
            WriterThread.AddEvent(t);
		}
		public static void Write(string text, EType type)
		{
            WriterThread.AddEvent(new Event(text, type));
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
	}
}
