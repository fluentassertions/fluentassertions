using System;
using System.Text.RegularExpressions;

using FluentAssertions.Execution;
#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class AssertionScopeSpecs
    {
        [TestMethod]
        public void When_disposed_it_should_throw_any_failures()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;
            ;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            try
            {
                act();
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Failure1"));
            }
        }

        [TestMethod]
        public void When_multiple_scopes_are_nested_it_should_throw_all_failures_from_the_outer_scope()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure2");

                using (var deeplyNestedScope = new AssertionScope())
                {
                    deeplyNestedScope.FailWith("Failure3");
                }
            }

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;
            ;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            try
            {
                act();
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("Failure1"));
                Assert.IsTrue(exception.Message.Contains("Failure2"));
                Assert.IsTrue(exception.Message.Contains("Failure3"));
            }
        }

        [TestMethod]
        public void When_a_nested_scope_is_discarded_its_failures_should_also_be_discarded()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure1");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure2");

                using (var deeplyNestedScope = new AssertionScope())
                {
                    deeplyNestedScope.FailWith("Failure3");
                    deeplyNestedScope.Discard();
                }
            }

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;
            ;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            try
            {
                act();
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.Contains("Failure1"));
                Assert.IsTrue(exception.Message.Contains("Failure2"));
                Assert.IsFalse(exception.Message.Contains("Failure3"));
            }
        }

        [TestMethod]
        public void When_the_same_failure_is_handled_twice_or_more_it_should_still_report_it_once()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var scope = new AssertionScope();

            AssertionScope.Current.FailWith("Failure");
            AssertionScope.Current.FailWith("Failure");

            using (var nestedScope = new AssertionScope())
            {
                nestedScope.FailWith("Failure");
                nestedScope.FailWith("Failure");
            }

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = scope.Dispose;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            try
            {
                act();
            }
            catch (Exception exception)
            {
                int matches = new Regex(".*Failure.*").Matches(exception.Message).Count;

                Assert.AreEqual(6, matches);
            }
        }

    }
}