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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
            var currentApp = Application.Current as App;
            string connectionString = currentApp.Configuration.GetSection("ConnectionStrings")["AppConnection"];

            MainWindowViewModel mwvm = new MainWindowViewModel(connectionString);
            DataContext = mwvm;

            InitializeComponent();
		}

		private void Button_Click_CreateUser(object sender, RoutedEventArgs e)
		{
			CreateUser createUser = new CreateUser();
			createUser.Show();
			this.Close();
		}

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            object user = null;
            
            if (this.DataContext != null)
            {
                user = ((MainWindowViewModel)this.DataContext).Login();
            }

            if (user is null)
			{
                MessageBox.Show($"Der er fejl i brugernavn eller password", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
			else
			{
                BookingView bookingView = new BookingView();
                bookingView.Show();
                this.Close();
            }
			
        }

        private void pwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((MainWindowViewModel)this.DataContext).EnteredPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}