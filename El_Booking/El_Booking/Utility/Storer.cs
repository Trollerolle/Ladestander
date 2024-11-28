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
        public readonly UserRepository UserRepository;
        public BookingRepository BookingRepository { get; }
        public TimeSlotRepository TimeSlotRepository { get; }
        public CarRepository CarRepository { get; }

        public Storer()
        {
            UserRepository = new UserRepository();
			TimeSlotRepository = new TimeSlotRepository();
			BookingRepository = new BookingRepository(TimeSlotRepository);
            CarRepository = new CarRepository();

        }
    }
}
