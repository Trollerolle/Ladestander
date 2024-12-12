using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        public ICommand DeleteBookingCommand { get; }

        public YourBookingViewModel(Storer storer, MainBookingViewModel mainBookingViewModel)
        {
            MainBookingViewModel = mainBookingViewModel;
            _storer = storer;
            DeleteBookingCommand = new DeleteBookingCommand(mainBookingViewModel, storer);
            MainBookingViewModel.PropertyChanged += OnMainBookingViewModelPropertyChanged;
        }

        private void OnMainBookingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged();
        }

    }
}
