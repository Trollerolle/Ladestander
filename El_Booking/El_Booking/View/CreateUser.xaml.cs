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
			MainWindow mainWindow= new MainWindow();
			mainWindow.Show();
			this.Close();
		}

		private void Button_Click_CreateUser(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
		}
	}
}
