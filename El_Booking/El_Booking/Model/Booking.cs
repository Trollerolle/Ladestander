using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
	public class Booking
	{
        int _timeSlot;
		public int TimeSlot 
		{ 
			get { return _timeSlot; }
			set { _timeSlot = value; } // skal vi fjerne setteren??
		}

		private int _chargingPoint;
        public int ChargingPoint 
		{
			get { return _chargingPoint; }
			set { _chargingPoint = value; }
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

		public Booking(int timeSlot, int chargingPoint, DateOnly date)
		{
			_timeSlot = timeSlot;
			_chargingPoint = chargingPoint;
			this._date = date;
		}
    }
}
