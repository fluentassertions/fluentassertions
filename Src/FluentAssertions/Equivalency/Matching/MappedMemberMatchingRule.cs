using System;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Matching;

/// <summary>
/// Allows mapping a member (property or field) of the expectation to a differently named member
/// of the subject-under-test using a member name and the target type.
/// </summary>
internal class MappedMemberMatchingRule<TExpectation, TSubject> : IMemberMatchingRule
{
    private readonly string expectationMemberName;
    private readonly string subjectMemberName;

    public MappedMemberMatchingRule(string expectationMemberName, string subjectMemberName)
    {
        if (IsNestedPath(expectationMemberName))
        {
            throw new ArgumentException("The expectation's member name cannot be a nested path", nameof(expectationMemberName));
        }

        if (IsNestedPath(subjectMemberName))
        {
            throw new ArgumentException("The subject's member name cannot be a nested path", nameof(subjectMemberName));
        }

        this.expectationMemberName = expectationMemberName;
        this.subjectMemberName = subjectMemberName;
    }

    private static bool IsNestedPath(string path) =>
        path.Contains('.', StringComparison.Ordinal) || path.Contains('[', StringComparison.Ordinal) || path.Contains(']', StringComparison.Ordinal);

    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options, AssertionChain assertionChain)
    {
        if (parent.Type.IsSameOrInherits(typeof(TExpectation)) && subject is TSubject &&
            expectedMember.Subject.Name == expectationMemberName)
        {
            var member = MemberFactory.Find(subject, subjectMemberName, parent);

            return member ?? throw new MissingMemberException(
                $"Subject of type {typeof(TSubject)} does not have member {subjectMemberName}");
        }

        return null;
    }
}
