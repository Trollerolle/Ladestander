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

		private readonly Storer _storer;
		public User_ _currentUser;
		public App _currentApp { get; init; }
        public ICommand UpdateCarCommand { get; }
        public ICommand UpdateUserCommand { get; }

		public UserViewModel(Storer storer)
        {

            _storer = storer;

            _currentApp = (App)Application.Current;
            string connectionString = _currentApp.Configuration.GetSection("ConnectionStrings")["BookingConnection"];
			_currentUser = _currentApp.CurrentUser ?? throw new Exception();
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

		// User Updating er nu sat som en Command klasse i Command mappen.
		//public RelayCommand UpdateUserCommand => new RelayCommand(
		//        execute => UpdateProfile(),
		//        canExecute => CanUpdateProfile()
		//        );

		//void UpdateProfile()
		//{

		//    User_ userToUpdate = _currentUser;

		//    try
		//    {
		//        if (NewEmail != null)
		//            userToUpdate.Email = NewEmail;

		//        if (NewPhoneNumber != null)
		//            userToUpdate.TelephoneNumber = NewPhoneNumber;

		//        if (NewPassword != null)
		//            userToUpdate.Password = NewPassword;
		//    }
		//    catch (NotSupportedException ex)
		//    {
		//        MessageBox.Show(ex.Message, "Fejl");
		//    }


		//    try
		//    {
		//        _storer.UserRepository.Update(userToUpdate);
		//        _currentApp.SetCurrentUser(userToUpdate);
		//        UserEmail = userToUpdate.Email;
		//        UserPhoneNumber = userToUpdate.TelephoneNumber;
		//        ClearProfileFields();
		//    }
		//    catch (Exception)
		//    {
		//        throw;
		//    }
		//}

		//void ClearProfileFields()
		//{
		//    NewEmail = null;
		//    NewPhoneNumber = null;
		//    NewPassword = null;
		//    NewPasswordAgain = null;
		//}

		//bool CanUpdateProfile()
		//{
		//    if (string.IsNullOrWhiteSpace(NewEmail) &&
		//        string.IsNullOrWhiteSpace(NewPhoneNumber) &&
		//        (string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(NewPasswordAgain)))
		//    {
		//        return false;
		//    }

		//    else
		//    {
		//        return true;
		//    }
		//}

		// Car Updating er nu sat som en Command klasse i Command mappen.
		//     public RelayCommand UpdateCarCommand => new RelayCommand(
		//             execute => UpdateCar(),
		//             canExecute => CanUpdateCar()
		//             );

		//     void UpdateCar()
		//     {

		//         Car newCar = new Car(brand: NewCarBrand, model: NewCarModel, licensePlate: NewLicensePlate);

		//         try
		//         {
		//             if (_currentUser.Car == null)
		//             {
		//                 _storer.CarRepository.Update(newCar);

		//                 // my Bil skal sættes i App
		//                 _currentUser.Car = newCar;
		//             }
		//             else
		//             {
		//                 _storer.CarRepository.Add(newCar);

		//		_currentUser.Car = newCar;
		//	}
		//}
		//         catch (Exception)
		//         {
		//	NewLicensePlate = null;
		//	NewCarBrand = null;
		//             NewCarModel = null;

		//             throw;
		//         }
		//     }

		//     bool CanUpdateCar()
		//     {
		//         if (string.IsNullOrWhiteSpace(NewCarModel) ||
		//             string.IsNullOrWhiteSpace(NewCarBrand) ||
		//             string.IsNullOrWhiteSpace(NewLicensePlate))
		//         {
		//             return false;
		//         }

		//         else
		//         {
		//             return true;
		//         }
		//     }
	}
}
