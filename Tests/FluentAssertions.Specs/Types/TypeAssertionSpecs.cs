using System;
using System.Globalization;

namespace FluentAssertionsAsync.Specs.Types
{
    /// <summary>
    /// Type assertion specs.
    /// </summary>
    public partial class TypeAssertionSpecs
    {
    }

    #region Internal classes used in unit tests

    [DummyClass("Expected", true)]
    public class ClassWithAttribute
    {
    }

    public class ClassWithInheritedAttribute : ClassWithAttribute
    {
    }

    public class ClassWithoutAttribute
    {
    }

    public class OtherClassWithoutAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DummyClassAttribute : Attribute
    {
        public string Name { get; }

        public bool IsEnabled { get; }

        public DummyClassAttribute(string name, bool isEnabled)
        {
            Name = name;
            IsEnabled = isEnabled;
        }
    }

    public interface IDummyInterface
    {
    }

    public interface IDummyInterface<T>
    {
    }

    public class ClassThatImplementsInterface : IDummyInterface, IDummyInterface<IDummyInterface>
    {
    }

    public class ClassThatDoesNotImplementInterface
    {
    }

    public class DummyBaseType<T> : IDummyInterface<IDummyInterface>
    {
    }

    public class ClassWithGenericBaseType : DummyBaseType<ClassWithGenericBaseType>
    {
    }

    public class ClassWithMembers
    {
        protected internal ClassWithMembers() { }

        private ClassWithMembers(string _) { }

        protected string PrivateWriteProtectedReadProperty { get => null; private set { } }

        internal string this[string str] { private get => str; set { } }

        protected internal string this[int i] { get => i.ToString(CultureInfo.InvariantCulture); private set { } }

        private void VoidMethod() { }

        private void VoidMethod(string _) { }
    }

    public class ClassExplicitlyImplementingInterface : IExplicitInterface
    {
        public string ImplicitStringProperty { get => null; private set { } }

        string IExplicitInterface.ExplicitStringProperty { set { } }

        public string ExplicitImplicitStringProperty { get; set; }

        string IExplicitInterface.ExplicitImplicitStringProperty { get; set; }

        public void ImplicitMethod() { }

        public void ImplicitMethod(string overload) { }

        void IExplicitInterface.ExplicitMethod() { }

        void IExplicitInterface.ExplicitMethod(string overload) { }

        public void ExplicitImplicitMethod() { }

        public void ExplicitImplicitMethod(string _) { }

        void IExplicitInterface.ExplicitImplicitMethod() { }

        void IExplicitInterface.ExplicitImplicitMethod(string overload) { }
    }

    public interface IExplicitInterface
    {
        string ImplicitStringProperty { get; }

        string ExplicitStringProperty { set; }

        string ExplicitImplicitStringProperty { get; set; }

        void ImplicitMethod();

        void ImplicitMethod(string overload);

        void ExplicitMethod();

        void ExplicitMethod(string overload);

        void ExplicitImplicitMethod();

        void ExplicitImplicitMethod(string overload);
    }

    public class ClassWithoutMembers
    {
    }

    public interface IPublicInterface
    {
    }

    internal interface IInternalInterface
    {
    }

    internal class InternalClass
    {
    }

    internal struct InternalStruct
    {
    }

    internal enum InternalEnum
    {
        Value1,
        Value2
    }

    internal class Nested
    {
        private class PrivateClass
        {
        }

        protected enum ProtectedEnum { }

        public interface IPublicInterface
        {
        }

        internal class InternalClass
        {
        }

        protected internal interface IProtectedInternalInterface
        {
        }
    }

    internal readonly struct TypeWithConversionOperators
    {
        private readonly int value;

        private TypeWithConversionOperators(int value)
        {
            this.value = value;
        }

        public static implicit operator int(TypeWithConversionOperators typeWithConversionOperators) =>
            typeWithConversionOperators.value;

        public static explicit operator byte(TypeWithConversionOperators typeWithConversionOperators) =>
            (byte)typeWithConversionOperators.value;
    }

    internal sealed class Sealed
    {
    }

    internal abstract class Abstract
    {
    }

    internal static class Static
    {
    }

    internal struct Struct
    {
    }

    public delegate void ExampleDelegate();

    internal class ClassNotInDummyNamespace
    {
    }

    internal class OtherClassNotInDummyNamespace
    {
    }

    #endregion
}

namespace AssemblyB
{
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

    /// <summary>
    /// A class that intentionally has the exact same name and namespace as the ClassC from the AssemblyB
    /// assembly. This class is used to test the behavior of comparisons on such types.
    /// </summary>
    internal class ClassC
    {
    }

#pragma warning restore 436
}

#region Internal classes used in unit tests

namespace DummyNamespace
{
    internal class ClassInDummyNamespace
    {
    }

    namespace InnerDummyNamespace
    {
        internal class ClassInInnerDummyNamespace
        {
        }
    }
}

namespace DummyNamespaceTwo
{
    internal class ClassInDummyNamespaceTwo
    {
    }
}

#endregion
