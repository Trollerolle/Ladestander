using El_Booking.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Commands
{
    public class NavigatePageCommand<Page> : CommandBase
    {
        //private readonly Navigation _navigation;
        //private Func<Page> _page;

        //public NavigatePageCommand(Navigation navigation, Func<> createViewModel)
        //{
        //    _navigation = navigation;
        //    _createViewModel = createViewModel;
        //}

        //public override void Execute(object? parameter)
        //{
        //    _navigation.CurrentViewModel = _createViewModel();   
        //}
        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
