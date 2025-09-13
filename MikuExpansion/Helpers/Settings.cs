#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP
using System.ComponentModel;

#if SILVERLIGHT
using System.Windows;
using System.IO.IsolatedStorage;
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.Storage;
using Windows.UI.Xaml.Data;
#endif

namespace MikuExpansion.Helpers
{
    /// <summary>
    /// A application settings entry.
    /// 
    /// It can notify changed events (NOT from outside this instance of
    /// <see cref="SettingEntry{T}"/> itself), can be used for a direct
    /// data binding, can check if the entry exists with <see cref="Exists(string)"/>.
    /// 
    /// You can make rely-on mechanism by using the changed event;)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Bindable]
    public sealed class SettingEntry<T> : INotifyPropertyChanged
    {

#if SILVERLIGHT
        internal static IsolatedStorageSettings localSettings
                    = IsolatedStorageSettings.ApplicationSettings;
#else
        internal static ApplicationDataContainer localSettings
                    = ApplicationData.Current.LocalSettings;
#endif

        /// <summary>
        /// The event that gets raised when <see cref="Value"/> gets changed.
        /// </summary>
        /// <seealso cref="SetSetting(string, object, bool, string)"/>
        /// <seealso cref="SetSetting{T}(string, T, bool, string)"/>
        public event PropertyChangedEventHandler PropertyChanged;

        public bool notifyListeners;

        public SetOnce<string> Key { get; set; }

        public string Name { get; set; }

        public T Value
        {
            get
            {

            }
            set
            {
                if (value.Equals(Value))
                    return;

#if SILVERLIGHT
                localSettings[Key] = value;
                localSettings.Save();
#else
                localSettings.Values[Key] = value;
#endif

                if (notifyListeners)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Key));
            }
        }

        /// <summary>
        /// Checks if the specified key exists in the preference database.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if there is a setting with the specified key, otherwise False.</returns>
        public static bool Exists(string key) =>
#if SILVERLIGHT
            localSettings.Contains(key);
#else
            localSettings.Values.ContainsKey(key);
#endif

        public static T GetSetting<T>(string key) =>
#if !SILVERLIGHT
                Exists(key) ? (T)localSettings.Values[key] : default(T);
#else
                Exists(key) ? (T)localSettings[key] : default(T);
#endif
    }
}
#endif