using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CryptoMonitor.Converters
{
    public class BigMoneyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double)
            {
                var num = value as double?;
                if (num==null) return Binding.DoNothing;
                var str = (num > 1E10 || num < .01) ? num?.ToString("G6") : num?.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("en-us")).Substring(1);
                return str;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
