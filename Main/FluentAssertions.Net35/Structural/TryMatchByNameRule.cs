using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    public class TryMatchByNameRule : IMatchingRule
    {
        public PropertyInfo FindMatch(PropertyInfo subjectProperty, object expectation, string propertyPath)
        {
            return expectation.GetType().FindProperty(subjectProperty.Name);
        }
    }
}