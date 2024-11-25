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

        public Storer(string connectionString)
        {
            UserRepository = new UserRepository(connectionString);
            BookingRepository = new BookingRepository(connectionString);
            TimeSlotRepository = new TimeSlotRepository(connectionString);
        }
    }
}
