using El_Booking.ViewModel.BookingVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace El_Booking.View.Converters
{

    public class TimeSlotLabelHelper : BaseMultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
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
    }
}
