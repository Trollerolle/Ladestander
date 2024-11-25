using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
	public class TimeSlot
	{
		private int _timeSlotID;

		public int TimeSlotID
		{
			get { return _timeSlotID; }
			set { _timeSlotID = value; }
		}

		private TimeOnly _timeSlotStart;

		public TimeOnly TimeSlotStart
		{
			get { return _timeSlotStart; }
			set { _timeSlotStart = value; }
		}

		private int _interval;

		public int Interval
		{
			get { return _interval; }
			set { _interval = value; }
		}

		public TimeSlot(int timeSlotID, TimeOnly timeSlotStart, int interval) 
		{ 
			this.TimeSlotID = timeSlotID;
			this.TimeSlotStart = timeSlotStart;
			this.Interval = interval;
		}
	}
}
