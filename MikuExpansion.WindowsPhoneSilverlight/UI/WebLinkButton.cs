using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MikuExpansion.UI
{
    public class WebLinkButton : HyperlinkButton
    {
        public WebLinkButton() : base()
        {
            Click += WebLinkButton_Click;
        }

        private void WebLinkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            new Microsoft.Phone.Tasks.WebBrowserTask
            {
                Uri = NavigateUri
            }.Show();
        }
    }
}
