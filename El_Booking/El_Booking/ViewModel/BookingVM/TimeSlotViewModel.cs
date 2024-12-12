using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Model;

namespace El_Booking.ViewModel.BookingVM
{
	public class TimeSlotViewModel
	{
		public readonly int TimeSlotID;

		public string Time { get; set; }
		public TimeOnly StartTime { get; set; }

		public int MondayFull { get; set; }
		public int TuesdayFull { get; set; }
		public int WednesdayFull { get; set; }
		public int ThursdayFull { get; set; }
		public int FridayFull { get; set; }

		public TimeSlotViewModel(TimeSlot timeSlot)
		{
			TimeSlotID = timeSlot.TimeSlotID;

			Time = $"{timeSlot.TimeSlotStart.ToString("HH:mm")} - {timeSlot.TimeSlotEnd.ToString("HH:mm")}";
			StartTime = timeSlot.TimeSlotStart;

			MondayFull = 0;
			TuesdayFull = 0;
			WednesdayFull = 0;
			ThursdayFull = 0;
			FridayFull = 0;
		}

		public void SetDayFull(int day)
		{
			switch (day)
			{
				case 0:
					MondayFull = 1;
					break;
				case 1:
					TuesdayFull = 1;
					break;
				case 2:
					WednesdayFull = 1;
					break;
				case 3:
					ThursdayFull = 1;
					break;
				case 4:
					FridayFull = 1;
					break;
			}
		}

		public void SetPassedAsGrey(int day)
		{
			switch (day)
			{
				case 0:
					MondayFull = 2;
					break;
				case 1:
					TuesdayFull = 2;
					break;
				case 2:
					WednesdayFull = 2;
					break;
				case 3:
					ThursdayFull = 2;
					break;
				case 4:
					FridayFull = 2;
					break;
			}
		}

		public void SetYoursAsOrange(int day)
		{
			switch (day)
			{
				case 0:
					MondayFull = 3;
					break;
				case 1:
					TuesdayFull = 3;
					break;
				case 2:
					WednesdayFull = 3;
					break;
				case 3:
					ThursdayFull = 3;
					break;
				case 4:
					FridayFull = 3;
					break;
			}
		}
	}
}
