using MikuExpansion.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MikuExpansion.Helpers
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = System.Convert.ToBoolean(value);

            if (parameter != null)
                boolValue = !boolValue;

            return boolValue.ToVisibility();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value.Equals(System.Windows.Visibility.Visible);
    }
}
