using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FluentAssertions.Extensions;
using FluentAssertions.Formatting;
using FluentAssertions.Specs.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Formatting;

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

    private class A;

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
        act.Should().Throw<XunitException>().WithMessage("*at*index 37*");
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
        result.Should().Contain("Member 'ThrowingProperty' threw an exception: 'CustomMessage'");
    }

    [Fact]
    public void When_an_exception_contains_an_inner_exception_they_should_both_appear_in_the_error_message()
    {
        // Arrange
        Exception subject = new("OuterExceptionMessage", new InvalidOperationException("InnerExceptionMessage"));

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Contain("OuterExceptionMessage")
            .And.Contain("InnerExceptionMessage");
    }

    [Fact]
    public void When_the_object_is_a_generic_type_without_custom_string_representation_it_should_show_the_properties()
    {
        // Arrange
        var stuff = new List<Stuff<int>>
        {
            new()
            {
                StuffId = 1,
                Description = "Stuff_1",
                Children = new List<int> { 1, 2, 3, 4 }
            },
            new()
            {
                StuffId = 2,
                Description = "Stuff_2",
                Children = new List<int> { 1, 2, 3, 4 }
            }
        };

        var expectedStuff = new List<Stuff<int>>
        {
            new()
            {
                StuffId = 1,
                Description = "Stuff_1",
                Children = new List<int> { 1, 2, 3, 4 }
            },
            new()
            {
                StuffId = 2,
                Description = "WRONG_DESCRIPTION",
                Children = new List<int> { 1, 2, 3, 4 }
            }
        };

        // Act
        Action act = () => stuff.Should().NotBeNull()
            .And.Equal(expectedStuff, (t1, t2) => t1.StuffId == t2.StuffId && t1.Description == t2.Description);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage(
            """
            Expected stuff to be equal to 
            {
                FluentAssertions.Specs.Formatting.FormatterSpecs+Stuff`1[[System.Int32*]]
                {
                    Children = {1, 2, 3, 4}, 
                    Description = "Stuff_1", 
                    StuffId = 1
                }, 
                FluentAssertions.Specs.Formatting.FormatterSpecs+Stuff`1[[System.Int32*]]
                {
                    Children = {1, 2, 3, 4}, 
                    Description = "WRONG_DESCRIPTION", 
                    StuffId = 2
                }
            }, but 
            {
                FluentAssertions.Specs.Formatting.FormatterSpecs+Stuff`1[[System.Int32*]]
                {
                    Children = {1, 2, 3, 4}, 
                    Description = "Stuff_1", 
                    StuffId = 1
                }, 
                FluentAssertions.Specs.Formatting.FormatterSpecs+Stuff`1[[System.Int32*]]
                {
                    Children = {1, 2, 3, 4}, 
                    Description = "Stuff_2", 
                    StuffId = 2
                }
            } differs at index 1.
            """);
    }

    [Fact]
    public void When_the_object_is_a_user_defined_type_it_should_show_the_name_on_the_initial_line()
    {
        // Arrange
        var stuff = new StuffRecord(42, "description", new(24), new List<int> { 10, 20, 30, 40 });

        // Act
        Action act = () => stuff.Should().BeNull();

        // Assert
        act.Should().Throw<XunitException>()
            .Which.Message.Should().Be(
            """
            Expected stuff to be <null>, but found FluentAssertions.Specs.Formatting.FormatterSpecs+StuffRecord
            {
                RecordChildren = {10, 20, 30, 40}, 
                RecordDescription = "description", 
                RecordId = 42, 
                SingleChild = FluentAssertions.Specs.Formatting.FormatterSpecs+ChildRecord
                {
                    ChildRecordId = 24
                }
            }.
            """);
    }

    [Fact]
    public void When_the_object_is_an_anonymous_type_it_should_show_the_properties_recursively()
    {
        // Arrange
        var stuff = new
        {
            Description = "absent",
            SingleChild = new { ChildId = 8 },
            Children = new[] { 1, 2, 3, 4 },
        };

        var expectedStuff = new
        {
            SingleChild = new { ChildId = 4 },
            Children = new[] { 10, 20, 30, 40 },
        };

        // Act
        Action act = () => stuff.Should().Be(expectedStuff);

        // Assert
        act.Should().Throw<XunitException>()
            .Which.Message.Should().Be(
            """
            Expected stuff to be 
            {
                Children = {10, 20, 30, 40}, 
                SingleChild = 
                {
                    ChildId = 4
                }
            }, but found 
            {
                Children = {1, 2, 3, 4}, 
                Description = "absent", 
                SingleChild = 
                {
                    ChildId = 8
                }
            }.
            """);
    }

    [Fact]
    public void When_the_object_is_a_list_of_anonymous_type_it_should_show_the_properties_recursively_with_newlines_and_indentation()
    {
        // Arrange
        var stuff = new[]
        {
            new
            {
                Description = "absent",
            },
            new
            {
                Description = "absent",
            },
        };

        var expectedStuff = new[]
        {
            new
            {
                ComplexChildren = new[]
                {
                    new { Property = "hello" },
                    new { Property = "goodbye" },
                },
            },
        };

        // Act
        Action act = () => stuff.Should().BeEquivalentTo(expectedStuff);

        // Assert
        act.Should().Throw<XunitException>()
            .Which.Message.Should().StartWith(
            """
            Expected stuff to be a collection with 1 item(s), but 
            {
                {
                    Description = "absent"
                }, 
                {
                    Description = "absent"
                }
            }
            contains 1 item(s) more than

            {
                {
                    ComplexChildren = 
                    {
                        {
                            Property = "hello"
                        }, 
                        {
                            Property = "goodbye"
                        }
                    }
                }
            }.
            """);
    }

    [Fact]
    public void When_the_object_is_an_empty_anonymous_type_it_should_show_braces_on_the_same_line()
    {
        // Arrange
        var stuff = new
        {
        };

        // Act
        Action act = () => stuff.Should().BeNull();

        // Assert
        act.Should().Throw<XunitException>()
            .Which.Message.Should().Match("*but found { }*");
    }

    [Fact]
    public void When_the_object_is_a_tuple_it_should_show_the_properties_recursively()
    {
        // Arrange
        (int TupleId, string Description, List<int> Children) stuff = (1, "description", new() { 1, 2, 3, 4 });

        (int, string, List<int>) expectedStuff = (2, "WRONG_DESCRIPTION", new List<int> { 4, 5, 6, 7 });

        // Act
        Action act = () => stuff.Should().Be(expectedStuff);

        // Assert
        act.Should().Throw<XunitException>()
            .Which.Message.Should().Be(
            """
            Expected stuff to be equal to 
            {
                Item1 = 2, 
                Item2 = "WRONG_DESCRIPTION", 
                Item3 = {4, 5, 6, 7}
            }, but found 
            {
                Item1 = 1, 
                Item2 = "description", 
                Item3 = {1, 2, 3, 4}
            }.
            """);
    }

    [Fact]
    public void When_the_object_is_a_record_it_should_show_the_properties_recursively()
    {
        // Arrange
        var stuff = new StuffRecord(
            RecordId: 9,
            RecordDescription: "descriptive",
            SingleChild: new(ChildRecordId: 80),
            RecordChildren: new() { 4, 5, 6, 7 });

        var expectedStuff = new
        {
            RecordDescription = "WRONG_DESCRIPTION",
        };

        // Act
        Action act = () => stuff.Should().Be(expectedStuff);

        // Assert
        act.Should().Throw<XunitException>()
            .Which.Message.Should().Be(
            """
            Expected stuff to be 
            {
                RecordDescription = "WRONG_DESCRIPTION"
            }, but found FluentAssertions.Specs.Formatting.FormatterSpecs+StuffRecord
            {
                RecordChildren = {4, 5, 6, 7}, 
                RecordDescription = "descriptive", 
                RecordId = 9, 
                SingleChild = FluentAssertions.Specs.Formatting.FormatterSpecs+ChildRecord
                {
                    ChildRecordId = 80
                }
            }.
            """);
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
        var node = head;

        int maxDepth = 10;
        int iterations = (maxDepth / 2) + 1; // Each iteration adds two levels of depth to the graph

        foreach (int i in Enumerable.Range(0, iterations))
        {
            var newHead = new Node();
            node.Children.Add(newHead);
            node = newHead;
        }

        // Act
        string result = Formatter.ToString(head, new FormattingOptions
        {
            MaxDepth = maxDepth
        });

        // Assert
        result.Should().ContainEquivalentOf($"maximum recursion depth of {maxDepth}");
    }

    [Fact]
    public void When_the_maximum_recursion_depth_is_never_reached_it_should_render_the_entire_graph()
    {
        // Arrange
        var head = new Node();
        var node = head;

        int iterations = 10;

        foreach (int i in Enumerable.Range(0, iterations))
        {
            var newHead = new Node();
            node.Children.Add(newHead);
            node = newHead;
        }

        // Act
        string result = Formatter.ToString(head, new FormattingOptions
        {
            // Each iteration adds two levels of depth to the graph
            MaxDepth = (iterations * 2) + 1
        });

        // Assert
        result.Should().NotContainEquivalentOf("maximum recursion depth");
    }

    [Fact]
    public void When_formatting_a_collection_exceeds_the_max_line_count_it_should_cut_off_the_result()
    {
        // Arrange
        var collection = Enumerable.Range(0, 20)
            .Select(i => new StuffWithAField
            {
                Description = $"Property {i}",
                Field = $"Field {i}",
                StuffId = i
            })
            .ToArray();

        // Act
        string result = Formatter.ToString(collection, new FormattingOptions
        {
            MaxLines = 50
        });

        // Assert
        result.Should().Match("*Output has exceeded*50*line*");
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
        result.Should().BeOneOf("0.33333334F", "0.333333343F");
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
        result.Should().BeOneOf("0.3333333333333333", "0.33333333333333331");
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
        Task<int> bar = new TaskCompletionSource<int>().Task;

        // Act
        string result = Formatter.ToString(bar);

        // Assert
        result.Should().Be("System.Threading.Tasks.Task`1[System.Int32] {Status=WaitingForActivation}");
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

    private class MyKey
    {
        public int KeyProp { get; set; }
    }

    private class MyValue
    {
        public int ValueProp { get; set; }
    }

    [Fact]
    public void When_formatting_a_dictionary_it_should_format_keys_and_values()
    {
        // Arrange
        var subject = new Dictionary<MyKey, MyValue>
        {
            [new MyKey { KeyProp = 13 }] = new() { ValueProp = 37 }
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Match("*{*[*MyKey*KeyProp = 13*] = *MyValue*ValueProp = 37*}*");
    }

    [Fact]
    public void When_formatting_an_empty_dictionary_it_should_be_clear_from_the_message()
    {
        // Arrange
        var subject = new Dictionary<int, int>();

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Match("{empty}");
    }

    [Fact]
    public void When_formatting_a_large_dictionary_it_should_limit_the_number_of_formatted_entries()
    {
        // Arrange
        var subject = Enumerable.Range(0, 50).ToDictionary(e => e, e => e);

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Match("*…18 more…*");
    }

    [Fact]
    public void
        When_formatting_multiple_items_with_a_custom_string_representation_using_line_breaks_it_should_end_lines_with_a_comma()
    {
        // Arrange
        var subject = new[] { typeof(A), typeof(B) };

        // Act
        string result = Formatter.ToString(subject, new FormattingOptions { UseLineBreaks = true });

        // Assert
        result.Should().Contain($"FluentAssertions.Specs.Formatting.FormatterSpecs+A, {Environment.NewLine}");
        result.Should().Contain($"FluentAssertions.Specs.Formatting.FormatterSpecs+B{Environment.NewLine}");
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
#pragma warning disable 169, CA1823, IDE0044, RCS1169
        private string privateField;
#pragma warning restore 169, CA1823, IDE0044, RCS1169
    }

    public class Stuff<TChild> : BaseStuff
    {
        public List<TChild> Children { get; set; }
    }

    private record StuffRecord(int RecordId, string RecordDescription, ChildRecord SingleChild, List<int> RecordChildren);

    private record ChildRecord(int ChildRecordId);

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
    public void When_a_base_class_has_a_custom_formatter_it_should_override_the_default_formatters()
    {
        // Arrange
        Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

        var subject = new SomeClassInheritedFromClassWithCustomFormatterLvl1
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be("Property = SomeValue", "it should use my custom formatter");
    }

    [Fact]
    public void When_there_are_multiple_custom_formatters_it_should_select_a_more_specific_one()
    {
        // Arrange
        Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

        var subject = new SomeClassInheritedFromClassWithCustomFormatterLvl2
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be("Property is SomeValue", "it should use my custom formatter");
    }

    [Fact]
    public void When_a_base_class_has_multiple_custom_formatters_it_should_work_the_same_as_for_the_base_class()
    {
        // Arrange
        Configuration.Current.ValueFormatterDetectionMode = ValueFormatterDetectionMode.Scan;

        var subject = new SomeClassInheritedFromClassWithCustomFormatterLvl3
        {
            Property = "SomeValue"
        };

        // Act
        string result = Formatter.ToString(subject);

        // Assert
        result.Should().Be("Property is SomeValue", "it should use my custom formatter");
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

    public class SomeClassInheritedFromClassWithCustomFormatterLvl1 : SomeClassWithCustomFormatter;

    public class SomeClassInheritedFromClassWithCustomFormatterLvl2 : SomeClassInheritedFromClassWithCustomFormatterLvl1;

    public class SomeClassInheritedFromClassWithCustomFormatterLvl3 : SomeClassInheritedFromClassWithCustomFormatterLvl2;

    public static class CustomFormatter
    {
        [ValueFormatter]
        public static int Bar(SomeClassWithCustomFormatter _)
        {
            return -1;
        }

        [ValueFormatter]
        public static void Foo(SomeClassWithCustomFormatter value, FormattedObjectGraph output)
        {
            output.AddFragment("Property = " + value.Property);
        }

        [ValueFormatter]
        [SuppressMessage("ReSharper", "CA1801")]
        public static void Foo(SomeOtherClassWithCustomFormatter _, FormattedObjectGraph output)
        {
            throw new XunitException("Should never be called");
        }

        [ValueFormatter]
        public static void Foo(SomeClassInheritedFromClassWithCustomFormatterLvl2 value, FormattedObjectGraph output)
        {
            output.AddFragment("Property is " + value.Property);
        }

        [ValueFormatter]
        public static void Foo2(SomeClassInheritedFromClassWithCustomFormatterLvl2 value, FormattedObjectGraph output)
        {
            output.AddFragment("Property is " + value.Property);
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
            "    IntProperty = 0" + Environment.NewLine +
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

        protected override MemberInfo[] GetMembers(Type type)
        {
            return base
                .GetMembers(type)
                .Where(e => e.GetUnderlyingType() != typeof(string))
                .ToArray();
        }

        protected override string TypeDisplayName(Type type) => type.Name;
    }

    [Fact]
    public void When_defining_a_custom_enumerable_value_formatter_it_should_respect_the_overrides()
    {
        // Arrange
        var values = new CustomClass[]
        {
            new() { IntProperty = 1 },
            new() { IntProperty = 2 }
        };

        var formatter = new SingleItemValueFormatter();
        using var _ = new FormatterScope(formatter);

        // Act
        string str = Formatter.ToString(values);

        str.Should().Match(Environment.NewLine +
            "{*FluentAssertions*FormatterSpecs+CustomClass" + Environment.NewLine +
            "    {" + Environment.NewLine +
            "        IntProperty = 1, " + Environment.NewLine +
            "        StringProperty = <null>" + Environment.NewLine +
            "    },*…1 more…*}*");
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

// Due to the tests that call Configuration.Current
[CollectionDefinition("FormatterSpecs", DisableParallelization = true)]
public class FormatterSpecsDefinition;

internal class ExceptionThrowingClass
{
    public string ThrowingProperty => throw new InvalidOperationException("CustomMessage");
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

    public static Node Default { get; } = new();

    public List<Node> Children { get; set; }
}
