using El_Booking.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace El_Booking.View
{

    public partial class CreateUserView : UserControl
    {
        public CreateUserView()
        {
            InitializeComponent();
        }

        private void pwdBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((CreateUserViewModel)this.DataContext).EnteredPassword = ((PasswordBox)sender).Password;
            }
        }

        private void pwdBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {

            PasswordBox pwdBox = (PasswordBox)sender;

            if (this.DataContext != null)
            {
                ((CreateUserViewModel)this.DataContext).EnteredPasswordAgain = pwdBox.Password;

                // if the two passwords doesn't match
                if (pwdBox.Password != ((CreateUserViewModel)this.DataContext).EnteredPassword)
                {
                    pwdBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    pwdBox.ClearValue(TextBox.BorderBrushProperty);
                }
            }
        }

        private void PhonenumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
