using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FluentAssertions.Silverlight.Specs
{
    public class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string someProperty;

        public string SomeProperty
        {
            get { return someProperty; }
            set
            {
                someProperty = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SomeProperty"));
            }
        }

        public void ChangeProperty(string value)
        {
            SomeProperty = value;
        }
    }
}
