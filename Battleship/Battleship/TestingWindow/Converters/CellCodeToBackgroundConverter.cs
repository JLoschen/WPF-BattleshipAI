using System;
using System.Globalization;
using System.Windows.Data;

namespace Battleship.TestingWindow
{
    public class CellCodeToBackgroundConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (value is int)
            //{
            //    var cellCode = (int)value;
            //    switch (cellCode)
            //    {
            //        case 0:
            //            return new SolidColorBrush(Colors.Transparent);
            //        case 106:
            //            return new SolidColorBrush(Colors.Blue);
            //    }
            //    return new SolidColorBrush(Colors.Red);
            //}

            //return new SolidColorBrush(Colors.Transparent);
            return (int) value == 0;
        }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
