using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using El_Booking.Commands;
using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;

namespace El_Booking.ViewModel
{
    public class CreateUserViewModel : BaseViewModel
    {

        public ICommand NavigateLoginCommand { get; }
        public ICommand CreateUserCommand { get; }

        public CreateUserViewModel(Storer storer, Navigation navigation) 
        { 
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigation, () => new LoginViewModel(storer, navigation));
            CreateUserCommand = new CreateUserCommand(this, storer);
        }

        private string? _enteredEmail;
        public string? EnteredEmail
        {
            get { return _enteredEmail; }
            set { 
                _enteredEmail = value;
                OnPropertyChanged();
                }
        }

        private string? _enteredPhoneNumber;
        public string? EnteredPhoneNumber
        {
            get { return _enteredPhoneNumber; }
            set { 
                _enteredPhoneNumber = value;
				OnPropertyChanged();
			    }

        }

        private string? _enteredFirstName;
        public string? EnteredFirstName
        {
            get { return _enteredFirstName; }
            set { 
                _enteredFirstName = value;
				OnPropertyChanged();
			    }
        }

        private string? _enteredLastName;
        public string? EnteredLastName
        {
            get { return _enteredLastName; }
            set { 
                _enteredLastName = value;
				OnPropertyChanged();
			    }

        }

        private string? _enteredPassword;
        public string? EnteredPassword
        {
            get { return _enteredPassword; }
            set { 
                _enteredPassword = value;
				OnPropertyChanged();
			    }

        }

        private string? _enteredPasswordAgain;
		public string? EnteredPasswordAgain
		{
			get { return _enteredPasswordAgain; }
			set { 
                _enteredPasswordAgain = value;
				OnPropertyChanged();
			    }

		}
	}
}
