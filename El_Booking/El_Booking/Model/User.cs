using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
	// ændre til internal
    public class User
    {
		private int _userID;

		public int UserID
		{
			get { return _userID; }
			set { _userID = value; }
		}


		private string _email;

		public string Email
		{
			get { return _email; }
			set { _email = value; }
		}

		private string _telephoneNumber;

		public string TelephoneNumber
		{
			get { return _telephoneNumber; }
			set { _telephoneNumber = value; }
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

		private string _password;

		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		public User(string email, string telephoneNumber, string firstName, string lastName, string password = null) 
		{ 
			Email = email;
			TelephoneNumber = telephoneNumber;
			FirstName = firstName;
			LastName = lastName;
			Password = password;
		}

        public User(int userID, string email, string telephoneNumber, string firstName, string lastName) 
			: this(email, telephoneNumber, firstName, lastName)
        {
            UserID = userID;
			
        }

    }
}
