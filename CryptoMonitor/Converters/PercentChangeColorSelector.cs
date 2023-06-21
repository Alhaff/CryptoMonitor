using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CryptoMonitor.Converters
{
    public class PercentChangeColorSelector : IValueConverter
    {
        private readonly Color _decreasePercent = Colors.Red;
        private readonly Color _increasePercent = Colors.Green;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                if (value == null) return new SolidColorBrush(_decreasePercent);
                return (double)value >= 0 ? new SolidColorBrush(_increasePercent) : new SolidColorBrush(_decreasePercent);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
