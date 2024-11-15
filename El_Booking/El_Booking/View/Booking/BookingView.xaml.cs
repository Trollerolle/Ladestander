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
using El_Booking.ViewModel;

namespace El_Booking.View
{
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : Window
    {
        private Model.User user;

        public Model.User User
        {
            get { return user; }
            set { user = value; }
        }



        public BookingView(Model.User loggedInUser)
        {
            

            var currentApp = Application.Current as App;
            string connectionString = (currentApp.Configuration.GetSection("ConnectionStrings")["BookingConnection"]);

            BookingViewModel bvm = new BookingViewModel(connectionString);
            DataContext = bvm;

            User = loggedInUser;
            InitializeComponent();
            Main.Content = new BookingWeek(User);
        }
        private void BtnClickBookingWeek(object sender, RoutedEventArgs e)
        {
            Main.Content = new BookingWeek(User);
        }
        private void BtnClickYourBooking(object sender, RoutedEventArgs e)
        {
            Main.Content = new YourBooking(User);
        }
        private void BtnClickProfile(object sender, RoutedEventArgs e)
        {
            //Main.Content = new Page2();
        }
        private void BtnClickLogOut(object sender, RoutedEventArgs e)
        {
            //Main.Content = new Page2();
        }
    }
}
