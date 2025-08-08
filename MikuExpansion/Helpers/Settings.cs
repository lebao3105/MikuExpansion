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
    /// <summary>
    /// Class for your application settings.
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
#if SILVERLIGHT
        private static IsolatedStorageSettings localSettings
                    = IsolatedStorageSettings.ApplicationSettings;
#else
        private static ApplicationDataContainer localSettings =
                    ApplicationData.Current.LocalSettings;
#endif

        public static Settings Instance { get; private set; }

        /// <summary>
        /// The event that gets raised when a setting, in YOUR
        /// choice, gets changed.
        /// <seealso cref="SetSetting(string, object, bool, string)"/>
        /// <seealso cref="SetSetting{T}(string, T, bool, string)"/>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor. You will want to use <see cref="Instance"/> afterward.
        /// Only need to be called once in the lifecycle.
        /// </summary>
        public Settings()
        {
            Instance = this;
        }

        /// <summary>
        /// Convenient access to a application preference.
        /// Can be used to set the preference value too.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>An <see cref="object"/>.</returns>
        public object this[string key]
        {
            get { return GetSetting<object>(key); }
            set { SetSetting(key, value); }
        }

        /// <summary>
        /// Sets and saves a preference. Do nothing if
        /// <paramref name="value"/> .Equals()
        /// the current value known on disk.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="notifyListeners">
        /// If there are watches for the setting, this will notify them.
        /// </param>
        /// <param name="propertyName">Used to notify listeners. If not
        /// specified, <paramref name="key"/> will be used.
        /// </param>
        public void SetSetting<T>(
            string key, T value, bool notifyListeners = false,
            [CallerMemberName] string propertyName = null)
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

        /// <summary>
        /// Sets and saves a preference. Do nothing if
        /// <paramref name="value"/> .Equals()
        /// the current value known on disk.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="notifyListeners">
        /// If there are watches for the setting, this will notify them.
        /// </param>
        /// <param name="propertyName">Used to notify listeners. If not
        /// specified, <paramref name="key"/> will be used.
        /// </param>
        public void SetSetting(string key, object value, bool notifyListeners = false,
            [CallerMemberName] string propertyName = null)
            => SetSetting<object>(key, value, notifyListeners, propertyName);

        public T GetSetting<T>(string key)
        {
            try
            {
#if !SILVERLIGHT
                return Exists(key) ? (T)localSettings.Values[key] : default(T);
#else
                return Exists(key) ? (T)localSettings[key] : default(T);
#endif
            }
            catch
            {
                return default(T);
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

        private void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
#endif
            }