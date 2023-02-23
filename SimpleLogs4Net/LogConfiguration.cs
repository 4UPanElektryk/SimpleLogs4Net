using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleLogs4Net
{
    public class LogConfiguration
    {
        internal static string _Dir;
        internal static bool _FileOutputEnabled;
        internal static bool _ConsoleOutputEnabled;
        internal static string _Prefix;
        internal static string _LogFormatting;
        internal static EType _DefaultType = EType.Normal;

        public LogConfiguration(string dir, OutputStream stream, string prefix = "LOG", string logformatting = "[$date-$time][$type]$msg"
        {
            _Dir = dir;
            _Prefix = prefix;
            _LogFormatting = logformatting;
            Initializer.InitStream(stream);
            if (_FileOutputEnabled)
            {
                Initializer.InitDirectory(dir);
            }
        }
        public void Initialize(string dir, OutputStream stream)
        {
            new LogConfiguration(dir,stream);
        }
    }
}
