using System;
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
        private readonly IEquivalencyValidationContext context;

        #endregion

        public EnumerableEquivalencyValidator(IEquivalencyValidator parent, IEquivalencyValidationContext context)
        {
            this.parent = parent;
            this.context = context;
            Recursive = false;
        }

        public bool Recursive { get; set; }

        public OrderingRuleCollection OrderingRules { get; set; }

        public void Execute<T>(object[] subject, T[] expectation)
        {
            if (AssertIsNotNull(expectation, subject) && EnumerableEquivalencyValidatorExtensions.AssertCollectionsHaveSameCount(subject, expectation))
            {
                if (Recursive)
                {
                    using (context.TraceBlock(path => $"Structurally comparing {subject} and expectation {expectation} at {path}"))
                    {
                        AssertElementGraphEquivalency(subject, expectation);
                    }
                }
                else
                {
                    using (context.TraceBlock(path => $"Comparing subject {subject} and expectation {expectation} at {path} using simple value equality"))
                    {
                        subject.Should().BeEquivalentTo(expectation);
                    }
                }
            }
        }

        private bool AssertIsNotNull(object expectation, object[] subject)
        {
            return AssertionScope.Current
                .ForCondition(!ReferenceEquals(expectation, null))
                .FailWith("Expected {context:subject} to be <null>, but found {0}.", new object[] { subject });
        }

        private void AssertElementGraphEquivalency<T>(object[] subjects, T[] expectations)
        {
            matchedSubjectIndexes = new HashSet<int>();

            foreach (int index in Enumerable.Range(0, expectations.Length))
            {
                T expectation = expectations[index];

                if (!OrderingRules.IsOrderingStrictFor(context))
                {
                    using (context.TraceBlock(path => $"Finding the best match of {expectation} within all items in {subjects} at {path}[{index}]"))
                    {
                        LooselyMatchAgainst(subjects, expectation, index);
                    }
                }
                else
                {
                    using (context.TraceBlock(path => $"Strictly comparing expectation {expectation} at {path} to item with index {index} in {subjects}"))
                    {
                        StrictlyMatchAgainst(subjects, expectation, index);
                    }
                }
            }
        }

        private HashSet<int> matchedSubjectIndexes;

        private void LooselyMatchAgainst<T>(IList<object> subjects, T expectation, int expectationIndex)
        {
            var results = new AssertionResultSet();

            foreach (int index in Enumerable.Range(0, subjects.Count))
            {
                if (!matchedSubjectIndexes.Contains(index))
                {
                    object subject = subjects[index];

                    using (context.TraceBlock(path => $"Comparing subject at {path}[{index}] with the expectation at {path}[{expectationIndex}]"))
                    {
                        string[] failures = TryToMatch(subject, expectation, expectationIndex);

                        results.AddSet(index, failures);
                        if (results.ContainsSuccessfulSet)
                        {
                            context.TraceSingle(_ => $"It's a match");
                            matchedSubjectIndexes.Add(index);
                            break;
                        }
                        else
                        {
                            context.TraceSingle(_ => $"Contained {failures.Length} failures");
                        }
                    }
                }
            }

            foreach (string failure in results.SelectClosestMatchFor(expectationIndex))
            {
                AssertionScope.Current.AddPreFormattedFailure(failure);
            }
        }

        private string[] TryToMatch<T>(object subject, T expectation, int expectationIndex)
        {
            using (var scope = new AssertionScope())
            {
                parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex.ToString(), subject, expectation));

                return scope.Discard();
            }
        }

        private void StrictlyMatchAgainst<T>(object[] subjects, T expectation, int expectationIndex)
        {
            parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex.ToString(), subjects[expectationIndex], expectation));
        }
    }
}
