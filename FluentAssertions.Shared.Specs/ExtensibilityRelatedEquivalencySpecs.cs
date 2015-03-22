using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssertions.Equivalency;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    /// <summary>
    /// Test Class containing specs over the extensibility points of ShouldBeEquivalentTo
    /// </summary>
    [TestClass]
    public class ExtensibilityRelatedEquivalencySpecs
    {
        #region Selection Rules

        [TestMethod]
        public void When_a_selection_rule_is_added_it_should_be_evaluated_after_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ExcludeForeignKeysSelectionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_selection_rule_is_added_it_should_appear_in_the_exception_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Name = "123",
            };

            var expected = new
            {
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ExcludeForeignKeysSelectionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("*{0}*", typeof(ExcludeForeignKeysSelectionRule).Name));
        }

        internal class ExcludeForeignKeysSelectionRule : IMemberSelectionRule
        {
            public bool OverridesStandardIncludeRules
            {
                get { return false; }
            }

            public IEnumerable<SelectedMemberInfo> SelectMembers(IEnumerable<SelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
            {
                return selectedMembers.Where(pi => !pi.Name.EndsWith("Id")).ToArray();
            }

            bool IMemberSelectionRule.IncludesMembers
            {
                get { return OverridesStandardIncludeRules; }
            }
        }

        #region Obsolete

        [TestMethod]
        [Obsolete]
        public void When_an_obsolete_selection_rule_is_added_it_should_be_evaluated_after_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ObsoleteExcludeForeignKeysSelectionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        [Obsolete]
        public void When_an_obsolete_selection_rule_is_added_it_should_appear_in_the_exception_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Name = "123",
            };

            var expected = new
            {
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ObsoleteExcludeForeignKeysSelectionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("*{0}*", typeof(ExcludeForeignKeysSelectionRule).Name));
        }

        [Obsolete]
        internal class ObsoleteExcludeForeignKeysSelectionRule : ISelectionRule
        {
            public IEnumerable<PropertyInfo> SelectProperties(IEnumerable<PropertyInfo> selectedProperties, ISubjectInfo context)
            {
                return selectedProperties.Where(pi => !pi.Name.EndsWith("Id")).ToArray();
            }
        }

        #endregion

        #endregion

        #region Matching Rules

        [TestMethod]
        public void When_a_matching_rule_is_added_it_should_preceed_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                Name = "123",
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ForeignKeyMatchingRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_matching_rule_is_added_it_should_appear_in_the_exception_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
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

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ForeignKeyMatchingRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("*{0}*", typeof(ForeignKeyMatchingRule).Name));
        }

        internal class ForeignKeyMatchingRule : IMemberMatchingRule
        {
            public SelectedMemberInfo Match(SelectedMemberInfo subjectMember, object expectation, string memberPath, IEquivalencyAssertionOptions config)
            {
                string name = subjectMember.Name;
                if (name.EndsWith("Id"))
                {
                    name = name.Replace("Id", "");
                }

#if !WINRT && !WINDOWS_PHONE_APP
                return SelectedMemberInfo.Create(expectation.GetType().GetProperty(name));
#else
                return SelectedMemberInfo.Create(expectation.GetType()
                                  .GetRuntimeProperty(name));
#endif
            }
        }

        #region Obsolete

        [TestMethod]
        [Obsolete]
        public void When_an_obsolete_matching_rule_is_added_it_should_preceed_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                NameId = "123",
                SomeValue = "hello"
            };

            var expected = new
            {
                Name = "123",
                SomeValue = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ObsoleteForeignKeyMatchingRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        [Obsolete]
        public void When_an_obsolete_matching_rule_is_added_it_should_appear_in_the_exception_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
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

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new ObsoleteForeignKeyMatchingRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("*{0}*", typeof(ObsoleteForeignKeyMatchingRule).Name));
        }

        [Obsolete]
        internal class ObsoleteForeignKeyMatchingRule : IMatchingRule
        {
            public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
            {
                string name = subjectProperty.Name;
                if (name.EndsWith("Id"))
                {
                    name = name.Replace("Id", "");
                }

#if !WINRT && !WINDOWS_PHONE_APP
                return expectation.GetType().GetProperty(name);
#else
                return expectation.GetType()
                                  .GetRuntimeProperty(name);
#endif
            }
        }

        #endregion

        #endregion

        #region Assertion Rules
        
        [TestMethod]
        public void When_equally_named_properties_are_type_incompatible_and_assertion_rule_exists_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Type = typeof(String),
            };

            var other = new
            {
                Type = typeof(String).AssemblyQualifiedName,
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(other,
                o => o
                    .Using<object>(c => ((Type)c.Subject).AssemblyQualifiedName.Should().Be((string)c.Expectation))
                    .When(si => si.SelectedMemberPath == "Type"));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_assertion_is_overridden_for_a_predicate_it_should_use_the_provided_action()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Date = 14.July(2012).At(12, 59, 59)
            };

            var expectation = new
            {
                Date = 14.July(2012).At(13, 0, 0)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expectation, options => options
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                .When(info => info.SelectedMemberPath.EndsWith("Date")));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_assertion_is_overridden_for_all_types_it_should_use_the_provided_action_for_all_properties()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
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

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(expectation, options =>
                options
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                    .WhenTypeIs<DateTime>());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_a_nullable_property_is_overriden_with_a_custom_asserrtion_it_should_use_it()
        {
            var actual = new SimpleWithNullable
            {
                nullableIntegerProperty = 1,
                strProperty = "I haz a string!"
            };

            var expected = new SimpleWithNullable
            {
                strProperty = "I haz a string!"
            };

            actual.ShouldBeEquivalentTo(expected,
                opt => opt.Using<Int64>(c => c.Subject.Should().BeInRange(0, 10)).WhenTypeIs<Int64>());
        }

        internal class SimpleWithNullable
        {
            public Int64? nullableIntegerProperty { get; set; }

            public string strProperty { get; set; }
        }

        [TestMethod]
        public void When_an_assertion_rule_is_added_it_should_preceed_all_existing_rules()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Created = 8.July(2012).At(22, 9)
            };

            var expected = new
            {
                Created = 8.July(2012).At(22, 10)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new RelaxingDateTimeAssertionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_assertion_rule_is_added_it_appear_in_the_exception_message()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = 8.July(2012).At(22, 9);

            var expected = 8.July(2012).At(22, 10);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new RelaxingDateTimeAssertionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format("*{0}*", typeof(RelaxingDateTimeAssertionRule).Name));
        }

        [TestMethod]
        public void When_an_assertion_rule_matches_the_root_object_the_assertion_rule_should_not_apply_to_the_root_object()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = 8.July(2012).At(22, 9);

            var expected = 8.July(2012).At(22, 10);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.ShouldBeEquivalentTo(
                expected,
                options => options.Using(new RelaxingDateTimeAssertionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void When_multiple_asertion_rules_are_added__they_should_be_executed_from_right_to_left()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Created = 8.July(2012).At(22, 9)
            };

            var expected = new
            {
                Created = 8.July(2012).At(22, 10)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    subject.ShouldBeEquivalentTo(expected,
                        opts => opts.Using(new AlwaysFailAssertionRule()).Using(new RelaxingDateTimeAssertionRule()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow(
                "a different assertion rule should handle the comparision before the exception throwing assertion rule is hit");
        }

        [TestMethod]
        public void When_an_assertion_rule_added_with_the_fluent_api_matches_the_root_object_the_assertion_rule_should_not_apply_to_the_root_object()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = 8.July(2012).At(22, 9);

            var expected = 8.July(2012).At(22, 10);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                subject.ShouldBeEquivalentTo(
                    expected,
                    options =>
                    options.Using<DateTime>(
                        context => context.Subject.Should().BeCloseTo(context.Expectation, 1000 * 60))
                        .WhenTypeIs<DateTime>());

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        internal class AlwaysFailAssertionRule : IAssertionRule
        {
            public bool AssertEquality(IEquivalencyValidationContext context)
            {
                throw new Exception("Failed");
            }
        }

        internal class RelaxingDateTimeAssertionRule : IAssertionRule
        {
            public bool AssertEquality(IEquivalencyValidationContext context)
            {
                if (context.Subject is DateTime)
                {
                    ((DateTime)context.Subject).Should().BeCloseTo((DateTime)context.Expectation, 1000 * 60);
                    return true;
                }
                return false;
            }
        }

        [TestMethod]
        public void When_multiple_asertion_rules_are_added_with_the_fluent_api_they_should_be_executed_from_right_to_left()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    subject.ShouldBeEquivalentTo(expected,
                        opts =>
                            opts.Using<object>(context => { throw new Exception(); })
                                .When(s => true)
                                .Using<object>(context => { })
                                .When(s => true));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow(
                "a different assertion rule should handle the comparision before the exception throwing assertion rule is hit");
        }

        #endregion

        #region Equivalency Steps

        [TestMethod]
        public void When_an_equivalency_step_handles_the_comparison_later_equivalency_steps_should_not_be_ran()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    subject.ShouldBeEquivalentTo(expected,
                        opts =>
                            opts.Using(new AlwayHandleEquivalencyStep())
                                .Using(new ThrowExceptionEquivalencyStep<InvalidOperationException>()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_equivalency_does_not_handle_the_comparison_later_equivalency_steps_should_stil_be_ran()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    subject.ShouldBeEquivalentTo(expected,
                        opts =>
                            opts.Using(new NeverHandleEquivalencyStep())
                                .Using(new ThrowExceptionEquivalencyStep<InvalidOperationException>()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void When_multiple_equivalency_steps_are_added_they_should_be_executed_from_right_to_left()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new ClassWithOnlyAProperty();
            var expected = new ClassWithOnlyAProperty();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () =>
                    subject.ShouldBeEquivalentTo(expected,
                        opts =>
                            opts.Using(new ThrowExceptionEquivalencyStep<NotSupportedException>())
                                .Using(new ThrowExceptionEquivalencyStep<InvalidOperationException>()));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<NotSupportedException>();
        }


        internal class ThrowExceptionEquivalencyStep<TException> : CanHandleAnythingEquivalencyStep where TException : Exception, new()
        {
            public override bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
            {
                throw new TException();
            }
        }

        internal class AlwayHandleEquivalencyStep : CanHandleAnythingEquivalencyStep
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