using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class RecordSpecs
{
    [Fact]
    public async Task When_the_subject_is_a_record_it_should_compare_it_by_its_members()
    {
        var actual = new MyRecord { StringField = "foo", CollectionProperty = new[] { "bar", "zip", "foo" } };

        var expected = new MyRecord { StringField = "foo", CollectionProperty = new[] { "foo", "bar", "zip" } };

        await actual.Should().BeEquivalentToAsync(expected);
    }

    [Fact]
    public async Task When_the_subject_is_a_record_struct_it_should_compare_it_by_its_members()
    {
        var actual = new MyRecordStruct("foo", new[] { "bar", "zip", "foo" });

        var expected = new MyRecordStruct("foo", new[] { "bar", "zip", "foo" });

        await actual.Should().BeEquivalentToAsync(expected);
    }

    [Fact]
    public async Task When_the_subject_is_a_record_it_should_mention_that_in_the_configuration_output()
    {
        var actual = new MyRecord { StringField = "foo", };

        var expected = new MyRecord { StringField = "bar", };

        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected);

        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Compare records by their members*");
    }

    [Fact]
    public async Task When_a_record_should_be_treated_as_a_value_type_it_should_use_its_equality_for_comparing()
    {
        var actual = new MyRecord { StringField = "foo", CollectionProperty = new[] { "bar", "zip", "foo" } };

        var expected = new MyRecord { StringField = "foo", CollectionProperty = new[] { "foo", "bar", "zip" } };

        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, o => o
            .ComparingByValue<MyRecord>());

        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Expected*MyRecord*but found*MyRecord*")
            .WithMessage("*Compare*MyRecord by value*");
    }

    [Fact]
    public async Task When_all_records_should_be_treated_as_value_types_it_should_use_equality_for_comparing()
    {
        var actual = new MyRecord { StringField = "foo", CollectionProperty = new[] { "bar", "zip", "foo" } };

        var expected = new MyRecord { StringField = "foo", CollectionProperty = new[] { "foo", "bar", "zip" } };

        Func<Task> act = async () => await actual.Should().BeEquivalentToAsync(expected, o => o
            .ComparingRecordsByValue());

        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*Expected*MyRecord*but found*MyRecord*")
            .WithMessage("*Compare records by value*");
    }

    [Fact]
    public async Task
        When_all_records_except_a_specific_type_should_be_treated_as_value_types_it_should_compare_that_specific_type_by_its_members()
    {
        var actual = new MyRecord { StringField = "foo", CollectionProperty = new[] { "bar", "zip", "foo" } };

        var expected = new MyRecord { StringField = "foo", CollectionProperty = new[] { "foo", "bar", "zip" } };

        await actual.Should().BeEquivalentToAsync(expected, o => o
            .ComparingRecordsByValue()
            .ComparingByMembers<MyRecord>());
    }

    [Fact]
    public async Task When_global_record_comparing_options_are_chained_it_should_ensure_the_last_one_wins()
    {
        var actual = new MyRecord { CollectionProperty = new[] { "bar", "zip", "foo" } };

        var expected = new MyRecord { CollectionProperty = new[] { "foo", "bar", "zip" } };

        await actual.Should().BeEquivalentToAsync(expected, o => o
            .ComparingRecordsByValue()
            .ComparingRecordsByMembers());
    }

    private record MyRecord
    {
        public string StringField;

        public string[] CollectionProperty { get; init; }
    }

    private record struct MyRecordStruct(string StringField, string[] CollectionProperty)
    {
        public string StringField = StringField;
    }
}
