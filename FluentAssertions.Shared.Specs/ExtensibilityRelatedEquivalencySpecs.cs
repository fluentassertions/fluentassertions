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
            public IEnumerable<ISelectedMemberInfo> SelectMembers(IEnumerable<ISelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
            {
                return selectedMembers.Where(pi => !pi.Name.EndsWith("Id")).ToArray();
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
            public ISelectedMemberInfo Match(ISelectedMemberInfo subjectMember, object expectation, string memberPath, IEquivalencyAssertionOptions config)
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

        internal class RelaxingDateTimeAssertionRule : IAssertionRule
        {
            public bool AssertEquality(IEquivalencyValidationContext context)
            {
                if (context.Subject is DateTime)
                {
                    ((DateTime) context.Subject).Should().BeCloseTo((DateTime) context.Expectation, 1000*60);
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
}