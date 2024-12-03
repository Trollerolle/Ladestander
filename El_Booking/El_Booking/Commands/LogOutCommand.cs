using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;
using El_Booking.ViewModel;
using El_Booking.ViewModel.BookingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.System;
using User = El_Booking.Model.User;

namespace El_Booking.Commands
{
    public class LogOutCommand : CommandBase
    {

        private readonly Navigation _navigation;
        private readonly Storer _storer;
        public MainBookingViewModel MainBookingViewModel { get; set; }
        public LogOutCommand(Navigation navigation, Storer storer, MainBookingViewModel mainBookingViewModel)
        {
            _navigation = navigation;
            _storer = storer;
            MainBookingViewModel = mainBookingViewModel;
        }

        public override void Execute(object? parameter)
        {
            MainBookingViewModel = null;
			_navigation.CurrentViewModel = new LoginViewModel(_storer, _navigation);
        }
    }
}
