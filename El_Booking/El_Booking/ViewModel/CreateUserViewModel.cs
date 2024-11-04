using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Model;

namespace El_Booking.ViewModel
{
    internal class CreateUserViewModel
    {
        private string _enteredEmail;

        public string EnteredEmail
        {
            get { return _enteredEmail; }
            set { _enteredEmail = value; }
        }

        private string _enteredPhoneNumber;

        public string EnteredPhoneNumber
        {
            get { return _enteredPhoneNumber; }
            set { _enteredPhoneNumber = value; }

        }

        private string _enteredFirstName;

        public string EnteredFirstName
        {
            get { return _enteredFirstName; }
            set { _enteredFirstName = value; }
        }

        private string _enteredLastName;

        public string EnteredLastName
        {
            get { return _enteredLastName; }
            set { _enteredLastName = value; }

        }

        private string _enteredPassword;

        public string EnteredPassword
        {
            get { return _enteredPassword; }
            set { _enteredPassword = value; }

        }

        private string _enteredPasswordAgain;

		public string EnteredPasswordAgain
		{
			get { return _enteredPasswordAgain; }
			set { _enteredPasswordAgain = value; }

		}

        UserRepository _userRepo;

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
                string.Equals(EnteredPassword, EnteredPasswordAgain)
                )
                return false;

            return true;
        }

        public void CreateNewUser()
        {
            if (CheckIfUserExists(EnteredEmail))
                throw new Exception("Email already exists");

            User newUser = new User(EnteredEmail, EnteredPhoneNumber, EnteredFirstName, EnteredLastName, EnteredPassword);

            _userRepo.Add(newUser);
        }
        
        public bool CheckIfUserExists(string email)
        {
            if (_userRepo.GetBy(EnteredEmail) != null)
            {
                return true;
            }
            else 
                return false;
        }
	}
}
