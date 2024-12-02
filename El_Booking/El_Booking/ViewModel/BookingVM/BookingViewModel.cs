﻿using El_Booking.Model;
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

namespace El_Booking.ViewModel.BookingVM
{
    public class BookingViewModel : BaseViewModel
    {

        public ICommand MakeBookingCommand { get; }

        public DateOnly MondayOfWeek { get; set; } // Dato for mandagen i den valgte uge.
        private readonly DateOnly _startingDate;
        private readonly Storer _storer;

		public BookingViewModel(Storer storer)
		{
            _startingDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            if (_startingDate.DayOfWeek == DayOfWeek.Saturday)
                _startingDate = _startingDate.AddDays(2);
            else if (_startingDate.DayOfWeek == DayOfWeek.Sunday)
                _startingDate = _startingDate.AddDays(1);

            _storer = storer;

            MakeBookingCommand = new MakeBookingCommand(this, storer);

			WeekNr = DateUtils.GetIso8601WeekOfYear(_startingDate);
			MondayOfWeek = _startingDate.StartOfWeek();

			GetCurrentTimeSlots(MondayOfWeek);
            GetCurrentDays(MondayOfWeek);

            var CurrentApp = Application.Current as App;

            HasBooking = CurrentApp.CurrentUser.Booking is null ? false : true;
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
                days.Add(monday.AddDays(i).ToString("dd/MM")); // hvorfor formaterer den / som . ??

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

        private bool _hasBooking;
        public bool HasBooking 
        {   get => _hasBooking;
            set
            {   
                _hasBooking = value;
                OnPropertyChanged();
            }
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

            SelectedDay = null;
            SelectedTimeSlot = null;
            WeekNr = DateUtils.GetIso8601WeekOfYear(MondayOfWeek);
            GetCurrentTimeSlots(MondayOfWeek) ;
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
    }

    public class TimeSlotViewModel
    {
        public readonly int TimeSlotID;

        public string MondayStart { get; set; }
        public string TuesdayStart { get; set; }
        public string WednesdayStart { get; set; }
        public string ThursdayStart { get; set; }
        public string FridayStart { get; set; }

        public bool MondayFull { get; set; }
        public bool TuesdayFull { get; set; }
        public bool WednesdayFull { get; set; }
        public bool ThursdayFull { get; set; }
        public bool FridayFull { get; set; }

        public TimeSlotViewModel(TimeSlot timeSlot)
        {
            TimeSlotID = timeSlot.TimeSlotID;

            string startTime = timeSlot.TimeSlotStart.ToString("HH-mm");
            MondayStart = startTime;
            TuesdayStart = startTime;
            WednesdayStart = startTime;
            ThursdayStart = startTime;
            FridayStart = startTime;

            MondayFull = false;
            TuesdayFull = false;
            WednesdayFull = false;
            ThursdayFull = false;
            FridayFull = false;
        }

        public void SetDayFull(int day)
        {
            switch (day)
            {
                case 0:
                    MondayFull = true;
                    break;
                case 1:
                    TuesdayFull = true;
                    break;
                case 2:
                    WednesdayFull = true;
                    break;
                case 3:
                    ThursdayFull = true;
                    break;
                case 4:
                    FridayFull = true;
                    break;
            }
        }
    }
}
