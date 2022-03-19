using System;
namespace SimpleLogs4Net
{
    public class Event
    {
		public DateTime _DateTime;
		public string _Text;
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
			_DateTime = dateTime;
			_Text = text;
			_Type = type;
		}
		public Event(string text, Type type)
		{
			_DateTime = DateTime.UtcNow;
			_Text = text;
			_Type = type;
		}
	}
}
