# Usage
First you need to add Initialize 
```cs
Log log = new Log("Directory where you want to store logs");
```
Example of Usage
```cs
using System;
using SimpleLogs4Net;
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
What Logs Look like
```text
[15.4.2022-7:39:45][NORMAL]text
[15.4.2022-7:39:45][INFO]text
[15.4.2022-7:39:45][WARRNING]text
[15.4.2022-7:39:45][ERROR]text
[15.4.2022-7:39:45][CRITICAL_ERROR]text
[15.4.2022-7:39:45][CRITICAL_ERROR][MULTILINE][8][12]
{
text1
text2
text3
text4
text5
}

```
