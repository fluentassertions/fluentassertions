using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using FluentAssertions.Equivalency;
using FluentAssertions.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    /// <summary>
    /// Test Class containing specs over the extensibility points of Should().BeEquivalentTo
    /// </summary>
    public class ExtensibilityRelatedEquivalencySpecs
    {
        #region Selection Rules

        [Fact]
        public void When_a_selection_rule_is_added_it_should_be_evaluated_after_all_existing_rules()
        {
            // Arrange
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                SomeValue = "hello"
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(
                expected,
                options => options.Using(new ExcludeForeignKeysSelectionRule()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_selection_rule_is_added_it_should_appear_in_the_exception_message()
        {
            // Arrange
            var subject = new
            {
                Name = "123",
            };

            var expected = new
            {
                SomeValue = "hello"
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(
                expected,
                options => options.Using(new ExcludeForeignKeysSelectionRule()));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(string.Format("*{0}*", typeof(ExcludeForeignKeysSelectionRule).Name));
        }

        internal class ExcludeForeignKeysSelectionRule : IMemberSelectionRule
        {
            public bool OverridesStandardIncludeRules
            {
                get { return false; }
            }

            public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, IMemberInfo context, IEquivalencyAssertionOptions config)
            {
                return selectedMembers.Where(pi => !pi.Name.EndsWith("Id")).ToArray();
            }

            bool IMemberSelectionRule.IncludesMembers
            {
                get { return OverridesStandardIncludeRules; }
            }
        }

        #endregion

        #region Matching Rules

        [Fact]
        public void When_a_matching_rule_is_added_it_should_precede_all_existing_rules()
        {
            // Arrange
            var subject = new
            {
                Name = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(
                expected,
                options => options.Using(new ForeignKeyMatchingRule()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_matching_rule_is_added_it_should_appear_in_the_exception_message()
        {
            // Arrange
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                Name = "1234",
                SomeValue = "hello"
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(
                expected,
                options => options.Using(new ForeignKeyMatchingRule()));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(string.Format("*{0}*", typeof(ForeignKeyMatchingRule).Name));
        }

        internal class ForeignKeyMatchingRule : IMemberMatchingRule
        {
            public SelectedMemberInfo Match(SelectedMemberInfo expectedMember, object subject, string memberPath, IEquivalencyAssertionOptions config)
            {
                string name = expectedMember.Name;
                if (name.EndsWith("Id"))
                {
                    name = name.Replace("Id", "");
                }

                return SelectedMemberInfo.Create(subject.GetType().GetRuntimeProperty(name));
            }
        }

        #endregion

        #region Assertion Rules

        [Fact]
        public void When_property_of_other_is_incompatible_with_generic_type_the_message_should_include_generic_type()
        {
            // Arrange
            var subject = new
            {
                Id = "foo",
            };

            var other = new
            {
                Id = 0.5d,
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other,
                o => o
                    .Using<string>(c => c.Subject.Should().Be(c.Expectation))
                    .When(si => si.SelectedMemberPath == "Id"));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*member Id from expectation*System.String*System.Double*");
        }

        [Fact]
        public void When_property_of_subject_is_incompatible_with_generic_type_the_message_should_include_generic_type()
        {
            // Arrange
            var subject = new
            {
                Id = 0.5d,
            };

            var other = new
            {
                Id = "foo",
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other,
                o => o
                    .Using<string>(c => c.Subject.Should().Be(c.Expectation))
                    .When(si => si.SelectedMemberPath == "Id"));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*member Id from subject*System.String*System.Double*");
        }

        [Fact]
        public void When_equally_named_properties_are_both_incompatible_with_generic_type_the_message_should_include_generic_type()
        {
            // Arrange
            var subject = new
            {
                Id = 0.5d,
            };

            var other = new
            {
                Id = 0.5d,
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other,
                o => o
                    .Using<string>(c => c.Subject.Should().Be(c.Expectation))
                    .When(si => si.SelectedMemberPath == "Id"));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*member Id from subject*System.String*System.Double*member Id from expectation*System.String*System.Double*");
        }

        [Fact]
        public void When_property_of_other_is_null_the_failure_message_should_not_complain_about_its_type()
        {
            // Arrange
            var subject = new
            {
                Id = "foo",
            };

            var other = new
            {
                Id = null as double?,
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other,
                o => o
                    .Using<string>(c => c.Subject.Should().Be(c.Expectation))
                    .When(si => si.SelectedMemberPath == "Id"));

            // Assert
            act.Should().Throw<XunitException>()
                .Which.Message.Should()
                .Contain("Expected member Id to be <null>, but found \"foo\"")
                    .And.NotContain("from expectation");
        }

        [Fact]
        public void When_property_of_subject_is_null_the_failure_message_should_not_complain_about_its_type()
        {
            // Arrange
            var subject = new
            {
                Id = null as double?,
            };

            var other = new
            {
                Id = "bar",
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other,
                o => o
                    .Using<string>(c => c.Subject.Should().Be(c.Expectation))
                    .When(si => si.SelectedMemberPath == "Id"));

            // Assert
            act.Should().Throw<XunitException>()
                .Which.Message.Should()
                .Contain("Expected member Id to be \"bar\", but found <null>")
                    .And.NotContain("from subject");
        }

        [Fact]
        public void When_equally_named_properties_are_both_null_it_should_succeed()
        {
            // Arrange
            var subject = new
            {
                Id = null as double?,
            };

            var other = new
            {
                Id = null as string,
            };

            // Act / Assert
            subject.Should().BeEquivalentTo(other,
                o => o
                    .Using<string>(c => c.Subject.Should().Be(c.Expectation))
                    .When(si => si.SelectedMemberPath == "Id"));
        }

        [Fact]
        public void When_equally_named_properties_are_type_incompatible_and_assertion_rule_exists_it_should_not_throw()
        {
            // Arrange
            var subject = new
            {
                Type = typeof(string),
            };

            var other = new
            {
                Type = typeof(string).AssemblyQualifiedName,
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(other,
                o => o
                    .Using<object>(c => ((Type)c.Subject).AssemblyQualifiedName.Should().Be((string)c.Expectation))
                    .When(si => si.SelectedMemberPath == "Type"));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assertion_is_overridden_for_a_predicate_it_should_use_the_provided_action()
        {
            // Arrange
            var subject = new
            {
                Date = 14.July(2012).At(12, 59, 59)
            };

            var expectation = new
            {
                Date = 14.July(2012).At(13, 0, 0)
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
                .When(info => info.SelectedMemberPath.EndsWith("Date")));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assertion_is_overridden_for_all_types_it_should_use_the_provided_action_for_all_properties()
        {
            // Arrange
            var subject = new
            {
                Date = 21.July(2012).At(11, 8, 59),
                Nested = new
                {
                    NestedDate = 14.July(2012).At(12, 59, 59)
                }
            };

            var expectation = new
            {
                Date = 21.July(2012).At(11, 9, 0),
                Nested = new
                {
                    NestedDate = 14.July(2012).At(13, 0, 0)
                }
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options =>
                options
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
                    .WhenTypeIs<DateTime>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_nullable_property_is_overriden_with_a_custom_assertion_it_should_use_it()
        {
            // Arrange
            var actual = new SimpleWithNullable
            {
                NullableIntegerProperty = 1,
                StringProperty = "I haz a string!"
            };

            var expected = new SimpleWithNullable
            {
                StringProperty = "I haz a string!"
            };

            // Act / Assert
            actual.Should().BeEquivalentTo(expected,
                opt => opt.Using<long>(c => c.Subject.Should().BeInRange(0, 10)).WhenTypeIs<long?>());
        }

        internal class SimpleWithNullable
        {
            public long? NullableIntegerProperty { get; set; }

            public string StringProperty { get; set; }
        }

        [Fact]
        public void When_an_assertion_rule_is_added_it_should_precede_all_existing_rules()
        {
            // Arrange
            var subject = new
            {
                Created = 8.July(2012).At(22, 9)
            };

            var expected = new
            {
                Created = 8.July(2012).At(22, 10)
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(
                expected,
                options => options.Using(new RelaxingDateTimeEquivalencyStep()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_assertion_rule_is_added_it_appear_in_the_exception_message()
        {
            // Arrange
            var subject = new
            {
                Property = 8.July(2012).At(22, 9)
            };

            var expected = new
            {
                Property = 8.July(2012).At(22, 11)
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(
                expected,
                options => options.Using(new RelaxingDateTimeEquivalencyStep()));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage($"*{typeof(RelaxingDateTimeEquivalencyStep).Name}*");
        }

        [Fact]
        public void When_multiple_steps_are_added_they_should_be_evaluated_first_to_last()
        {
            // Arrange
            var subject = new
            {
                Created = 8.July(2012).At(22, 9)
            };

            var expected = new
            {
                Created = 8.July(2012).At(22, 10)
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected, opts => opts
                .Using(new RelaxingDateTimeEquivalencyStep())
                .Using(new AlwaysFailOnDateTimesEquivalencyStep()));

            // Assert
            act.Should().NotThrow(
                "a different assertion rule should handle the comparison before the exception throwing assertion rule is hit");
        }

        internal class AlwaysFailOnDateTimesEquivalencyStep : IEquivalencyStep
        {
            public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
            {
                return context.Expectation is DateTime;
            }

            public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config) =>
                throw new Exception("Failed");
        }

        internal class RelaxingDateTimeEquivalencyStep : IEquivalencyStep
        {
            public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
            {
                return context.Expectation is DateTime;
            }

            public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                ((DateTime)context.Subject).Should().BeCloseTo((DateTime)context.Expectation, 1.Minutes());
                return true;
            }
        }

        [Fact]
        public void When_multiple_assertion_rules_are_added_with_the_fluent_api_they_should_be_executed_from_right_to_left()
        {
            // Arrange
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            // Act
            Action act =
                () =>
                    subject.Should().BeEquivalentTo(expected,
                        opts =>
                            opts.Using<object>(context => throw new Exception())
                                .When(s => true)
                                .Using<object>(context => { })
                                .When(s => true));

            // Assert
            act.Should().NotThrow(
                "a different assertion rule should handle the comparison before the exception throwing assertion rule is hit");
        }

        [Fact]
        public void When_using_a_nested_equivalency_api_in_a_custom_assertion_rule_it_should_honor_the_rule()
        {
            // Arrange
            var subject = new ClassWithSomeFieldsAndProperties
            {
                Property1 = "value1",
                Property2 = "value2"
            };

            var expectation = new ClassWithSomeFieldsAndProperties
            {
                Property1 = "value1",
                Property2 = "value3"
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expectation, options => options
                .Using<ClassWithSomeFieldsAndProperties>(ctx => ctx.Subject.Should().BeEquivalentTo(ctx.Expectation, nestedOptions => nestedOptions.Excluding(x => x.Property2)))
                .WhenTypeIs<ClassWithSomeFieldsAndProperties>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_predicate_matches_after_auto_conversion_it_should_execute_the_assertion()
        {
            //Arrange
            var expectation = new
            {
                ThisIsMyDateTime = DateTime.Now
            };

            var actual = new
            {
                ThisIsMyDateTime = expectation.ThisIsMyDateTime.ToString(CultureInfo.InvariantCulture)
            };

            //Asserts
            actual.Should().BeEquivalentTo(expectation,
                options => options
                    .WithAutoConversion()
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
                    .WhenTypeIs<DateTime>());
        }

        #endregion

        #region Equivalency Steps

        [Fact]
        public void When_an_equivalency_step_handles_the_comparison_later_equivalency_steps_should_not_be_ran()
        {
            // Arrange
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            // Act
            Action act =
                () =>
                    subject.Should().BeEquivalentTo(expected,
                        opts =>
                            opts.Using(new AlwaysHandleEquivalencyStep())
                                .Using(new ThrowExceptionEquivalencyStep<InvalidOperationException>()));

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_user_equivalency_step_is_registered_it_should_run_before_the_built_in_steps()
        {
            // Arrange
            var actual = new
            {
                Property = 123
            };

            var expected = new
            {
                Property = "123"
            };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, options => options
                .Using(new EqualityEquivalencyStep()));

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*123*123*");
        }

        [Fact]
        public void When_an_equivalency_does_not_handle_the_comparison_later_equivalency_steps_should_still_be_ran()
        {
            // Arrange
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            // Act
            Action act =
                () =>
                    subject.Should().BeEquivalentTo(expected,
                        opts =>
                            opts.Using(new NeverHandleEquivalencyStep())
                                .Using(new ThrowExceptionEquivalencyStep<InvalidOperationException>()));

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_multiple_equivalency_steps_are_added_they_should_be_executed_in_registration_order()
        {
            // Arrange
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            // Act
            Action act =
                () =>
                    subject.Should().BeEquivalentTo(expected,
                        opts =>
                            opts.Using(new ThrowExceptionEquivalencyStep<NotSupportedException>())
                                .Using(new ThrowExceptionEquivalencyStep<InvalidOperationException>()));

            // Assert
            act.Should().Throw<NotSupportedException>();
        }

        internal class ThrowExceptionEquivalencyStep<TException> : CanHandleAnythingEquivalencyStep where TException : Exception, new()
        {
            public override bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                throw new TException();
            }
        }

        internal class AlwaysHandleEquivalencyStep : CanHandleAnythingEquivalencyStep
        {
            public override bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                return true;
            }
        }

        internal class NeverHandleEquivalencyStep : CanHandleAnythingEquivalencyStep
        {
            public override bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                return false;
            }
        }

        private class EqualityEquivalencyStep : CanHandleAnythingEquivalencyStep
        {
            public override bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                context.Subject.Should().Be(context.Expectation, context.Because, context.BecauseArgs);

                return true;
            }
        }

        internal class DoEquivalencyStep : CanHandleAnythingEquivalencyStep
        {
            private readonly Action doAction;

            public DoEquivalencyStep(Action doAction)
            {
                this.doAction = doAction;
            }

            public override bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                doAction();
                return true;
            }
        }

        internal abstract class CanHandleAnythingEquivalencyStep : IEquivalencyStep
        {
            public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
            {
                return true;
            }

            public abstract bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config);
        }

        #endregion
    }
}
