﻿using El_Booking.Model;
using El_Booking.Utility;
using El_Booking.ViewModel;
using El_Booking.ViewModel.BookingVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace El_Booking.Commands
{
    public class MakeBookingCommand : CommandBase
    {
        private BookingViewModel _bookingViewModel { get; }
        private YourBookingViewModel _yourBookingViewModel { get; set; }
        private Storer _storer { get; }

        //private readonly MainBookingViewModel _mainBookingViewModel;

        public MakeBookingCommand(BookingViewModel bookingViewModel, Storer storer)
        {
            _bookingViewModel = bookingViewModel;
            _storer = storer;

            _bookingViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {

            IEnumerable<TimeSlot> timeSlots = _storer.TimeSlotRepository.GetAll();
            DateOnly date = _bookingViewModel.MondayOfWeek.AddDays((int)_bookingViewModel.SelectedDay);

            try
            {
                Booking newBooking = new Booking()
                {
                    TimeSlot = timeSlots.ElementAt((int)_bookingViewModel.SelectedTimeSlot),
                    Date = date,
                    CarID = _bookingViewModel.MainBookingViewModel.CurrentCar.CarID
                };

                _storer.BookingRepository.Add(newBooking);
                newBooking = _storer.BookingRepository.GetBy(newBooking.CarID.ToString());
                _bookingViewModel.MainBookingViewModel.CurrentBooking = newBooking;
                _bookingViewModel.ChangeWeek(0);

				// Fjernet " Gå til \"Din Booking\" for at se detaljer" fra beskeden og sat den til at skifte til bookingen efter OK.
				MessageBox.Show($"Din booking er gennemført.", "Succes", MessageBoxButton.OK); 
				_bookingViewModel.MainBookingViewModel.SeeYourBookingCommand.Execute(parameter);

            }
            catch (Exception ex)
            {
                MessageBox.Show(" fejl besked som skal vises ", "Fejl", MessageBoxButton.OK);
            }
        }

        public override bool CanExecute(object? parameter)
        {
            if (
                _bookingViewModel.SelectedDay is not null &&
                _bookingViewModel.SelectedTimeSlot is not null &&
                base.CanExecute(parameter)
                )
                return true;

            return false;
        }
    }
}
