using System.Reflection;

namespace FluentAssertions.Types
{
    public static class CSharpAccessModifierExtensions
    {
        internal static CSharpAccessModifiers GetCSharpAccessModifier(this MethodBase methodBase)
        {
            if (methodBase.IsPrivate)
            {
                return CSharpAccessModifiers.Private;
            }
            if (methodBase.IsFamily)
            {
                return CSharpAccessModifiers.Protected;
            }
            if (methodBase.IsAssembly)
            {
                return CSharpAccessModifiers.Internal;
            }
            if (methodBase.IsPublic)
            {
                return CSharpAccessModifiers.Public;
            }
            if (methodBase.IsFamilyOrAssembly)
            {
                return CSharpAccessModifiers.ProtectedInternal;
            }

            return CSharpAccessModifiers.None;
        }

        internal static CSharpAccessModifiers GetCSharpAccessModifier(this System.Type type)
        {
            if (type.IsNestedPrivate)
            {
                return CSharpAccessModifiers.Private;
            }
            if (type.IsNestedFamily)
            {
                return CSharpAccessModifiers.Protected;
            }
            if (type.IsNestedAssembly)
            {
                return CSharpAccessModifiers.Internal;
            }
            if (type.IsPublic)
            {
                return CSharpAccessModifiers.Public;
            }
            if (type.IsNestedFamORAssem)
            {
                return CSharpAccessModifiers.ProtectedInternal;
            }

            return CSharpAccessModifiers.None;
        }
    }
}