using El_Booking.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Navigation _navigation;

        // Binding fra Xaml i MainWindow.
        public BaseViewModel CurrentViewModel { get { return _navigation.CurrentViewModel; } }

        public MainViewModel(Navigation navigation) 
        {
            _navigation = navigation;

            _navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged();
        }
    }
}
