using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.Utility;
using El_Booking.View;
using El_Booking.ViewModel;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Serialization.DataContracts;
using System.Windows;
using Windows.ApplicationModel.Appointments.DataProvider;
using Windows.ApplicationModel.Store;

namespace El_Booking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string ConnectionString => Configuration.GetSection("ConnectionStrings")[Connection];
        public IConfigurationRoot Configuration { get; private set; }

        private User _currentUser;

        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        private string _connection = "AppConnection";

        public string Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        public void SetCurrentConnection(string connection)
        {
            _connection = connection;
        }

        public void ClearConnection()
        {
            _connection = "AppConnection";
        }

        public void SetCurrentUser(Model.User user)
        {
            CurrentUser = user;
        }

        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }
        public void SetCurrentUsersCar(Model.Car car)
        {
            CurrentUser.Car = car;
        }
        public App()
        {

            var builder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            Storer storer = new Storer();

            Navigation navigation = new Navigation();
            navigation.CurrentViewModel = new LoginViewModel(storer, navigation);

            MainWindow = new MainWindow() 
            { 
                DataContext = new MainViewModel(navigation)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
