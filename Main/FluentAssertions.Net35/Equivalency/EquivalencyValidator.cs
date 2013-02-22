using System;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    internal class EquivalencyValidator : IEquivalencyValidator
    {
        #region Private Definitions

        private readonly IEquivalencyStep[] steps =
        {
            new TryConversionEquivalencyStep(),
            new ReferenceEqualityEquivalencyStep(),
            new ApplyAssertionRulesEquivalencyStep(),
            new EnumerableEquivalencyStep(),
            new ComplexTypeEquivalencyStep(),
            new SimpleEqualityEquivalencyStep()
        };

        #endregion

        public void AssertEquality(EquivalencyValidationContext context)
        {
            try
            {
                Verification.StartCollecting();
                AssertEqualityUsing(context);

                Verification.ThrowIfAny(context.Config.ToString());
            }
            catch (Exception)
            {
                Verification.StopCollecting();
                throw;
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