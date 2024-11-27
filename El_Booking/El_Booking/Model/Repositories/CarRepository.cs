using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Data.SqlClient;
using Windows.System;

namespace El_Booking.Model.Repositories
{
	public class CarRepository : IRepository<Car>
	{

		private App currentApp;
		private string _connString => currentApp.ConnectionString;

		public CarRepository(string connectionString)
		{
			currentApp = Application.Current as App;
		}
		public void Add(Car car)
		{
			string query = "EXEC [dbo].[usp_AddCar] @LicensePlate, @Brand, @Model;";

			using (SqlConnection connection = new SqlConnection(_connString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				command.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
				command.Parameters.AddWithValue("@Brand", car.Brand);
				command.Parameters.AddWithValue("@Model", car.Model);

				connection.Open();
				command.ExecuteNonQuery();
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
							licensePlate: (string)reader["LicensePlate"],
							brand: (string)reader["Brand"],
							model: (string)reader["Model"]
						);
					}
				}
			}

			return car;
		}

		public void Update(Car car)
		{
			string query = "EXEC [dbo].[usp_UpdateCar] @LicensePlate, @Brand, @Model;";
			// Så skal vi have flere SP ud fra hver kombination af data der skal opdateres ? tjek update i UserRepo.

			using (SqlConnection connection = new SqlConnection(_connString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				if (car.LicensePlate is not null)
					command.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
				if (car.Brand is not null)
					command.Parameters.AddWithValue("@Brand", car.Brand);
				if (car.Model is not null)
					command.Parameters.AddWithValue("@Model", car.Model);

				connection.Open();
				command.ExecuteNonQuery();
			}
		}
	}
}
