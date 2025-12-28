#if SILVERLIGHT
using System.Windows;
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
using Windows.UI.Xaml;
#endif

namespace MikuExpansion.Extensions
{
#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP
    public static class Visibilities
    {
        /// <summary>
        /// Negates <paramref name="self"/>, applying that
        /// change into <paramref name="self"/> itself.
        /// </summary>
        /// <param name="self"></param>
        public static void Negate(ref Visibility self)
        {
            self = NegateNoSet(self);
        }

        /// <summary>
        /// Same as <see cref="Negate(ref Visibility)"/>, but instead
        /// returns the "negated" value.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Visibility NegateNoSet(this Visibility self)
            => self.IsVisible() ? Visibility.Collapsed : Visibility.Visible;

        /// <summary>
        /// Returns true if <paramref name="self"/> equals
        /// <see cref="Visibility.Visible"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsVisible(this Visibility self) => self.Equals(Visibility.Visible);

        public static void MultipleSet(UIElement[] selfs, Visibility setTo)
        {
            foreach (var self in selfs)
                self.Visibility = setTo;
        }
        
#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP
        /// <summary>
        /// Returns <see cref="Visibility.Visible"/> if
        /// <paramref name="self"/> is true.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Visibility ToVisibility(this bool self)
            => self ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        /// Returns <see cref="Visibility.Visible"/> if
        /// <paramref name="self"/> is true.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Visibility ToVisibility(this bool? self)
            => self == true ? Visibility.Visible : Visibility.Collapsed;
#endif
    }
#endif
}