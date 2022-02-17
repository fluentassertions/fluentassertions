using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class MemberLessObjectsSpecs
    {
        [Fact]
        public void When_asserting_instances_of_an_anonymous_type_having_no_members_are_equivalent_it_should_fail()
        {
            // Arrange / Act
            Action act = () => new { }.Should().BeEquivalentTo(new { });

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_asserting_instances_of_a_class_having_no_members_are_equivalent_it_should_fail()
        {
            // Arrange / Act
            Action act = () => new ClassWithNoMembers().Should().BeEquivalentTo(new ClassWithNoMembers());

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_asserting_instances_of_Object_are_equivalent_it_should_fail()
        {
            // Arrange / Act
            Action act = () => new object().Should().BeEquivalentTo(new object());

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_asserting_instance_of_object_is_equivalent_to_null_it_should_fail_with_a_descriptive_message()
        {
            // Arrange
            object actual = new object();
            object expected = null;

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, "we want to test the failure {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected*to be <null>*we want to test the failure message*, but found System.Object*");
        }

        [Fact]
        public void When_asserting_null_is_equivalent_to_instance_of_object_it_should_fail()
        {
            // Arrange
            object actual = null;
            object expected = new object();

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*Expected*to be System.Object*but found <null>*");
        }

        [Fact]
        public void When_an_type_only_exposes_fields_but_fields_are_ignored_in_the_equivalence_comparision_it_should_fail()
        {
            // Arrange
            var object1 = new ClassWithOnlyAField { Value = 1 };
            var object2 = new ClassWithOnlyAField { Value = 101 };

            // Act
            Action act = () => object1.Should().BeEquivalentTo(object2, opts => opts.IncludingAllDeclaredProperties());

            // Assert
            act.Should().Throw<InvalidOperationException>("the objects have no members to compare.");
        }

        [Fact]
        public void
            When_an_type_only_exposes_properties_but_properties_are_ignored_in_the_equivalence_comparision_it_should_fail()
        {
            // Arrange
            var object1 = new ClassWithOnlyAProperty { Value = 1 };
            var object2 = new ClassWithOnlyAProperty { Value = 101 };

            // Act
            Action act = () => object1.Should().BeEquivalentTo(object2, opts => opts.ExcludingProperties());

            // Assert
            act.Should().Throw<InvalidOperationException>("the objects have no members to compare.");
        }

        [Fact]
        public void When_asserting_instances_of_arrays_of_types_in_System_are_equivalent_it_should_respect_the_runtime_type()
        {
            // Arrange
            object actual = new int[0];
            object expectation = new int[0];

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expectation);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_throwing_on_missing_members_and_there_are_no_missing_members_should_not_throw()
        {
            // Arrange
            var subject = new { Version = 2, Age = 36, };

            var expectation = new { Version = 2, Age = 36 };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation,
                options => options.ThrowingOnMissingMembers());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_throwing_on_missing_members_and_there_is_a_missing_member_should_throw()
        {
            // Arrange
            var subject = new { Version = 2 };

            var expectation = new { Version = 2, Age = 36 };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation,
                options => options.ThrowingOnMissingMembers());

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expectation has property subject.Age that the other object does not have*");
        }

        [Fact]
        public void When_throwing_on_missing_members_and_there_is_an_additional_property_on_subject_should_not_throw()
        {
            // Arrange
            var subject = new { Version = 2, Age = 36, Additional = 13 };

            var expectation = new { Version = 2, Age = 36 };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation,
                options => options.ThrowingOnMissingMembers());

            // Assert
            act.Should().NotThrow();
        }
    }
}
