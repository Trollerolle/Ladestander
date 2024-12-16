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
        public ICommand ChangeWeekBackwardCommand { get; }
		public ICommand ChangeWeekForwardCommand { get; }

		public DateOnly MondayOfWeek { get; set; } // Dato for mandagen i den valgte uge.
        internal readonly DateOnly _startingDate;
        private readonly Storer _storer;

        public MainBookingViewModel MainBookingViewModel { get; }

		public BookingViewModel(Storer storer, MainBookingViewModel mainBookingViewModel)
		{
            _startingDate = DateOnly.FromDateTime(DateTime.Today);
            if (_startingDate.DayOfWeek == DayOfWeek.Saturday)
                _startingDate = _startingDate.AddDays(2);
            else if (_startingDate.DayOfWeek == DayOfWeek.Sunday)
                _startingDate = _startingDate.AddDays(1);
            MainBookingViewModel = mainBookingViewModel;
            _storer = storer;

            MakeBookingCommand = new MakeBookingCommand(this, storer);
            ChangeWeekBackwardCommand = new ChangeWeekBackwardCommand(this);
            ChangeWeekForwardCommand = new ChangeWeekForwardCommand(this);

			WeekNr = DateUtils.GetIso8601WeekOfYear(_startingDate);
			MondayOfWeek = _startingDate.StartOfWeek();

			GetCurrentTimeSlots(MondayOfWeek);
            GetCurrentDays(MondayOfWeek);

            SetTimeSlotsAsPassed();

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

        internal void GetCurrentTimeSlots(DateOnly monday)
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


		// Prop til SetTimeslotsAsPassed som bruges til at gemme den "nærmeste" timeslot (Hvis klokken er 10:30 får man time slot fra 9:00-12:00)
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

        internal void SetTimeSlotsAsPassed()
        {

            int currentDayAsInt = (int)DateTime.Now.DayOfWeek-1;

            var Tid = DateTimeOffset.Now;

            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now); //Skal ændres tilbage til idag

            NearestTimeSlot = CurrentTimeSlots
            .Where(slot => slot.StartTime <= currentTime) // Kun time slots som er <= current time
            .OrderByDescending(slot => slot.StartTime)    // Sorter efter højeste først. (Hvis du kigger klokken 13, kan det være 6 12 eller 9, IKKE 15 som bliver sorteret til 12, 9, 6)
            .FirstOrDefault();                            // Vælg første, ( 12 )

            int? closestTimeSlotID;
            if (NearestTimeSlot.EndTime < currentTime) //Hvis klokken er efter slutningen på sidste timeslot
            {
                closestTimeSlotID = _nearestTimeSlot?.TimeSlotID + 1; //så skal den nærmest være +1 for at gøre den grå
            }
            else
            { 
                closestTimeSlotID = _nearestTimeSlot?.TimeSlotID; // Sætter det ID der er tættest til en int
            }






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

        internal void SetTimeSlotsAsYours()
        {
            int bookingTimeSlotID = MainBookingViewModel.CurrentBooking.TimeSlot.TimeSlotID; 
            int bookingDate = (int)MainBookingViewModel.CurrentBooking.Date.DayOfWeek-1;

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

        internal void GetCurrentDays(DateOnly monday)
        {
            List<string> days = new List<string>();
            
            for (int i = 0; i < 5; i++)
                days.Add(monday.AddDays(i).ToString("dd/MM/yyyy")); // format vises ud fra OS culture.

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

        // Dynamisk højde Binding
		public int RowHeight => (CurrentTimeSlots.Count * 50) + 2;
    }

}
