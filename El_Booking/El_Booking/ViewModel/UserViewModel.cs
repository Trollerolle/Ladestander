using El_Booking.Model;
using El_Booking.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using User_ = El_Booking.Model.User;

namespace El_Booking.ViewModel
{

    public class UserViewModel : BaseViewModel
    {

		public UserViewModel(string connectionString)
		{
			_userRepository = new UserRepository(connectionString);
			_currentApp = (App)Application.Current;
			_loggedInUser = _currentApp.CurrentUser ?? throw new Exception();
        }

        readonly IRepository<User_> _userRepository;
        public App _currentApp { get; init; }
		User_ _loggedInUser;

		public string UserEmail
		{ 
			get { return _loggedInUser.Email; }
			private set
			{
				_loggedInUser.Email = value;
				OnPropertyChanged();
			}
		}
        public string UserPhoneNumber 
		{ 
			get { return _loggedInUser.TelephoneNumber; }
			private set 
			{ 
				_loggedInUser.TelephoneNumber = value; 
				OnPropertyChanged();
			}
		}
        public string? CarDetails => GetCarDetails(_loggedInUser.Car);
        public string? LicensePlate => GetLicensePlate(_loggedInUser.Car);

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

        public RelayCommand UpdateUserCommand => new RelayCommand(
                execute => UpdateProfile(),
                canExecute => CanUpdateProfile()
                );

        void UpdateProfile()
		{
            
            User_ userToUpdate = _loggedInUser;
			
			try
			{
                if (NewEmail != null)
                    userToUpdate.Email = NewEmail;

                if (NewPhoneNumber != null)
                    userToUpdate.TelephoneNumber = NewPhoneNumber;

				if (NewPassword != null)
					userToUpdate.Password = NewPassword;
            }
			catch (NotSupportedException ex)
			{
				MessageBox.Show(ex.Message, "Fejl");
			}
			

			try
			{
				_userRepository.Update(userToUpdate);
				_currentApp.SetCurrentUser(userToUpdate);
				UserEmail = userToUpdate.Email;
				UserPhoneNumber = userToUpdate.TelephoneNumber;
				ClearProfileFields();
			}
			catch (Exception)
            {
                throw;
            }
		}

		void ClearProfileFields()
		{
			NewEmail = null;
			NewPhoneNumber = null;
			NewPassword = null;
			NewPasswordAgain = null;
		}

		bool CanUpdateProfile()
		{
			if (string.IsNullOrWhiteSpace(NewEmail) &&
				string.IsNullOrWhiteSpace(NewPhoneNumber) &&
				(string.IsNullOrWhiteSpace(NewPassword) || string.IsNullOrWhiteSpace(NewPasswordAgain)))
			{ 
				return false; 
			}

			else 
			{ 
				return true; 
			}
		}

		// Car Updating
        public RelayCommand UpdateCarCommand => new RelayCommand(
                execute => UpdateCar(),
                canExecute => CanUpdateCar()
                );

        void UpdateCar()
        {

            User_ userToUpdate = _loggedInUser;
			Car newCar = new Car(brand: NewCarBrand, model: NewCarModel, licensePlate: NewLicensePlate);

            userToUpdate.Car = newCar;

            try
            {
                _userRepository.Update(userToUpdate);

                // my bruger skal sættes i App
            }
            catch (Exception)
            {
                throw;
            }
        }

        bool CanUpdateCar()
        {
            if (string.IsNullOrWhiteSpace(NewCarModel) ||
                string.IsNullOrWhiteSpace(NewCarBrand) ||
                string.IsNullOrWhiteSpace(NewLicensePlate))
            {
                return false;
            }

            else
            {
                return true;
            }
        }
    }
}
