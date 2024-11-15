using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using El_Booking.ViewModel;
using Windows.System;
using El_Booking.Model;

namespace El_Booking.View
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class LoginView : Window
	{
		public LoginView()
		{
            var currentApp = Application.Current as App;
            string connectionString = currentApp.Configuration.GetSection("ConnectionStrings")["AppConnection"];

            LoginViewModel lvm = new LoginViewModel(connectionString);
            DataContext = lvm;

            InitializeComponent();
		}

		private void Button_Click_CreateUser(object sender, RoutedEventArgs e)
		{
			CreateUserView createUser = new CreateUserView();
			createUser.Show();
			this.Close();
		}

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Model.User user = null;
            
            if (this.DataContext != null)
            {
                user = ((LoginViewModel)this.DataContext).Login();
            }

            if (user is null)
			{
                MessageBox.Show($"Der er fejl i brugernavn eller password", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
			else
			{
                BookingView bookingView = new BookingView(user);
                bookingView.Show();
                this.Close();
            }
			
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