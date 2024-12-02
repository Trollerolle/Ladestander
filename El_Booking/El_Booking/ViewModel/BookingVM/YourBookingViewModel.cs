using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using El_Booking.Commands;
using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;
using El_Booking.View;
using Windows.ApplicationModel.Store;

namespace El_Booking.ViewModel.BookingVM
{
    public class YourBookingViewModel : BaseViewModel
    {
        public MainBookingViewModel _mainBookingViewModel { get; set; }
        public User CurrentUser => _mainBookingViewModel.CurrentUser;

        public Booking? CurrentBooking => _mainBookingViewModel.CurrentBooking;
        private readonly Storer _storer;

        public YourBookingViewModel(Storer storer, MainBookingViewModel mainBookingViewModel, DateTime? startingDate = null)
        {
            DateTime today = startingDate ?? DateTime.Today; // til test, så datoen den starter på kan ændres. Ellers dd.
            _storer = storer;
            _mainBookingViewModel = mainBookingViewModel;

        }


        public RelayCommand DeleteBookingCommand => new RelayCommand(
                execute => DeleteBooking(),
                canExecute => CurrentBooking is not null
                );

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
				_mainBookingViewModel.CurrentBooking = null;
                
                OnPropertyChanged();
            }
        }
    }
}
