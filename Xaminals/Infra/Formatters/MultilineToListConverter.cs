using System;
using Xamarin.Forms;

namespace Xaminals.Infra.Formatters
{
    public class MultilineToListConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            return value.ToString().Split(
                                              new[] { "\r\n", "\r", "\n" },
                                              StringSplitOptions.None
                                          );
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Join(Environment.NewLine, value);
        }

        #endregion
    }
}
