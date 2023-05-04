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
            LogConfiguration._ConsoleOutputEnabled = (int)stream % 2 != 0;
            LogConfiguration._FileOutputEnabled = (int)stream > 1;
        }
    }
}
