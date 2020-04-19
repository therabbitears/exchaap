using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Xaminals.Infra.Formatters
{
    public class ListToCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList valueAsList)
                return valueAsList.Count;

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
