using System;
using SimpleLogs4Net;
using System.Threading;

namespace SimpleLogs4Net.Tests
{
	internal class Program
	{
        static void Main(string[] args)
        {
            new LogConfiguration("Logs\\",OutputStream.Both,"Log","$time $trace $date $type $msg");
            Log.ClearLogs();
            Log.ChangeDefaultType(EType.Normal);
            Log.Write("Test");
            Log.Write("Test", EType.Informtion);
            Log.NextLog();
            Log.Write("Test", EType.Warning);
            Log.Write("Test", EType.Error);
            Log.Write("Test", EType.Critical_Error);
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
