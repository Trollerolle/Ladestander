using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using User_ = El_Booking.Model.User;

namespace El_Booking.View
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {

        User_ testUser;
        UserViewModel uvm;

        public UserPage()
        {

            Car userCar = new Car(brand: "Lamborghini", model: "Aventador", licensePlate: "2Cool4You");
            testUser = new User_(
                userID: -1,
                email: "sander@hotmail.com",
                telephoneNumber: "12121212",
                firstName: "Sander Elgaard",
                lastName: "Andersen",
                car: null);

            var currentApp = (App)Application.Current;
            currentApp.CurrentUser = currentApp.CurrentUser ?? testUser;
            string connString = currentApp.Configuration.GetSection("ConnectionStrings")["WindowsLoginConnection"];
            
            uvm = new UserViewModel(connString);

            InitializeComponent();
            DataContext = uvm;


        }
    }
}
