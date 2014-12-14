using System.Collections;
using System.Collections.Generic;
using FluentAssertions.Equivalency;

namespace FluentAssertions
{
    /// <summary>
    /// Represents a mutable collection of equivalency steps that can be reordered and/or ammended with additional
    /// custom equivalency steps. 
    /// </summary>
    public class EquivalencyStepCollection : IEnumerable<IEquivalencyStep>
    {
        private readonly List<IEquivalencyStep> steps = new List<IEquivalencyStep>();

        public EquivalencyStepCollection(IEnumerable<IEquivalencyStep> defaultSteps)
        {
            steps.AddRange(defaultSteps);
        }

        public IEnumerator<IEquivalencyStep> GetEnumerator()
        {
            return steps.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}