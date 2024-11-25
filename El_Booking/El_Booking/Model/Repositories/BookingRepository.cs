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
            string query = "EXEC [dbo].[usp_AddBooking] @Date, @TimeSlot, @ChargingPoint, @UserEmail;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", booking.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@TimeSlot", booking.TimeSlot +1);
                command.Parameters.AddWithValue("@ChargingPoint", booking.ChargingPoint +1);
                command.Parameters.AddWithValue("@UserEmail", booking.UserEmail);
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

        // bliver ikke pt. brugt
        public IEnumerable<Booking> GetAll()
        {
            List<Booking> bookings = new List<Booking>();
            DateOnly todaysDate = DateOnly.FromDateTime(DateTime.Today);

            string query = "EXEC [dbo].usp_GetBookings;";

            using (SqlConnection connection = new SqlConnection(_connString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookings.Add(
                            InstantiateBooking(reader)
                            );
                    }
                }
            }

            return bookings;
        }

        // bruges til at hente aktuelle bookinger for den uge der vises i BookingView
        public List<int[]> GetFullTimeSlots(DateOnly mondayOfWeek)
        {

            List<int[]> fullTimeSlots = new List<int[]>();
            string query = "EXECUTE usp_GetPlannedBookingsForWeek2 @Date;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", mondayOfWeek);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int timeSlot = (int)reader["TimeSlot"] -1; // -1 fordi C# indeks starter på 0
                    int day = (int)((DateTime)reader["Date_"]).DayOfWeek -1;

                    int[] fullTimeSlot = new int[] { timeSlot, day };

                    fullTimeSlots.Add(fullTimeSlot);
                }
            }

            return fullTimeSlots;
        }

        public List<string> GetTimeSlotValues()
        {
            List<string> timeSlotValues = new List<string>();
            string query = "SELECT StartTime FROM TimeSlots;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TimeSpan timeSlotValue = (TimeSpan)reader["StartTime"]; // Læs værdien som TimeSpan
                    timeSlotValues.Add(timeSlotValue.ToString(@"hh\:mm")); // Konverter til string
                }
            }

            return timeSlotValues;
        }

        public Booking? GetBy(string userEmail)
        {
            Booking? booking = null;

            string query = "EXEC [dbo].[usp_GetBooking] @Email;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", userEmail);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    booking = InstantiateBooking(reader);
                }
            }

            return booking;
        }

        public void Update(Booking entity)
        {
            throw new NotImplementedException();
        }

        Booking InstantiateBooking(SqlDataReader reader)
        {
            int timeSlot = (int)reader["TimeSlot"] -1; // -1 fordi C# er 0 indeks, men SQL starter ved 1
            int chargingPoint = (int)reader["ChargingPoint"] -1;
            DateOnly date = DateOnly.FromDateTime((DateTime)reader["Date_"]);

            return new Booking(timeSlot, chargingPoint, date);
        }
    }
}
