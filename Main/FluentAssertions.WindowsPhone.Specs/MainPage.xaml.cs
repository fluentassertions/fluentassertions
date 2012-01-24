using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;

namespace FluentAssertions.WindowsPhone.Specs
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            var testPage = UnitTestSystem.CreateTestPage() as IMobileTestPage;
            BackKeyPress += (x, xe) => xe.Cancel = testPage.NavigateBack();
            (Application.Current.RootVisual as PhoneApplicationFrame).Content = testPage; 
        }
    }
}