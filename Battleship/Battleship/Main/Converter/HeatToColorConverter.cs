using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship.Main.Converter
{
    public class HeatToColorConverter : IValueConverter
    {
        private readonly BrushConverter _bc = new BrushConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var heat = (float)value;

            if(heat < 0.10f)
                return (SolidColorBrush)_bc.ConvertFrom("#0000ff");
            if (heat < 0.2f)
                return (SolidColorBrush)_bc.ConvertFrom("#0D00F2");
            if (heat < 0.3f)
                return (SolidColorBrush)_bc.ConvertFrom("#1900E6");
            if (heat < 0.4f)
                return (SolidColorBrush)_bc.ConvertFrom("#2600D9");
            if (heat < 0.5f)
                return (SolidColorBrush)_bc.ConvertFrom("#3300CC");
            if (heat < 0.6f)
                return (SolidColorBrush)_bc.ConvertFrom("#4000BF");
            if (heat < 0.7f)
                return (SolidColorBrush)_bc.ConvertFrom("#4D00B2");
            if (heat < 0.8f)
                return (SolidColorBrush)_bc.ConvertFrom("#5900A6");
            if (heat < 0.9f)
                return (SolidColorBrush)_bc.ConvertFrom("#73008C");
            if (heat < 1.0f)
                return (SolidColorBrush)_bc.ConvertFrom("#800080");
            if (heat < 1.1f)
                return (SolidColorBrush)_bc.ConvertFrom("#8C0073");
            if (heat < 1.2f)
                return (SolidColorBrush)_bc.ConvertFrom("#990066");
            if (heat < 1.3f)
                return (SolidColorBrush)_bc.ConvertFrom("#A60059");
            if (heat < 1.4f)
                return (SolidColorBrush)_bc.ConvertFrom("#B2004C");
            if (heat < 1.5f)
                return (SolidColorBrush)_bc.ConvertFrom("#BF0040");
            if (heat < 1.6f)
                return (SolidColorBrush)_bc.ConvertFrom("#CC0033");
            if (heat < 1.7f)
                return (SolidColorBrush)_bc.ConvertFrom("#D90026");
            if (heat < 1.8f)
                return (SolidColorBrush)_bc.ConvertFrom("#E6001A");
            if (heat < 1.8f)
                return (SolidColorBrush)_bc.ConvertFrom("#F2000D");
            
            return (SolidColorBrush)_bc.ConvertFrom("#FF0000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
