using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Execution
{
    internal class SilverlightTestFramework : ITestFramework
    {
        public bool IsAvailable
        {
            get { return true; }
        }

        public void Throw(string message)
        {
            throw new AssertFailedException(message);
        }
    }
}
