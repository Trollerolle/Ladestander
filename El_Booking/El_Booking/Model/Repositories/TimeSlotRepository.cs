using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
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
                            timeSlotEnd: (TimeOnly)reader["TimeSlotEnd"]
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
