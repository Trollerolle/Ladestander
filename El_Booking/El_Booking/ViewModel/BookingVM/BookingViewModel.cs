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

            MainBookingViewModel.PropertyChanged += OnMainBookingViewModelPropertyChanged;

        }

        private void OnMainBookingViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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

        private void SetTimeSlotsAsPassed()
        {

            //CurrentDAy;
            //CurrentTime //9:00 Kigger 9:10 

            //timeSlots.Find(x => x.TimeSlotID == timeSlotID).SetDayFull(day);

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

        public int RowHeight { get; set; } = 252;
    }

    public class TimeSlotViewModel
    {
        public readonly int TimeSlotID;

        public string StartTime { get; set; }

        public int MondayFull { get; set; }
        public int TuesdayFull { get; set; }
        public int WednesdayFull { get; set; }
        public int ThursdayFull { get; set; }
        public int FridayFull { get; set; }

        public TimeSlotViewModel(TimeSlot timeSlot)
        {
            TimeSlotID = timeSlot.TimeSlotID;

            StartTime = timeSlot.TimeSlotStart.ToString("HH:mm");

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
