using El_Booking.Model;
using El_Booking.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Model.Repositories;

namespace El_Booking.ViewModel
{
    public class BookingViewModel : BaseViewModel
    {
        const int numberOfChargers = 2; // antal ladere. Skal ændres til at være dynamisk, når ChargingPointRepository virker.

        public Booking? booking; // Brugerens booking, hvis han har en.
        public DateOnly mondayOfweek; // Dato for mandagen i den valgte uge.

        int _weekNr; // ugenummeret for den valgte uge
        public int WeekNr
        {
            get { return _weekNr; }
            set
            {
                _weekNr = value;
                OnPropertyChanged();
            }
        }

        int _selectedTimeSlot; // den ladetid, der er klikket på
        public int SelectedTimeSlot
        {
            get { return _selectedTimeSlot; }
            set
            {
                _selectedTimeSlot = value;
                OnPropertyChanged();
            }
        }

        int _selectedDay; // den dag der er klikket på (man = 0, tirs = 1 osv.)
        public int SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                _selectedDay = value;
                OnPropertyChanged();
            }
        }



        public List<List<int>> TimeSlotAvailabilityView
        {
            get
            {
                var result = new List<List<int>>();
                for (int i = 0; i < _timeSlotAvailability.GetLength(0); i++)
                {
                    var row = new List<int>();
                    for (int j = 0; j < _timeSlotAvailability.GetLength(1); j++)
                    {
                        row.Add(_timeSlotAvailability[i, j] ? 1 : 0);
                    }
                    result.Add(row);
                }
                return result;
            }
        }



        private bool[,] _timeSlotAvailability; // false = ikke fyldt

        public bool[,] TimeSlotAvailability
        {
            get { return _timeSlotAvailability; }
            set 
            { 
                _timeSlotAvailability = value;
                OnPropertyChanged(nameof(TimeSlotAvailability));
                OnPropertyChanged(nameof(TimeSlotAvailabilityView));
            }
        }


        BookingRepository _bookingRepo;
        ChargingPointRepository _chargingPointRepo;
        TimeSlotRepository _timeSlotRepo;

        public BookingViewModel(string connectionString, DateTime? startingDate = null) 
        {
            DateTime today = startingDate ?? DateTime.Today; // til test, så datoen den starter på kan ændres. Ellers dd.

            _bookingRepo = new BookingRepository(connectionString);
            _chargingPointRepo = new ChargingPointRepository(connectionString);
            _timeSlotRepo = new TimeSlotRepository(connectionString);

            // TimeSlotValues er dynamisk ud fra hvor mange TimeSlots der er i databasen.
			TimeSlotValues = GenerateTimeSlotValues(_timeSlotRepo.GetAll());
			TimeSlotAvailability = new bool[TimeSlotValues.Count, 5]; // 5 for antal dage i ugen

            _weekNr = DateUtils.GetIso8601WeekOfYear(today);
            mondayOfweek = today.StartOfWeek();

            LoadFullTimeslots();
        }

        List<string> GenerateTimeSlotValues(IEnumerable<TimeSlot> timeSlots)
        {
            List<string> timeSlotsParameter = new List<string>();
            foreach (TimeSlot timeSlot in timeSlots)
            {
                timeSlotsParameter.Add(timeSlot.TimeSlotStart.ToString(@"hh\:mm")); // Konverter til string
            }
            return timeSlotsParameter;
		}

        void LoadFullTimeslots()
        {
            List<int[]> fullTimeSlots = _bookingRepo.GetFullTimeSlotsForWeek(mondayOfweek);

            

            foreach (int[] fullTimeSlot in fullTimeSlots)
            {
                int day = fullTimeSlot[0];
                int timeSlot = fullTimeSlot[1];
                
                TimeSlotAvailability[day, timeSlot] = true;
            }
        }

        private List<string> _timeSlotValues;
        public List<string> TimeSlotValues
        {
            get { return _timeSlotValues; }
            set 
            {  
                _timeSlotValues = value;
                OnPropertyChanged(nameof(TimeSlotValues));
            }
        }
   
        //public void CreateBooking()
        //{
        //    int charger = FindAvailableCharger(new bool[4, 2], SelectedTimeSlot);
        //    DateOnly selectedDate = mondayOfweek.AddDays(SelectedDay);

        //    Booking newBooking = new Booking(SelectedTimeSlot, charger, selectedDate);

        //    booking = newBooking;

        //    _bookingRepo.Add(booking);

        //    LoadFullTimeslots();
        //}

        int FindAvailableCharger(bool[,] day, int timeSlot)
        {

            for (int emptyCharger = 0; emptyCharger < numberOfChargers; emptyCharger++)
            {
                if (day[timeSlot, emptyCharger] != true)
                    return emptyCharger;
            }

            return -1;

        }
    }
}
