using System;
using SimpleLogs4Net;
using System.Threading;

namespace SimpleLogs4Net.Tests
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Log.DebugMsg("info");
			Log.DebugMsg("ok", EType.Normal);
			new LogConfiguration("Logs\\",OutputStream.Both,"Log");
			Log.ClearLogs();
			LogConfiguration.ChangeDefaultType(EType.Normal);
			Log.Write("Test");
			Log.Write("Test", EType.Normal);
			Log.Write("Test", EType.Informtion);
			Log.DebugMsg("info");
			Log.Write("Test", EType.Warning);
			Log.Write("Test", EType.Error);
			Log.NextLog();
			string[] d =
			{
				"d0",
				"d1",
				"d2",
				"d3"
			};
			Log.Write(d,EType.Normal);
			Console.ReadLine();
		}
	}
}
