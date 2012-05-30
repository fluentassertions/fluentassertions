using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Structural
{
    public class TryMatchByNameRule : IMatchingRule
    {
        public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
        {
            return expectation.FindProperty(subjectProperty.Name);
        }
    }
}