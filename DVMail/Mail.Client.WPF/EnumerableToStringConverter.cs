using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Mail.Client.WPF
{
    public class EnumerableToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            IEnumerable addresseeName = value as IEnumerable;
            if(addresseeName == null)
                throw new ArgumentException("Argument has a wrong type. Expected type IEnumerable.", nameof(value));
            StringBuilder builder = new StringBuilder();
            foreach (var addressee in addresseeName)
            {
                builder.Append($"{addressee.ToString()}, ");
            }
            return builder.ToString(0, builder.Length - 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
