using System.Windows;

namespace MikuExpansion.Extensions
{
    public static class Strings
    {
        /// <summary>
        /// Returns true if <paramref name="self"/> is not null, empty or only contains whitespaces.
        /// </summary>
        public static bool HasContent(this string self) => !string.IsNullOrWhiteSpace(self);

        public static T GetSetting<T>(this string self) => Helpers.Settings.Instance.GetSetting<T>(self);

        public static T GetAppResource<T>(this string self)
        {
            if (Application.Current.Resources.Contains(self))
                return (T)Application.Current.Resources[self];
            return default(T);
        }
    }
}
