using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching;

/// <summary>
/// Requires the subject to have a member with the exact same name as the expectation has.
/// </summary>
internal class MustMatchMemberByNameRule : IMemberMatchingRule
{
    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options, AssertionChain assertionChain)
    {
        IMember subjectMember = null;

        if (options.IncludedProperties != MemberVisibility.None)
        {
            PropertyInfo propertyInfo = subject.GetType().FindProperty(
                expectedMember.Subject.Name,
                options.IncludedProperties | MemberVisibility.ExplicitlyImplemented | MemberVisibility.DefaultInterfaceProperties);

            subjectMember = propertyInfo is not null && !propertyInfo.IsIndexer() ? new Property(propertyInfo, parent) : null;
        }

        if (subjectMember is null && options.IncludedFields != MemberVisibility.None)
        {
            FieldInfo fieldInfo = subject.GetType().FindField(
                expectedMember.Subject.Name,
                options.IncludedFields);

            subjectMember = fieldInfo is not null ? new Field(fieldInfo, parent) : null;
        }

        if (subjectMember is null)
        {
            assertionChain.FailWith(
                "Expectation has {0} that the other object does not have.", expectedMember.Expectation.AsNonFormattable());
        }
        else if (options.IgnoreNonBrowsableOnSubject && !subjectMember.IsBrowsable)
        {
            assertionChain.FailWith(
                "Expectation has {0} that is non-browsable in the other object, and non-browsable " +
                "members on the subject are ignored with the current configuration", expectedMember.Expectation.AsNonFormattable());
        }
        else
        {
            // Everything is fine
        }

        return subjectMember;
    }
}

