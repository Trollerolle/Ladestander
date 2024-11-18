using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Windows.System;

namespace El_Booking.Model.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        readonly string _connString;

        public BookingRepository(string connectionString)
        {
            _connString = connectionString;
        }

        public void Add(Booking booking)
        {
            string query = "EXEC [dbo].[usp_AddBooking] @Date";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", booking.Date);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int bookingID)
        {
            string query = "EXEC [dbo].[usp_DeleteBooking] @BookingID";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", bookingID);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Booking> GetAll()
        {
            throw new NotImplementedException();
        }

        public Booking GetBy(string bookingID)
        {
            Booking booking = null;

            string query = "EXEC [dbo].usp_GetBooking @BookingID;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BookingID", int.Parse(bookingID));
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        booking = new Booking
                        (
                            bookingID: (int)reader["BookingID"],
                            date: (DateOnly)reader["Date"]
                        );
                    }
                }
            }

            return booking;
        }

        public void Update(Booking entity)
        {
            throw new NotImplementedException();
        }
    }
}
