using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace El_Booking.Model.Repositories
{
    public class ChargingPointRepository : IRepository<ChargingPoint>
    {
        readonly string _connString;

        public ChargingPointRepository(string connectionString)
        {
            _connString = connectionString;
        }

        public void Add(ChargingPoint entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChargingPoint> GetAll()
        {
            throw new NotImplementedException();
        }

        public ChargingPoint GetBy(string chargingPointID)
        {
            ChargingPoint chargingPoint = null;

            string query = "EXEC [dbo].[usp_GetChargingPoint] @ChargingPointID;";

            using (SqlConnection connection = new SqlConnection(_connString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ChargingPointID", int.Parse(chargingPointID));
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        chargingPoint = new ChargingPoint
                        (
                            chargingPointID: (int)reader["ChargingPointID"]
                        );
                    }
                }
            }

            return chargingPoint;
        }

        public void Update(ChargingPoint entity)
        {
            throw new NotImplementedException();
        }
    }
}
