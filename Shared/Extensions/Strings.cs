#if SILVERLIGHT
using System.Windows;
#elif WINDOWS_RT
using Windows.UI.Xaml;
#endif

using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace MikuExpansion.Extensions
{
    public static class Strings
    {
        /// <summary>
        /// Returns true if <paramref name="self"/> is not null, empty or only contains whitespaces.
        /// </summary>
        public static bool HasContent(this string self) => !string.IsNullOrWhiteSpace(self);

#if WINDOWS_RT || SILVERLIGHT

        public static T GetSetting<T>(this string self) => Helpers.SettingEntry<T>.GetSetting<T>(self);

        public static T GetAppResource<T>(this string self)
            => self.IsAppResource() ? (T)Application.Current.Resources[self] : default(T);

        public static bool IsAppResource(this string self)
            =>
#if SILVERLIGHT
                Application.Current.Resources.Contains(self);
#else
                Application.Current.Resources.ContainsKey(self);
#endif

#endif

        public static Uri ToRelativeUri(this string self) => new Uri(self, UriKind.Relative);

        public static string ToJSONString(this object self)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(self.GetType())
                    .WriteObject(ms, self);
                return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
            }
        }

        public static T FromJSONString<T>(this string self)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(self)))
            {
                return (T)
                    new DataContractJsonSerializer(typeof(T))
                        .ReadObject(ms);
            }
        }
    }
}
