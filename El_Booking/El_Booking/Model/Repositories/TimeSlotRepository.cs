﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.SqlClient;
using Windows.ApplicationModel.Store;

namespace El_Booking.Model.Repositories
{
    public class TimeSlotRepository : IRepository<TimeSlot>
    {
		private App currentApp;
		private string _connString => currentApp.ConnectionString;

        public TimeSlotRepository()
        {
			currentApp = Application.Current as App;
		}

		public void Add(TimeSlot entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TimeSlot> GetAll()
        {
			List<TimeSlot> timeSlots = new List<TimeSlot>();

			string query = "EXEC [dbo].[usp_GetTimeSlots];";

			using (SqlConnection connection = new SqlConnection(_connString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				connection.Open();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						timeSlots.Add(new TimeSlot
						(
							timeSlotID: (int)reader["TimeSlotID"],
							timeSlotStart: TimeOnly.FromTimeSpan((TimeSpan)reader["TimeSlotStart"]),
							timeSlotEnd: TimeOnly.FromTimeSpan((TimeSpan)reader["TimeSlotEnd"])
						)
                        );
					}
				}
			}

			return timeSlots;
		}

		public TimeSlot GetBy(string parameter)
        {
            throw new NotImplementedException();
        }

        public void Update(TimeSlot entity)
        {
            throw new NotImplementedException();
        }
    }
}
