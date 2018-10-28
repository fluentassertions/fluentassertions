using System;
using System.Reflection;

namespace FluentAssertions.Common
{
    public static class CSharpAccessModifierExtensions
    {
        internal static CSharpAccessModifier GetCSharpAccessModifier(this MethodBase methodBase)
        {
            if (methodBase.IsPrivate)
            {
                return CSharpAccessModifier.Private;
            }

            if (methodBase.IsFamily)
            {
                return CSharpAccessModifier.Protected;
            }

            if (methodBase.IsAssembly)
            {
                return CSharpAccessModifier.Internal;
            }

            if (methodBase.IsPublic)
            {
                return CSharpAccessModifier.Public;
            }

            if (methodBase.IsFamilyOrAssembly)
            {
                return CSharpAccessModifier.ProtectedInternal;
            }

            if (methodBase.IsFamilyAndAssembly)
            {
                return CSharpAccessModifier.PrivateProtected;
            }

            return CSharpAccessModifier.InvalidForCSharp;
        }

        internal static CSharpAccessModifier GetCSharpAccessModifier(this FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPrivate)
            {
                return CSharpAccessModifier.Private;
            }

            if (fieldInfo.IsFamily)
            {
                return CSharpAccessModifier.Protected;
            }

            if (fieldInfo.IsAssembly)
            {
                return CSharpAccessModifier.Internal;
            }

            if (fieldInfo.IsPublic)
            {
                return CSharpAccessModifier.Public;
            }

            if (fieldInfo.IsFamilyOrAssembly)
            {
                return CSharpAccessModifier.ProtectedInternal;
            }

            if (fieldInfo.IsFamilyAndAssembly)
            {
                return CSharpAccessModifier.PrivateProtected;
            }

            return CSharpAccessModifier.InvalidForCSharp;
        }

        internal static CSharpAccessModifier GetCSharpAccessModifier(this Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();

            if (typeInfo.IsNestedPrivate)
            {
                return CSharpAccessModifier.Private;
            }

            if (typeInfo.IsNestedFamily)
            {
                return CSharpAccessModifier.Protected;
            }

            if (typeInfo.IsNestedAssembly || (typeInfo.IsClass && typeInfo.IsNotPublic))
            {
                return CSharpAccessModifier.Internal;
            }

            if (typeInfo.IsPublic || typeInfo.IsNestedPublic)
            {
                return CSharpAccessModifier.Public;
            }

            if (typeInfo.IsNestedFamORAssem)
            {
                return CSharpAccessModifier.ProtectedInternal;
            }

            return CSharpAccessModifier.InvalidForCSharp;
        }
    }
}
