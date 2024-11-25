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

        private Booking? _usersBooking;
        public Booking? UsersBooking
        {
            get { return _usersBooking; }
            set
            {
                _usersBooking = value;
                OnPropertyChanged();
            }
        }

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        private readonly Storer _storer;

        public YourBookingViewModel(Storer storer, DateTime? startingDate = null)
        {
            DateTime today = startingDate ?? DateTime.Today; // til test, så datoen den starter på kan ændres. Ellers dd.
            _storer = storer;

            var currentApp = Application.Current as App;
            CurrentUser = currentApp?.CurrentUser;

            UsersBooking = GetBooking(CurrentUser);
        }

        public Booking? GetBooking(User user)
        {
            return _storer.BookingRepository.GetBy(user.Email);
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
