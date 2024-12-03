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
		public int CarID { get; set; }

		public Booking(TimeSlot timeSlot, int chargingPointID, DateOnly date, int bookingID, int carID)
		{
			BookingID = bookingID;
			TimeSlot = timeSlot;
			ChargingPointID = chargingPointID;
			Date = date;
			CarID = carID;

		}

		public Booking()
		{ }
    }
}
