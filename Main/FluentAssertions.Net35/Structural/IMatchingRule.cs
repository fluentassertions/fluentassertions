using System.Reflection;

namespace FluentAssertions.Structural
{
    public interface IMatchingRule
    {
        PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath);
    }
}