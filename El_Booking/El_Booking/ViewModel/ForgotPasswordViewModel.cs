using El_Booking.Commands;
using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace El_Booking.ViewModel
{
    public class ForgotPasswordViewModel : BaseViewModel
    {

        public ICommand NavigateLoginCommand { get; }
		public ICommand ForgotPasswordCommand { get; }

        public ForgotPasswordViewModel(Storer storer, Navigation navigation) 
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigation, () => new LoginViewModel(storer, navigation));
			ForgotPasswordCommand = new ForgotPasswordCommand(this, storer);
        }
		private string? _enteredEmail;
		public string? EnteredEmail
		{
			get { return _enteredEmail; }
			set
			{
				_enteredEmail = value;
				OnPropertyChanged();
			}
		}
	}
}
