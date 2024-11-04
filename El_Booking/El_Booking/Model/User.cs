using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
    internal class User
    {
		private int userID;

		public int UserID
		{
			get { return userID; }
			set { userID = value; }
		}


		private string email;

		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		private string telephoneNumber;

		public string TelephoneNumber
		{
			get { return telephoneNumber; }
			set { telephoneNumber = value; }
		}

		private string firstName;

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}

		private string lastName;

		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}

		private string password;

		public string Password
		{
			get { return password; }
			set { password = value; }
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
