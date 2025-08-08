#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if SILVERLIGHT
using System.Windows;
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace MikuExpansion.UI
{
    public class WebLinkButton : HyperlinkButton
    {
        public WebLinkButton() : base()
        {
            Click += WebLinkButton_Click;
        }

        private
#if !SILVERLIGHT // Better: Ignore C# compiler warning
            async
#endif
            void WebLinkButton_Click(object sender, RoutedEventArgs e)
        {
#if SILVERLIGHT
            new Microsoft.Phone.Tasks.WebBrowserTask
            {
                Uri = NavigateUri
            }.Show();
#else
            await Windows.System.Launcher.LaunchUriAsync(NavigateUri);
#endif
        }
    }
}

#endif