using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    public class Basic
    {
        [Fact]
        public async Task Property_names_are_case_sensitive()
        {
            // Arrange
            var subject = new
            {
                Name = "John"
            };

            var other = new
            {
                name = "John"
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(other);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expectation*subject.name**other*not have*");
        }

        [Fact]
        public async Task Field_names_are_case_sensitive()
        {
            // Arrange
            var subject = new ClassWithFieldInUpperCase
            {
                Name = "John"
            };

            var other = new ClassWithFieldInLowerCase
            {
                name = "John"
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(other);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expectation*subject.name**other*not have*");
        }

        private class ClassWithFieldInLowerCase
        {
            [JetBrains.Annotations.UsedImplicitly]
#pragma warning disable SA1307
            public string name;
#pragma warning restore SA1307
        }

        private class ClassWithFieldInUpperCase
        {
            [JetBrains.Annotations.UsedImplicitly]
            public string Name;
        }

        [Fact]
        public async Task When_a_property_is_an_indexer_it_should_be_ignored()
        {
            // Arrange
            var expected = new ClassWithIndexer
            {
                Foo = "test"
            };

            var result = new ClassWithIndexer
            {
                Foo = "test"
            };

            // Act
            Func<Task> act = () => result.Should().BeEquivalentToAsync(expected);

            // Assert
            await act.Should().NotThrowAsync();
        }

        public class ClassWithIndexer
        {
            public object Foo { get; set; }

            public string this[int n] =>
                n.ToString(
                    CultureInfo.InvariantCulture);
        }

        [Fact]
        public async Task When_the_expected_object_has_a_property_not_available_on_the_subject_it_should_throw()
        {
            // Arrange
            var subject = new
            {
            };

            var other = new
            {
                // ReSharper disable once StringLiteralTypo
                City = "Rijswijk"
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(other);

            // Assert
            await act.Should().ThrowAsync<XunitException>().WithMessage(
                "Expectation has property subject.City that the other object does not have*");
        }

        [Fact]
        public async Task When_equally_named_properties_are_type_incompatible_it_should_throw()
        {
            // Arrange
            var subject = new
            {
                Type = "A"
            };

            var other = new
            {
                Type = 36
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(other);

            // Assert
            await act
                .Should().ThrowAsync<XunitException>()
                .WithMessage("Expected property subject.Type to be 36, but found*\"A\"*");
        }

        [Fact]
        public async Task When_multiple_properties_mismatch_it_should_report_all_of_them()
        {
            // Arrange
            var subject = new
            {
                Property1 = "A",
                Property2 = "B",
                SubType1 = new
                {
                    SubProperty1 = "C",
                    SubProperty2 = "D"
                }
            };

            var other = new
            {
                Property1 = "1",
                Property2 = "2",
                SubType1 = new
                {
                    SubProperty1 = "3",
                    SubProperty2 = "D"
                }
            };

            // Act
            Func<Task> act = () => subject.Should().BeEquivalentToAsync(other);

            // Assert
            await act
                .Should().ThrowAsync<XunitException>()
                .WithMessage("*property subject.Property1*to be \"1\", but \"A\" differs near \"A\"*")
                .WithMessage("*property subject.Property2*to be \"2\", but \"B\" differs near \"B\"*")
                .WithMessage("*property subject.SubType1.SubProperty1*to be \"3\", but \"C\" differs near \"C\"*");
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task Including_all_declared_properties_excludes_all_fields()
        {
            // Arrange
            var class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor",
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "consectetur"
            };

            var class2 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "foo",
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "consectetur"
            };

            // Act
            Func<Task> act =
                () => class1.Should().BeEquivalentToAsync(class2, opts => opts.IncludingAllDeclaredProperties());

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task Including_all_runtime_properties_excludes_all_fields()
        {
            // Arrange
            object class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Field2 = "ipsum",
                Field3 = "dolor",
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "consectetur"
            };

            object class2 = new ClassWithSomeFieldsAndProperties
            {
                Property1 = "sit",
                Property2 = "amet",
                Property3 = "consectetur"
            };

            // Act
            Func<Task> act =
                () => class1.Should().BeEquivalentToAsync(class2, opts => opts.IncludingAllRuntimeProperties());

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task Respecting_the_runtime_type_includes_both_properties_and_fields_included()
        {
            // Arrange
            object class1 = new ClassWithSomeFieldsAndProperties
            {
                Field1 = "Lorem",
                Property1 = "sit"
            };

            object class2 = new ClassWithSomeFieldsAndProperties();

            // Act
            Func<Task> act =
                () => class1.Should().BeEquivalentToAsync(class2, opts => opts.RespectingRuntimeTypes());

            // Assert
            (await act.Should().ThrowAsync<XunitException>()).Which.Message.Should().Contain("Field1").And.Contain("Property1");
        }

        [Fact]
        public async Task A_nested_class_without_properties_inside_a_collection_is_fine()
        {
            // Arrange
            var sut = new List<BaseClassPointingToClassWithoutProperties>
            {
                new()
                {
                    Name = "theName"
                }
            };

            // Act / Assert
            await sut.Should().BeEquivalentToAsync(new[]
            {
                new BaseClassPointingToClassWithoutProperties
                {
                    Name = "theName"
                }
            });
        }

        internal class BaseClassPointingToClassWithoutProperties
        {
            public string Name { get; set; }

            public ClassWithoutProperty ClassWithoutProperty { get; } = new();
        }

        internal class ClassWithoutProperty
        {
        }
    }
}
