using System;
using System.Globalization;
using Xamarin.Forms;
using Xaminals;

namespace Loffers.Infra.Formatters
{
    public class MessageBubbleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool _converted)
            {
                if (_converted)
                    return App.BaseThemeSecondaryColor;
            }

            return App.BaseThemeColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return App.BaseThemeColor;
        }
    }
}
