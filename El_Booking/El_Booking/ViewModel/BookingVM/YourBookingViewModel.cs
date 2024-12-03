using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public MainBookingViewModel MainBookingViewModel { get; }

        public User CurrentUser => MainBookingViewModel.CurrentUser;

        public Booking? CurrentBooking => MainBookingViewModel.CurrentBooking;

        private readonly Storer _storer;

        public YourBookingViewModel(Storer storer, MainBookingViewModel mainBookingViewModel)
        {
            MainBookingViewModel = mainBookingViewModel;
            _storer = storer;
            MainBookingViewModel.PropertyChanged += OnMainBookingViewModelPropertyChanged;
        }

        private void OnMainBookingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged();
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
				_storer.BookingRepository.Delete(MainBookingViewModel.CurrentBooking.BookingID);
				MainBookingViewModel.CurrentBooking = null;
            }
        }
    }
}
