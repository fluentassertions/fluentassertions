using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Is responsible for the exact run-time behavior of a structural equality comparison.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    public class EquivalencyAssertionOptions<TSubject> : IEquivalencyAssertionOptions
    {
        #region Private Definitions

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<ISelectionRule> selectionRules = new List<ISelectionRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IMatchingRule> matchingRules = new List<IMatchingRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<IAssertionRule> assertionRules = new List<IAssertionRule>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CyclicReferenceHandling cyclicReferenceHandling = CyclicReferenceHandling.ThrowException;

        #endregion

        private EquivalencyAssertionOptions()
        {
            Using(new MustMatchByNameRule());

            WhenTypeIs<string>().Using(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));

            WhenTypeIs<DateTime>().Using(
                ctx => ctx.Subject.Should().Be(ctx.Expectation, ctx.Reason, ctx.ReasonArgs));
        }

        /// <summary>
        /// Gets a configuration that compares all declared properties of the subject with equally named properties of the expectation,
        /// and includes the entire object graph. The names of the properties between the subject and expectation must match.
        /// </summary>
        public static Func<EquivalencyAssertionOptions<TSubject>> Default = () =>
        {
            var config = new EquivalencyAssertionOptions<TSubject>();
            config.IncludingNestedObjects();
            config.IncludingAllDeclaredProperties();

            return config;
        };

        /// <summary>
        /// Gets a configuration that by default doesn't include any of the subject's properties and doesn't consider any nested objects
        /// or collections.
        /// </summary>
        public static Func<EquivalencyAssertionOptions<TSubject>> Empty =
            () => { return new EquivalencyAssertionOptions<TSubject>(); };

        /// <summary>
        /// Gets an ordered collection of selection rules that define what properties are included.
        /// </summary>
        public IEnumerable<ISelectionRule> SelectionRules
        {
            get { return selectionRules; }
        }

        /// <summary>
        /// Gets an ordered collection of matching rules that determine which subject properties are matched with which
        /// expectation properties.
        /// </summary>
        public IEnumerable<IMatchingRule> MatchingRules
        {
            get { return matchingRules; }
        }

        /// <summary>
        /// Gets an ordered collection of assertion rules that determine how subject properties are compared for equality with
        /// expectation properties.
        /// </summary>
        public IEnumerable<IAssertionRule> AssertionRules
        {
            get { return assertionRules; }
        }

        /// <summary>
        /// Gets value indicating whether the equality check will include nested collections and complex types.
        /// </summary>
        public bool IsRecursive { get; private set; }

        /// <summary>
        /// Gets value indicating how cyclic references should be handled. By default, it will throw an exception.
        /// </summary>
        public CyclicReferenceHandling CyclicReferenceHandling
        {
            get { return cyclicReferenceHandling; }
        }

        /// <summary>
        /// Adds all public properties of the subject as far as they are defined on the declared type. 
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> IncludingAllDeclaredProperties()
        {
            WithoutSelectionRules();
            Using(new AllDeclaredPublicPropertiesSelectionRule());
            return this;
        }

        /// <summary>
        /// Adds all public properties of the subject based on its run-time type rather than its declared type.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> IncludingAllRuntimeProperties()
        {
            WithoutSelectionRules();
            Using(new AllRuntimePublicPropertiesSelectionRule());
            return this;
        }

        /// <summary>
        /// Tries to match the properties of the subject with equally named properties on the expectation. Ignores those 
        /// properties that don't exist on the expectation.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> ExcludingMissingProperties()
        {
            WithoutMatchingRules();
            matchingRules.Add(new TryMatchByNameRule());
            return this;
        }

        /// <summary>
        /// Requires the expectation to have properties which are equally named to properties on the subject.
        /// </summary>
        /// <returns></returns>
        public EquivalencyAssertionOptions<TSubject> ThrowingOnMissingProperties()
        {
            WithoutMatchingRules();
            matchingRules.Add(new MustMatchByNameRule());
            return this;
        }

        /// <summary>
        /// Excludes the specified (nested) property from the structural equality check.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Excluding(Expression<Func<TSubject, object>> propertyExpression)
        {
            Using(new ExcludePropertyByPathSelectionRule(propertyExpression.GetPropertyPath()));
            return this;
        }

        /// <summary>
        /// Excludes a (nested) property based on a predicate from the structural equality check.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Excluding(Expression<Func<ISubjectInfo, bool>> predicate)
        {
            Using(new ExcludePropertyByPredicateSelectionRule(predicate));
            return this;
        }

        /// <summary>
        /// Includes the specified property in the equality check.
        /// </summary>
        /// <remarks>
        /// This overrides the default behavior of including all declared properties.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> Including(Expression<Func<TSubject, object>> propertyExpression)
        {
            RemoveSelectionRule<AllDeclaredPublicPropertiesSelectionRule>();
            RemoveSelectionRule<AllRuntimePublicPropertiesSelectionRule>();

            Using(new IncludePropertySelectionRule(propertyExpression.GetPropertyInfo()));
            return this;
        }

        /// <summary>
        /// Allows overriding the way structural equality is applied to (nested) objects of tyoe <typeparamref name="TPropertyType"/>
        /// </summary>
        public Restriction<TPropertyType> WhenTypeIs<TPropertyType>()
        {
            return new Restriction<TPropertyType>(info => info.RuntimeType.IsSameOrInherits(typeof(TPropertyType)), this);
        }

        /// <summary>
        /// Allows overriding the way structural equality is applied to particular properties.
        /// </summary>
        /// <param name="predicate">
        /// A predicate based on the <see cref="ISubjectInfo"/> of the subject that is used to identify the property for which the
        /// override applies.
        /// </param>
        public Restriction When(Func<ISubjectInfo, bool> predicate)
        {
            return new Restriction(predicate, this);
        }

        /// <summary>
        /// Causes the structural equality check to include nested collections and complex types.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> IncludingNestedObjects()
        {
            IsRecursive = true;
            return this;
        }

        /// <summary>
        /// Causes the structural equality check to ignore any cyclic references.
        /// </summary>
        /// <remarks>
        /// By default, cyclic references within the object graph will cause an exception to be thrown.
        /// </remarks>
        public EquivalencyAssertionOptions<TSubject> IgnoringCyclicReferences()
        {
            cyclicReferenceHandling = CyclicReferenceHandling.Ignore;
            return this;
        }

        private void RemoveSelectionRule<T>() where T : ISelectionRule
        {
            foreach (T selectionRule in selectionRules.OfType<T>().ToArray())
            {
                selectionRules.Remove(selectionRule);
            }
        }

        /// <summary>
        /// Clears all selection rules, including those that were added by default.
        /// </summary>
        public void WithoutSelectionRules()
        {
            selectionRules.Clear();
        }

        /// <summary>
        /// Clears all matching rules, including those that were added by default.
        /// </summary>
        public void WithoutMatchingRules()
        {
            matchingRules.Clear();
        }

        /// <summary>
        /// Adds a selection rule to the ones allready added by default and which is evaluated after all existing rules.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Using(ISelectionRule selectionRule)
        {
            selectionRules.Add(selectionRule);
            return this;
        }

        /// <summary>
        /// Adds a matching rule to the ones allready added by default and which is evaluated before all existing rules.
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Using(IMatchingRule matchingRule)
        {
            matchingRules.Insert(0, matchingRule);
            return this;
        }

        /// <summary>
        /// Adds a matching rule to the ones allready added by default and which is evaluated before all existing rules
        /// </summary>
        public EquivalencyAssertionOptions<TSubject> Using(IAssertionRule assertionRule)
        {
            assertionRules.Insert(0, assertionRule);
            return this;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine("\nUsing configuration:");
            foreach (var rule in selectionRules)
            {
                builder.AppendLine("- " + rule);
            }

            foreach (var rule in matchingRules)
            {
                builder.AppendLine("- " + rule);
            }

            foreach (var rule in assertionRules)
            {
                builder.AppendLine("- " + rule);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Defines additional overrides when used with <see cref="EquivalencyAssertionOptions{TSubject}.When"/>
        /// </summary>
        public class Restriction
        {
            private readonly Func<ISubjectInfo, bool> predicate;
            private readonly EquivalencyAssertionOptions<TSubject> config;

            internal Restriction(Func<ISubjectInfo, bool> predicate, EquivalencyAssertionOptions<TSubject> config)
            {
                this.predicate = predicate;
                this.config = config;
            }

            /// <param name="action">
            /// The assertion to execute for the predicate.
            /// </param>
            public EquivalencyAssertionOptions<TSubject> Using<T>(Action<IAssertionContext<T>> action)
            {
                config.assertionRules.Insert(0, new AssertionRule<T>(predicate, action));
                return config;
            }
        }

        /// <summary>
        /// Defines additional overrides when used with <see cref="EquivalencyAssertionOptions{TSubject}.When"/>
        /// </summary>
        public class Restriction<TPropertyType>
        {
            private readonly Func<ISubjectInfo, bool> predicate;
            private readonly EquivalencyAssertionOptions<TSubject> config;

            internal Restriction(Func<ISubjectInfo, bool> predicate, EquivalencyAssertionOptions<TSubject> config)
            {
                this.predicate = predicate;
                this.config = config;
            }

            /// <param name="action">
            /// The assertion to execute for the predicate.
            /// </param>
            public EquivalencyAssertionOptions<TSubject> Using(Action<IAssertionContext<TPropertyType>> action)
            {
                config.assertionRules.Insert(0, new AssertionRule<TPropertyType>(predicate, action));
                return config;
            }
        }
    }
}