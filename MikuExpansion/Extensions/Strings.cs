#if SILVERLIGHT
using System.Windows;
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.UI.Xaml;
#endif

namespace MikuExpansion.Extensions
{
    public static class Strings
    {
        /// <summary>
        /// Returns true if <paramref name="self"/> is not null, empty or only contains whitespaces.
        /// </summary>
        public static bool HasContent(this string self) => !string.IsNullOrWhiteSpace(self);

#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP
        public static T GetSetting<T>(this string self) => Helpers.SettingEntry<T>.GetSetting<T>(self);

        public static T GetAppResource<T>(this string self)
        {
#if SILVERLIGHT
            if (Application.Current.Resources.Contains(self))
#else
            if (Application.Current.Resources.ContainsKey(self))
#endif
                return (T)Application.Current.Resources[self];
            return default(T);
        }
#endif
        }
}
