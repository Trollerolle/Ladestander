using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
	public class Booking
	{
        private TimeSlot _timeSlot;
		public TimeSlot TimeSlot
		{ 
			get { return _timeSlot; }
			set { _timeSlot = value; }
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

		private int _bookingID;
		public int BookingID
		{
			get { return _bookingID; }
			set { _bookingID = value; }
		}

		private string? _userEmail;
		public string? UserEmail
		{
			get { return _userEmail; }
			set { _userEmail = value; }
		}

		public Booking(TimeSlot timeSlot, int chargingPointID, DateOnly date, int bookingID)
		{
			this.BookingID = bookingID;
			_timeSlot = timeSlot;
			_chargingPointID = chargingPointID;
			this._date = date;
		}

		public Booking()
		{ }
    }
}
