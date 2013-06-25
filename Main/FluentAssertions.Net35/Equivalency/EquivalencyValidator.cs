using System;
using System.Collections.Generic;
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

        private readonly List<IEquivalencyStep> steps = new List<IEquivalencyStep>()
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
                scope.AddContext("configuration", context.Config.ToString());
                scope.BecauseOf(context.Reason, context.ReasonArgs);

                AssertEqualityUsing(context);
            }
        }

        public void AssertEqualityUsing(EquivalencyValidationContext context)
        {
            AssertionScope.Current.AddContext("context", context.IsRoot ? "subject" : context.PropertyDescription);

            if (!context.ContainsCyclicReference)
            {
                foreach (IEquivalencyStep strategy in steps.Where(s => s.CanHandle(context)))
                {
                    if (strategy.Handle(context, this))
                    {
                        break;
                    }
                }
            }
            else
            {
                context.HandleCyclicReference();
            }
        }
    }
}