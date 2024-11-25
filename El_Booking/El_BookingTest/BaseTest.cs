using _User = El_Booking.Model.User;
using El_Booking.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text;
using Windows.System;

namespace El_BookingTest
{

    [TestClass]
    public class BaseTest
    {
        public string WPFApp_ConnString = InitConfiguration().GetSection("ConnectionStrings")["AppConnection"];
        public string dbo_ConnString = InitConfiguration().GetSection("ConnectionStrings")["WindowsLoginConnection"];
        string local_ConnString = InitConfiguration().GetSection("ConnectionStrings")["LocalConnection"];

        public _User u1, u2, u3, u4, u5;
        public Booking b1, b2, b3, b4, b5, b6;

        [TestCleanup]
        public void Cleanup()
        {
            string dbo = InitConfiguration().GetSection("ConnectionStrings")["WindowsLoginConnection"];
            string query = $"DELETE FROM [Users] WHERE [firstName] = 'test';";

            using (SqlConnection connection = new SqlConnection(dbo))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertUsers()
        {
            u1 = new _User(email: "test1mail@gmail.com", telephoneNumber: "12121212", firstName: "test", lastName: "1", password: "pwTest1");
            u2 = new _User(email: "test2mail@gmail.com", telephoneNumber: "21212121", firstName: "test", lastName: "2", password: "pwTest2");
            u3 = new _User(email: "test3mail@gmail.com", telephoneNumber: "13131313", firstName: "test", lastName: "3", password: "pwTest3");
            u4 = new _User(email: "test4mail@gmail.com", telephoneNumber: "14141414", firstName: "test", lastName: "4", password: "pwTest4");
            u5 = new _User(email: "test5mail@gmail.com", telephoneNumber: "15151515", firstName: "test", lastName: "5", password: "pwTest5");
            List<_User> users = new List<_User>()
            { u1, u2, u3, u4, u5 };

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Email], [PhoneNumber], [Password]) VALUES");

            foreach (_User user in users)
            {
                sb.Append($"('{user.FirstName}', '{user.LastName}', '{user.Email}', {user.TelephoneNumber}, '{user.Password}'), ");
            }

            sb[sb.Length - 2] = Convert.ToChar(";");

            string query = sb.ToString();

            using (SqlConnection connection = new SqlConnection(dbo_ConnString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void InsertBookings()
        {
            
            b1 = new Booking(2, 1, new DateOnly(2024, 10, 14)) { UserEmail = u1.Email }; // er før testdagen 
            b2 = new Booking(2, 1, new DateOnly(2024, 10, 15)) { UserEmail = u1.Email }; // til at test Available Time Slots
            b3 = new Booking(2, 2, new DateOnly(2024, 10, 15)) { UserEmail = u2.Email }; // til at test Available Time Slots
            b4 = new Booking(2, 1, new DateOnly(2024, 10, 17)) { UserEmail = u3.Email }; // til at test Available Time Slots
            b5 = new Booking(2, 2, new DateOnly(2024, 10, 17)) { UserEmail = u4.Email }; // til at test Available Time Slots
            b6 = new Booking(1, 1, DateOnly.FromDateTime(DateTime.Today).AddDays(1)) { UserEmail = u5.Email }; // til at test GetBy


            List<Booking> bookings = new List<Booking>() 
            { b1, b2, b3, b4, b5, b6};

            StringBuilder sb = new StringBuilder();
            sb.Append($"INSERT INTO [dbo].[Bookings] ([Date_], [TimeSlot], [ChargingPoint], [UserID]) VALUES ");

            foreach (Booking booking in bookings)
            {
                sb.Append($"('{booking.Date.ToString("yyyy-MM-dd")}', {booking.TimeSlot}, {booking.ChargingPoint}, (SELECT [UserID] FROM [Users] WHERE [Email] = '{booking.UserEmail}')), ");
            }

            sb[sb.Length - 2] = Convert.ToChar(";");

            string query = sb.ToString();

            using (SqlConnection connection = new SqlConnection(dbo_ConnString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static IConfiguration InitConfiguration()
        {

            var config = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            return config;
        }
    }
}
