using System;
using System.Collections.Generic;

using FluentAssertions.Common;
using FluentAssertions.Formatting;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif NUNIT
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestCaseAttribute;
using AssertFailedException = NUnit.Framework.AssertionException;
using TestInitializeAttribute = NUnit.Framework.SetUpAttribute;
using Assert = NUnit.Framework.Assert;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class FormatterSpecs
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

        [TestMethod]
        public void When_the_object_is_a_generic_type_without_custom_string_representation_it_should_show_the_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var stuff = new List<Stuff<int>>
            {
                new Stuff<int>
                {
                    StuffId = 1,
                    Description = "Stuff_1",
                    Childs = new List<int> { 1, 2, 3, 4 }
                },
                new Stuff<int>
                {
                    StuffId = 2,
                    Description = "Stuff_2",
                    Childs = new List<int> { 1, 2, 3, 4 }
                }
            };

            var expectedStuff = new List<Stuff<int>>
            {
                new Stuff<int>
                {
                    StuffId = 1,
                    Description = "Stuff_1",
                    Childs = new List<int> { 1, 2, 3, 4 }
                },
                new Stuff<int>
                {
                    StuffId = 2,
                    Description = "WRONG_DESCRIPTION",
                    Childs = new List<int> { 1, 2, 3, 4 }
                }
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => stuff.Should().NotBeNull()
                .And.Equal(expectedStuff, (t1, t2) => t1.StuffId == t2.StuffId && t1.Description == t2.Description);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("*Childs =*")
                .WithMessage("*Description =*")
                .WithMessage("*StuffId =*");
        }

        [TestMethod]
        public void When_the_to_string_override_throws_it_should_use_the_default_behavior()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new NullThrowingToStringImplementation();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            string result = Formatter.ToString(subject);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            result.Should().Contain("SomeProperty");
        }
        
        public class BaseStuff
        {
            public int StuffId { get; set; }
            public string Description { get; set; }
        }

        public class Stuff<TChild> : BaseStuff
        {
            public List<TChild> Childs { get; set; }
        }

#if !WINRT && !__IOS__

        [TestMethod]
        public void When_a_custom_formatter_exists_in_any_loaded_assembly_it_should_override_the_default_formatters()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;
            
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

        [TestMethod]
        public void When_no_custom_formatter_exists_in_the_specified_assembly_it_should_use_the_default()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
             Configuration.Current.ValueFormatterAssembly = "FluentAssertions";

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
            result.Should().Be(subject.ToString());
        }
        
        [TestMethod]
        public void When_formatter_scanning_is_disabled_it_should_use_the_default_formatters()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Disabled;
            
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
            result.Should().Be(subject.ToString());
        }
        
        [TestMethod]
        public void When_no_formatter_scanning_is_configured_it_should_use_the_default_formatters()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Services.ResetToDefaults();
            
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
            result.Should().Be(subject.ToString());
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

#endif
    }

    internal class ExceptionThrowingClass
    {
        public string ThrowingProperty
        {
            get { throw new InvalidOperationException(); }
        }
    }

    internal class NullThrowingToStringImplementation
    {
        public NullThrowingToStringImplementation()
        {
            SomeProperty = "SomeProperty";
        }

        public string SomeProperty { get; set; }

        public override string ToString()
        {
            return null;
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