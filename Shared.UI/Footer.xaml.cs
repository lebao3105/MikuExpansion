#if SILVERLIGHT || WINDOWS_PHONE || WINDOWS_PHONE_APP || WINDOWS_UWP

using MikuExpansion.Extensions;
using static MikuExpansion.Resources.Strings; // worst practice
using System.Linq;

#if SILVERLIGHT
using System.Windows;
using System.Windows.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace MikuExpansion.UI
{
    /// <summary>
    /// Footer for Pages.
    /// </summary>
    public sealed partial class Footer : Grid
    {
#region Click events
        public delegate void OnBackPushDlg();
        public delegate void OnForwardPushDlg();

        /// <summary>
        /// Back button click event.
        /// <see cref="quitInsteadOfBack"/>  only changes the content of
        /// the back button itself, and does not make the application quit
        /// or back (using <see cref="System.Windows.Navigation.NavigationService.GoBack"/>
        /// or related) either.
        /// You do that instead, along with anything else you want.
        /// </summary>
        /// <seealso cref="OnForwardPush"/>
        /// <seealso cref="quitInsteadOfBack"/>
        public event OnBackPushDlg OnBackPush;

        /// <summary>
        /// Forward button click event.
        /// Does not call <see cref="System.Windows.Navigation.NavigationService.GoForward"/>
        /// or related functions. You have to do that yourself.
        /// </summary>
        /// <seealso cref="OnBackPush"/>
        public event OnForwardPushDlg OnForwardPush;
#endregion

#region DependencyProperties
        public static DependencyProperty quitInsteadOfBackProperty =
            DependencyProperty.Register("quitInsteadOfBack", typeof(bool), typeof(Footer), new PropertyMetadata(false));

        public static DependencyProperty allowContinuingProperty =
            DependencyProperty.Register("allowContinuing", typeof(bool), typeof(Footer), new PropertyMetadata(true));

        public static DependencyProperty BackTextProperty =
            DependencyProperty.Register("BackText", typeof(string), typeof(Footer), new PropertyMetadata(null));

        public static DependencyProperty ContinueTextProperty =
            DependencyProperty.Register("ContinueText", typeof(string), typeof(Footer), new PropertyMetadata(Forward));
#endregion

#region Properties
        /// <summary>
        /// Indicates whether the application should quit instead of
        /// going back to the previous page. Also changes the back
        /// button content.
        /// </summary>
        /// <seealso cref="OnBackPush"/>
        public bool quitInsteadOfBack
        {
            get { return (bool)GetValue(quitInsteadOfBackProperty); }
            set
            {
                SetValue(quitInsteadOfBackProperty, value);
                // User defined string
                if (!new string[] { Back, Quit }.Contains(BackText))
                    return;
                BackText = value ? Quit : Back;
            }
        }

        /// <summary>
        /// If this is true, the forward button will be disabled.
        /// </summary>
        public bool allowContinuing
        {
            get { return (bool)GetValue(allowContinuingProperty); }
            set {
                SetValue(allowContinuingProperty, value);
                forwardBtn.Visibility = value.ToVisibility(); // TODO: Never have to do this anymore
            }
        }

        public string BackText
        {
            get { return (string)GetValue(BackTextProperty); }
            set { SetValue(BackTextProperty, value); }
        }

        public string ContinueText
        {
            get { return (string)GetValue(ContinueTextProperty); }
            set { SetValue(ContinueTextProperty, value); }
        }
#endregion

        public Footer()
        {
            DataContext = this;
            this.InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            OnBackPush?.Invoke();
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {
            OnForwardPush?.Invoke();
        }
    }
}

#endif