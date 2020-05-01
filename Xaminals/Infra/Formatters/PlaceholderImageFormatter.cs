using System;
using System.Globalization;
using Xamarin.Forms;

namespace exchaup.Infra.Formatters
{
    /// <summary>
    /// PlaceholderImageFormatter
    /// </summary>
    public class PlaceholderImageFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ImageSource.FromUri(new Uri(value.ToString()));

            return ImageSource.FromFile(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
