using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xaminals.Infra.Formatters
{
    public class DatetimeToStringConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var datetime = (DateTime)value;
            ////put your custom formatting here
            //return datetime.ToLocalTime().ToString("g");
            if (datetime.Date == DateTime.Now.Date)
                return string.Format("{0}", datetime.ToString("h:mm tt", CultureInfo.InvariantCulture));

            if (datetime.Date == DateTime.Now.Date.AddDays(1))
                return string.Format("Tom. {0}", datetime.ToString("h:mm tt", CultureInfo.InvariantCulture));

            if (datetime.Date.Month == DateTime.Now.Month)
            {
                var suffix = "th";
                switch (datetime.Date.Day)
                {
                    case 1:
                    case 21:
                    case 31:
                        suffix = "st";
                        break;
                    case 2:
                    case 22:
                        suffix = "nd";
                        break;
                    case 3:
                    case 23:
                        suffix = "rd";
                        break;
                }

                return string.Format("Till {0}{1} {2} ", datetime.Date.Day, suffix, datetime.ToString("h:mm tt", CultureInfo.InvariantCulture));
            }

            return datetime.ToString("d MMM hh:mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
