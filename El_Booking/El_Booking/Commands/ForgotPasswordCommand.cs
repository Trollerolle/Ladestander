using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using El_Booking.Utility;
using El_Booking.ViewModel;

namespace El_Booking.Commands
{
	public class ForgotPasswordCommand : CommandBase
	{
		private ForgotPasswordViewModel _forgotPasswordViewModel { get; }
		private Storer _storer { get; }

		public ForgotPasswordCommand(ForgotPasswordViewModel forgotPasswordViewModel, Storer storer)
		{
			_forgotPasswordViewModel = forgotPasswordViewModel;
			_storer = storer;

			_forgotPasswordViewModel.PropertyChanged += OnViewModelPropertyChanged;
		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCanExecuteChanged();
		}
		public override void Execute(object? parameter)
		{

				_storer.UserRepository.Forgot(_forgotPasswordViewModel.EnteredEmail);

				MessageBox.Show($"Hvis din mail eksisterer i vores system, vil du modtage en mail med et nyt midlertidigt kodeord. I tilfælde af at du allerede har bedt om et inden for 15 min, vil du ikke modtage en mail.", "Succes", MessageBoxButton.OK);

			_forgotPasswordViewModel.NavigateLoginCommand.Execute(parameter);

		}
		public override bool CanExecute(object? parameter)
		{
			if (
				!string.IsNullOrEmpty(_forgotPasswordViewModel.EnteredEmail) &&
				base.CanExecute(parameter)
				)
				return true;

			return false;
		}
	}
}
