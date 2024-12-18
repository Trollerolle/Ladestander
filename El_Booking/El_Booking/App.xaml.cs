﻿using El_Booking.Model;
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

    public partial class App : Application
    {
        public string ConnectionString => Configuration.GetSection("ConnectionStrings")[Connection];
        public IConfigurationRoot Configuration { get; private set; }

        private string _connection = "AppConnection";

        private string Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        internal void SetCurrentConnection(string connection)
        {
            Connection = connection;
        }

        internal void ClearConnection()
        {
            Connection = "AppConnection";
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
