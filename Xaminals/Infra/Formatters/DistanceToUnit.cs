using System;
using Xamarin.Forms;

namespace Xaminals.Infra.Formatters
{
    public class DistanceToUnit : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var val = System.Convert.ToDouble(value);
            if (App.Context.SettingsModel.UnitOfMeasurement == (int)App.UnitOfMeasurement.Miles)
                return string.Format("{0:#.#}mi", (val / 1609));
            
            return string.Format("{0:#.#}km", (val / 1000));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Join(Environment.NewLine, value);
        }

        #endregion
    }
}
