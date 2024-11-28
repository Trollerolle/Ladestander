using El_Booking.Model;
using El_Booking.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Utility;
using El_Booking.Commands;
using System.Collections.ObjectModel;

namespace El_Booking.ViewModel.BookingVM
{
    public class BookingViewModel : BaseViewModel
    {

		private readonly Storer _storer;

		public BookingViewModel(Storer storer, DateTime? startingDate = null)
		{
			DateTime today = startingDate ?? DateTime.Today; // til test, så datoen den starter på kan ændres. Ellers dd.

			_storer = storer;

			// TimeSlotValues er dynamisk ud fra hvor mange TimeSlots der er i databasen.
			TimeSlotValues = GenerateTimeSlotValues(_storer.TimeSlotRepository.GetAll());
			TimeSlotAvailability = new bool[TimeSlotValues.Count, 5]; // 5 for antal dage i ugen

			WeekNr = DateUtils.GetIso8601WeekOfYear(today);
			MondayOfWeek = today.StartOfWeek();

			LoadFullTimeslots();
            SetDaysOfWeekDays();
		}

		const int numberOfChargers = 2; // antal ladere. Skal ændres til at være dynamisk, når ChargingPointRepository virker.

        public Booking? booking; // Brugerens booking, hvis han har en.
        
        private DateOnly _mondayOfWeek; // Dato for mandagen i den valgte uge.

        public DateOnly MondayOfWeek
        {
            get { return _mondayOfWeek; }
            set 
            { 
                _mondayOfWeek = value;
                OnPropertyChanged();
            }
        }


        private int _weekNr; // ugenummeret for den valgte uge
        public int WeekNr
        {
            get { return _weekNr; }
            set
            {
                _weekNr = value;
                OnPropertyChanged();
            }
        }

        int? _selectedTimeSlot; // den ladetid, der er klikket på
        public int? SelectedTimeSlot
        {
            get { return _selectedTimeSlot; }
            set
            {
                _selectedTimeSlot = value;
                OnPropertyChanged();
            }
        }

        int? _selectedDay; // den dag der er klikket på (man = 0, tirs = 1 osv.)
        public int? SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                _selectedDay = value;
                OnPropertyChanged();
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

        List<string> GenerateTimeSlotValues(IEnumerable<TimeSlot> timeSlots)
        {
            List<string> timeSlotsParameter = new List<string>();
            foreach (TimeSlot timeSlot in timeSlots)
            {
                timeSlotsParameter.Add(timeSlot.TimeSlotStart.ToString(@"HH\:mm")); // Konverter til string
            }
            return timeSlotsParameter;
        }


        public RelayCommand ChangeWeekForwardCommand => new RelayCommand(
        execute => ChangeWeek(1),
        canExecute => NotMoreThanMonthInFuture()
        );

        public RelayCommand ChangeWeekBackwardCommand => new RelayCommand(
        execute => ChangeWeek(-1),
        canExecute => NotLessThanCurrentWeek()
        );

        public void ChangeWeek(int difference) // forskellen kan være -1 eller 1. Men kan også bruges til at ændre flere uger på en gang fx 3 uger frem
        {
            MondayOfWeek = MondayOfWeek.AddDays(7 * difference);

            WeekNr = DateUtils.GetIso8601WeekOfYear(MondayOfWeek.ToDateTime(new TimeOnly()));
            SetDaysOfWeekDays();
            LoadFullTimeslots() ;
        }

        //public List<DateOnly> daysOfWeekDates = new List<DateOnly>();
        private ObservableCollection<string> _daysOfWeekDates = new ObservableCollection<string>();

        public ObservableCollection<string> DaysOfWeekDates
        {
            get { return _daysOfWeekDates; }
            set 
            { 
                _daysOfWeekDates = value;
                OnPropertyChanged();
            }
        }


        public void SetDaysOfWeekDays()
        {
            DaysOfWeekDates.Insert(0, MondayOfWeek.ToShortDateString());
            
            for (int i = 1; i < 5; i++) //i starter på 1 fordi vi har puttet mandag ind
            {
                DaysOfWeekDates.Insert(i, MondayOfWeek.AddDays(i).ToShortDateString());
            }
            OnPropertyChanged(nameof(DaysOfWeekDates));
            
        }

        public bool NotLessThanCurrentWeek()
        {
            DateTime currentDay = DateTime.Today;
            return MondayOfWeek > currentDay.StartOfWeek();
        }

        public bool NotMoreThanMonthInFuture()
        {
            DateOnly limit = DateOnly.FromDateTime(DateTime.Today);
            return MondayOfWeek <= (limit.AddDays(30));
        }




        void LoadFullTimeslots()
        {
            List<int[]> fullTimeSlots = _storer.BookingRepository.GetFullTimeSlotsForWeek(MondayOfWeek);

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




    }
}
