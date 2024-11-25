using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.View;
using Windows.ApplicationModel.Store;

namespace El_Booking.ViewModel
{
	internal class YourBookingViewModel : BaseViewModel
	{
		BookingRepository _bookingRepo;

		private Booking _usersBooking;

		public Booking UsersBooking
		{
			get { return _usersBooking; }
			set
			{
				_usersBooking = value;
				OnPropertyChanged();
			}
		}


		public YourBookingViewModel(string connectionString, DateTime? startingDate = null) 
		{
			DateTime today = startingDate ?? DateTime.Today; // til test, så datoen den starter på kan ændres. Ellers dd.

			_bookingRepo = new BookingRepository(connectionString);
		}

		public void GetBooking(User user)
		{
			UsersBooking = _bookingRepo.GetBy(user.Email);
		}

		public RelayCommand DeleteBookingCommand => new RelayCommand(
				execute => DeleteBooking(),
				canExecute => HasBooking()
				);

		bool HasBooking()
		{
			return UsersBooking != null;
		}

		public void DeleteBooking()
		{

			var result = MessageBox.Show(
				"Er du sikker på, at du vil slette din booking?",
				"Bekræft sletning",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);

			if (result == MessageBoxResult.Yes)
			{
				//_bookingRepo.Delete(UsersBooking.BookingID);
				UsersBooking = null;
				OnPropertyChanged();
			}
		}
	}
}
