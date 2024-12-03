using El_Booking.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using User_ = El_Booking.Model.User;

namespace El_BookingTest
{

    [TestClass]
    public class UserTest
    {

        User_ u1;

        [TestInitialize]
        public void Init()
        {
            u1 = new User_();
        }

        [TestMethod]
        public void SetEmail_ValidEmail_SetsEmail()
        {
            var email = "valid@email.com";
            u1.Email = email;
            Assert.AreEqual(email, u1.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid email address")]
        public void SetEmail_IsNotValidEmail_ThrowsException()
        {
            var email = "not_vaild_email";
            u1.Email = email;
        }

        [TestMethod]
        public void SetPhonenumber_CountryCodeInFront_SetsPhonenumber()
        {
            var pn = "+45 12121212";
            u1.TelephoneNumber = pn;
            Assert.AreEqual(pn, u1.TelephoneNumber);
        }

        [TestMethod]
        public void SetPhonenumber_DoubleZeroInFront_SetsPhonenumber()
        {
            var pn = "00450 12121212";
            u1.TelephoneNumber = pn;
            Assert.AreEqual(pn, u1.TelephoneNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid Phonenumber")]
        public void SetPhonenumber_CountryCodeAndNoSpace_ThrowsException()
        {
            var pn = "004512121212";
            u1.TelephoneNumber = pn;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid Phonenumber")]
        public void SetPhonenumber_TooShort_ThrowsException()
        {
            var pn = "1";
            u1.TelephoneNumber = pn;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid Phonenumber")]
        public void SetPhonenumber_TooLong_ThrowsException()
        {
            var pn = "12121212121212121212121";
            u1.TelephoneNumber = pn;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid Firstname")]
        public void SetFirstName_TooLong_ThrowsException()
        {
            var fn = new string(char.Parse("a"), 51);
            u1.FirstName = fn;
        }

        [TestMethod]
        public void SetFirstname_TwoNames_SetsFirstName()
        {
            var fn = "sander elgaard";
            var expected = "Sander Elgaard";
            u1.FirstName = fn;
            Assert.AreEqual(expected, u1.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Too long Last Name")]
        public void SetLastName_TooLong_ThrowsException()
        {
            var ln = new string(char.Parse("a"), 51);
            u1.FirstName = ln;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Only one Last Name")]
        public void SetLastName_TwoNames_ThrowsException()
        {
            var ln = "Elgaard Andersen";
            u1.LastName = ln;
        }

        [TestMethod]
        public void SetFirstname_Valid_SetsFirstName()
        {
            var fn = "andersen";
            var expected = "Andersen";
            u1.LastName = fn;
            Assert.AreEqual(expected, u1.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid Password")]
        public void SetPassword_TooLong_ThrowsException()
        {
            StringBuilder sb = new StringBuilder(new string(char.Parse("a"), 19));
            sb.Append("A1!");
            u1.Password = sb.ToString();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid Password")]
        public void SetPassword_TooShort_ThrowsException()
        {
            string pwd = "Aa1!";
            u1.Password = pwd;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Not a valid Password")]
        public void SetPassword_Whitespaces_ThrowsException()
        {
            var pwd = "My S3cret P@ssword";
            u1.Password = pwd;
        }

        [TestMethod]
        public void SetPassword_Valid_SetsPassword()
        {
            var pwd = "S3cretP@ssword";
            u1.Password = pwd;
            Assert.AreEqual(pwd, u1.Password);
        }

        [TestMethod]
        public void SetPassword_Pik_SetsPassword()
        {
            var pwd = "Pik";
            u1.Password = pwd;
            Assert.AreEqual(pwd, u1.Password);
        }
    }
}
