using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using El_Booking.ViewModel;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.IdentityModel.Protocols;

namespace El_Booking.View
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            var currentApp = Application.Current as App;
            string connectionString = currentApp.Configuration.GetSection("ConnectionStrings")["AppConnection"];

            CreateUserViewModel cuvm = new CreateUserViewModel(connectionString);
            DataContext = cuvm;

            InitializeComponent();
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_CreateUser(object sender, RoutedEventArgs e)
        {

            string? userCredentials = 
                ((CreateUserViewModel)this.DataContext).CheckIfUserExists(tbEmail.Text, tbPhone.Text);

            if (!string.IsNullOrEmpty(userCredentials))
                MessageBox.Show($"Bruger med: \"{userCredentials}\" er allerede oprettet.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);

            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }

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
