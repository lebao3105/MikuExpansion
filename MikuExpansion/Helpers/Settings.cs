using MikuExpansion.Extensions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#if SILVERLIGHT
using System.Windows;
using System.IO.IsolatedStorage;
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.Storage;
#endif

namespace MikuExpansion.Helpers
{
#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP
    public sealed class SettingEntry<T> : INotifyPropertyChanged
    {
#if SILVERLIGHT
        private static IsolatedStorageSettings localSettings
                    = IsolatedStorageSettings.ApplicationSettings;
#else
        private static ApplicationDataContainer localSettings
                    = ApplicationData.Current.LocalSettings;
#endif
        /// <summary>
        /// The event that gets raised when a setting, in YOUR
        /// choice, gets changed.
        /// <seealso cref="SetSetting(string, object, bool, string)"/>
        /// <seealso cref="SetSetting{T}(string, T, bool, string)"/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool notifyListeners;

        public T Value
        {
            get
            {
#if !SILVERLIGHT
                return Exists(key) ? (T)localSettings.Values[key] : default(T);
#else
                return Exists(key) ? (T)localSettings[key] : default(T);
#endif
            }
            set
            {
                if (value.Equals(GetSetting<T>(key)))
                    return;

#if SILVERLIGHT
                localSettings[key] = value;
                localSettings.Save();
#else
                localSettings.Values[key] = value;
#endif

                if (notifyListeners)
                    OnPropertyChanged(propertyName.HasContent() ? propertyName : key);
            }
        }

        /// <summary>
        /// Checks if the specified key exists in the preference database.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if there is a setting with the specified key, otherwise False.</returns>
        public bool Exists(string key) =>
#if SILVERLIGHT
            localSettings.Contains(key);
#else
            localSettings.Values.ContainsKey(key);
#endif
    }
#endif
}