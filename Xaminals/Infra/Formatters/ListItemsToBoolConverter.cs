using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace exchaup.Infra.Formatters
{
    public class ListItemsToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IList valueAsList)
                return valueAsList.Count == 0;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
