using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentAssertions.Common;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class MethodInfoAssertionSpecs
    {
        #region BeVirtual

        [Fact]
        public void When_asserting_a_method_is_virtual_and_it_is_then_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsVirtual).GetParameterlessMethod("PublicVirtualDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_virtual_but_it_is_not_then_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithNonVirtualPublicMethods).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method Void FluentAssertions.Specs.ClassWithNonVirtualPublicMethods.PublicDoNothing" +
                    " to be virtual because we want to test the error message," +
                    " but it is not virtual.");
        }

        #endregion

        #region NotBeVirtual

        [Fact]
        public void When_asserting_a_method_is_not_virtual_and_it_is_not_then_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithNonVirtualPublicMethods).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeVirtual();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_virtual_but_it_is_then_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsVirtual).GetParameterlessMethod("PublicVirtualDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeVirtual("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method *ClassWithAllMethodsVirtual.PublicVirtualDoNothing" +
                    " not to be virtual because we want to test the error message," +
                    " but it is.");
        }

        #endregion

        #region BeDecoratedWithOfT

        [Fact]
        public void When_asserting_a_method_is_decorated_with_attribute_and_it_is_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_MethodImpl_attribute_and_it_is_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetParameterlessMethod("DoNotInlineMe");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_constructor_is_decorated_with_MethodImpl_attribute_and_it_is_it_succeeds()
        {
            // Arrange
            ConstructorInfo constructorMethodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetConstructor(Type.EmptyTypes);

            // Act
            Action act = () =>
                constructorMethodInfo.Should().BeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_MethodImpl_attribute_and_it_is_not_it_throws()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothing to be decorated with " +
                        "System.Runtime.CompilerServices.MethodImplAttribute, but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_MethodImpl_attribute_with_no_options_and_it_is_it_throws()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetParameterlessMethod("NoOptions");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodWithImplementationAttribute.NoOptions to be decorated with " +
                        "System.Runtime.CompilerServices.MethodImplAttribute, but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_MethodImpl_attribute_with_zero_as_options_and_it_is_it_throws()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetParameterlessMethod("ZeroOptions");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodWithImplementationAttribute.ZeroOptions to be decorated with " +
                        "System.Runtime.CompilerServices.MethodImplAttribute, but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_class_is_decorated_with_MethodImpl_attribute_and_it_is_not_it_throws()
        {
            // Arrange
            var type = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute);

            // Act
            Action act = () =>
                type.Should().BeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected type FluentAssertions.Specs.ClassWithAllMethodsDecoratedWithDummyAttribute to be decorated with " +
                        "System.Runtime.CompilerServices.MethodImplAttribute, but the attribute was not found.");
        }

        [Fact]
        public void When_a_method_is_decorated_with_an_attribute_it_should_allow_chaining_assertions_on_it()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () => methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>().Which.Filter.Should().BeFalse();

            // Assert
            act.Should().Throw<XunitException>();
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_an_attribute_but_it_is_not_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing to be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was not found.");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_BeDecoratedWith_it_should_throw()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () => methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_attribute_matching_a_predicate_and_it_is_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>(d => d.Filter);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_MethodImpl_attribute_matching_a_predicate_and_it_is_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetParameterlessMethod("DoNotInlineMe");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<MethodImplAttribute>(x => x.Value == MethodImplOptions.NoInlining);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_an_attribute_matching_a_predicate_but_it_is_not_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeDecoratedWith<DummyMethodAttribute>(d => !d.Filter, "because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodsThatAreNotDecoratedWithDummyAttribute.PublicDoNothing to be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_an_MethodImpl_attribute_matching_a_predicate_but_it_is_not_it_throws()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetParameterlessMethod("DoNotInlineMe");

            // Act
            Action act = () =>
               methodInfo.Should().BeDecoratedWith<MethodImplAttribute>(x => x.Value == MethodImplOptions.AggressiveInlining);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithMethodWithImplementationAttribute.DoNotInlineMe to be decorated with " +
                        "System.Runtime.CompilerServices.MethodImplAttribute, but that attribute was not found.");
        }

        [Fact]
        public void When_asserting_a_method_is_decorated_with_an_attribute_and_multiple_attributes_match_continuation_using_the_matched_value_should_fail()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothingWithSameAttributeTwice");

            // Act
            Action act =
                () =>
                    methodInfo.Should()
                        .BeDecoratedWith<DummyMethodAttribute>()
                        .Which.Filter.Should()
                        .BeTrue();

            // Assert
            act.Should().Throw<XunitException>();
        }

        #endregion

        #region NotBeDecoratedWithOfT

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_attribute_and_it_is_not_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_MethodImpl_attribute_and_it_is_not_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodsThatAreNotDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_constructor_is_not_decorated_with_MethodImpl_attribute_and_it_is_not_it_succeeds()
        {
            // Arrange
            ConstructorInfo constructorMethodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetConstructor(new List<Type> { typeof(string) });

            // Act
            Action act = () =>
                constructorMethodInfo.Should().NotBeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_an_attribute_but_it_is_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>("because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothing to not be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was found.");
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_MethodImpl_attribute_and_it_is_it_throws()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithMethodWithImplementationAttribute).GetParameterlessMethod("DoNotInlineMe");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<MethodImplAttribute>();

            // Assert
            act.Should().Throw<XunitException>()
            .WithMessage(
                "Expected method Void FluentAssertions.Specs.ClassWithMethodWithImplementationAttribute.DoNotInlineMe to not be decorated with " +
                    "System.Runtime.CompilerServices.MethodImplAttribute, but that attribute was found.");
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_attribute_matching_a_predicate_and_it_is_not_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>(d => !d.Filter);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_NotBeDecoratedWith_it_should_throw()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () => methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>(isMatchingAttributePredicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("isMatchingAttributePredicate");
        }

        [Fact]
        public void When_asserting_a_method_is_not_decorated_with_an_attribute_matching_a_predicate_but_it_is_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsDecoratedWithDummyAttribute).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeDecoratedWith<DummyMethodAttribute>(d => d.Filter, "because we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected method Void FluentAssertions.Specs.ClassWithAllMethodsDecoratedWithDummyAttribute.PublicDoNothing to not be decorated with " +
                        "FluentAssertions.Specs.DummyMethodAttribute because we want to test the error message," +
                        " but that attribute was found.");
        }

        #endregion

        #region BeAsync

        [Fact]
        public void When_asserting_a_method_is_async_and_it_is_then_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsAsync).GetParameterlessMethod("PublicAsyncDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeAsync();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_async_but_it_is_not_then_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithNonAsyncMethods).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().BeAsync("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected method Task FluentAssertions.Specs.ClassWithNonAsyncMethods.PublicDoNothing" +
                    " to be async because we want to test the error message," +
                    " but it is not.");
        }

        #endregion

        #region NotBeAsync

        [Fact]
        public void When_asserting_a_method_is_not_async_and_it_is_not_then_it_succeeds()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithNonAsyncMethods).GetParameterlessMethod("PublicDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeAsync();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_asserting_a_method_is_not_async_but_it_is_then_it_throws_with_a_useful_message()
        {
            // Arrange
            MethodInfo methodInfo = typeof(ClassWithAllMethodsAsync).GetParameterlessMethod("PublicAsyncDoNothing");

            // Act
            Action act = () =>
                methodInfo.Should().NotBeAsync("we want to test the error {0}", "message");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*ClassWithAllMethodsAsync.PublicAsyncDoNothing*" +
                    "not to be async*because we want to test the error message*");
        }

        #endregion
    }

    #region Internal classes used in unit tests

    internal class ClassWithAllMethodsVirtual
    {
        public virtual void PublicVirtualDoNothing()
        {
        }

        internal virtual void InternalVirtualDoNothing()
        {
        }

        protected virtual void ProtectedVirtualDoNothing()
        {
        }
    }

    internal interface IInterfaceWithPublicMethod
    {
        void PublicDoNothing();
    }

    internal class ClassWithNonVirtualPublicMethods : IInterfaceWithPublicMethod
    {
        public void PublicDoNothing()
        {
        }

        internal void InternalDoNothing()
        {
        }

        protected void ProtectedDoNothing()
        {
        }
    }

    internal class ClassWithAllMethodsDecoratedWithDummyAttribute
    {
        [DummyMethod(Filter = true)]
        public void PublicDoNothing()
        {
        }

        [DummyMethod(Filter = true)]
        [DummyMethod(Filter = false)]
        public void PublicDoNothingWithSameAttributeTwice()
        {
        }

        [DummyMethod]
        protected void ProtectedDoNothing()
        {
        }

        [DummyMethod]
        private void PrivateDoNothing()
        {
        }
    }

    internal class ClassWithMethodsThatAreNotDecoratedWithDummyAttribute
    {
        public void PublicDoNothing()
        {
        }

        protected void ProtectedDoNothing()
        {
        }

        private void PrivateDoNothing()
        {
        }
    }

    internal class ClassWithAllMethodsAsync
    {
        public async Task PublicAsyncDoNothing()
        {
            await Task.Yield();
        }

        internal async Task InternalAsyncDoNothing()
        {
            await Task.Yield();
        }

        protected async Task ProtectedAsyncDoNothing()
        {
            await Task.Yield();
        }
    }

    internal class ClassWithNonAsyncMethods
    {
        public Task PublicDoNothing()
        {
            return null;
        }

        internal Task InternalDoNothing()
        {
            return null;
        }

        protected Task ProtectedDoNothing()
        {
            return null;
        }
    }

    internal class ClassWithMethodWithImplementationAttribute
    {
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public ClassWithMethodWithImplementationAttribute() { }

        public ClassWithMethodWithImplementationAttribute(string ignoreMe) { }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DoNotInlineMe() { }

        [MethodImpl]
        public void NoOptions() { }

        [MethodImpl((MethodImplOptions)0)]
        public void ZeroOptions() { }
    }

    #endregion
}
