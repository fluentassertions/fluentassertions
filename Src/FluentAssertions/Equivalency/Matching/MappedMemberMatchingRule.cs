using System;
using System.Text.RegularExpressions;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Matching
{
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
                throw new ArgumentException("The expectation's member name cannot be a nested path");
            }

            if (Regex.IsMatch(subjectMemberName, $@"[\.\[\]]"))
            {
                throw new ArgumentException("The subject's member name cannot be a nested path");
            }

            this.expectationMemberName = expectationMemberName;
            this.subjectMemberName = subjectMemberName;
        }

        public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyAssertionOptions options)
        {
            if (parent.Type.IsSameOrInherits(typeof(TExpectation)) && subject is TSubject)
            {
                if (expectedMember.Name == expectationMemberName)
                {
                    var member = MemberFactory.Find(subject, subjectMemberName, expectedMember.Type, parent);
                    if (member is null)
                    {
                        throw new ArgumentException(
                            $"Subject of type {typeof(TSubject)} does not have member {subjectMemberName}");
                    }

                    return member;
                }
            }

            return null;
        }
    }
}
