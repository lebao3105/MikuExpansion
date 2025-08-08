using System.Windows;

namespace MikuExpansion.Extensions
{
    public static class Booleans
    {
        /// <summary>
        /// Just do what the name says.
        /// </summary>
        /// <param name="self"></param>
        /// <returns>1 if <paramref name="self"/> is true, otherwise 0.</returns>
        public static int OneIfTrue(this bool self) => self ? 1 : 0;

        /// <summary>
        /// The same as <see cref="OneIfTrue(bool)"/> but returns a character instead.
        /// Confirmed to be useful for HTTP requests. WhatsWin showed me that.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static char OneAsCharIfTrue(this bool self) => self ? '1' : '0';

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

        /// <summary>
        /// Returns !<paramref name="self"/>. Notice the exclaimation mark.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool Negate(this bool self) => !self;
    }
}
