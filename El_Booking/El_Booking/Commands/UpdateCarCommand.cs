using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Model;
using El_Booking.Utility;
using El_Booking.ViewModel;
using El_Booking.ViewModel.BookingVM;


namespace El_Booking.Commands
{
	public class UpdateCarCommand : CommandBase
	{
		private UserViewModel _userViewModel { get; }
		private Storer _storer { get; }

		public UpdateCarCommand(UserViewModel userViewModel, Storer storer)
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

			Car carToUpdate = _userViewModel.MainBookingViewModel.CurrentCar is not null ? _userViewModel.MainBookingViewModel.CurrentCar : new Car() { UserID = (int)_userViewModel.MainBookingViewModel.CurrentUser.UserID };

			try
			{

				carToUpdate.Brand = _userViewModel.NewCarBrand;
				carToUpdate.Model = _userViewModel.NewCarModel;
				carToUpdate.LicensePlate = _userViewModel.NewLicensePlate;
				if (_userViewModel.MainBookingViewModel.CurrentCar != null)
				{
					_storer.CarRepository.Update(carToUpdate);
				}
				else
				{
					_storer.CarRepository.Add(carToUpdate);
					carToUpdate = _storer.CarRepository.GetBy(_userViewModel.MainBookingViewModel.CurrentUser.UserID.ToString());
				}
				_userViewModel.MainBookingViewModel.CurrentCar = carToUpdate;

			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				// Uanset succes eller fejl, ryd felterne
				ClearNewCarFields();
			}
		}
		public override bool CanExecute(object? parameter)
		{
			if (string.IsNullOrWhiteSpace(_userViewModel.NewCarModel) ||
				string.IsNullOrWhiteSpace(_userViewModel.NewCarBrand) ||
				string.IsNullOrWhiteSpace(_userViewModel.NewLicensePlate))
			{
				return false;
			}

			else
			{
				return true;
			}
		}

		void ClearNewCarFields()
		{
			_userViewModel.NewLicensePlate = null;
			_userViewModel.NewCarBrand = null;
			_userViewModel.NewCarModel = null;
		}
	}
}
