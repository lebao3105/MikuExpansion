using System;
using System.Linq;

#if SILVERLIGHT
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
#endif

namespace MikuExpansion.UI
{
    public sealed partial class PageTitle : UserControl
    {
        public static readonly DependencyProperty
            TitleProperty = DependencyProperty.Register(
                "Title", typeof(string), typeof(PageTitle), null
            );

        public static readonly DependencyProperty
            SectionProperty = DependencyProperty.Register(
                "Section", typeof(string), typeof(PageTitle), null
            );

        /// <summary>
        /// The page's title (e.g Languages or Saved medias).
        /// One page can have multiple titles, and only one is
        /// shown on the screen (of course). If you do that,
        /// you won't need to manually set this property.
        /// See <see cref="TriggerTitleAnimation"/>.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Where the page belongs to (e.g WhatsWin in general
        /// or Setup for the first-run wizard).
        /// </summary>
        public string Section
        {
            get { return (string)GetValue(SectionProperty); }
            set { SetValue(SectionProperty, value); }
        }

        private string[] Titles = null;

        public PageTitle()
        {
            InitializeComponent();
            DataContext = this;

            var titleShowAnim = (Storyboard)Resources["titleShowAnim"];
            var titleHideAnim = (Storyboard)Resources["titleHideAnim"];

            titleHideAnim.Completed += (_, __) =>
            {
                if (Titles == null)
                    return;

                Random rnd = new Random();
                var indx = rnd.Next(0, Titles.Count());

                while (indx == Array.IndexOf(Titles, pageTitle.Text))
                {
                    indx = rnd.Next(0, Titles.Count());
                }

                pageTitle.Text = Titles[indx];
            };

            titleShowAnim.Completed += (_, __) =>
            {
                titleShowAnim.Stop();
                titleHideAnim.Begin();
            };
        }

        /// <summary>
        /// Starts randomizing for <see cref="Titles"/> value from
        /// <paramref name="titles"/>.
        /// </summary>
        /// <param name="titles">List of titles to be used.</param>
        /// <exception cref="ArgumentException"/>
        public void TriggerTitleAnimation(string[] titles)
        {
            if (titles.Count() == 0)
                throw new ArgumentException("Empty list");

            Titles = titles;
            pageTitle.Text = Titles[0];

            ((Storyboard)Resources["titleShowAnim"]).Begin();
        }
    }
}
