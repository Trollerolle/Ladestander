﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using El_Booking.Utility;
using Microsoft.Data.SqlClient;
using Windows.Devices.SmartCards;
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

            string query = "EXEC usp_AddBooking @Date, @TimeSlotID, @CarID;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", booking.Date.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@TimeSlotID", booking.TimeSlot.TimeSlotID);
                command.Parameters.AddWithValue("@CarID", booking.CarID);

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
            TimeSlot timeSlot = _storer.TimeSlotRepository.GetAll().First(ts => ts.TimeSlotID == timeSlotID);
            int carID = (int)reader["CarID"];

			return new Booking(timeSlot, chargingPointID, date, bookingID, carID);
        }
    }
}
