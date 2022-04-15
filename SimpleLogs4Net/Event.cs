using System;
namespace SimpleLogs4Net
{
    public class Event
    {
		public DateTime _DateTime;
		public string _Text;
		public string[] _MultiineText;
        internal bool _IsMultiLine;
        public Type _Type;
		public enum Type
		{
			Normal,
			Informtion,
			Warrning,
			Error,
			Critical_Error
		}
		public Event(string text, Type type, DateTime dateTime)
		{
            _IsMultiLine = false;
            _DateTime = dateTime;
			_Text = text;
			_Type = type;
		}
		public Event(string text, Type type)
		{
			_IsMultiLine = false;
			_DateTime = DateTime.UtcNow;
			_Text = text;
			_Type = type;
		}
		public Event(string[] text, Type type, DateTime dateTime)
		{
            _IsMultiLine = true;
            _DateTime = dateTime;
			_MultiineText = text;
			_Type = type;
		}
		public Event(string[] text, Type type)
		{
            _IsMultiLine = true;
            _DateTime = DateTime.UtcNow;
			_MultiineText = text;
			_Type = type;
		}
	}
}
