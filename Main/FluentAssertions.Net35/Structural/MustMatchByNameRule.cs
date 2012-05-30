using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Structural
{
    public class MustMatchByNameRule : IMatchingRule
    {
        public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
        {
            PropertyInfo compareeProperty = expectation.FindProperty(subjectProperty.Name);

            if (compareeProperty == null)
            {
                string path = (propertyPath.Length > 0) ? propertyPath + "." : "property ";

                Execute.Verification.FailWith(
                    "Subject has " + path + subjectProperty.Name + " that the other object does not have.");
            }

            return compareeProperty;
        }
    }
}