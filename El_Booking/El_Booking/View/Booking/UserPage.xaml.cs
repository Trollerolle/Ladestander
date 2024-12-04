using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.ViewModel;
using El_Booking.ViewModel.BookingVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using User_ = El_Booking.Model.User;

namespace El_Booking.View.Booking
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {

        public UserPage()
        {
            InitializeComponent();
        }

        private void pwdBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((UserViewModel)this.DataContext).NewPassword = ((PasswordBox)sender).Password;
            }
        }

        private void pwdBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((UserViewModel)this.DataContext).NewPasswordAgain = ((PasswordBox)sender).Password;
            }
        }
    }
}
