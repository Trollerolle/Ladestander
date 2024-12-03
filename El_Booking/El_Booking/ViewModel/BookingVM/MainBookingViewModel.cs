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
        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set {  
                _currentUser = value;
                OnPropertyChanged();
                }
        }

        private Car? _currentCar;

        public Car? CurrentCar
        {
            get { return _currentCar; }
            set {
				_currentCar = value;
                OnPropertyChanged();
                }
        }


        private Booking? _currentBooking;

        public Booking? CurrentBooking
        {
            get { return _currentBooking; }
            set { 
                _currentBooking = value;
                OnPropertyChanged();
                }
        }

        public ICommand LogOutCommand { get; }

        Page UserPage_ { get; }
        Page BookingWeekPage_ { get; }
        Page YourBookingPage_ { get; }

        public MainBookingViewModel(Storer storer, Navigation navigation, User user)
        {
            LogOutCommand = new LogOutCommand(navigation, storer, this);

            CurrentUser = user;

            CurrentCar = user.CarID is not null ? storer.CarRepository.GetBy(user.UserID.ToString()) : null;

			CurrentBooking = user.BookingID is not null ? storer.BookingRepository.GetBy(user.CarID.ToString()) : null;


			UserPage_ = new UserPage()
            {
                DataContext = new UserViewModel(storer, this)
            };
            BookingWeekPage_ = new BookingWeekPage()
            {
                DataContext = new BookingViewModel(storer, this)
            };
            YourBookingPage_ = new YourBookingPage()
            {
                DataContext = new YourBookingViewModel(this)
            };

            CurrentPage = BookingWeekPage_;
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
