using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship.TestingWindow
{
    public class CellCodeToFillConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var cellCode = (int) value;
                switch (cellCode)
                {
                    case 0:
                        return new SolidColorBrush(Colors.Transparent);
                    case 106:
                        return new SolidColorBrush(Colors.Blue);
                }
                return new SolidColorBrush(Colors.Red);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
