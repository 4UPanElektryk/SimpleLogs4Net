using System;
using SimpleLogs4Net;
using System.Threading;

namespace SimpleLogs4Net.Tests
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Log.Initialize(AppDomain.CurrentDomain.BaseDirectory + "Logs\\",OutputStream.Both, "Log");
            Log.ClearLogs();
            Log.ChangeDefaultType(EType.Informtion);
            Log.Write("Test");
            Log.Write("Test", EType.Informtion);
            Log.Write("Test", EType.Error);
            Log.Write("Test", EType.Warning);
            Log.Write("Test", EType.Informtion);
            Log.NextLog();
            Log.AddEvent(new Event("text", EType.Normal));
            Log.AddEvent(new Event("text", EType.Error));
            Log.AddEvent(new Event("text", EType.Warning));
            Log.AddEvent(new Event("text", EType.Informtion));
            Log.AddEvent(new Event("text", EType.Normal));
            Log.AddEvent(new Event("text", EType.Critical_Error));
            string[] d =
            {
                "d0",
                "d1",
                "d2",
                "d3"
            };
            Log.AddEvent(new Event(d,EType.Normal));
            Console.ReadLine();
        }
	}
}
