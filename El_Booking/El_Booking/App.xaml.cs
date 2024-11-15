using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;

namespace El_Booking
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Model.User CurrenctUser { get; private set; }

        public void SetCurrentUser(Model.User user)
        {
            CurrenctUser = user;
        }

        public void ClearCurrentUser(Model.User currentUser)
        {
            CurrenctUser = null;
        }
        public App()
        {

            var builder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

        }
    }

}
