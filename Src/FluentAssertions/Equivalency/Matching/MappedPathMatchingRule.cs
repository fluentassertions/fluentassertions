using System;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Matching
{
    /// <summary>
    /// Allows mapping a member (property or field) of the expectation to a differently named member
    /// of the subject-under-test using a nested member path in the form of "Parent.NestedCollection[].Member"
    /// </summary>
    internal class MappedPathMatchingRule : IMemberMatchingRule
    {
        private readonly MemberPath expectationPath;
        private readonly MemberPath subjectPath;

        public MappedPathMatchingRule(string expectationMemberPath, string subjectMemberPath)
        {
            Guard.ThrowIfArgumentIsNullOrEmpty(expectationMemberPath,
                nameof(expectationMemberPath), "A member path cannot be null");
            Guard.ThrowIfArgumentIsNullOrEmpty(subjectMemberPath,
                nameof(subjectMemberPath), "A member path cannot be null");

            expectationPath = new MemberPath(expectationMemberPath);
            subjectPath = new MemberPath(subjectMemberPath);

            if (expectationPath.GetContainsSpecificCollectionIndex() || subjectPath.GetContainsSpecificCollectionIndex())
            {
                throw new ArgumentException("Mapping properties containing a collection index must use the [] format without specific index.");
            }

            if (!expectationPath.HasSameParentAs(subjectPath))
            {
                throw new ArgumentException("The member paths must have the same parent.");
            }
        }

        public IMember Match(IMember expectedMember, object subject, INode parent, IEquivalencyAssertionOptions options)
        {
            MemberPath path = expectationPath;
            if (expectedMember.RootIsCollection)
            {
                path = path.WithCollectionAsRoot();
            }
            
            if (path.IsEquivalentTo(expectedMember.PathAndName))
            {
                var member = MemberFactory.Find(subject, subjectPath.MemberName, expectedMember.Type, parent);
                if (member is null)
                {
                    throw new ArgumentException(
                        $"Subject of type {subject?.GetType().Name} does not have member {subjectPath.MemberName}");
                }

                return member;
            }

            return null;
        }
    }
}
