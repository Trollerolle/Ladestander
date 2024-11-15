using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
	public class Booking
	{
		private int _bookingID;

		public int BookingID
		{
			get { return _bookingID; }
			set { _bookingID = value; }
		}

		private DateOnly _date;

		public DateOnly Date
		{
			get { return _date; }
			set { _date = value; }
		}

		public Booking(int bookingID, DateOnly date)
		{
			this.BookingID = bookingID;
			this._date = date;
		}
	}
}
