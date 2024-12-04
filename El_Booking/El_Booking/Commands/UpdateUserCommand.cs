using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using El_Booking.Utility;
using El_Booking.ViewModel;
using El_Booking.ViewModel.BookingVM;
using Windows.ApplicationModel.Store;
using El_Booking.Model;
using System.ComponentModel;
using El_Booking.View.Booking;

namespace El_Booking.Commands
{
	public class UpdateUserCommand : CommandBase
	{
		private UserViewModel _userViewModel { get; }
		private Storer _storer { get; }

		public UpdateUserCommand(UserViewModel userViewModel, Storer storer)
		{
			_userViewModel = userViewModel;
			_storer = storer;

			_userViewModel.PropertyChanged += OnViewModelPropertyChanged;

		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCanExecuteChanged();
		}

		public override void Execute(object? parameter)
		{

			User userToUpdate = _userViewModel._currentUser;

			try
			{
				if (_userViewModel.NewEmail != null)
					userToUpdate.Email = _userViewModel.NewEmail;

				if (_userViewModel.NewPhoneNumber != null)
					userToUpdate.TelephoneNumber = _userViewModel.NewPhoneNumber;

				if (_userViewModel.NewPassword != null)
				{
					if (CheckCurrentPassword())
						userToUpdate.Password = _userViewModel.NewPassword;
					else
						throw new NotSupportedException("Your old password didn't match");
                }
					
			}
			catch (NotSupportedException ex)
			{
				MessageBox.Show(ex.Message, "Fejl");
			}


			try
			{
				_storer.UserRepository.Update(userToUpdate);
				_userViewModel.MainBookingViewModel.CurrentUser = userToUpdate;
				_userViewModel.UserEmail = userToUpdate.Email;
				_userViewModel.UserPhoneNumber = userToUpdate.TelephoneNumber;
				ClearProfileFields();
			}
			catch (Exception)
			{
				throw;
			}
		}

		private bool CheckCurrentPassword()
		{
			CheckPasswordView checkPwdView = new CheckPasswordView();
			checkPwdView.ShowDialog();

            if (checkPwdView.Success)
			{
				return _storer.UserRepository.Login(_userViewModel._currentUser.Email, checkPwdView.CurrentPassword);
            }
			
			return false;
			
		}

        public override bool CanExecute(object? parameter)
		{
			if (string.IsNullOrWhiteSpace(_userViewModel.NewEmail) &&
				string.IsNullOrWhiteSpace(_userViewModel.NewPhoneNumber) &&
				(string.IsNullOrWhiteSpace(_userViewModel.NewPassword) || string.IsNullOrWhiteSpace(_userViewModel.NewPasswordAgain)))
			{
				return false;
			}

			else
			{
				return true;
			}
		}


		void ClearProfileFields()
		{
			_userViewModel.NewEmail = null;
			_userViewModel.NewPhoneNumber = null;
			_userViewModel.NewPassword = null;
			_userViewModel.NewPasswordAgain = null;
		}

	}
}
