using System;
using System.Globalization;
using Xamarin.Forms;

namespace exchaup.Infra.Formatters
{
    public class MessageBubbleDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var option = LayoutOptions.StartAndExpand;
            if (value is bool _converted)
            {
                if (_converted)
                    option =  LayoutOptions.EndAndExpand;
            }

            return option;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return LayoutOptions.Start;
        }
    }
}
