using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using MikuExpansion.Helpers;
using System.Collections.ObjectModel;

namespace MikuExpansion.Tests
{
    public struct TestResult
    {
        public string name { get; set; }
        public string data { get; set; }
        public string status { get; set; }
    }

    public partial class MainPage : PhoneApplicationPage
    {
        private sealed class UriIntType : MultiTypeInfo
        {
            protected override IEnumerable<Type> GetTypesNoCtor()
                => new Type[] { typeof(Uri), typeof(int) };
        }

        private string TEST_PAGE = "https://lebao3105.github.io";
        private int TEST_NUMB = 0;

        public ObservableCollection<TestResult> mtResult { get; private set; }
            = new ObservableCollection<TestResult>();

        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
        }
        
        private MultiType<UriIntType> uritest;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            uritest = new MultiType<UriIntType>();
            PopulateTests();
        }

        private void PopulateTests()
        {
            // TODO: Move tests outta here
            mtResult.Clear();

            bool uritestIsANum = uritest.GetValueType().Equals(typeof(int));

            if (uritestIsANum)
                mtResult.Add(new TestResult
                {
                    name = "Is a number?",
                    data = uritestIsANum.ToString(),
                    status = uritestIsANum ? TEST_NUMB.ToString() : TEST_PAGE
                });
            else
                mtResult.Add(new TestResult
                {
                    name = "Is a URI?",
                    data = true.ToString(),
                    status = TEST_PAGE
                });
        }

        private void valueTest_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = (sender as TextBox).Text;
            if (int.TryParse(value, out TEST_NUMB))
                uritest.Set(TEST_NUMB);
            else
                try
                {
                    uritest.Set(new Uri(value));
                }
                catch (UriFormatException)
                {
                    // do nothing
                    return;
                }

            PopulateTests();
        }

        private void timestopBtn_Click(object sender, EventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {

        }

        private void helpBtn_Click(object sender, EventArgs e)
        {
            switch (panorama.SelectedIndex)
            {
                case 0: // welcome
                    break; // do nothing

                case 1: // multiple types
                    MessageBox.Show(Tests.Resources.AppResources.MultiTypes_Des);
                    break;
            }
        }
    }
}