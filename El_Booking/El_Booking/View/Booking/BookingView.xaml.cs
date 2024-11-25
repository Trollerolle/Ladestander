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
using System.Windows.Shapes;
using El_Booking.Model.Repositories;
using El_Booking.ViewModel;
using Windows.ApplicationModel.Store;

namespace El_Booking.View
{
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : Window
    {
        private Model.User _currentUser;

        public Model.User CurrentUser
		{
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        public BookingView()
        {

            var currentApp = Application.Current as App;
			CurrentUser = currentApp?.CurrentUser;

			string connectionString = (currentApp.Configuration.GetSection("ConnectionStrings")["BookingConnection"]);

            // til test ------------------------------------------------- \\
            //if (CurrentUser == null)
            //{
            //    UserRepository ur = new UserRepository(connectionString);
            //    currentApp.CurrentUser = ur.GetBy("1");
            //}
            //// ---------------------------------------------------------- \\

            //BookingViewModel bvm = new BookingViewModel(connectionString);
            //DataContext = bvm;


            InitializeComponent();
            Main.Content = new YourBooking();

        }
        private void BtnClickBookingWeek(object sender, RoutedEventArgs e)
        {
            Main.Content = new BookingWeek();
        }
        private void BtnClickYourBooking(object sender, RoutedEventArgs e)
        {
            Main.Content = new YourBooking();
        }
        private void BtnClickProfile(object sender, RoutedEventArgs e)
        {
            Main.Content = new UserPage();
        }
        private void BtnClickLogOut(object sender, RoutedEventArgs e)
        {
            (Application.Current as App)?.ClearCurrentUser();

            LoginView loginView = new LoginView();
            loginView.Show();

            this.Close();
        }
    }
}
