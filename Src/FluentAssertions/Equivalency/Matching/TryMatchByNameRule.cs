using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Matching
{
    /// <summary>
    /// Finds a member of the expectation with the exact same name, but doesn't require it.
    /// </summary>
    internal class TryMatchByNameRule : IMemberMatchingRule
    {
        public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyAssertionOptions config)
        {
            PropertyInfo property = subject.GetType().FindProperty(expectedMember.Name, expectedMember.Type);
            if ((property is not null) && !property.IsIndexer())
            {
                return new Property(property, parent);
            }

            FieldInfo field = subject.GetType().FindField(expectedMember.Name, expectedMember.Type);
            return (field is not null) ? new Field(field, parent) : null;
        }

        /// <inheritdoc />
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return "Try to match member by name";
        }
    }
}
