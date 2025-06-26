using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Xunit;

namespace FluentAssertions.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    [SuppressMessage("Class Design", "AV1010:Member hides inherited member")]
    public class MemberHiding
    {
        [Fact]
        public void Ignores_properties_hidden_by_the_derived_class()
        {
            // Arrange
            var subject = new SubclassAHidingProperty<string>
            {
                Property = "DerivedValue"
            };

            ((BaseWithProperty)subject).Property = "ActualBaseValue";

            var expectation = new SubclassBHidingProperty<string>
            {
                Property = "DerivedValue"
            };

            ((AnotherBaseWithProperty)expectation).Property = "ExpectedBaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void Ignores_properties_of_the_same_runtime_types_hidden_by_the_derived_class()
        {
            // Arrange
            var subject = new SubclassHidingStringProperty
            {
                Property = "DerivedValue"
            };

            ((BaseWithStringProperty)subject).Property = "ActualBaseValue";

            var expectation = new SubclassHidingStringProperty
            {
                Property = "DerivedValue"
            };

            ((BaseWithStringProperty)expectation).Property = "ExpectedBaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void Includes_hidden_property_of_the_base_when_using_a_reference_to_the_base()
        {
            // Arrange
            BaseWithProperty subject = new SubclassAHidingProperty<string>
            {
                Property = "ActualDerivedValue"
            };

            // FA doesn't know the compile-time type of the subject, so even though we pass a reference to the base-class,
            // at run-time, it'll start finding the property on the subject starting from the run-time type, and thus ignore the
            // hidden base-class field
            ((SubclassAHidingProperty<string>)subject).Property = "BaseValue";

            AnotherBaseWithProperty expectation = new SubclassBHidingProperty<string>
            {
                Property = "ExpectedDerivedValue"
            };

            expectation.Property = "BaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation);
        }

        [Fact]
        public void Run_type_typing_ignores_hidden_properties_even_when_using_a_reference_to_the_base_class()
        {
            // Arrange
            var subject = new SubclassAHidingProperty<string>
            {
                Property = "DerivedValue"
            };

            ((BaseWithProperty)subject).Property = "ActualBaseValue";

            AnotherBaseWithProperty expectation = new SubclassBHidingProperty<string>
            {
                Property = "DerivedValue"
            };

            expectation.Property = "ExpectedBaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, o => o.PreferringRuntimeMemberTypes());
        }

        [Fact]
        public void Including_the_derived_property_excludes_the_hidden_property()
        {
            // Arrange
            var subject = new SubclassAHidingProperty<string>
            {
                Property = "DerivedValue"
            };

            ((BaseWithProperty)subject).Property = "ActualBaseValue";

            var expectation = new SubclassBHidingProperty<string>
            {
                Property = "DerivedValue"
            };

            ((AnotherBaseWithProperty)expectation).Property = "ExpectedBaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, opt => opt
                .Including(o => o.Property));
        }

        [Fact]
        public void Excluding_the_property_hiding_the_base_class_one_does_not_reveal_the_latter()
        {
            // Arrange
            var subject = new SubclassAHidingProperty<string>();

            ((BaseWithProperty)subject).Property = "ActualBaseValue";

            var expectation = new SubclassBHidingProperty<string>();

            ((AnotherBaseWithProperty)expectation).Property = "ExpectedBaseValue";

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, o => o
                .Excluding(b => b.Property));

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("*No members were found *");
        }

        private class BaseWithProperty
        {
            [UsedImplicitly]
            public object Property { get; set; }
        }

        private class SubclassAHidingProperty<T> : BaseWithProperty
        {
            [UsedImplicitly]
            public new T Property { get; set; }
        }

        private class BaseWithStringProperty
        {
            [UsedImplicitly]
            public string Property { get; set; }
        }

        private class SubclassHidingStringProperty : BaseWithStringProperty
        {
            [UsedImplicitly]
            public new string Property { get; set; }
        }

        private class AnotherBaseWithProperty
        {
            [UsedImplicitly]
            public object Property { get; set; }
        }

        private class SubclassBHidingProperty<T> : AnotherBaseWithProperty
        {
            public new T Property
            {
                get;
                set;
            }
        }

        [Fact]
        public void Ignores_fields_hidden_by_the_derived_class()
        {
            // Arrange
            var subject = new SubclassAHidingField
            {
                Field = "DerivedValue"
            };

            ((BaseWithField)subject).Field = "ActualBaseValue";

            var expectation = new SubclassBHidingField
            {
                Field = "DerivedValue"
            };

            ((AnotherBaseWithField)expectation).Field = "ExpectedBaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options.IncludingFields());
        }

        [Fact]
        public void Includes_hidden_field_of_the_base_when_using_a_reference_to_the_base()
        {
            // Arrange
            BaseWithField subject = new SubclassAHidingField
            {
                Field = "BaseValueFromSubject"
            };

            // FA doesn't know the compile-time type of the subject, so even though we pass a reference to the base-class,
            // at run-time, it'll start finding the field on the subject starting from the run-time type, and thus ignore the
            // hidden base-class field
            ((SubclassAHidingField)subject).Field = "BaseValueFromExpectation";

            AnotherBaseWithField expectation = new SubclassBHidingField
            {
                Field = "ExpectedDerivedValue"
            };

            expectation.Field = "BaseValueFromExpectation";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options.IncludingFields());
        }

        [Fact]
        public void Run_type_typing_ignores_hidden_fields_even_when_using_a_reference_to_the_base_class()
        {
            // Arrange
            var subject = new SubclassAHidingField
            {
                Field = "DerivedValue"
            };

            ((BaseWithField)subject).Field = "ActualBaseValue";

            AnotherBaseWithField expectation = new SubclassBHidingField
            {
                Field = "DerivedValue"
            };

            expectation.Field = "ExpectedBaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options.IncludingFields().PreferringRuntimeMemberTypes());
        }

        [Fact]
        public void Including_the_derived_field_excludes_the_hidden_field()
        {
            // Arrange
            var subject = new SubclassAHidingField
            {
                Field = "DerivedValue"
            };

            ((BaseWithField)subject).Field = "ActualBaseValue";

            var expectation = new SubclassBHidingField
            {
                Field = "DerivedValue"
            };

            ((AnotherBaseWithField)expectation).Field = "ExpectedBaseValue";

            // Act / Assert
            subject.Should().BeEquivalentTo(expectation, options => options
                .IncludingFields()
                .Including(o => o.Field));
        }

        [Fact]
        public void Excluding_the_field_hiding_the_base_class_one_does_not_reveal_the_latter()
        {
            // Arrange
            var subject = new SubclassAHidingField();

            ((BaseWithField)subject).Field = "ActualBaseValue";

            var expectation = new SubclassBHidingField();

            ((AnotherBaseWithField)expectation).Field = "ExpectedBaseValue";

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
                .IncludingFields()
                .Excluding(b => b.Field));

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("*No members were found *");
        }

        private class BaseWithField
        {
            [UsedImplicitly]
            public string Field;
        }

        private class SubclassAHidingField : BaseWithField
        {
            [UsedImplicitly]
            public new string Field;
        }

        private class AnotherBaseWithField
        {
            [UsedImplicitly]
            public string Field;
        }

        private class SubclassBHidingField : AnotherBaseWithField
        {
            [UsedImplicitly]
            public new string Field;
        }
    }
}
