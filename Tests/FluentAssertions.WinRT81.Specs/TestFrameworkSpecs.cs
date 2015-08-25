using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class TestFrameworkSpecs
    {
        [UITestMethod]
        public void When_an_assertion_fails_in_an_ui_test_it_should_throw_the_correct_exception_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => 0.Should().Be(1);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }
    }
}