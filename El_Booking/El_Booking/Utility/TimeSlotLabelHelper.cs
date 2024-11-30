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

            int? selectedRow = (int?)values[2];
            int ? selectedColumn = (int?)values[3];

            if (values[0] is int rowIndex && values[1] is int columIndex && selectedColumn is not null)
            {
                if (rowIndex == selectedRow && columIndex == selectedColumn)
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
