namespace SimpleLogs4Net
{
    public class LogConfiguration
    {
        internal static string _Dir;
        internal static bool _FileOutputEnabled;
        internal static bool _ConsoleOutputEnabled;
        internal static bool _DebugOutputEnabled = true;
        internal static string _Prefix;
        internal static string _LogFormatting;
        internal static EType _DefaultType = EType.Normal;
        public LogConfiguration()
        {
            Initializer.InitStream(OutputStream.Console);
        }
        public LogConfiguration(string dir, OutputStream stream, string prefix = "Log", string logformatting = "[$date-$time][$type]$trace: $msg")
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
        public static void Initialize(string dir, OutputStream stream)
        {
            new LogConfiguration(dir, stream);
        }
        public static void ChangeStream(OutputStream stream)
        {
            Initializer.InitStream(stream);
        }
        public static void ChangeDefaultType(EType type)
        {
            _DefaultType = type;
        }
        public static void ChangeDebugOutputEnabled(bool enabled)
        {
            _DebugOutputEnabled = enabled;
        }
    }
}
