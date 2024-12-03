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

        public bool Success { get; }
        public CheckPasswordView()
        {
            InitializeComponent();
        }

        private void pwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((LoginViewModel)this.DataContext).EnteredPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
