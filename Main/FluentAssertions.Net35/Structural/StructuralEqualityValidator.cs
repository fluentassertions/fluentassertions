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
        
        private readonly IStructuralEqualityStep[] steps = {
            new TryConversionEqualityStep(),
            new ReferenceEqualityStep(),
            new ApplyAssertionRulesEqualityStep(), 
            new DateTimeEqualityStep(),
            new EnumerableEqualityStep(),
            new ComplexTypeEqualityStep(), 
            new FinalEqualityStep()
        };

        #endregion

        public void AssertEquality(StructuralEqualityContext context)
        {
            try
            {
                Verification.SubjectName = context.FullPropertyPath;

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
            }
            finally
            {
                Verification.SubjectName = null;
            }
        }
    }
}