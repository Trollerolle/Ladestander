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

		public List<int[]> GetFullTimeSlot(DateOnly monday)
		{
            List<int[]> fullTimeSlots = new List<int[]>();
            string query = "EXECUTE usp_GetFullTimeSlotsForWeek @Date;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Date", monday);
                connection.Open();

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //-1 fordi søndag er nul og vi læser 3. (3-1 = 2) I vores array er onsdag = 2 da mandag er 0. Men dayesOfWeek er mandag 1.
					int day = (int)((DateTime)reader["Date_"]).DayOfWeek -1; 
					int timeSlotID = (int)reader["TimeSlotID"];


                    int[] fullTimeSlot = [timeSlotID, day];

                    fullTimeSlots.Add(fullTimeSlot);
                }
            }

            return fullTimeSlots;
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
