using El_Booking.Utility;
using El_Booking.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : BaseViewModel
    {
        private readonly Navigation _navigation;
        private Func<TViewModel> _createViewModel;

        public NavigateCommand(Navigation navigation, Func<TViewModel> createViewModel)
        {
            _navigation = navigation;
            _createViewModel = createViewModel;
        }

        public override void Execute(object? parameter)
        {
            _navigation.CurrentViewModel = _createViewModel();   
        }
    }
}
