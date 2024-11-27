using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
    public class Car
    {
		public string LicensePlate { get; set; }
		public string Brand { get; set; }
        public string Model { get; set; }


        public Car(string licensePlate, string brand, string model) 
        {
			LicensePlate = licensePlate;
			Brand = brand;
            Model = model;

        }
        public Car() 
        { 
        }
    }
}
