using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    public static class SubjectInfoExtensions
    {
        /// <summary>
        /// Checks if the subject info setter has the given access modifier.
        /// </summary>
        /// <param name="subjectInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info setter should have.</param>
        /// <returns>True if the subject info setter has the given access modifier, false otherwise.</returns>
        public static bool WhichSetterHas(this ISubjectInfo subjectInfo, CSharpAccessModifier accessModifier)
        {
            return subjectInfo.SelectedMemberInfo.SetAccessModifier == accessModifier;
        }

        /// <summary>
        /// Checks if the subject info setter does not have the given access modifier.
        /// </summary>
        /// <param name="subjectInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info setter should not have.</param>
        /// <returns>True if the subject info setter does not have the given access modifier, false otherwise.</returns>
        public static bool WhichSetterDoesNotHave(this ISubjectInfo subjectInfo, CSharpAccessModifier accessModifier)
        {
            return subjectInfo.SelectedMemberInfo.SetAccessModifier != accessModifier;
        }

        /// <summary>
        /// Checks if the subject info getter has the given access modifier.
        /// </summary>
        /// <param name="subjectInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info getter should have.</param>
        /// <returns>True if the subject info getter has the given access modifier, false otherwise.</returns>
        public static bool WhichGetterHas(this ISubjectInfo subjectInfo, CSharpAccessModifier accessModifier)
        {
            return subjectInfo.SelectedMemberInfo.GetAccessModifier == accessModifier;
        }

        /// <summary>
        /// Checks if the subject info getter does not have the given access modifier.
        /// </summary>
        /// <param name="subjectInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info getter should not have.</param>
        /// <returns>True if the subject info getter does not have the given access modifier, false otherwise.</returns>
        public static bool WhichGetterDoesNotHave(this ISubjectInfo subjectInfo, CSharpAccessModifier accessModifier)
        {
            return subjectInfo.SelectedMemberInfo.GetAccessModifier != accessModifier;
        }
    }
}