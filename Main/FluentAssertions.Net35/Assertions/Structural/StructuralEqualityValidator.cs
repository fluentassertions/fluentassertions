using FluentAssertions.Common;

namespace FluentAssertions.Assertions.Structure
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
            new StringEqualityStep(),
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
                Verification.SubjectName = context.PropertyPath;

                if (!context.ContainsCyclicReference)
                {
                    foreach (IStructuralEqualityStep strategy in steps)
                    {
                        if (strategy.Execute(context, this))
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

    internal interface IStructuralEqualityValidator
    {
        void AssertEquality(StructuralEqualityContext context);
    }
}