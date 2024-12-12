using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Model;
using El_Booking.Utility;
using El_Booking.ViewModel.BookingVM;
using System.Windows;

namespace El_Booking.Commands
{
    public class DeleteBookingCommand : CommandBase
    {
        private MainBookingViewModel _mainBookingViewModel { get; }
        public Booking? CurrentBooking => _mainBookingViewModel.CurrentBooking;
        private Storer _storer { get; }

        public DeleteBookingCommand(MainBookingViewModel mainBookingViewModel, Storer storer) 
        { 
            _mainBookingViewModel = mainBookingViewModel;
            _storer = storer;

            _mainBookingViewModel.PropertyChanged += OnMainBookingViewModelPropertyChanged;
        }

        private void OnMainBookingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }


        public override void Execute(object? parameter)
        {
            var result = MessageBox.Show(
                "Er du sikker på, at du vil slette din booking?",
                "Bekræft sletning",
                MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _storer.BookingRepository.Delete(_mainBookingViewModel.CurrentBooking.BookingID);
                _mainBookingViewModel.CurrentBooking = null;
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return (CurrentBooking is not null) ? true : false;
        }

    }
}
