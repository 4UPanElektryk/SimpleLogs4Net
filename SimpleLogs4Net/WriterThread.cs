using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
namespace SimpleLogs4Net
{
    internal class WriterThread
    {
        private static Queue<Event> _EventQueue = new Queue<Event>();
        public WriterThread()
        {
            Thread t = new Thread(() => Loop());
            t.Start();
        }
        private static void Loop()
        {
            while (true)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                WriteAllInQueue();
                sw.Stop();
                int dur = (int)(1000 - sw.ElapsedMilliseconds);
                if (dur > 0)
                {
                    Thread.Sleep(dur);
                }
            }
        }
        public static void AddEvent(Event logEvent, bool skipConsole = false)
        {
            _EventQueue.Enqueue(logEvent);
            if (skipConsole){ return; }
            #region Console Output
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
                Console.WriteLine(logEvent._Text);
                Console.ResetColor();
            }
            else if (LogConfiguration._ConsoleOutputEnabled)
            {
                Console.Write(EventToString(logEvent));
            }
            #endregion
        }
        public static void WriteAllInQueue()
        {
            while (_EventQueue.Count > 0)
            {
                Write(_EventQueue.Dequeue());
            };
        }
        private static void Write(Event logEvent)
        {
            if (!LogConfiguration._FileOutputEnabled)
                return;
            #region File Output
            if (Log._CurrentFile != null)
            {
                if (File.Exists(Log._CurrentFile))
                {
                    using (StreamWriter sw = File.AppendText(Log._CurrentFile))
                    {
                        sw.Write(EventToString(logEvent));
                    }
                }
                else
                {
                    Log.FindNewFile();
                    File.WriteAllText(Log._CurrentFile, EventToString(logEvent));
                }
            }
            else
            {
                Log.FindNewFile();
                File.WriteAllText(Log._CurrentFile, EventToString(logEvent));
            }
            #endregion
        }
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
            output = output.Replace("$msg", logEvent._Text);
            return output + "\r\n";
        }

    }
}
