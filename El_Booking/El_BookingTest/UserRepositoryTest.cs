using El_Booking.Model;
using Microsoft.Data.SqlClient;
using System.Data;
using Windows.System;

namespace El_BookingTest
{
    [TestClass]
    public class UserRepositoryTest
    {

        string WPFApp = BaseTest.InitConfiguration().GetSection("ConnectionStrings")["AppConnection"];
        string dbo = BaseTest.InitConfiguration().GetSection("ConnectionStrings")["WindowsLoginConnection"];
        string local = BaseTest.InitConfiguration().GetSection("ConnectionStrings")["LocalConnection"];

        UserRepository Repo_WPFApp, Repo_DBO;

        El_Booking.Model.User u1, u2, u3;

        [TestInitialize]
        public void Init()
        {
            Repo_WPFApp = new UserRepository(local); // skift connectionstring 

            u1 = new El_Booking.Model.User(email: "test1mail@gmail.com", telephoneNumber: "11111111", firstName: "test", lastName: "1", password: "pwTest1");
            u2 = new El_Booking.Model.User(email: "test2mail@gmail.com", telephoneNumber: "22222222", firstName: "test", lastName: "2", password: "pwTest2");
            u3 = new El_Booking.Model.User(email: "test3mail@gmail.com", telephoneNumber: "33333333", firstName: "test", lastName: "3", password: "pwTest3");

            string query = $"INSERT INTO [Users] VALUES ('{u3.FirstName}', '{u3.LastName}', '{u3.Email}', '{u3.TelephoneNumber}', '{u3.Password}');";
            using (SqlConnection connection = new SqlConnection(dbo))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            string query = $"DELETE FROM [Users] WHERE [Email] LIKE '%test%';";

            using (SqlConnection connection = new SqlConnection(dbo))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [TestMethod]
        public void GetByUserFound()
        {
            El_Booking.Model.User result = Repo_WPFApp.GetBy(u3.TelephoneNumber);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetByUserNotFound()
        {
            El_Booking.Model.User result = Repo_WPFApp.GetBy("notAUserEmail");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddUserSuccess()
        {
            El_Booking.Model.User result = Repo_WPFApp.GetBy(u2.Email);
            Assert.IsNull(result);

            Repo_WPFApp.Add(u2);

            result = Repo_WPFApp.GetBy(u2.Email);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoginSuccessfull()
        {

            // tjekker at brugeren er der
            El_Booking.Model.User u = Repo_WPFApp.GetBy(u3.Email);
            Assert.IsNotNull(u);

            bool LoginSuccess = Repo_WPFApp.Login(u3.Email, u3.Password);
            Assert.IsTrue(LoginSuccess);
        }

        [TestMethod]
        public void LoginUnsuccessfullPassword()
        {
            string pw = "Not the password"; 

            // tjekker at brugeren er der
            El_Booking.Model.User u = Repo_WPFApp.GetBy(u3.TelephoneNumber);
            Assert.IsTrue(u.Password != pw);

            bool LoginSuccess = Repo_WPFApp.Login(u3.Email, pw);
            Assert.IsFalse(LoginSuccess);
        }
    }
}