using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking;
using El_Booking.Model;
using El_Booking.Model.Repositories;
using El_Booking.ViewModel;
using Microsoft.Data.SqlClient;

namespace El_BookingTest
{
    [TestClass]
    public class BookingViewModelTest : BaseTest
    {

        DateTime TodaysDate = new DateTime(2024, 10, 15); // sæt den dato som skal testes på
        BookingViewModel bookingVM, bookingVM2;

        [TestInitialize]
        public void init()
        {
            InsertUsers();
            InsertBookings();

            bookingVM = new BookingViewModel(WPFApp_ConnString, TodaysDate);
        }

        [TestMethod]
        public void GetCorrectWeekNumber()
        {
            Assert.IsTrue(bookingVM.WeekNr == 42);
        }

        [TestMethod]
        public void GetCorrectMonday()
        {
            Assert.IsTrue(bookingVM.mondayOfweek == new DateOnly(2024, 10, 14));
        }

        [TestMethod]
        public void LoadBookings()
        {
            Assert.IsTrue(bookingVM.TimeSlotAvailability[1, 1]);
            Assert.IsTrue(bookingVM.TimeSlotAvailability[3, 1]);
        }

        [TestMethod]
        public void ConstructBookingViewModel()
        {
            bookingVM2 = new BookingViewModel(WPFApp_ConnString);

            Assert.IsNull(bookingVM2.booking);
        }

        //[TestMethod]
        //public void CreateNewBooking()
        //{
        //    bookingVM2 = new BookingViewModel(WPFApp_ConnString, TodaysDate) 
        //    { 
        //        SelectedDay = 1, // tirsdag
        //        SelectedTimeSlot = 1, // anden interval
        //    };

        //    Assert.IsNull(bookingVM2.booking);
        //    Assert.IsFalse(bookingVM2.Tuesday[1, 0]);

        //    bookingVM2.CreateBooking();

        //    Assert.IsNotNull(bookingVM2.booking);
        //    Assert.IsTrue(bookingVM2.Tuesday[1, 0]);

        //}
    }
}
