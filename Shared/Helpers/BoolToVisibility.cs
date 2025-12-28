#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP
using System;
using System.Globalization;
using MikuExpansion.Extensions;

#if SILVERLIGHT
using System.Windows;
using System.Windows.Data;
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace MikuExpansion.Helpers
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
#if SILVERLIGHT
        public object Convert(object value, Type targetType, object parameter, CultureInfo _)
#else
        public object Convert(object value, Type targetType, object parameter, string _)
#endif
        {
            var boolValue = System.Convert.ToBoolean(value);

            if (parameter != null)
                boolValue = !boolValue;

            return boolValue.ToVisibility();
        }

#if SILVERLIGHT
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#else
        public object ConvertBack(object value, Type targetType, object parameter, string _)
#endif
            => value.Equals(Visibility.Visible);
    }
}
#endif
