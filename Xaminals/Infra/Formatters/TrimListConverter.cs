using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace exchaup.Infra.Formatters
{
    public class TrimListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keep = System.Convert.ToInt32(parameter);
            if (value != null && value is IList values && values.Count > keep)
            {
                List<object> lst = new List<object>();
                for (int i = 0; i < keep - 1; i++)
                {
                    lst.Add(values[i]);
                }

                return lst;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
