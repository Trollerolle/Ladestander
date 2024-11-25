using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using El_Booking.Model;
using El_Booking.ViewModel;

namespace El_Booking.View
{
    /// <summary>
    /// Interaction logic for YourBooking.xaml
    /// </summary>
    public partial class YourBooking : Page
    {

		private Model.User _currentUser;

		public Model.User CurrentUser
		{
			get { return _currentUser; }
			set { _currentUser = value; }
		}

		public YourBooking()
        {
			InitializeComponent();

			var currentApp = Application.Current as App;
			CurrentUser = currentApp?.CurrentUser;

			string connectionString = (currentApp.Configuration.GetSection("ConnectionStrings")["BookingConnection"]);

			YourBookingViewModel ybvm = new YourBookingViewModel(connectionString);
			DataContext = ybvm;

			ybvm.GetBooking(CurrentUser);

		
		}

    }
}
