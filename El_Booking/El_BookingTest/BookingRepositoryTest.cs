using El_Booking.Model;
using El_Booking.Model.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;

namespace El_BookingTest
{
    [TestClass]
    public class BookingRepositoryTest : BaseTest
    {

        BookingRepository BookingRepo_WPFApp;

        [TestInitialize]
        public void Init()
        {
            InsertUsers();
            InsertBookings();

            BookingRepo_WPFApp = new BookingRepository(WPFApp_ConnString);
        }

        [TestMethod]
        public void Add()
        {
            Booking booking = new Booking(1, 1, new DateOnly(2024, 11, 29));
            booking.UserEmail = u1.Email;

            BookingRepo_WPFApp.Add(booking);

            Booking? result = BookingRepo_WPFApp.GetBy(booking.UserEmail);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetFullTimeSlots()
        {
            List<int[]> result = BookingRepo_WPFApp.GetFullTimeSlots(new DateOnly(2024, 10, 14));
            Assert.IsTrue(result[0].SequenceEqual(new int[] { 1 ,1 }));
            Assert.IsTrue(result[1].SequenceEqual(new int[] { 3, 1 }));
        }

        [TestMethod]
        public void GetBy()
        {
            Booking? result = BookingRepo_WPFApp.GetBy(u5.Email);
            StringAssert.Equals(b6.ToString(), result.ToString());
        }
    }
}
