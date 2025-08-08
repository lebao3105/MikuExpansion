using System.Windows;

namespace MikuExpansion.Extensions
{
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
    }
}