using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Battleship.TestingWindow
{
    public class GameStateToReadyButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GameState)
            {
                switch ((GameState)value)
                {
                    case GameState.AIsTurn:
                    case GameState.HumansTurn:
                    case GameState.Waiting:
                        return Visibility.Hidden;
                }
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
