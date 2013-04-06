using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context)
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
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent)
        {
            if (AssertExpectationIsCollection(context))
            {
                var subject = ((IEnumerable)context.Subject).Cast<object>().ToArray();
                var expectation = ((IEnumerable)context.Expectation).Cast<object>().ToArray();

                if (AssertCollectionsHaveEqualLength(context, subject, expectation))
                {
                    if (context.IsRoot || context.Config.IsRecursive)
                    {
                        EnumerateElements(context, subject, expectation, parent);
                    }
                    else
                    {
                        subject.Should().Equal(expectation, context.Reason, context.ReasonArgs);
                    }
                }

            }

            return true;
        }

        private void EnumerateElements(EquivalencyValidationContext context, object[] subjects, object[] expectations,
            IEquivalencyValidator parent)
        {
            if (!subjects.SequenceEqual(expectations))
            {
                for (int index = 0; index < expectations.Length; index++)
                {
                    var oldContext = Verification.Context;

                    var results = new Dictionary<int, CollectingVerificationContext>();
                    for (int subjectIndex = 0; subjectIndex < subjects.Length; subjectIndex++)
                    {
                        object subject = subjects[subjectIndex];
                        var tmpContext = new CollectingVerificationContext();
                        Verification.Context = tmpContext;

                        parent.AssertEqualityUsing(context.CreateForCollectionItem(index, subject, expectations[index]));

                        results[subjectIndex] = tmpContext;

                        if (!tmpContext.HasFailures)
                        {
                            break;
                        }
                    }

                    Verification.Context = oldContext;

                    if (results.All(v => v.Value.HasFailures))
                    {
                        int fewestFailures = results.Values.Min(r => r.FailureCount);
                        var bestResults = results.Where(r => r.Value.FailureCount == fewestFailures).ToArray();

                        var bestMatch = (bestResults.Any(r => r.Key == index)) ? bestResults.Single(r => r.Key == index) : bestResults.First();

                        foreach (var failure in bestMatch.Value.Failures)
                        {
                            Verification.Context.HandleFailure(failure);
                        }
                    }
                }
            }
        }

        private static bool AssertExpectationIsCollection(EquivalencyValidationContext context)
        {
            return context.Verification
                .ForCondition(IsCollection(context.Expectation))
                .FailWith((context.IsRoot ? "Subject" : context.PropertyDescription) +
                    " is a collection and cannot be compared with a non-collection type.",
                    context.Subject, context.Subject.GetType().FullName);
        }

        private static bool AssertCollectionsHaveEqualLength(EquivalencyValidationContext context, object[] subject, object[] expectation)
        {
            return context.Verification
                .ForCondition(subject.Length == expectation.Length)
                .FailWith(
                    "Expected " + (context.IsRoot ? "subject" : context.PropertyDescription) +
                        " to be a collection with {0} item(s){reason}, but found {1}.",
                    expectation.Length, subject.Length);
        }

        private static bool IsCollection(object value)
        {
            return (!(value is string) && (value is IEnumerable));
        }
    }
}