#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP

using System;

#if SILVERLIGHT
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace MikuExpansion.UI
{
    public partial class RTextBlock : UserControl
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(RTextBlock), new PropertyMetadata(null));

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        private DispatcherTimer timer = new DispatcherTimer
        {
            Interval = new TimeSpan(1000)
        };

        public RTextBlock()
        {
            InitializeComponent();
            mainTextBlock.DataContext = this;
        }

        private void scrollviewer_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void scrollviewer_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Tick += (ss, ee) =>
            {
                //each time set the offset to scrollviewer.HorizontalOffset + 5
                scrollviewer.ScrollToHorizontalOffset(scrollviewer.HorizontalOffset + 5);
                //if the scrollviewer scrolls to the end, scroll it back to the start.
                if (scrollviewer.HorizontalOffset == scrollviewer.ScrollableWidth)
                    scrollviewer.ScrollToHorizontalOffset(0);
            };
            timer.Start();
        }
    }
}

#endif