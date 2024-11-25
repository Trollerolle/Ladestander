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

		private TimeOnly _timeSlotEnd;

		public TimeOnly TimeSlotEnd
		{
			get { return _timeSlotEnd; }
			set { _timeSlotEnd = value; }
		}

		public TimeSlot(int timeSlotID, TimeOnly timeSlotStart, TimeOnly timeSlotEnd) 
		{ 
			this.TimeSlotID = timeSlotID;
			this.TimeSlotStart = timeSlotStart;
			this.TimeSlotEnd = timeSlotEnd;
		}
	}
}
