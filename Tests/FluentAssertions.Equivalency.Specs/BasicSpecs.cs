using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs;

public class BasicSpecs
{
    [Fact]
    public void A_null_configuration_is_invalid()
    {
        // Arrange
        var actual = new { };
        var expectation = new { };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expectation, config: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public void A_null_as_the_configuration_is_not_valid_for_inequivalency_assertions()
    {
        // Arrange
        var actual = new { };
        var expectation = new { };

        // Act
        Action act = () => actual.Should().NotBeEquivalentTo(expectation, config: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("config");
    }

    [Fact]
    public void When_expectation_is_null_it_should_throw()
    {
        // Arrange
        var subject = new { };

        // Act
        Action act = () => subject.Should().BeEquivalentTo<object>(null);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected subject to be <null>, but found { }*");
    }

    [Fact]
    public void When_comparing_nested_collection_with_a_null_value_it_should_fail_with_the_correct_message()
    {
        // Arrange
        var subject = new[] { new MyClass { Items = new[] { "a" } } };

        var expectation = new[] { new MyClass() };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expectation);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected*subject[0].Items*null*, but found*\"a\"*");
    }

    public class MyClass
    {
        public IEnumerable<string> Items { get; set; }
    }

    [Fact]
    public void When_subject_is_null_it_should_throw()
    {
        // Arrange
        SomeDto subject = null;

        // Act
        Action act = () => subject.Should().BeEquivalentTo(new { });

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected subject*to be*, but found <null>*");
    }

    [Fact]
    public void When_subject_and_expectation_are_null_it_should_not_throw()
    {
        // Arrange
        SomeDto subject = null;

        // Act
        Action act = () => subject.Should().BeEquivalentTo<object>(null);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_subject_and_expectation_are_compared_for_equivalence_it_should_allow_chaining()
    {
        // Arrange
        SomeDto subject = null;

        // Act
        Action act = () => subject.Should().BeEquivalentTo<object>(null)
            .And.BeNull();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_subject_and_expectation_are_compared_for_equivalence_with_config_it_should_allow_chaining()
    {
        // Arrange
        SomeDto subject = null;

        // Act
        Action act = () => subject.Should().BeEquivalentTo<object>(null, opt => opt)
            .And.BeNull();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_subject_and_expectation_are_compared_for_non_equivalence_it_should_allow_chaining()
    {
        // Arrange
        SomeDto subject = null;

        // Act
        Action act = () => subject.Should().NotBeEquivalentTo<object>(new { })
            .And.BeNull();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_subject_and_expectation_are_compared_for_non_equivalence_with_config_it_should_allow_chaining()
    {
        // Arrange
        SomeDto subject = null;

        // Act
        Action act = () => subject.Should().NotBeEquivalentTo<object>(new { }, opt => opt)
            .And.BeNull();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_equivalence_on_a_value_type_from_system_it_should_not_do_a_structural_comparision()
    {
        // Arrange

        // DateTime is used as an example because the current implementation
        // would hit the recursion-depth limit if structural equivalence were attempted.
        var date1 = new { Property = 1.January(2011) };

        var date2 = new { Property = 1.January(2011) };

        // Act
        Action act = () => date1.Should().BeEquivalentTo(date2);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_an_object_hides_object_equals_it_should_be_compared_using_its_members()
    {
        // Arrange
        var actual = new VirtualClassOverride { Property = "Value", OtherProperty = "Actual" };

        var expected = new VirtualClassOverride { Property = "Value", OtherProperty = "Expected" };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expected);

        // Assert
        act.Should().Throw<XunitException>("*OtherProperty*Expected*Actual*");
    }

    public class VirtualClass
    {
        public string Property { get; set; }

        public new virtual bool Equals(object obj)
        {
            return obj is VirtualClass other && other.Property == Property;
        }
    }

    public class VirtualClassOverride : VirtualClass
    {
        public string OtherProperty { get; set; }
    }

    [Fact]
    public void When_treating_a_value_type_in_a_collection_as_a_complex_type_it_should_compare_them_by_members()
    {
        // Arrange
        var subject = new[] { new ClassWithValueSemanticsOnSingleProperty { Key = "SameKey", NestedProperty = "SomeValue" } };

        var expected = new[]
        {
            new ClassWithValueSemanticsOnSingleProperty { Key = "SameKey", NestedProperty = "OtherValue" }
        };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected,
            options => options.ComparingByMembers<ClassWithValueSemanticsOnSingleProperty>());

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*NestedProperty*SomeValue*OtherValue*");
    }

    [Fact]
    public void When_treating_a_value_type_as_a_complex_type_it_should_compare_them_by_members()
    {
        // Arrange
        var subject = new ClassWithValueSemanticsOnSingleProperty { Key = "SameKey", NestedProperty = "SomeValue" };

        var expected = new ClassWithValueSemanticsOnSingleProperty { Key = "SameKey", NestedProperty = "OtherValue" };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected,
            options => options.ComparingByMembers<ClassWithValueSemanticsOnSingleProperty>());

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*NestedProperty*SomeValue*OtherValue*");
    }

    [Fact]
    public void When_treating_a_type_as_value_type_but_it_was_already_marked_as_reference_type_it_should_throw()
    {
        // Arrange
        var subject = new ClassWithValueSemanticsOnSingleProperty { Key = "Don't care" };

        var expected = new ClassWithValueSemanticsOnSingleProperty { Key = "Don't care" };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, options => options
            .ComparingByMembers<ClassWithValueSemanticsOnSingleProperty>()
            .ComparingByValue<ClassWithValueSemanticsOnSingleProperty>());

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage(
            $"*compare {nameof(ClassWithValueSemanticsOnSingleProperty)}*value*already*members*");
    }

    [Fact]
    public void When_treating_a_type_as_reference_type_but_it_was_already_marked_as_value_type_it_should_throw()
    {
        // Arrange
        var subject = new ClassWithValueSemanticsOnSingleProperty { Key = "Don't care" };

        var expected = new ClassWithValueSemanticsOnSingleProperty { Key = "Don't care" };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, options => options
            .ComparingByValue<ClassWithValueSemanticsOnSingleProperty>()
            .ComparingByMembers<ClassWithValueSemanticsOnSingleProperty>());

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage(
            $"*compare {nameof(ClassWithValueSemanticsOnSingleProperty)}*members*already*value*");
    }

    [Fact]
    public void When_treating_a_complex_type_in_a_collection_as_a_value_type_it_should_compare_them_by_value()
    {
        // Arrange
        var subject = new[] { new { Address = IPAddress.Parse("1.2.3.4"), Word = "a" } };

        var expected = new[] { new { Address = IPAddress.Parse("1.2.3.4"), Word = "a" } };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected,
            options => options.ComparingByValue<IPAddress>());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_treating_a_complex_type_as_a_value_type_it_should_compare_them_by_value()
    {
        // Arrange
        var subject = new { Address = IPAddress.Parse("1.2.3.4"), Word = "a" };

        var expected = new { Address = IPAddress.Parse("1.2.3.4"), Word = "a" };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected,
            options => options.ComparingByValue<IPAddress>());

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_treating_a_null_type_as_value_type_it_should_throw()
    {
        // Arrange
        var subject = new object();
        var expected = new object();

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByValue(null));

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("type");
    }

    [Fact]
    public void When_treating_a_null_type_as_reference_type_it_should_throw()
    {
        // Arrange
        var subject = new object();
        var expected = new object();

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByMembers(null));

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("type");
    }

    [Fact]
    public void When_comparing_an_open_type_by_members_it_should_succeed()
    {
        // Arrange
        var subject = new Option<int[]>(new[] { 1, 3, 2 });
        var expected = new Option<int[]>(new[] { 1, 2, 3 });

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByMembers(typeof(Option<>)));

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_threating_open_type_as_reference_type_and_a_closed_type_as_value_type_it_should_compare_by_value()
    {
        // Arrange
        var subject = new Option<int[]>(new[] { 1, 3, 2 });
        var expected = new Option<int[]>(new[] { 1, 2, 3 });

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByMembers(typeof(Option<>))
            .ComparingByValue<Option<int[]>>());

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_threating_open_type_as_value_type_and_a_closed_type_as_reference_type_it_should_compare_by_members()
    {
        // Arrange
        var subject = new Option<int[]>(new[] { 1, 3, 2 });
        var expected = new Option<int[]>(new[] { 1, 2, 3 });

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByValue(typeof(Option<>))
            .ComparingByMembers<Option<int[]>>());

        // Assert
        act.Should().NotThrow();
    }

    private readonly struct Option<T> : IEquatable<Option<T>>
        where T : class
    {
        public T Value { get; }

        public Option(T value)
        {
            Value = value;
        }

        public bool Equals(Option<T> other) =>
            EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override bool Equals(object obj) =>
            obj is Option<T> other && Equals(other);

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
    }

    [Fact]
    public void When_threating_any_type_as_reference_type_it_should_exclude_primitive_types()
    {
        // Arrange
        var subject = new { Value = 1 };
        var expected = new { Value = 2 };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByMembers<object>());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*be 2*found 1*");
    }

    [Fact]
    public void When_threating_an_open_type_as_reference_type_it_should_exclude_primitive_types()
    {
        // Arrange
        var subject = new { Value = 1 };
        var expected = new { Value = 2 };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByMembers(typeof(IEquatable<>)));

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*be 2*found 1*");
    }

    [Fact]
    public void When_threating_a_primitive_type_as_a_reference_type_it_should_throw()
    {
        // Arrange
        var subject = new { Value = 1 };
        var expected = new { Value = 2 };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected, opt => opt
            .ComparingByMembers<int>());

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Cannot compare a primitive type*Int32*");
    }

    [Fact]
    public void When_a_type_originates_from_the_System_namespace_it_should_be_treated_as_a_value_type()
    {
        // Arrange
        var subject = new { UriBuilder = new UriBuilder("http://localhost:9001/api"), };

        var expected = new { UriBuilder = new UriBuilder("https://localhost:9002/bapi"), };

        // Act
        Action act = () => subject.Should().BeEquivalentTo(expected);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expected*UriBuilder* to be https://localhost:9002/bapi, but found http://localhost:9001/api*");
    }

    [Fact]
    public void When_asserting_equivalence_on_a_string_it_should_use_string_specific_failure_messages()
    {
        // Arrange
        string s1 = "hello";
        string s2 = "good-bye";

        // Act
        Action act = () => s1.Should().BeEquivalentTo(s2);

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("*to be*\"good-bye\" with a length of 8, but \"hello\" has a length of 5*");
    }

    [Fact]
    public void When_asserting_equivalence_of_strings_typed_as_objects_it_should_compare_them_as_strings()
    {
        // Arrange

        // The convoluted construction is so the compiler does not optimize the two objects to be the same.
        object s1 = new string('h', 2);
        object s2 = "hh";

        // Act
        Action act = () => s1.Should().BeEquivalentTo(s2);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_asserting_equivalence_of_ints_typed_as_objects_it_should_use_the_runtime_type()
    {
        // Arrange
        object s1 = 1;
        object s2 = 1;

        // Act
        Action act = () => s1.Should().BeEquivalentTo(s2);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_all_field_of_the_object_are_equal_equivalency_should_pass()
    {
        // Arrange
        var object1 = new ClassWithOnlyAField { Value = 1 };
        var object2 = new ClassWithOnlyAField { Value = 1 };

        // Act
        Action act = () => object1.Should().BeEquivalentTo(object2);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_number_values_are_convertible_it_should_treat_them_as_equivalent()
    {
        // Arrange
        var actual = new Dictionary<string, long> { ["001"] = 1L, ["002"] = 2L };

        var expected = new Dictionary<string, int> { ["001"] = 1, ["002"] = 2 };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expected);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void When_all_field_of_the_object_are_not_equal_equivalency_should_fail()
    {
        // Arrange
        var object1 = new ClassWithOnlyAField { Value = 1 };
        var object2 = new ClassWithOnlyAField { Value = 101 };

        // Act
        Action act = () => object1.Should().BeEquivalentTo(object2);

        // Assert
        act.Should().Throw<XunitException>();
    }

    [Fact]
    public void When_a_field_on_the_subject_matches_a_property_the_members_should_match_for_equivalence()
    {
        // Arrange
        var onlyAField = new ClassWithOnlyAField { Value = 1 };
        var onlyAProperty = new ClassWithOnlyAProperty { Value = 101 };

        // Act
        Action act = () => onlyAField.Should().BeEquivalentTo(onlyAProperty);

        // Assert
        act.Should().Throw<XunitException>().WithMessage("Expected property onlyAField.Value*to be 101, but found 1.*");
    }

    [Fact]
    public void When_asserting_equivalence_including_only_fields_it_should_not_match_properties()
    {
        // Arrange
        var onlyAField = new ClassWithOnlyAField { Value = 1 };
        object onlyAProperty = new ClassWithOnlyAProperty { Value = 101 };

        // Act
        Action act = () => onlyAProperty.Should().BeEquivalentTo(onlyAField, opts => opts.ExcludingProperties());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expectation has field onlyAProperty.Value that the other object does not have.*");
    }

    [Fact]
    public void When_asserting_equivalence_including_only_properties_it_should_not_match_fields()
    {
        // Arrange
        var onlyAField = new ClassWithOnlyAField { Value = 1 };
        var onlyAProperty = new ClassWithOnlyAProperty { Value = 101 };

        // Act
        Action act = () => onlyAField.Should().BeEquivalentTo(onlyAProperty, opts => opts.IncludingAllDeclaredProperties());

        // Assert
        act.Should().Throw<XunitException>()
            .WithMessage("Expectation has property onlyAField.Value that the other object does not have*");
    }

    [Fact]
    public void
        When_asserting_equivalence_of_objects_including_enumerables_it_should_print_the_failure_message_only_once()
    {
        // Arrange
        var record = new { Member1 = "", Member2 = new[] { "", "" } };

        var record2 = new { Member1 = "different", Member2 = new[] { "", "" } };

        // Act
        Action act = () => record.Should().BeEquivalentTo(record2);

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            @"Expected property record.Member1* to be*""different"" with a length of 9, but*"""" has a length of 0*");
    }

    [Fact]
    public void When_asserting_object_equivalence_against_a_null_value_it_should_properly_throw()
    {
        // Act
        Action act = () => ((object)null).Should().BeEquivalentTo("foo");

        // Assert
        act.Should().Throw<XunitException>().WithMessage("*foo*null*");
    }

    [Fact]
    public void When_the_graph_contains_guids_it_should_properly_format_them()
    {
        // Arrange
        var actual =
            new[] { new { Id = Guid.NewGuid(), Name = "Name" } };

        var expected =
            new[] { new { Id = Guid.NewGuid(), Name = "Name" } };

        // Act
        Action act = () => actual.Should().BeEquivalentTo(expected);

        // Assert
        act.Should().Throw<XunitException>().WithMessage("Expected property actual[0].Id*to be *-*, but found *-*");
    }

    [Fact]
    public void Empty_array_segments_can_be_compared_for_equivalency()
    {
        // Arrange
        var actual = new ClassWithArraySegment();
        var expected = new ClassWithArraySegment();

        // Act / Assert
        actual.Should().BeEquivalentTo(expected);
    }

    private class ClassWithArraySegment
    {
        public ArraySegment<byte> Segment { get; set; }
    }
}
