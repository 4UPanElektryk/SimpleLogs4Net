using System;
using System.Diagnostics;

namespace SimpleLogs4Net
{
    public class Event
    {
		public DateTime _DateTime;
		public string _Text;
		public string[] _MultiineText;
		public string _Trace;
        internal bool _IsMultiLine;
        public EType _Type;
		public Event(string text, EType type, DateTime dateTime)
		{
			_Trace = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
			_IsMultiLine = false;
            _DateTime = dateTime;
			_Text = text;
			_Type = type;
		}
		public Event(string text, EType type)
		{
			_Trace = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
			_IsMultiLine = false;
			_DateTime = DateTime.UtcNow;
			_Text = text;
			_Type = type;
		}
		public Event(string[] text, EType type, DateTime dateTime)
		{
			_Trace = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
			_IsMultiLine = true;
            _DateTime = dateTime;
			_MultiineText = text;
			_Type = type;
		}
		public Event(string[] text, EType type)
		{
			_Trace = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
			_IsMultiLine = true;
            _DateTime = DateTime.UtcNow;
			_MultiineText = text;
			_Type = type;
		}
	}
}
