using El_Booking.Model;
using El_Booking.Model.Repositories;
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
using Windows.System;
using User = El_Booking.Model.User;

namespace El_Booking.Commands
{
    public class LogInCommand : CommandBase
    {

        private readonly Navigation _navigation;
        private readonly Storer _storer;
        private LoginViewModel _loginViewModel;

        public LogInCommand(LoginViewModel loginViewModel, Navigation navigation, Storer storer) 
        {
            _navigation = navigation;
            _loginViewModel = loginViewModel;
            _storer = storer;

            _loginViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            string email = _loginViewModel.EnteredEmail;
            string password = _loginViewModel.EnteredPassword;

            if (_storer.UserRepository.Login(email, password))
            {
                LogUserIn(email);
				var currentApp = Application.Current as App;
                currentApp?.SetCurrentConnection("BookingConnection");
				_navigation.CurrentViewModel = new MainBookingViewModel(_storer, _navigation);
            }

            else
                MessageBox.Show("Entered Email or Password are not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(_loginViewModel.EnteredEmail) &&
                !string.IsNullOrWhiteSpace(_loginViewModel.EnteredPassword) &&
                base.CanExecute(parameter);

            // https://www.youtube.com/watch?v=DNez3wIpHeE&list=PLA8ZIAm2I03hS41Fy4vFpRw8AdYNBXmNm&index=4
        }

        void LogUserIn(string email)
        {
            try
            {
                User loggedInUser = _storer.UserRepository.GetBy(email);
                var currentApp = Application.Current as App;
                currentApp?.SetCurrentUser(loggedInUser);
            }
            catch { throw; }
        }
    }
}
