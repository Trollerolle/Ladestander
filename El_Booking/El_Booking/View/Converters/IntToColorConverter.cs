using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace El_Booking.View.Converters
{

    public class IntToColorConverter : IValueConverter
    {
         public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int full)
            {
                switch (value)
                {

                    case 1:
                        return new SolidColorBrush(Colors.Red);
                        break;

                    case 2:
                        return new SolidColorBrush(Colors.Gray);
                        break;

                    case 3:
                        return new SolidColorBrush(Colors.Orange);
                        break;
                    default:
                        return new SolidColorBrush(Colors.Green);
                        break;

                }


                //return full ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

