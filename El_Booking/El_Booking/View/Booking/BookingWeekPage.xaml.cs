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
using System.Windows.Navigation;
using System.Windows.Shapes;
using El_Booking.Model.Repositories;
using El_Booking.ViewModel;

namespace El_Booking.View
{
    /// <summary>
    /// Interaction logic for BookingWeek.xaml
    /// </summary>
    public partial class BookingWeek : Page
    {
        private Model.User _currentUser;

        public Model.User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }
        public BookingWeek()
        {
            var currentApp = Application.Current as App;
            CurrentUser = currentApp?.CurrentUser;

            string connectionString = (currentApp.Configuration.GetSection("ConnectionStrings")["WindowsLoginConnection"]);

            if (CurrentUser == null)
            {
                UserRepository ur = new UserRepository(connectionString);
                currentApp.CurrentUser = ur.GetBy("Ren3ersej@gmail.com");
            }

            InitializeComponent();

            BookingViewModel bvm = new BookingViewModel(connectionString);
            DataContext = bvm;


        }
    }
}
