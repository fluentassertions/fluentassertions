using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
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

        public void Execute<T>(T[] subject, object[] expectation)
        {
            if (AssertLengthEquality(subject.Length, expectation.Length))
            {
                if (Recursive)
                {
                    AssertElementGraphEquivalency(subject, expectation);
                }
                else
                {
                    subject.Should().BeEquivalentTo(expectation);
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

        private void AssertElementGraphEquivalency<T>(T[] subjects, object[] expectations)
        {
            matchedSubjectIndexes = new List<int>();

            foreach (int index in Enumerable.Range(0, expectations.Length))
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

        private List<int> matchedSubjectIndexes;

        private void LooselyMatchAgainst<T>(IList<T> subjects, object expectation, int expectationIndex)
        {
            var results = new AssertionResultSet();

            foreach (int index in Enumerable.Range(0, subjects.Count))
            {
                if (!matchedSubjectIndexes.Contains(index))
                {
                    T subject = subjects[index];

                    results.AddSet(index, TryToMatch(subject, expectation, expectationIndex));
                    if (results.ContainsSuccessfulSet)
                    {
                        matchedSubjectIndexes.Add(index);
                        break;
                    }
                }
            }

            foreach (string failure in results.SelectClosestMatchFor(expectationIndex))
            {
                AssertionScope.Current.FailWith(failure);
            }
        }

        private string[] TryToMatch<T>(T subject, object expectation, int expectationIndex)
        {
            using (var scope = new AssertionScope())
            {
                parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex, subject, expectation));

                return scope.Discard();
            }
        }

        private void StrictlyMatchAgainst<T>(T[] subjects, object expectation, int expectationIndex)
        {
            parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex, subjects[expectationIndex], expectation));
        }
    }
}