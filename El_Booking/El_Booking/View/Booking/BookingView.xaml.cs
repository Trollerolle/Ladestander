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

namespace El_Booking.View
{
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : Window
    {
        public BookingView()
        {
            InitializeComponent();
            Main.Content = new BookingWeek();
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
            //Main.Content = new Page2();
        }
        private void BtnClickLogOut(object sender, RoutedEventArgs e)
        {
            //Main.Content = new Page2();
        }
    }
}
