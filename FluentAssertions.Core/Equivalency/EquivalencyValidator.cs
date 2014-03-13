using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    public class EquivalencyValidator : IEquivalencyValidator
    {
        #region Private Definitions

        private readonly IEquivalencyAssertionOptions config;

        private readonly List<IEquivalencyStep> steps = new List<IEquivalencyStep>
        {
            new TryConversionEquivalencyStep(),
            new ReferenceEqualityEquivalencyStep(),
            new ApplyAssertionRulesEquivalencyStep(),
            new DictionaryEquivalencyStep(),
            new EnumerableEquivalencyStep(),
            new ComplexTypeEquivalencyStep(),
            new SimpleEqualityEquivalencyStep()
        };

        #endregion

        public EquivalencyValidator(IEquivalencyAssertionOptions config)
        {
            this.config = config;
        }

        /// <summary>
        /// Provides access the list of steps that are executed in the order of appearance during an equivalency test.
        /// </summary>
        public IList<IEquivalencyStep> Steps
        {
            get { return steps; }
        }

        public void AssertEquality(EquivalencyValidationContext context)
        {
            using (var scope = new AssertionScope())
            {
                scope.AddReportable("configuration", config.ToString());
                scope.AddNonReportable("objects", new ObjectTracker(config.CyclicReferenceHandling));

                scope.BecauseOf(context.Reason, context.ReasonArgs);

                AssertEqualityUsing(context);
            }
        }

        public void AssertEqualityUsing(EquivalencyValidationContext context)
        {
            if (!config.AllowInfiniteRecursion &&
                MaximumRecusionDepthMet(context))
            {
                AssertionScope.Current.FailWith(
                    "The maximum recursion depth was reached.  " +
                    "The maximum recursion depth limitation prevents stack overflow from " +
                    "occuring when certain types of cycles exist in the object graph " +
                    "or the object graph's depth is very high or infinite.  " +
                    "This limitation may be disabled using the config parameter." +
                    Environment.NewLine + Environment.NewLine +
                    "The property path when max depth was hit was: " +
                    context.PropertyPath);

                return;
            }

            AssertionScope scope = AssertionScope.Current;
            scope.AddNonReportable("context", context.IsRoot ? "subject" : context.PropertyDescription);
            scope.AddNonReportable("subject", context.Subject);
            scope.AddNonReportable("expectation", context.Expectation);

            var objectTracker = scope.Get<ObjectTracker>("objects");

            if (!objectTracker.IsCyclicReference(new ObjectReference(context.Subject, context.PropertyPath)))
            {
                foreach (IEquivalencyStep strategy in steps.Where(s => s.CanHandle(context, config)))
                {
                    if (strategy.Handle(context, this, config))
                    {
                        break;
                    }
                }
            }
        }

        private static bool MaximumRecusionDepthMet(EquivalencyValidationContext context)
        {
            const int MaxDepth = 10;

            var depth =
                context.PropertyPath.Cast<char>().Count(chr => chr == '.');

            return depth >= MaxDepth;
        }
    }
}