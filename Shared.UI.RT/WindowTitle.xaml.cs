using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MikuExpansion.Extensions;

namespace MikuExpansion.UI
{
    public sealed partial class WindowTitle : UserControl
    {
        public static DependencyProperty TitleProperty =
            UIExts.RegisterDepProperty<string, UserControl>(
                nameof(Title), "Unnamed Window"
                );

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public WindowTitle()
        {
            this.InitializeComponent();
        }
    }
}
