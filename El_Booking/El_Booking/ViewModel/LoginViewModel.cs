using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using El_Booking.Commands;
using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;

namespace El_Booking.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(Storer storer, Navigation navigation)
        {
            NavigateCreateUserCommand = new NavigateCommand<CreateUserViewModel>(navigation, () => new CreateUserViewModel(storer, navigation));
            NavigateForgotPasswordCommand = new NavigateCommand<ForgotPasswordViewModel>(navigation, () => new ForgotPasswordViewModel(storer, navigation));
            LogInCommand = new LogInCommand(this, navigation, storer);

        }

        private string? _enteredEmail;
        public string? EnteredEmail
        {
            get { return _enteredEmail; }
            set
            {
                _enteredEmail = value;
                OnPropertyChanged();
            }
        }

        private string? _enteredPassword;
        public string? EnteredPassword
        {
            get { return _enteredPassword; }
            set
            {
                _enteredPassword = value;
                OnPropertyChanged();
            }

        }

        public ICommand NavigateCreateUserCommand { get; }
        public ICommand NavigateForgotPasswordCommand { get; }
        public ICommand LogInCommand { get; }

    }
}
