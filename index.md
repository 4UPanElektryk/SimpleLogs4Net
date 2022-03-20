# Usage
First you need to add Initialize 
```cs
Log log = new Log("Directory where you want to store logs");
```
Example of Usage
```cs
using System;

namespace SimpleLogs4Net.Tests
{
    internal class Program
    {
        static Log log;
        static void Main(string[] args)
        {
            log = new Log(AppDomain.CurrentDomain.BaseDirectory + "Logs\\");
            Log.AddEvent(new Event("text", Event.Type.Normal));
            Log.AddEvent(new Event("text", Event.Type.Informtion));
            Log.AddEvent(new Event("text", Event.Type.Warrning));
            Log.AddEvent(new Event("text", Event.Type.Error));
            Log.AddEvent(new Event("text", Event.Type.Critical_Error));
        }
    }
}
```
