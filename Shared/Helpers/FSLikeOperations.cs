using System;

#if SILVERLIGHT
using System.IO.IsolatedStorage;
#elif WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_APP
using Windows.Storage;
#endif

namespace MikuExpansion.Helpers
{
    /// <summary>
    /// File-system-like operations.
    /// This includes:
    /// <para>Move between locations (or directories in file systems)</para>
    /// <para>Do stuff while staying in a specific location (file system directory)</para>
    /// <para>Specify full path anytime</para>
    /// This is very useful in registry editing, especially when you need to handle a lot
    /// of values inside a certain key. It saves you a lot time copy-pasting paths.
    /// </summary>
    public abstract class FSLikeOperations
    {
        /// <summary>
        /// Something like C:\ or D:\, or HKEY_LOCAL_MACHINE.
        /// </summary>
        public NotNullable<string> CurrentRoot { get; set; }

        /// <summary>
        /// The path that goes after <see cref="CurrentRoot"/>.
        /// </summary>
        public string CurrentPath { get; set; }

        public void GoTo(NotNullable<string> root, string path)
        {
            CurrentRoot = root;
            CurrentPath = path;
        }

        public bool SetSomeThing<T>(NotNullable<string> target, T value)
            => SetSomeThing(CurrentRoot, CurrentPath, target, value);
        public bool SetSomeThing<T>(string path, NotNullable<string> target, T value)
            => SetSomeThing(CurrentRoot, path, target, value);
        public abstract bool SetSomeThing<T>(NotNullable<string> root, string path, NotNullable<string> target, T value);

        public T GetSomeThing<T>(string target)
            => GetSomeThing<T>(CurrentRoot, CurrentPath, target);
        public T GetSomeThing<T>(string path, string target)
            => GetSomeThing<T>(CurrentRoot, path, target);
        public abstract T GetSomeThing<T>(NotNullable<string> root, string path, string target);
    }
}
