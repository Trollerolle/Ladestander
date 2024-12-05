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
using System.Windows.Input;
using El_Booking.View.Booking;
using System.Windows;
using System.Diagnostics;
using System.ComponentModel;

namespace El_Booking.ViewModel.BookingVM
{
    public class BookingViewModel : BaseViewModel
    {

        public ICommand MakeBookingCommand { get; }

        public DateOnly MondayOfWeek { get; set; } // Dato for mandagen i den valgte uge.
        private readonly DateOnly _startingDate;
        private readonly Storer _storer;

        public MainBookingViewModel MainBookingViewModel { get; }

		public BookingViewModel(Storer storer, MainBookingViewModel mainBookingViewModel)
		{
            _startingDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            if (_startingDate.DayOfWeek == DayOfWeek.Saturday)
                _startingDate = _startingDate.AddDays(2);
            else if (_startingDate.DayOfWeek == DayOfWeek.Sunday)
                _startingDate = _startingDate.AddDays(1);
            MainBookingViewModel = mainBookingViewModel;
            _storer = storer;

            MakeBookingCommand = new MakeBookingCommand(this, storer);

			WeekNr = DateUtils.GetIso8601WeekOfYear(_startingDate);
			MondayOfWeek = _startingDate.StartOfWeek();

			GetCurrentTimeSlots(MondayOfWeek);
            GetCurrentDays(MondayOfWeek);

            SetTimeSlotsAsPassed();

            if (MainBookingViewModel.CurrentBooking != null)
            {
                if (MainBookingViewModel.CurrentBooking.Date >= MondayOfWeek && MainBookingViewModel.CurrentBooking.Date <= MondayOfWeek.AddDays(5))
                {
                    SetTimeSlotsAsYours();
                }
            }

            MainBookingViewModel.PropertyChanged += OnMainBookingViewModelPropertyChanged;

        }

        private void OnMainBookingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
			GetCurrentTimeSlots(MondayOfWeek);

            if (DateOnly.FromDateTime(DateTime.Today) >= MondayOfWeek)
            { 
                SetTimeSlotsAsPassed();
            }

            if (MainBookingViewModel.CurrentBooking != null)
            {
                if (MainBookingViewModel.CurrentBooking.Date >= MondayOfWeek && MainBookingViewModel.CurrentBooking.Date <= MondayOfWeek.AddDays(5))
                {
                    SetTimeSlotsAsYours();
                }
            }

            OnPropertyChanged();
        }

        private ObservableCollection<TimeSlotViewModel> _currentTimeSlots;
        public ObservableCollection<TimeSlotViewModel> CurrentTimeSlots
        {
            get => _currentTimeSlots;
            set
            {
                _currentTimeSlots = value;
                OnPropertyChanged();
            }
        } 

        private void GetCurrentTimeSlots(DateOnly monday)
        {
            IEnumerable<TimeSlot> availableTimeSlots = _storer.TimeSlotRepository.GetAll();

            List<TimeSlotViewModel> timeSlots = new List<TimeSlotViewModel>();

            foreach (TimeSlot timeSlot in availableTimeSlots)
            {
                timeSlots.Add(new TimeSlotViewModel(timeSlot));
            }

            List<int[]> fullSlots = _storer.TimeSlotRepository.GetFullTimeSlot(monday);

            if (fullSlots.Count > 0)
            {
                foreach (int[] fullSlot in fullSlots)
                {
                    int timeSlotID = fullSlot[0];
                    int day = fullSlot[1];

                    timeSlots.Find(x => x.TimeSlotID == timeSlotID).SetDayFull(day);
                }
            }

            CurrentTimeSlots = new ObservableCollection<TimeSlotViewModel>(timeSlots);

        }


        //Prop til SetTimeslotsAsPassed som bruges til at gemme den
        private TimeSlotViewModel _nearestTimeSlot;
        public TimeSlotViewModel NearestTimeSlot
        {
            get => _nearestTimeSlot;
            set
            {
                _nearestTimeSlot = value;
                OnPropertyChanged(); 
            }
        }

        private void SetTimeSlotsAsPassed()
        {

            int currentDayAsInt = (int)DateTime.Now.DayOfWeek-1;

            var Tid = DateTimeOffset.Now;

            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now); //Skal ændres tilbage til idag

            NearestTimeSlot = CurrentTimeSlots
            .Where(slot => slot.StartTime <= currentTime) // Kun time slots som er <= current time
            .OrderByDescending(slot => slot.StartTime)    // Sorter efter højeste først. (Hvis du kigger klokken 13, kan det være 6 12 eller 9, IKKE 15 som bliver sorteret til 12, 9, 6)
            .FirstOrDefault();                                           // Vælg første, ( 12 )

            int? closestTimeSlotID = _nearestTimeSlot?.TimeSlotID; // Sætter det ID der er tættest til en int


            //// Listen er af int[] som er: [TimeSlotID, Day]
            //List<int[]> passedSlots = new List<int[]>();
                     

            for (int day = 0; day <= currentDayAsInt; day++) // Kør fra mandag til idag
            {
                foreach (var timeSlot in CurrentTimeSlots) // For hver timeslot
                {
                    // hvis dagen er mindre end idag, ELLER dagen ER idag OG timeslottet er mindre end eller lig closestTimeSlot
                    if (day < currentDayAsInt || (day == currentDayAsInt && timeSlot.TimeSlotID < closestTimeSlotID))
                    {
                        // Sæt til grå
                        timeSlot.SetPassedAsGrey(day);
                    }
                }
            }
            OnPropertyChanged(nameof(CurrentTimeSlots)); 

        }

        private void SetTimeSlotsAsYours()
        {
            int bookingTimeSlotID = MainBookingViewModel.CurrentBooking.TimeSlot.TimeSlotID; //2;//User.Booking.TimeSlotID;
            int bookingDate = (int)MainBookingViewModel.CurrentBooking.Date.DayOfWeek-1;//4;//(int)User.Booking.Date; //date parsed til int

            //CurrentTimeSlots.Find(x => x.TimeSlotID == bookingTimeSlotID).SetYoursAsOrange(bookingDate);
            var timeSlot = CurrentTimeSlots.FirstOrDefault(x => x.TimeSlotID == bookingTimeSlotID);
            if (timeSlot != null)
            {
                timeSlot.SetYoursAsOrange(bookingDate);
            }

        }


        private ObservableCollection<string> _currentDays;
        public ObservableCollection<string> CurrentDays
        {
            get => _currentDays;
            set
            {
                _currentDays = value;
                OnPropertyChanged();
            }
        }

        private void GetCurrentDays(DateOnly monday)
        {
            List<string> days = new List<string>();
            
            for (int i = 0; i < 5; i++)
                days.Add(monday.AddDays(i).ToString("dd/MM/yyyy")); // hvorfor formaterer den / som . ??

            CurrentDays = new ObservableCollection<string>(days);
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

        public bool HasBooking => MainBookingViewModel.CurrentBooking != null ? true : false;

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

            SelectedDay = null;
            SelectedTimeSlot = null;
            WeekNr = DateUtils.GetIso8601WeekOfYear(MondayOfWeek);
            GetCurrentTimeSlots(MondayOfWeek) ;
            GetCurrentDays(MondayOfWeek) ;

            if (DateOnly.FromDateTime(DateTime.Today) >= MondayOfWeek)
            {
                SetTimeSlotsAsPassed();
            }

            if (MainBookingViewModel.CurrentBooking != null)
            {
                if (MainBookingViewModel.CurrentBooking.Date >= MondayOfWeek && MainBookingViewModel.CurrentBooking.Date <= MondayOfWeek.AddDays(5))
                {
                    SetTimeSlotsAsYours();
                }
            };
        }

        private bool NotLessThanCurrentWeek()
        {
            DateTime currentDay = DateTime.Today;
            return MondayOfWeek > currentDay.StartOfWeek();
        }

        private bool NotMoreThanMonthInFuture()
        {
            DateOnly limit = DateOnly.FromDateTime(DateTime.Today);
            return MondayOfWeek <= (limit.AddDays(30));
        }
		public int RowHeight => (CurrentTimeSlots.Count * 50) + 2;
    }

    public class TimeSlotViewModel
    {
        public readonly int TimeSlotID;

        public string Time { get; set; }
        public TimeOnly StartTime { get; set; }

        public int MondayFull { get; set; }
        public int TuesdayFull { get; set; }
        public int WednesdayFull { get; set; }
        public int ThursdayFull { get; set; }
        public int FridayFull { get; set; }

        public TimeSlotViewModel(TimeSlot timeSlot)
        {
            TimeSlotID = timeSlot.TimeSlotID;

            Time = $"{timeSlot.TimeSlotStart.ToString("HH:mm")} - {timeSlot.TimeSlotEnd.ToString("HH:mm")}";
            StartTime = timeSlot.TimeSlotStart;

            MondayFull = 0;
            TuesdayFull = 0;
            WednesdayFull = 0;
            ThursdayFull = 0;
            FridayFull = 0;
        }

        public void SetDayFull(int day)
        {
            switch (day)
            {
                case 0:
                    MondayFull = 1;
                    break;
                case 1:
                    TuesdayFull = 1;
                    break;
                case 2:
                    WednesdayFull = 1;
                    break;
                case 3:
                    ThursdayFull = 1;
                    break;
                case 4:
                    FridayFull = 1;
                    break;
            }
        }

        public void SetPassedAsGrey(int day)
        {
            switch (day)
            {
                case 0:
                    MondayFull = 2;
                    break;
                case 1:
                    TuesdayFull = 2;
                    break;
                case 2:
                    WednesdayFull = 2;
                    break;
                case 3:
                    ThursdayFull = 2;
                    break;
                case 4:
                    FridayFull = 2;
                    break;
            }
        }

        public void SetYoursAsOrange(int day)
        {
            switch (day)
            {
                case 0:
                    MondayFull = 3;
                    break;
                case 1:
                    TuesdayFull = 3;
                    break;
                case 2:
                    WednesdayFull = 3;
                    break;
                case 3:
                    ThursdayFull = 3;
                    break;
                case 4:
                    FridayFull = 3;
                    break;
            }
        }
    }
}
