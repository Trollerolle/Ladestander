using El_Booking.Utility;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Windows;
using System.Windows.Documents;
using Windows.System;

namespace El_Booking.Model.Repositories
{
    // ændre til internal
    public class UserRepository : IRepository<User>
    {
		private readonly App currentApp;
		private string _connString => currentApp.ConnectionString;
        private readonly Storer _storer;

		public UserRepository(Storer storer)
        {
			currentApp = Application.Current as App;
            _storer = storer;
        }

        public void Add(User user)
        {

            string query = "EXEC [dbo].[usp_AddUserToUsers] @FirstName, @LastName, @Email, @PhoneNumber, @Password;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PhoneNumber", user.TelephoneNumber);
                command.Parameters.AddWithValue("@Password", user.Password);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool Login(string email, string password)
        {
            string query = "[dbo].[usp_Login]";

            using (SqlConnection connection = new SqlConnection(_connString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                // @ReturnVal can be any name
                var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                command.ExecuteNonQuery();

                int result = (int)returnParameter.Value;

                return result == 0 ? true : false;

            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public User? GetBy(string param)
        {
            User? user = null;

            string query = "EXEC usp_GetUserBy @Parameter;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Parameter", param);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        
                        user = new User
                        (
                            userID: (int)reader["UserID"],
                            email: (string)reader["Email"],
                            telephoneNumber: (string)reader["PhoneNumber"],
                            firstName: (string)reader["FirstName"],
                            lastName: (string)reader["LastName"],
                            password: null,
                            car: null

                        );
                        try
                        {

                            int carID = (int)reader["CarID"];
                            user.Car = _storer.CarRepository.GetBy(carID.ToString());
                            user.Booking = _storer.BookingRepository.GetBy(carID.ToString());

                        }
                        catch (Exception InvalidCastException)
                        {
                        }
                    }
                }
            }

            return user;
        }

        public void Update(User updatedUser)
        {
            string queryWithPassword = "EXEC [dbo].[usp_UpdateUser] @UserID, @FirstName, @LastName, @Email, @PhoneNumber, @Password;";
            string queryWithoutPassword = "EXEC [dbo].[usp_UpdateUser] @UserID, @FirstName, @LastName, @Email, @PhoneNumber";

            string query = updatedUser.Password is null ? queryWithoutPassword : queryWithPassword;

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", updatedUser.UserID);
                command.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
                command.Parameters.AddWithValue("@LastName", updatedUser.LastName);
                command.Parameters.AddWithValue("@Email", updatedUser.Email);
                command.Parameters.AddWithValue("@PhoneNumber", updatedUser.TelephoneNumber);

                if (updatedUser.Password is not null)
                    command.Parameters.AddWithValue("@Password", updatedUser.Password);
                
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
