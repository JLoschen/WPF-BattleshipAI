using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Battleship.Core;

namespace Battleship.TestingWindow
{
    public class CellCodeToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var cellCode = (int)value;
                switch (cellCode)
                {
                    case Constants.Miss:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                    //case Constants.HitPatrolBoat:
                        return Visibility.Visible;
                }
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
