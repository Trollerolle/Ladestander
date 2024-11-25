using User_ = El_Booking.Model.User;
using El_Booking.Model.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using Windows.System;

namespace El_BookingTest
{
    [TestClass]
    public class UserRepositoryTest : BaseTest
    {

        UserRepository Repo_WPFApp, Repo_DBO;

        [TestInitialize]
        public void Init()
        {
            Repo_WPFApp = new UserRepository(WPFApp_ConnString);

            InsertUsers();
        }

        [TestMethod]
        public void GetByUserFound()
        {
            User_ result = Repo_WPFApp.GetBy(u3.TelephoneNumber);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetByUserNotFound()
        {
            User_ result = Repo_WPFApp.GetBy("notAnUserEmail");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddUserSuccess()
        {
            User_ userToAdd = new User_(email: "testmail@gmail.com", telephoneNumber: "90909090", firstName: "test", lastName: "0", password: "pwd0");
            User_ result = Repo_WPFApp.GetBy(userToAdd.Email);
            Assert.IsNull(result);

            Repo_WPFApp.Add(userToAdd);

            result = Repo_WPFApp.GetBy(userToAdd.Email);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoginSuccessfull()
        {

            // tjekker at brugeren er der
            User_ u = Repo_WPFApp.GetBy(u3.Email);
            Assert.IsNotNull(u);

            bool LoginSuccess = Repo_WPFApp.Login(u3.Email, u3.Password);
            Assert.IsTrue(LoginSuccess);
        }

        [TestMethod]
        public void LoginUnsuccessfullPassword()
        {
            string pw = "Not the password"; 

            // tjekker at brugeren er der
            User_ u = Repo_WPFApp.GetBy(u3.TelephoneNumber);
            Assert.IsTrue(u.Password != pw);

            bool LoginSuccess = Repo_WPFApp.Login(u3.Email, pw);
            Assert.IsFalse(LoginSuccess);
        }

        [TestMethod]
        public void Update_AValidEmail_UserUpdated()
        {
            User_ userToUpdate = Repo_WPFApp.GetBy(u3.Email);

            userToUpdate.Email = "AValid@email.com";

            Repo_WPFApp.Update(userToUpdate);

            User_ result = Repo_WPFApp.GetBy("AValid@email.com");

            Assert.IsNotNull(result);
            Assert.AreEqual(userToUpdate.UserID, result.UserID);
            Assert.AreNotEqual(u3.Email, result.Email);
        }

        [TestMethod]
        public void Update_AValidPassword_UserUpdated()
        {
            User_ userToUpdate = Repo_WPFApp.GetBy(u3.Email);

            userToUpdate.Password = "new pwd";

            Repo_WPFApp.Update(userToUpdate);

            User_ result = Repo_WPFApp.GetBy(u3.Email);

            Assert.IsNotNull(result);
            Assert.AreEqual(userToUpdate.UserID, result.UserID);
            Assert.AreNotEqual("new pwd", result.Password);
        }
    }
}