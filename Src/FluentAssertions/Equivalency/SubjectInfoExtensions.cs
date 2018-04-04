using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    public static class SubjectInfoExtensions
    {
        /// <summary>
        /// Checks if the subject info setter has the given access modifier.
        /// </summary>
        /// <param name="memberInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info setter should have.</param>
        /// <returns>True if the subject info setter has the given access modifier, false otherwise.</returns>
        public static bool WhichSetterHas(this IMemberInfo memberInfo, CSharpAccessModifier accessModifier)
        {
            return memberInfo.SelectedMemberInfo.GetSetAccessModifier() == accessModifier;
        }

        /// <summary>
        /// Checks if the subject info setter does not have the given access modifier.
        /// </summary>
        /// <param name="memberInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info setter should not have.</param>
        /// <returns>True if the subject info setter does not have the given access modifier, false otherwise.</returns>
        public static bool WhichSetterDoesNotHave(this IMemberInfo memberInfo, CSharpAccessModifier accessModifier)
        {
            return memberInfo.SelectedMemberInfo.GetSetAccessModifier() != accessModifier;
        }

        /// <summary>
        /// Checks if the subject info getter has the given access modifier.
        /// </summary>
        /// <param name="memberInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info getter should have.</param>
        /// <returns>True if the subject info getter has the given access modifier, false otherwise.</returns>
        public static bool WhichGetterHas(this IMemberInfo memberInfo, CSharpAccessModifier accessModifier)
        {
            return memberInfo.SelectedMemberInfo.GetGetAccessModifier() == accessModifier;
        }

        /// <summary>
        /// Checks if the subject info getter does not have the given access modifier.
        /// </summary>
        /// <param name="memberInfo">The subject info being checked.</param>
        /// <param name="accessModifier">The access modifier that the subject info getter should not have.</param>
        /// <returns>True if the subject info getter does not have the given access modifier, false otherwise.</returns>
        public static bool WhichGetterDoesNotHave(this IMemberInfo memberInfo, CSharpAccessModifier accessModifier)
        {
            return memberInfo.SelectedMemberInfo.GetGetAccessModifier() != accessModifier;
        }
    }
}
