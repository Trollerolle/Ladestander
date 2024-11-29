using El_Booking.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El_Booking.Utility
{
    public class Storer
    {
        public UserRepository UserRepository { get; }
        public BookingRepository BookingRepository { get; }
        public TimeSlotRepository TimeSlotRepository { get; }
        public CarRepository CarRepository { get; }

        public Storer()
        {
            CarRepository = new CarRepository();
            UserRepository = new UserRepository(CarRepository);
			TimeSlotRepository = new TimeSlotRepository();
			BookingRepository = new BookingRepository(TimeSlotRepository);

        }
    }
}
