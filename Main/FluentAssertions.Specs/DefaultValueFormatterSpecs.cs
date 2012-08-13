using System;
using System.Collections.Generic;
using FluentAssertions.Formatting;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class DefaultValueFormatterSpecs
    {
        [TestMethod]
        public void When_value_contains_cyclic_reference_it_should_create_descriptive_error_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var parent = new Node();
            parent.Children.Add(new Node());
            parent.Children.Add(parent);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = Formatter.ToString(parent);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().ContainEquivalentOf("cyclic reference");
        }

        [TestMethod]
        public void When_a_property_throws_an_exception_it_should_ignore_that_and_still_create_a_descriptive_error_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ExceptionThrowingClass();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = Formatter.ToString(subject);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Contain("Property 'ThrowingProperty' threw an exception");
        }

#if !WINRT

        #region Attribute Based Formatting

        [TestMethod]
        public void When_a_custom_formatter_exists_in_the_current_assembly_it_should_override_the_default_formatters()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new SomeClassWithCustomFormatter
            {
                Property = "SomeValue"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = Formatter.ToString(subject);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Be("Property = SomeValue");
        }

        public class SomeClassWithCustomFormatter
        {
            public string Property { get; set; }

            public override string ToString()
            {
                return "The value of my property is " + Property;
            }
        }

        public class SomeOtherClassWithCustomFormatter
        {
            public string Property { get; set; }

            public override string ToString()
            {
                return "The value of my property is " + Property;
            }
        }

        public static class CustomFormatter
        {
            [ValueFormatter]
            public static string Foo(SomeClassWithCustomFormatter value)
            {
                return "Property = " + value.Property;
            }

            [ValueFormatter]
            public static string Foo(SomeOtherClassWithCustomFormatter value)
            {
                Assert.Fail("Should never be called");
                return "";
            }
        }

        #endregion

#endif
    }

    internal class ExceptionThrowingClass
    {
        public string ThrowingProperty
        {
            get { throw new InvalidOperationException(); }
        }
    }

    internal class Node
    {
        public Node()
        {
            Children = new List<Node>();
        }

        private static readonly Node _default = new Node();

        public static Node Default
        {
            get { return _default; }
        }

        public List<Node> Children { get; set; }
    }
}