using El_Booking.Utility;
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
			set 
			{
                Regex regex = new Regex(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$");

                if (!regex.IsMatch(value))
					throw new NotSupportedException("Not a valid email address");

				else
					_email = value;
		    }
		}

        // Regex: https://learn.microsoft.com/da-dk/dotnet/standard/base-types/regular-expression-language-quick-reference

        private string _telephoneNumber;
		public string TelephoneNumber
		{
			get { return _telephoneNumber; }
            set
            {
                Regex regex = new Regex(@"^(((\+|00)[0-9]{2,3})\s{1})?[1-9][0-9]{7,14}$");

                if (!regex.IsMatch(value))
                    throw new NotSupportedException("Not a valid Phonenumber address");

                else
                    _telephoneNumber = value;
            }
        }

        private string _firstName;
		public string FirstName
		{
			get { return _firstName; }
            set
            {
                if (value.Length > 50)
                    throw new NotSupportedException("Too long First Name");

                else
				{
					string[] names = value.Split(' ');
					for (int i = 0; i < names.Length; i++) 
						names[i] = names[i].FirstCharToUpper();

					_firstName = String.Join(" ", names);
				}
            }
        }

		private string _lastName;
		public string LastName
		{
			get { return _lastName; }
            set
            {

                if (value.Length > 50)
                    throw new NotSupportedException("Too long Last Name");

				else if (value.Split(' ').Length > 1)
                    throw new NotSupportedException("Only one Last Name");

                else
                _lastName = value.FirstCharToUpper();
            }
        }

		private string? _password;
		public string? Password
		{
			get { return _password; }
            set
            {
				/*
					Minimum 6 and maximum 21 characters,
					at least one uppercase letter,
					one lowercase letter,
					one number and 
					one special character
				*/
                Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,21}$");

                if (value == null)
					_password = null;

				else if (!regex.IsMatch(value))
                    throw new NotSupportedException("Not a valid Password");
                    
                else
					_password = value;
            }
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
    }
}
