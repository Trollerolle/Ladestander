using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.SqlClient;
using Windows.Security.Authentication.OnlineId;
using Windows.System;

namespace El_Booking.Model.Repositories
{
	public class CarRepository : IRepository<Car>
	{

		private App currentApp;
		private string _connString => currentApp.ConnectionString;

		public CarRepository()
		{
			currentApp = Application.Current as App;
		}
		public void Add(Car car)
		{
			string query = "EXEC [dbo].[usp_AddCar] @LicensePlate, @Brand, @Model, @UserID, @ScopeCarID OUTPUT;";
			using (SqlConnection connection = new SqlConnection(_connString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
				command.Parameters.AddWithValue("@Brand", car.Brand);
				command.Parameters.AddWithValue("@Model", car.Model);
				command.Parameters.AddWithValue("@UserID", car.UserID);





				SqlParameter scopeCarIDParam = new SqlParameter("@ScopeCarID", SqlDbType.Int)
				{
					Direction = ParameterDirection.Output
				};
				command.Parameters.Add(scopeCarIDParam);

				connection.Open();
				command.ExecuteNonQuery();

				//currentApp.CurrentUser.Car.CarID = (int)scopeCarIDParam.Value;
			}

		}

		public void Delete(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Car> GetAll()
		{
			throw new NotImplementedException();
		}

		public Car? GetBy(string parameter)
		{
			Car? car = null;

			string query = "EXEC usp_GetCarBy @Parameter;";

			using (SqlConnection connection = new SqlConnection(_connString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@Parameter", parameter);
				connection.Open();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						car = new Car
						(
							carID: (int)reader["CarID"],
							licensePlate: (string)reader["LicensePlate"],
							brand: (string)reader["Brand"],
							model: (string)reader["Model"],
							userID: int.Parse(parameter)
						);
					}
				}
			}

			return car;
		}

		public void Update(Car car)
		{
			string query = "EXEC [dbo].[usp_UpdateCar] @CarID, @Brand, @Model, @LicensePlate;";
			// Så skal vi have flere SP ud fra hver kombination af data der skal opdateres ? tjek update i UserRepo.

			using (SqlConnection connection = new SqlConnection(_connString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@CarID", car.CarID);
				command.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
				command.Parameters.AddWithValue("@Brand", car.Brand);
				command.Parameters.AddWithValue("@Model", car.Model);

				connection.Open();
				command.ExecuteNonQuery();
			}
		}
	}
}
