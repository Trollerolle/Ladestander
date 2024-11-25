using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace El_Booking.Model.Repositories
{
    public class TimeSlotRepository : IRepository<TimeSlot>
    {
        readonly string _connString;

        public TimeSlotRepository(string connectionString)
        {
            _connString = connectionString;
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
							timeSlotStart: TimeOnly.FromTimeSpan((TimeSpan)reader["StartTime"]),
                            interval: (int)reader["Interval"]
						)
                        );
					}
				}
			}

			return timeSlots;
		}

		public TimeSlot GetBy(string parameter)
        {
            TimeSlot timeSlot = null;

            string query = "EXEC [dbo].[usp_GetTimeSlot] @Parameter;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Parameter", int.Parse(parameter));
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        timeSlot = new TimeSlot
                        (
                            timeSlotID: (int)reader["TimeSlotID"],
                            timeSlotStart: (TimeOnly)reader["TimeSlotStart"],
                            interval: (int)reader["Interval"]
                        );
                    }
                }
            }

            return timeSlot;
        }


        public void Update(TimeSlot entity)
        {
            throw new NotImplementedException();
        }
    }
}
