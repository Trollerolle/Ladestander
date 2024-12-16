﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using El_Booking.Utility;
using El_Booking.ViewModel;
using El_Booking.ViewModel.BookingVM;

namespace El_Booking.Commands
{
	public class ChangeWeekBackwardCommand : CommandBase
	{
		private BookingViewModel _bookingViewModel { get; }
		DateTime currentDay = DateTime.Today;
		public ChangeWeekBackwardCommand(BookingViewModel bookingViewModel)
		{
			_bookingViewModel = bookingViewModel;

			_bookingViewModel.PropertyChanged += OnViewModelPropertyChanged;
		}

		private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCanExecuteChanged();
		}
		public override void Execute(object? parameter)
		{
			_bookingViewModel.MondayOfWeek = _bookingViewModel.MondayOfWeek.AddDays(7 * -1);

			_bookingViewModel.SelectedDay = null;
			_bookingViewModel.SelectedTimeSlot = null;
			_bookingViewModel.WeekNr = DateUtils.GetIso8601WeekOfYear(_bookingViewModel.MondayOfWeek);
			_bookingViewModel.GetCurrentTimeSlots(_bookingViewModel.MondayOfWeek);
			_bookingViewModel.GetCurrentDays(_bookingViewModel.MondayOfWeek);

			if (DateOnly.FromDateTime(DateTime.Today) >= _bookingViewModel.MondayOfWeek)
			{
				_bookingViewModel.SetTimeSlotsAsPassed();
			}

			if (_bookingViewModel.MainBookingViewModel.CurrentBooking != null)
			{
				if (_bookingViewModel.MainBookingViewModel.CurrentBooking.Date >= _bookingViewModel.MondayOfWeek && _bookingViewModel.MainBookingViewModel.CurrentBooking.Date <= _bookingViewModel.MondayOfWeek.AddDays(5))
				{
					_bookingViewModel.SetTimeSlotsAsYours();
				}
			}

		}
		public override bool CanExecute(object? parameter)
		{
			if (currentDay.DayOfWeek == DayOfWeek.Saturday)
				currentDay = currentDay.AddDays(2);
			else if (currentDay.DayOfWeek == DayOfWeek.Sunday)
				currentDay = currentDay.AddDays(1);
			if 
			(
			_bookingViewModel.MondayOfWeek > currentDay.StartOfWeek()
			)
			return true;
			
			return false;
		}
	}
}