using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Model
{
    public class Car
    {
        public string Brand { get; }
        public string Model { get; }
        public string LicensePlate { get; }

        public Car(string brand, string model, string licensePlate) 
        {
            Brand = brand;
            Model = model;
            LicensePlate = licensePlate;
        }
    }
}
