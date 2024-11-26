using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;
using El_Booking.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.System;
using User = El_Booking.Model.User;

namespace El_Booking.Commands
{
    public class LogOutCommand : CommandBase
    {

        private readonly Navigation _navigation;
        private readonly Storer _storer;

        public LogOutCommand(Navigation navigation, Storer storer)
        {
            _navigation = navigation;
            _storer = storer;
        }

        public override void Execute(object? parameter)
        {
            var currentApp = Application.Current as App;
            currentApp?.ClearCurrentUser();
			currentApp?.ClearConnection();

			_navigation.CurrentViewModel = new LoginViewModel(_storer, _navigation);
        }
    }
}
