using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using El_Booking.Utility;
using Microsoft.Data.SqlClient;
using Windows.System;

namespace El_Booking.Model.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly App currentApp;
        private string _connString => currentApp.ConnectionString;
        private readonly Storer _storer;

        public BookingRepository(Storer storer)
        {
			currentApp = Application.Current as App;
            _storer = storer;
        }

        public void Add(Booking booking)
        {

            int carID = currentApp.CurrentUser.Car.CarID;

            string query = "EXEC usp_AddBooking1 @Date, @TimeSlotID, @CarID;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", booking.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@TimeSlotID", booking.TimeSlot.TimeSlotID);
                command.Parameters.AddWithValue("@CarID", carID);

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

        // bruges til at hente aktuelle bookinger for den uge der vises i BookingView
        public List<int[]> GetFullTimeSlotsForWeek(DateOnly mondayOfWeek)
        {

            List<int[]> fullTimeSlots = new List<int[]>();
            string query = "EXECUTE usp_GetFullTimeSlotsForWeek @Date;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", mondayOfWeek);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int timeSlot = (int)reader["TimeSlotID"] -1; // -1 fordi C# indeks starter på 0
                    int day = (int)((DateTime)reader["Date_"]).DayOfWeek -1;

                    int[] fullTimeSlot = [timeSlot, day];

                    fullTimeSlots.Add(fullTimeSlot);
                }
            }

            return fullTimeSlots;
        }

        public Booking? GetBy(string carID)
        {
            Booking? booking = null;

            string query = "EXEC [dbo].[usp_GetBooking] @CarID;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CarID", int.Parse(carID));
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
            int timeSlotID = (int)reader["TimeSlotID"];
			int chargingPointID = (int)reader["ChargingPointID"];
            DateOnly date = DateOnly.FromDateTime((DateTime)reader["Date_"]);
			int bookingID = (int)reader["BookingID"];
            TimeSlot timeSlot = _storer.TimeSlots.First(ts => ts.TimeSlotID == timeSlotID);

			return new Booking(timeSlot, chargingPointID, date, bookingID);
        }
    }
}
