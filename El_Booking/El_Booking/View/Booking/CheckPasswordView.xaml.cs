using El_Booking.ViewModel;
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

namespace El_Booking.View.Booking
{
    /// <summary>
    /// Interaction logic for CheckPasswordView.xaml
    /// </summary>
    public partial class CheckPasswordView : Window
    {

        public bool Success { get; private set; }
        public string CurrentPassword { get; private set; }

        public CheckPasswordView()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            CurrentPassword = this.pwdCurrentPassword.Password;
            Success = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Success = false;
            Close();
        }

        private void pwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(pwdCurrentPassword.Password))
                btnConfirm.IsEnabled = true;
        }
    }
}
