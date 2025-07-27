using MikuExpansion.Extensions;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;

namespace MikuExpansion.Helpers
{
    /// <summary>
    /// Class for your application settings.
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        private static IsolatedStorageSettings localSettings
                    = IsolatedStorageSettings.ApplicationSettings;

        public static Settings Instance;

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

            if (notifyListeners)
                OnPropertyChanged(propertyName.HasContent() ? propertyName : key);

            localSettings[key] = value;
            localSettings.Save();
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
            => localSettings.Contains(key) ? (T)localSettings[key] : default(T);

        /// <summary>
        /// Checks if the specified key exists in the preference database.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if there is a setting with the specified key, otherwise False.</returns>
        public bool Exists(string key) => localSettings.Contains(key);

        private void OnPropertyChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}