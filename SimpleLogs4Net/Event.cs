﻿using System;
using System.Diagnostics;

namespace SimpleLogs4Net
{
    public class Event
    {
		public DateTime _DateTime;
		public string _Text;
		public string _Trace;
        internal bool _IsMultiLine;
        public EType _Type;
		public Event(string text, EType type, DateTime dateTime)
		{
			_IsMultiLine = false;
            _DateTime = dateTime;
			_Text = text;
			_Type = type;
		}
		public Event(string text, EType type)
		{
			_IsMultiLine = false;
			_DateTime = DateTime.UtcNow;
			_Text = text;
			_Type = type;
		}
	}
}
