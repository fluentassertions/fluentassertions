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
    }
}