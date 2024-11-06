using El_Booking.Model;
using Windows.System;

namespace El_BookingTest
{
    [TestClass]
    public class UserRepositoryTest
    {

        string elBooking = BaseTest.InitConfiguration().GetSection("ConnectionStrings")["AppConnection"];

        UserRepository Repo1;

        El_Booking.Model.User u1, u2, u3;

        [TestInitialize]
        public void Init()
        {
            Repo1 = new UserRepository(elBooking);

            u1 = new El_Booking.Model.User(email: "test1mail@gmail.com", telephoneNumber: "11111111", firstName: "test", lastName: "1", password: "pwTest1");
            u2 = new El_Booking.Model.User(email: "test2mail@gmail.com", telephoneNumber: "22222222", firstName: "test", lastName: "2", password: "pwTest2");
            u3 = new El_Booking.Model.User(email: "test3mail@gmail.com", telephoneNumber: "33333333", firstName: "test", lastName: "3", password: "pwTest3");
        }

        [TestMethod]
        public void GetByUserFound()
        {
            El_Booking.Model.User result = Repo1.GetBy("sander@gmail.com");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetByUserNotFound()
        {
            El_Booking.Model.User result = Repo1.GetBy("notAUserEmail");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddUserSuccess()
        {
            Repo1.Add(u3);
        }
    }
}