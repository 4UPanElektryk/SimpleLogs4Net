using System;
namespace SimpleLogs4Net
{
    [Flags]
    public enum OutputStream
    {
        None = 0,
        Console = 1,
        File = 2,
        Both = 3,
    }
}