using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Model;

namespace El_Booking.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel
    {
        UserRepository _userRepo;
        public MainWindowViewModel(string ConnectionString)
        {
            _userRepo = new UserRepository(ConnectionString);
        }
        
        private string _enteredEmail;
        public string EnteredEmail
        {
            get { return _enteredEmail; }
            set
            {
                _enteredEmail = value;
                OnPropertyChanged();
            }
        }

        private string _enteredPassword;

        public string EnteredPassword
        {
            get { return _enteredPassword; }
            set
            {
                _enteredPassword = value;
                OnPropertyChanged();
            }

        }


        public User? Login()
        {
            User user = null;
            if (_userRepo.Login(EnteredEmail, EnteredPassword))
            { 
                user = _userRepo.GetBy(EnteredEmail);
            }
            return user;    

        }



        //public RelayCommand LoginCommand => new RelayCommand(
        //execute => _userRepo.Login(EnteredEmail, EnteredPassword),
        //canExecute => CanLogin()
        //);

        //bool CanLogin()
        //{
        //    if (
        //        string.IsNullOrEmpty(EnteredEmail) ||
        //        string.IsNullOrEmpty(EnteredPassword)
        //        )
        //        return false;

        //    return true;
        //}

    }
}
