using El_Booking.Model;
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
using Windows.Devices.SmartCards;

namespace El_Booking.Commands
{
    public class MakeBookingCommand : CommandBase
    {
        private BookingViewModel _bookingViewModel { get; }
        private Storer _storer { get; }


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
                _bookingViewModel.MainBookingViewModel.CurrentBooking = _storer.BookingRepository.GetBy(newBooking.CarID.ToString());

				MessageBox.Show($"Din booking er gennemført.", "Succes", MessageBoxButton.OK);
                _bookingViewModel.MainBookingViewModel.SeeYourBookingCommand.Execute(parameter);

            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show("Din valgte booking kunne ikke foretages, prøv igen.", "Fejl", MessageBoxButton.OK);
                _bookingViewModel.GetCurrentTimeSlots(_bookingViewModel.MondayOfWeek);
                _bookingViewModel.SetTimeSlotsAsPassed();
            }
			catch (NullReferenceException ex)
			{
				MessageBox.Show("Din bruger har ikke opdateret sin bil.", "Fejl", MessageBoxButton.OK);
				_bookingViewModel.MainBookingViewModel.SeeProfileCommand.Execute(parameter);
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
