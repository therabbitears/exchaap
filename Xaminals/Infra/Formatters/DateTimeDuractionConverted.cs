using System;
using System.Globalization;
using Xamarin.Forms;

namespace exchaup.Infra.Formatters
{
    public class DateTimeDuractionConverted : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime converted)
            {
                if (DateTime.UtcNow.Subtract(converted).TotalSeconds < 60)
                    return "Just now";
                else if (DateTime.UtcNow.Subtract(converted).TotalMinutes < 60)
                    return string.Format("{0} min(s) ago", Math.Round(DateTime.UtcNow.Subtract(converted).TotalMinutes));
                else if (DateTime.UtcNow.Date.Subtract(converted.Date).TotalHours < 24)
                    return string.Format("{0} hrs(s) ago", Math.Round(DateTime.UtcNow.Subtract(converted).TotalHours));
                else if (DateTime.UtcNow.Date.Subtract(converted).TotalDays < 7)
                    return string.Format("{0} day(s) ago", Math.Round(DateTime.UtcNow.Subtract(converted).TotalDays));

                return converted.ToString("dd, MMM yy");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
