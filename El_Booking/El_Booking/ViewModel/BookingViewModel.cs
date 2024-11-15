using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Model;

namespace El_Booking.ViewModel
{
    internal class BookingViewModel : BaseViewModel
    {
        UserRepository _userRepo;
        public BookingViewModel(string ConnectionString)
        { 
            _userRepo = new UserRepository(ConnectionString);
        }
    }
}
