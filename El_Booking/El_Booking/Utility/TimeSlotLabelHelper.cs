using El_Booking.ViewModel.BookingVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace El_Booking.Utility
{

    public class TimeSlotLabelHelper : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int rowIndex && values[1] is IList<string> timeSlotValues)
            {
                if (rowIndex >= 0 && rowIndex < timeSlotValues.Count)
                {
                    return timeSlotValues[rowIndex];
                }
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class SelectedCellHelper : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            int columnIndex = (int)values[0];

            int? selectedRow = (int?)values[1];
            int ? selectedColumn = (int?)values[2];
            
            TimeSlotViewModel row = (TimeSlotViewModel)values[3];

            int dayIsFull = 0;
            switch (columnIndex)
            {
                case 0:
                    dayIsFull = row.MondayFull;
                    break;
                case 1:
                    dayIsFull = row.TuesdayFull;
                    break;
                case 2:
                    dayIsFull = row.WednesdayFull;
                    break;
                case 3:
                    dayIsFull = row.ThursdayFull;
                    break;
                case 4:
                    dayIsFull = row.FridayFull;
                    break;
            }

            if (dayIsFull == 0 && selectedColumn is not null)
            {

                if (row.TimeSlotID -1 == selectedRow && columnIndex == selectedColumn)
                {
                    return Visibility.Visible;
                }

            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
