using El_Booking.Commands;
using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using User_ = El_Booking.Model.User;

namespace El_Booking.ViewModel.BookingVM
{

    public class UserViewModel : BaseViewModel
    {

		public User_ _currentUser => _currentApp.CurrentUser;
		public App _currentApp { get; init; }
        public ICommand UpdateCarCommand { get; }
        public ICommand UpdateUserCommand { get; }

		public UserViewModel(Storer storer)
        {
			UpdateCarCommand = new UpdateCarCommand(this, storer);
			UpdateUserCommand = new UpdateUserCommand(this, storer);

            _currentApp = (App)Application.Current;
        }

        public string UserEmail
        {
            get { return _currentUser.Email; }
            internal set
            {
				_currentUser.Email = value;
                OnPropertyChanged();
            }
        }
        public string UserPhoneNumber
        {
            get { return _currentUser.TelephoneNumber; }
            internal set
            {
				_currentUser.TelephoneNumber = value;
                OnPropertyChanged();
            }
        }
        public string? CarDetails => GetCarDetails(_currentUser.Car);
        public string? LicensePlate => GetLicensePlate(_currentUser.Car);

        static string? GetLicensePlate(Car? car)
        {
            if (car == null) return null;

            return car.LicensePlate;
        }

        static string? GetCarDetails(Car? car)
        {
            if (car == null)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.Append(car.Brand);
            sb.Append(", ");
            sb.Append(car.Model);
            return sb.ToString();
        }

        private string? _newEmail;
        public string? NewEmail
        {
            get { return _newEmail; }
            set
            {
                _newEmail = value;
                OnPropertyChanged();
            }
        }

        private string? _newPhoneNumber;
        public string? NewPhoneNumber
        {
            get { return _newPhoneNumber; }
            set
            {
                _newPhoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string? _newPassword;
        public string? NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string? _newPasswordAgain;
        public string? NewPasswordAgain
        {
            get { return _newPasswordAgain; }
            set
            {
                _newPasswordAgain = value;
                OnPropertyChanged();
            }
        }

        private string? _newCarModel;
        public string? NewCarModel
        {
            get { return _newCarModel; }
            set
            {
                _newCarModel = value;
                OnPropertyChanged();
            }
        }

        private string? _newCarBrand;
        public string? NewCarBrand
        {
            get { return _newCarBrand; }
            set
            {
                _newCarBrand = value;
                OnPropertyChanged();
            }
        }

        private string? _newLicensePlate;
        public string? NewLicensePlate
        {
            get { return _newLicensePlate; }
            set
            {
                _newLicensePlate = value;
                OnPropertyChanged();
            }
        }

	}
}
