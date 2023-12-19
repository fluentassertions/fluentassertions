using System;
using System.Text.RegularExpressions;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency.Matching;

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
        if (Regex.IsMatch(expectationMemberName, @"[\.\[\]]"))
        {
            throw new ArgumentException("The expectation's member name cannot be a nested path", nameof(expectationMemberName));
        }

        if (Regex.IsMatch(subjectMemberName, @"[\.\[\]]"))
        {
            throw new ArgumentException("The subject's member name cannot be a nested path", nameof(subjectMemberName));
        }

        this.expectationMemberName = expectationMemberName;
        this.subjectMemberName = subjectMemberName;
    }

    public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyOptions options)
    {
        if (parent.Type.IsSameOrInherits(typeof(TExpectation)) && subject is TSubject &&
            expectedMember.Name == expectationMemberName)
        {
            var member = MemberFactory.Find(subject, subjectMemberName, parent);

            return member ?? throw new MissingMemberException(
                $"Subject of type {typeof(TSubject)} does not have member {subjectMemberName}");
        }

        return null;
    }
}
