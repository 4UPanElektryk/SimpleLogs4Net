using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleLogs4Net
{
    internal class Initializer
    {
        public static void InitDirectory(string dir)
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
        }
        public static void InitStream(OutputStream stream)
        {
            switch (stream)
            {
                case OutputStream.None:
                    LogConfiguration._FileOutputEnabled = false;
                    LogConfiguration._ConsoleOutputEnabled = false;
                    break;
                case OutputStream.Console:
                    LogConfiguration._FileOutputEnabled = false;
                    LogConfiguration._ConsoleOutputEnabled = true;
                    break;
                case OutputStream.File:
                    LogConfiguration._FileOutputEnabled = true;
                    LogConfiguration._ConsoleOutputEnabled = false;
                    break;
                case OutputStream.Both:
                    LogConfiguration._FileOutputEnabled = true;
                    LogConfiguration._ConsoleOutputEnabled = true;
                    break;
                default:
                    break;
            }
        }

    }
}
