using System;

namespace ENETCare.Presentation.MVC.Models
{
	public class Notification
	{
		public NotificationLevel Level
		{
			get;
			set;
		}

		public string Message
		{
			get;
			set;
		}

		public string AlertStyle
		{
			get
			{
				switch (Level)
				{
					case NotificationLevel.Info:
						return "alert-success";
					case NotificationLevel.Warning:
						return "alert-danger";
					case NotificationLevel.Error:
						return "alert-warning";
					default:
						return "";
				}
			}
		}
	}

	public enum NotificationLevel
	{
		Info = 0,
		Warning = 1,
		Error = 2
	}
}