using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using El_Booking.Model;

namespace El_Booking.ViewModel
{
    internal class CreateUserViewModel : BaseViewModel
    {
         UserRepository _userRepo;
        public CreateUserViewModel(string ConnectionString) 
        { 
        _userRepo = new UserRepository(ConnectionString);
        }
        private string _enteredEmail;

        public string EnteredEmail
        {
            get { return _enteredEmail; }
            set { 
                _enteredEmail = value;
                OnPropertyChanged();
                }
        }

        private string _enteredPhoneNumber;

        public string EnteredPhoneNumber
        {
            get { return _enteredPhoneNumber; }
            set { 
                _enteredPhoneNumber = value;
				OnPropertyChanged();
			    }

        }

        private string _enteredFirstName;

        public string EnteredFirstName
        {
            get { return _enteredFirstName; }
            set { 
                _enteredFirstName = value;
				OnPropertyChanged();
			    }
        }

        private string _enteredLastName;

        public string EnteredLastName
        {
            get { return _enteredLastName; }
            set { 
                _enteredLastName = value;
				OnPropertyChanged();
			    }

        }

        private string _enteredPassword;

        public string EnteredPassword
        {
            get { return _enteredPassword; }
            set { 
                _enteredPassword = value;
				OnPropertyChanged();
			    }

        }

        private string _enteredPasswordAgain;

		public string EnteredPasswordAgain
		{
			get { return _enteredPasswordAgain; }
			set { 
                _enteredPasswordAgain = value;
				OnPropertyChanged();
			    }

		}

       

        public RelayCommand CreateUserCommand => new RelayCommand(
                execute => CreateNewUser(),
                canExecute => CanCreate()
				);

        bool CanCreate()
        {
            if (
                string.IsNullOrEmpty(EnteredEmail) ||
                string.IsNullOrEmpty(EnteredPhoneNumber) ||
                string.IsNullOrEmpty(EnteredFirstName) ||
                string.IsNullOrEmpty(EnteredLastName) ||
                string.IsNullOrEmpty(EnteredPassword) ||
                EnteredPassword != EnteredPasswordAgain
                )
                return false;

            return true;
        }

        public void CreateNewUser()
        {
            string? userCredentials =
                CheckIfUserExists(EnteredEmail, EnteredPhoneNumber);

            if (userCredentials == null)
            {
                User newUser = new User(EnteredEmail, EnteredPhoneNumber, EnteredFirstName, EnteredLastName, EnteredPassword);
                _userRepo.Add(newUser);
            }
                
        }
        
        // return usercredentials, or null if no user was found
        public string? CheckIfUserExists(params string[] args)
        {
            foreach (string arg in args)
            {
                if (_userRepo.GetBy(arg) != null)
                {
                    return arg;
                }
            }

            return null;

        }
	}
}
