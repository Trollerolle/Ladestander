using Microsoft.Data.SqlClient;
using System.Windows;

namespace El_Booking.Model
{
    // ændre til internal
    public class UserRepository : IRepository<User>
    {
        readonly string _connString;

        public UserRepository(string connectionString)
        {
            this._connString = connectionString;
        }

        public void Add(User user)
        {

            string query = "EXEC [dbo].[usp_CreateUserAndLogin] @FirstName, @LastName, @Email, @PhoneNumber, @Password;";

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

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetBy(string param)
        {
            User user = null;

            string query = "EXEC uspGetUserBy @Parameter;";

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
                            // skal UserID med??
                            email: (string)reader["Email"],
                            telephoneNumber: (string)reader["PhoneNumber"],
                            firstName: (string)reader["FirstName"],
                            lastName: (string)reader["LastName"]
                        );
                    }
                }
            }

            return user;
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
