using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace El_Booking.Model
{
    public class User
    {
		private int? _userID;
		public int? UserID
		{
			get { return _userID; }
			set { _userID = value; }
		}

		private string _email;
		public string Email
		{
			get { return _email; }
			set { _email = ValidateEmail(value); }
		}

		private string _telephoneNumber;
		public string TelephoneNumber
		{
			get { return _telephoneNumber; }
			set { _telephoneNumber = ValidatePhonenumber(value); }
		}

		private string _firstName;
		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; }
		}

		private string _lastName;
		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; }
		}

		private string? _password;
		public string? Password
		{
			get { return _password; }
			set { _password = value; }
		}

		private Car? _Car;

		public Car? Car
		{
			get { return _Car; }
			set { _Car = value; }
		}

		// til oprettelse af bruger
		public User(string email, string telephoneNumber, string firstName, string lastName, string? password = null, int? userID = null, Car? car = null) 
		{
			UserID = userID;
			Email = email;
			TelephoneNumber = telephoneNumber;
			FirstName = firstName;
			LastName = lastName;
			Password = password;
			Car = car;
		}

		// til test
		public User() 
		{ 
		}

        private string ValidatePhonenumber(string value)
        {
            if (!validPhonenumber(value))
                throw new NotSupportedException("Not a valid Phonenumber");

            return value;
        }

        private bool validPhonenumber(string value)
        {
            Regex regex = new Regex("^\\+?[1-9][0-9]{7,14}$");
            return regex.IsMatch(value);
        }

        private string ValidateEmail(string value)
        {
            if (!validEmail(value))
                throw new NotSupportedException("Not a valid email address");

            return value;
        }

        private bool validEmail(string value)
        {
            return Regex.IsMatch(value, @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$");
        }
    }
}
