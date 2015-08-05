using System;
using System.Collections.Generic;
using System.Reflection;

using FluentAssertions.Types;

using Internal.Main.Test;
using Internal.Other.Test;
using Internal.Other.Test.Common;

#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class TypeSelectorSpecs
    {
        [TestMethod]
        public void When_selecting_types_that_derive_from_a_specific_class_it_should_return_the_correct_types()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------            
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Assembly assembly = typeof(ClassDerivedFromSomeBaseClass).Assembly;
#else
            Assembly assembly = typeof(ClassDerivedFromSomeBaseClass).GetTypeInfo().Assembly;
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<Type> types = AllTypes.From(assembly).ThatDeriveFrom<SomeBaseClass>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            types.Should()
                .HaveCount(1)
                .And.Contain(typeof(ClassDerivedFromSomeBaseClass));
        }

        [TestMethod]
        public void When_selecting_types_that_derive_from_a_specific_generic_class_it_should_return_the_correct_types()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Assembly assembly = typeof(ClassDerivedFromSomeGenericBaseClass).Assembly;
#else
            Assembly assembly = typeof(ClassDerivedFromSomeGenericBaseClass).GetTypeInfo().Assembly;
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            TypeSelector types = AllTypes.From(assembly).ThatDeriveFrom<SomeGenericBaseClass<int>>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            types.ToArray().Should()
                .HaveCount(1)
                .And.Contain(typeof(ClassDerivedFromSomeGenericBaseClass));
        }

        [TestMethod]
        public void When_selecting_types_that_implement_a_specific_interface_it_should_return_the_correct_types()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Assembly assembly = typeof(ClassImplementingSomeInterface).Assembly;
#else
            Assembly assembly = typeof(ClassImplementingSomeInterface).GetTypeInfo().Assembly;
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<Type> types = AllTypes.From(assembly).ThatImplement<ISomeInterface>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            types.Should()
                .HaveCount(2)
                .And.Contain(typeof(ClassImplementingSomeInterface))
                .And.Contain(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }

        [TestMethod]
        public void When_selecting_types_that_are_decorated_with_a_specific_attribute_it_should_return_the_correct_types()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;
#else
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<Type> types = AllTypes.From(assembly).ThatAreDecoratedWith<SomeAttribute>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            types.Should()
                .HaveCount(2)
                .And.Contain(typeof(ClassWithSomeAttribute))
                .And.Contain(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }

        [TestMethod]
        public void When_selecting_types_from_specific_namespace_it_should_return_the_correct_types()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;
#else
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<Type> types = AllTypes.From(assembly).ThatAreInNamespace("Internal.Other.Test");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            types.Should()
                .HaveCount(1)
                .And.Contain(typeof(SomeOtherClass));
        }

        [TestMethod]
        public void When_selecting_types_from_specific_namespace_or_sub_namespaces_it_should_return_the_correct_types()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;
#else
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<Type> types = AllTypes.From(assembly).ThatAreUnderNamespace("Internal.Other.Test");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            types.Should()
                .HaveCount(2)
                .And.Contain(typeof(SomeOtherClass))
                .And.Contain(typeof(SomeCommonClass));
        }

        [TestMethod]
        public void When_combining_type_selection_filters_it_should_return_the_correct_types()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
#if !WINRT && !WINDOWS_PHONE_APP && !CORE_CLR
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;
#else
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;
#endif

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreDecoratedWith<SomeAttribute>()
                .ThatImplement<ISomeInterface>()
                .ThatAreInNamespace("Internal.Main.Test");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            types.Should()
                .HaveCount(1)
                .And.Contain(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }
    }
}

#region Internal classes used in unit tests

namespace Internal.Main.Test
{
    internal class SomeBaseClass
    {
    }

    internal class ClassDerivedFromSomeBaseClass : SomeBaseClass
    {
    }

    internal class SomeGenericBaseClass<T>
    {
        public T Value { get; set; }
    }

    internal class ClassDerivedFromSomeGenericBaseClass : SomeGenericBaseClass<int>
    {
    }

    internal interface ISomeInterface
    {
    }

    internal class ClassImplementingSomeInterface : ISomeInterface
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class SomeAttribute : Attribute
    {
    }

    [Some]
    internal class ClassWithSomeAttribute
    {
        public string Property1 { get; set; }

        public void Method1()
        {
        }
    }

    [Some]
    internal class ClassWithSomeAttributeThatImplementsSomeInterface : ISomeInterface
    {
        public string Property2 { get; set; }

        public void Method2()
        {
        }
    }
}

namespace Internal.Other.Test
{
    internal class SomeOtherClass
    {
    }
}

namespace Internal.Other.Test.Common
{
    internal class SomeCommonClass
    {
    }
}

#endregion