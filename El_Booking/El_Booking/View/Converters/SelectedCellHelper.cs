using El_Booking.ViewModel.BookingVM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace El_Booking.View.Converters
{
    public class SelectedCellHelper : BaseMultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            int columnIndex = (int)values[0];

            int? selectedRow = (int?)values[1];
            int? selectedColumn = (int?)values[2];

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

                if (row.TimeSlotID - 1 == selectedRow && columnIndex == selectedColumn)
                {
                    return Visibility.Visible;
                }

            }

            return Visibility.Collapsed;
        }
    }
}
