using El_Booking.Model.Repositories;
using El_Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Commands;
using System.Windows.Input;
using System.Windows.Controls;
using El_Booking.View.Booking;
using El_Booking.Utility;

namespace El_Booking.ViewModel.BookingVM
{
    public class MainBookingViewModel : BaseViewModel
    {

        public ICommand LogOutCommand { get; }

        Page UserPage_ { get; }
        Page BookingWeekPage_ { get; }
        Page YourBookingPage_ { get; }

        public MainBookingViewModel(Storer storer, Navigation navigation, DateTime? startingDate = null)
        {
            DateTime today = startingDate ?? DateTime.Today; // til test, så datoen den starter på kan ændres. Ellers dd.

            LogOutCommand = new LogOutCommand(navigation, storer);

            UserPage_ = new UserPage()
            {
                DataContext = new UserViewModel(storer)
            };
            BookingWeekPage_ = new BookingWeekPage()
            {
                DataContext = new BookingViewModel(storer)
            };
            YourBookingPage_ = new YourBookingPage()
            {
                DataContext = new YourBookingViewModel(storer)
            };

            CurrentPage = UserPage_;
        }

        public RelayCommand SeeProfileCommand => new RelayCommand(
        execute => { CurrentPage = UserPage_; }
        );

        public RelayCommand SeeBookingWeekCommand => new RelayCommand(
        execute => { CurrentPage = BookingWeekPage_; }
        );

        public RelayCommand SeeYourBookingCommand => new RelayCommand(
        execute => { CurrentPage = YourBookingPage_; }
        );

        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
    }
}
