using System.Collections;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return IsCollection(context.Subject);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            if (AssertExpectationIsCollection(context.Expectation))
            {
                var validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.IsRoot || config.IsRecursive,
                    OrderingRules = config.OrderingRules
                };

                validator.Execute(ToArray(context.Subject), ToArray(context.Expectation));
            }

            return true;
        }

        private static bool AssertExpectationIsCollection(object expectation)
        {
            return AssertionScope.Current
                .ForCondition(IsCollection(expectation))
                .FailWith("{context:Subject} is a collection and cannot be compared with a non-collection type.");
        }

        private static bool IsCollection(object value)
        {
            return (!(value is string) && (value is IEnumerable));
        }

        private object[] ToArray(object value)
        {
            return ((IEnumerable)value).Cast<object>().ToArray();
        }
    }

    /// <summary>
    /// Executes a single equivalency assertion on two collections, optionally recursive and with or without strict ordering.
    /// </summary>
    internal class EnumerableEquivalencyValidator
    {
        #region Private Definitions

        private readonly IEquivalencyValidator parent;
        private readonly EquivalencyValidationContext context;

        #endregion

        public EnumerableEquivalencyValidator(IEquivalencyValidator parent, EquivalencyValidationContext context)
        {
            this.parent = parent;
            this.context = context;
            Recursive = false;
        }

        public bool Recursive { get; set; }

        public OrderingRuleCollection OrderingRules { get; set; }

        public void Execute(object[] subject, object[] expectation)
        {
            if (AssertLengthEquality(subject.Length, expectation.Length))
            {
                if (Recursive)
                {
                    AssertElementGraphEquivalency(subject, expectation);
                }
                else
                {
                    subject.Should().Equal(expectation);
                }
            }
        }

        private bool AssertLengthEquality(int subjectLength, int expectationLength)
        {
            return AssertionScope.Current
                .ForCondition(subjectLength == expectationLength)
                .FailWith("Expected {context:subject} to be a collection with {0} item(s){reason}, but found {1}.",
                    expectationLength, subjectLength);
        }

        private void AssertElementGraphEquivalency(object[] subjects, object[] expectations)
        {
            for (int index = 0; index < expectations.Length; index++)
            {
                object expectation = expectations[index];

                if (!OrderingRules.IsOrderingStrictFor(context))
                {
                    LooselyMatchAgainst(subjects, expectation, index);
                }
                else
                {
                    StrictlyMatchAgainst(subjects, expectation, index);
                }
            }
        }

        private void LooselyMatchAgainst(object[] subjects, object expectation, int expectationIndex)
        {
            var results = new AssertionResultSet();

            for (int index = 0; index < subjects.Length; index++)
            {
                object subject = subjects[index];

                results.AddSet(index, TryToMatch(subject, expectation, expectationIndex));
                if (results.ContainsSuccessfulSet)
                {
                    break;
                }
            }

            foreach (string failure in results.SelectClosestMatchFor(expectationIndex))
            {
                AssertionScope.Current.FailWith(failure);
            }
        }

        private string[] TryToMatch(object subject, object expectation, int expectationIndex)
        {
            using (var scope = new AssertionScope())
            {
                parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex, subject, expectation));

                return scope.Discard();
            }
        }

        private void StrictlyMatchAgainst(object[] subjects, object expectation, int expectationIndex)
        {
            parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex, subjects[expectationIndex], expectation));
        }
    }
}