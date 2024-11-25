using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
	public class Booking
	{
        string _timeSlotStart;
		public string TimeSlotStart
		{ 
			get { return _timeSlotStart; }
			set { _timeSlotStart = value; }
		}

		private int _chargingPointID;
        public int ChargingPointID 
		{
			get { return _chargingPointID; }
			set { _chargingPointID = value; }
		}

        private DateOnly _date;
		public DateOnly Date
		{
			get { return _date; }
			set { _date = value; }
		}

		private string? _userEmail;
		public string? UserEmail
		{
			get { return _userEmail; }
			set { _userEmail = value; }
		}

		public Booking(string timeSlotStart, int chargingPointID, DateOnly date)
		{
			_timeSlotStart = timeSlotStart;
			_chargingPointID = chargingPointID;
			this._date = date;
		}
    }
}
