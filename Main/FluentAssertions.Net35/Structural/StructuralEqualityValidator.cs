using System;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    internal class StructuralEqualityValidator : IStructuralEqualityValidator
    {
        #region Private Definitions

        private readonly IStructuralEqualityStep[] steps =
        {
            new TryConversionEqualityStep(),
            new ReferenceEqualityStep(),
            new ApplyAssertionRulesEqualityStep(),
            new EnumerableEqualityStep(),
            new ComplexTypeEqualityStep(),
            new FinalEqualityStep()
        };

        #endregion

        public void AssertEquality(StructuralEqualityContext context)
        {
            try
            {
                AssertEqualityUsing(context);
            }
            catch (Exception exc)
            {
                Execute.Verification.FailWith(exc.Message + "\n" + context.Config);
            }
        }

        public void AssertEqualityUsing(StructuralEqualityContext context)
        {
            ExecuteUsingSubjectName(context.PropertyDescription, () =>
            {
                if (!context.ContainsCyclicReference)
                {
                    foreach (IStructuralEqualityStep strategy in steps.Where(s => s.CanHandle(context)))
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