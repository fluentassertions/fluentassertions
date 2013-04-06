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
        /// Returns the steps that are executed in the order of appearance during an equivalency test.
        /// </summary>
        public IList<IEquivalencyStep> Steps
        {
            get { return steps; }
        }

        public void AssertEquality(EquivalencyValidationContext context)
        {
            try
            {
                var verificationContext = new CollectingVerificationContext();
                Verification.Context = verificationContext;

                AssertEqualityUsing(context);

                verificationContext.ThrowIfAny(context.Config.ToString());
            }
            finally
            {
                Verification.Context = null;
            }
        }

        public void AssertEqualityUsing(EquivalencyValidationContext context)
        {
            ExecuteUsingSubjectName(context.PropertyDescription, () =>
            {
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
            });
        }

        private void ExecuteUsingSubjectName(string subjectName, Action action)
        {
            try
            {
                Verification.SubjectName = subjectName;
                action();
            }
            finally
            {
                Verification.SubjectName = null;
            }
        }
    }
}