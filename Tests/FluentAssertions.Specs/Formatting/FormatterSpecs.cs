using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Extensions;
using FluentAssertions.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    // Due to the tests that call Configuration.Current
    [CollectionDefinition("FormatterSpecs", DisableParallelization = true)]
    public class FormatterSpecsDefinition { }

    [Collection("FormatterSpecs")]
    public class FormatterSpecs
    {
        [Fact]
        public void When_value_contains_cyclic_reference_it_should_create_descriptive_error_message()
        {
            // Arrange
            var parent = new Node();
            parent.Children.Add(new Node());
            parent.Children.Add(parent);

            // Act
            string result = Formatter.ToString(parent);

            // Assert
            result.Should().ContainEquivalentOf("cyclic reference");
        }

        [Fact]
        public void When_the_same_object_appears_twice_in_the_graph_at_different_paths()
        {
            // Arrange
            var a = new A();
            var b = new B
            {
                X = a,
                Y = a
            };

            // Act
            Action act = () => b.Should().BeNull();

            // Assert
            var exception = act.Should().Throw<XunitException>().Which;
            exception.Message.Should().NotContainEquivalentOf("cyclic");
        }

        private class A
        {
        }

        private class B
        {
            public A X { get; set; }
            public A Y { get; set; }
        }

        [Fact]
        public void When_the_subject_or_expectation_contains_reserved_symbols_it_should_escape_then()
        {
            // Arrange
            string result = "{ a : [{ b : \"2016-05-23T10:45:12Z\" } ]}";

            string expectedJson = "{ a : [{ b : \"2016-05-23T10:45:12Z\" }] }";

            // Act
            Action act = () => result.Should().Be(expectedJson);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*near*index 37*");
        }

        [Fact]
        public void When_a_timespan_is_one_tick_it_should_be_formatted_as_positive()
        {
            // Arrange
            var time = TimeSpan.FromTicks(1);

            // Act
            string result = Formatter.ToString(time);

            // Assert
            result.Should().NotStartWith("-");
        }

        [Fact]
        public void When_a_timespan_is_minus_one_tick_it_should_be_formatted_as_negative()
        {
            // Arrange
            var time = TimeSpan.FromTicks(-1);

            // Act
            string result = Formatter.ToString(time);

            // Assert
            result.Should().StartWith("-");
        }

        [Fact]
        public void When_a_datetime_is_very_close_to_the_edges_of_a_datetimeoffset_it_should_not_crash()
        {
            // Arrange
            var dateTime = DateTime.MinValue + 1.Minutes();

            // Act
            string result = Formatter.ToString(dateTime);

            // Assert
            result.Should().Be("<00:01:00>");
        }

        [Fact]
        public void When_the_minimum_value_of_a_datetime_is_provided_it_should_return_a_clear_representation()
        {
            // Arrange
            var dateTime = DateTime.MinValue;

            // Act
            string result = Formatter.ToString(dateTime);

            // Assert
            result.Should().Be("<0001-01-01 00:00:00.000>");
        }

        [Fact]
        public void When_the_maximum_value_of_a_datetime_is_provided_it_should_return_a_clear_representation()
        {
            // Arrange
            var dateTime = DateTime.MaxValue;

            // Act
            string result = Formatter.ToString(dateTime);

            // Assert
            result.Should().Be("<9999-12-31 23:59:59.9999999>");
        }

        [Fact]
        public void When_a_property_throws_an_exception_it_should_ignore_that_and_still_create_a_descriptive_error_message()
        {
            // Arrange
            var subject = new ExceptionThrowingClass();

            // Act
            string result = Formatter.ToString(subject);

            // Assert
            result.Should().Contain("Member 'ThrowingProperty' threw an exception");
        }

        [Fact]
        public void When_the_object_is_a_generic_type_without_custom_string_representation_it_should_show_the_properties()
        {
            // Arrange
            var stuff = new List<Stuff<int>>
            {
                new Stuff<int>
                {
                    StuffId = 1,
                    Description = "Stuff_1",
                    Children = new List<int> {1, 2, 3, 4}
                },
                new Stuff<int>
                {
                    StuffId = 2,
                    Description = "Stuff_2",
                    Children = new List<int> {1, 2, 3, 4}
                }
            };

            var expectedStuff = new List<Stuff<int>>
            {
                new Stuff<int>
                {
                    StuffId = 1,
                    Description = "Stuff_1",
                    Children = new List<int> {1, 2, 3, 4}
                },
                new Stuff<int>
                {
                    StuffId = 2,
                    Description = "WRONG_DESCRIPTION",
                    Children = new List<int> {1, 2, 3, 4}
                }
            };

            // Act
            Action act = () => stuff.Should().NotBeNull()
                .And.Equal(expectedStuff, (t1, t2) => t1.StuffId == t2.StuffId && t1.Description == t2.Description);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Children =*")
                .WithMessage("*Description =*")
                .WithMessage("*StuffId =*");
        }

        [Fact]
        public void When_the_to_string_override_throws_it_should_use_the_default_behavior()
        {
            // Arrange
            var subject = new NullThrowingToStringImplementation();

            // Act
            string result = Formatter.ToString(subject);

            // Assert
            result.Should().Contain("SomeProperty");
        }

        [Fact]
        public void
            When_the_maximum_recursion_depth_is_met_it_should_give_a_descriptive_message()
        {
            // Arrange
            var head = new Node();

            foreach (int i in Enumerable.Range(0, 20))
            {
                var newHead = new Node();
                newHead.Children.Add(head);
                head = newHead;
            }

            // Act
            string result = Formatter.ToString(head);

            // Assert
            result.Should().ContainEquivalentOf("maximum recursion depth");
        }

        [Fact]
        public void When_formatting_a_byte_array_it_should_limit_the_items()
        {
            // Arrange
            byte[] value = new byte[1000];
            new Random().NextBytes(value);

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Match("{0x*, …968 more…}");
        }

        [Fact]
        public void When_formatting_with_default_behavior_it_should_include_non_private_fields()
        {
            // Arrange
            var stuffWithAField = new StuffWithAField { Field = "Some Text" };

            // Act
            string result = Formatter.ToString(stuffWithAField);

            // Assert
            result.Should().Contain("Field").And.Contain("Some Text");
            result.Should().NotContain("privateField");
        }

        [Fact]
        public void When_formatting_unsigned_integer_it_should_have_c_sharp_postfix()
        {
            // Arrange
            uint value = 12U;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12u");
        }

        [Fact]
        public void When_formatting_long_integer_it_should_have_c_sharp_postfix()
        {
            // Arrange
            long value = 12L;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12L");
        }

        [Fact]
        public void When_formatting_unsigned_long_integer_it_should_have_c_sharp_postfix()
        {
            // Arrange
            ulong value = 12UL;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12UL");
        }

        [Fact]
        public void When_formatting_short_integer_it_should_have_f_sharp_postfix()
        {
            // Arrange
            short value = 12;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12s");
        }

        [Fact]
        public void When_formatting_unsigned_short_integer_it_should_have_f_sharp_postfix()
        {
            // Arrange
            ushort value = 12;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12us");
        }

        [Fact]
        public void When_formatting_byte_it_should_use_hexadecimal_notation()
        {
            // Arrange
            byte value = 12;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("0x0C");
        }

        [Fact]
        public void When_formatting_signed_byte_it_should_have_f_sharp_postfix()
        {
            // Arrange
            sbyte value = 12;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12y");
        }

        [Fact]
        public void When_formatting_single_it_should_have_c_sharp_postfix()
        {
            // Arrange
            float value = 12;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12F");
        }

        [Fact]
        public void When_formatting_single_positive_infinity_it_should_be_property_reference()
        {
            // Arrange
            float value = float.PositiveInfinity;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("Single.PositiveInfinity");
        }

        [Fact]
        public void When_formatting_single_negative_infinity_it_should_be_property_reference()
        {
            // Arrange
            float value = float.NegativeInfinity;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("Single.NegativeInfinity");
        }

        [Fact]
        public void When_formatting_single_it_should_have_max_precision()
        {
            // Arrange
            float value = 1 / 3F;

            // Act
            string result = Formatter.ToString(value);

            // Assert
#if NETCOREAPP3_0
            result.Should().Be("0.33333334F");
#else
            result.Should().Be("0.333333343F");
#endif
        }

        [Fact]
        public void When_formatting_single_not_a_number_it_should_just_say_nan()
        {
            // Arrange
            float value = float.NaN;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            // NaN is not even equal to itself so its type does not matter.
            result.Should().Be("NaN");
        }

        [Fact]
        public void When_formatting_double_integer_it_should_have_decimal_point()
        {
            // Arrange
            double value = 12;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12.0");
        }

        [Fact]
        public void When_formatting_double_with_big_exponent_it_should_have_exponent()
        {
            // Arrange
            double value = 1E+30;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("1E+30");
        }

        [Fact]
        public void When_formatting_double_positive_infinity_it_should_be_property_reference()
        {
            // Arrange
            double value = double.PositiveInfinity;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("Double.PositiveInfinity");
        }

        [Fact]
        public void When_formatting_double_negative_infinity_it_should_be_property_reference()
        {
            // Arrange
            double value = double.NegativeInfinity;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("Double.NegativeInfinity");
        }

        [Fact]
        public void When_formatting_double_not_a_number_it_should_just_say_nan()
        {
            // Arrange
            double value = double.NaN;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            // NaN is not even equal to itself so its type does not matter.
            result.Should().Be("NaN");
        }

        [Fact]
        public void When_formatting_double_it_should_have_max_precision()
        {
            // Arrange
            double value = 1 / 3D;

            // Act
            string result = Formatter.ToString(value);

            // Assert
#if NETCOREAPP3_0
            result.Should().Be("0.3333333333333333");
#else
            result.Should().Be("0.33333333333333331");
#endif
        }

        [Fact]
        public void When_formatting_decimal_it_should_have_c_sharp_postfix()
        {
            // Arrange
            decimal value = 12;

            // Act
            string result = Formatter.ToString(value);

            // Assert
            result.Should().Be("12M");
        }

        [Fact]
        public void When_formatting_a_pending_task_it_should_return_the_task_status()
        {
            // Arrange
            Task<int> bar = Task.Delay(100000).ContinueWith(_ => 42);

            // Act
            string result = Formatter.ToString(bar);

            // Assert
            result.Should().Be("System.Threading.Tasks.ContinuationResultTaskFromTask`1[System.Int32] {Status=WaitingForActivation}");
        }

        [Fact]
        public void When_formatting_a_completion_source_it_should_include_the_underlying_task()
        {
            // Arrange
            var completionSource = new TaskCompletionSource<int>();

            // Act
            string result = Formatter.ToString(completionSource);

            // Assert
            result.Should().Match("*TaskCompletionSource*Task*System.Int32*Status=WaitingForActivation*");
        }

        public class BaseStuff
        {
            public int StuffId { get; set; }
            public string Description { get; set; }
        }

        public class StuffWithAField
        {
            public int StuffId { get; set; }
            public string Description { get; set; }
            public string Field;
#pragma warning disable 169
            private string privateField;
#pragma warning restore 169
        }

        public class Stuff<TChild> : BaseStuff
        {
            public List<TChild> Children { get; set; }
        }

        [Fact]
        public void When_a_custom_formatter_exists_in_any_loaded_assembly_it_should_override_the_default_formatters()
        {
            // Arrange
            Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

            var subject = new SomeClassWithCustomFormatter
            {
                Property = "SomeValue"
            };

            // Act
            string result = Formatter.ToString(subject);

            // Assert
            result.Should().Be("Property = SomeValue", "it should use my custom formatter");
        }

        [Fact]
        public void When_no_custom_formatter_exists_in_the_specified_assembly_it_should_use_the_default()
        {
            // Arrange
            Configuration.Current.ValueFormatterAssembly = "FluentAssertions";

            var subject = new SomeClassWithCustomFormatter
            {
                Property = "SomeValue"
            };

            // Act
            string result = Formatter.ToString(subject);

            // Assert
            result.Should().Be(subject.ToString());
        }

        [Fact]
        public void When_formatter_scanning_is_disabled_it_should_use_the_default_formatters()
        {
            // Arrange
            Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Disabled;

            var subject = new SomeClassWithCustomFormatter
            {
                Property = "SomeValue"
            };

            // Act
            string result = Formatter.ToString(subject);

            // Assert
            result.Should().Be(subject.ToString());
        }

        [Fact]
        public void When_no_formatter_scanning_is_configured_it_should_use_the_default_formatters()
        {
            // Arrange
            Services.ResetToDefaults();
            Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Disabled;

            var subject = new SomeClassWithCustomFormatter
            {
                Property = "SomeValue"
            };

            // Act
            string result = Formatter.ToString(subject);

            // Assert
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
                throw new XunitException("Should never be called");
            }
        }

        [Fact]
        public void When_defining_a_custom_value_formatter_it_should_respect_the_overrides()
        {
            // Arrange
            var value = new CustomClass();
            var formatter = new CustomClassValueFormatter();
            using var _ = new FormatterScope(formatter);

            // Act
            string str = Formatter.ToString(value);

            // Assert
            str.Should().Match(
"*CustomClass" + Environment.NewLine +
"{" + Environment.NewLine +
"        IntProperty = 0" + Environment.NewLine +
"}*");
        }

        private class CustomClass
        {
            public int IntProperty { get; set; }
            public string StringProperty { get; set; }
        }

        private class CustomClassValueFormatter : DefaultValueFormatter
        {
            protected override int SpacesPerIndentionLevel => 8;

            public override bool CanHandle(object value) => value is CustomClass;

            protected override IEnumerable<SelectedMemberInfo> GetMembers(Type type) =>
                base.GetMembers(type).Where(e => e.MemberType != typeof(string));

            protected override string TypeDisplayName(Type type) => type.Name;
        }

        [Fact]
        public void When_defining_a_custom_enumerable_value_formatter_it_should_respect_the_overrides()
        {
            // Arrange
            var values = new CustomClass[]
            {
                new CustomClass{ IntProperty = 1 },
                new CustomClass{ IntProperty = 2 }
            };

            var formatter = new SingleItemValueFormatter();
            using var _ = new FormatterScope(formatter);

            // Act
            string str = Formatter.ToString(values);

            // Assert
            str.Should().Match(
"{FluentAssertions.Specs.FormatterSpecs+CustomClass" + Environment.NewLine +
"   {" + Environment.NewLine +
"      IntProperty = 1" + Environment.NewLine +
"      StringProperty = <null>" + Environment.NewLine +
"   }, …1 more…}*");
        }

        private class SingleItemValueFormatter : EnumerableValueFormatter
        {
            protected override int MaxItems => 1;

            public override bool CanHandle(object value) => value is IEnumerable<CustomClass>;
        }

        private sealed class FormatterScope : IDisposable
        {
            private readonly IValueFormatter formatter;

            public FormatterScope(IValueFormatter formatter)
            {
                this.formatter = formatter;
                Formatter.AddFormatter(formatter);
            }

            public void Dispose() => Formatter.RemoveFormatter(formatter);
        }
    }

    internal class ExceptionThrowingClass
    {
        public string ThrowingProperty => throw new InvalidOperationException();
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

        public static Node Default { get; } = new Node();

        public List<Node> Children { get; set; }
    }
}
