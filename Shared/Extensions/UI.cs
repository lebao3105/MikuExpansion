#if WINDOWS_RT || SILVERLIGHT

using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;

namespace MikuExpansion.Extensions
{
    public static class UIExts
    {
        public static DependencyProperty RegisterDepProperty<T, TContainer>(
            string name, PropertyMetadata metadata = null)
            => DependencyProperty.Register(name, typeof(T), typeof(TContainer), metadata);

        public static DependencyProperty RegisterDepProperty<T, TContainer>(
            string name, T defaultValue = default(T))
            => RegisterDepProperty<T, TContainer>(name, new PropertyMetadata(defaultValue));
    }
}

#endif