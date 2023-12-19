using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertionsAsync.Types;
using Internal.Main.Test;
using Xunit;

namespace FluentAssertionsAsync.Specs.Types;

public class MethodInfoSelectorSpecs
{
    [Fact]
    public void When_method_info_selector_is_created_with_a_null_type_it_should_throw()
    {
        // Arrange
        MethodInfoSelector methodInfoSelector;

        // Act
        Action act = () => methodInfoSelector = new MethodInfoSelector((Type)null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("types");
    }

    [Fact]
    public void When_method_info_selector_is_created_with_a_null_type_list_it_should_throw()
    {
        // Arrange
        MethodInfoSelector methodInfoSelector;

        // Act
        Action act = () => methodInfoSelector = new MethodInfoSelector((Type[])null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("types");
    }

    [Fact]
    public void When_method_info_selector_is_null_then_should_should_throw()
    {
        // Arrange
        MethodInfoSelector methodInfoSelector = null;

        // Act
        var act = () => methodInfoSelector.Should();

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("methodSelector");
    }

    [Fact]
    public void When_selecting_methods_from_types_in_an_assembly_it_should_return_the_applicable_methods()
    {
        // Arrange
        Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

        // Act
        IEnumerable<MethodInfo> methods = assembly.Types()
            .ThatAreDecoratedWith<SomeAttribute>()
            .Methods();

        // Assert
        methods.Should()
            .HaveCount(2)
            .And.Contain(m => m.Name == "Method1")
            .And.Contain(m => m.Name == "Method2");
    }

    [Fact]
    public void When_selecting_methods_that_are_public_or_internal_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatArePublicOrInternal;

        // Assert
        const int PublicMethodCount = 2;
        const int InternalMethodCount = 1;
        methods.Should().HaveCount(PublicMethodCount + InternalMethodCount);
    }

    [Fact]
    public void When_selecting_methods_decorated_with_specific_attribute_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWith<DummyMethodAttribute>().ToArray();

        // Assert
        methods.Should().HaveCount(2);
    }

    [Fact]
    public void When_selecting_methods_not_decorated_with_specific_attribute_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWith<DummyMethodAttribute>().ToArray();

        // Assert
        methods.Should()
            .NotBeEmpty()
            .And.NotContain(m => m.Name == "PublicVirtualVoidMethodWithAttribute")
            .And.NotContain(m => m.Name == "ProtectedVirtualVoidMethodWithAttribute");
    }

    [Fact]
    public void When_selecting_methods_that_return_a_specific_type_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatReturn<string>().ToArray();

        // Assert
        methods.Should().HaveCount(2);
    }

    [Fact]
    public void When_selecting_methods_that_do_not_return_a_specific_type_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatDoNotReturn<string>().ToArray();

        // Assert
        methods.Should().HaveCount(5);
    }

    [Fact]
    public void When_selecting_methods_without_return_value_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatReturnVoid.ToArray();

        // Assert
        methods.Should().HaveCount(4);
    }

    [Fact]
    public void When_selecting_methods_with_return_value_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatDoNotReturnVoid.ToArray();

        // Assert
        methods.Should().HaveCount(3);
    }

    [Fact]
    public void When_combining_filters_to_filter_methods_it_should_return_only_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods()
            .ThatArePublicOrInternal
            .ThatReturnVoid
            .ToArray();

        // Assert
        methods.Should().HaveCount(2);
    }

    [Fact]
    public void When_selecting_methods_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWith<DummyMethodAttribute>().ToArray();

        // Assert
        methods.Should().BeEmpty();
    }

    [Fact]
    public void
        When_selecting_methods_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWithOrInherit<DummyMethodAttribute>().ToArray();

        // Assert
        methods.Should().ContainSingle();
    }

    [Fact]
    public void When_selecting_methods_not_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWith<DummyMethodAttribute>().ToArray();

        // Assert
        methods.Should().ContainSingle();
    }

    [Fact]
    public void
        When_selecting_methods_not_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWithOrInherit<DummyMethodAttribute>().ToArray();

        // Assert
        methods.Should().BeEmpty();
    }

    [Fact]
    public void When_selecting_methods_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWith<DummyMethodNonInheritableAttributeAttribute>()
            .ToArray();

        // Assert
        methods.Should().BeEmpty();
    }

    [Fact]
    public void
        When_selecting_methods_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods =
            type.Methods().ThatAreDecoratedWithOrInherit<DummyMethodNonInheritableAttributeAttribute>().ToArray();

        // Assert
        methods.Should().BeEmpty();
    }

    [Fact]
    public void
        When_selecting_methods_not_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWith<DummyMethodNonInheritableAttributeAttribute>()
            .ToArray();

        // Assert
        methods.Should().ContainSingle();
    }

    [Fact]
    public void When_selecting_methods_that_are_abstract_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithAbstractAndVirtualMethods);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreAbstract().ToArray();

        // Assert
        methods.Should().HaveCount(3);
    }

    [Fact]
    public void When_selecting_methods_that_are_not_abstract_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithAbstractAndVirtualMethods);

        // Act
        IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotAbstract().ToArray();

        // Assert
        methods.Should().HaveCount(10);
    }

    [Fact]
    public void When_selecting_methods_that_are_async_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithAsyncAndNonAsyncMethod);

        // Act
        MethodInfo[] methods = type.Methods().ThatAreAsync().ToArray();

        // Assert
        methods.Should().ContainSingle()
            .Which.Name.Should().Be("PublicAsyncMethod");
    }

    [Fact]
    public void When_selecting_methods_that_are_not_async_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithAsyncAndNonAsyncMethod);

        // Act
        MethodInfo[] methods = type.Methods().ThatAreNotAsync().ToArray();

        // Assert
        methods.Should().ContainSingle()
            .Which.Name.Should().Be("PublicNonAsyncMethod");
    }

    [Fact]
    public void When_selecting_methods_that_are_virtual_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        MethodInfo[] methods = type.Methods().ThatAreVirtual().ToArray();

        // Assert
        methods.Should()
            .NotBeEmpty()
            .And.Contain(m => m.Name == "PublicVirtualVoidMethodWithAttribute")
            .And.Contain(m => m.Name == "ProtectedVirtualVoidMethodWithAttribute");
    }

    [Fact]
    public void When_selecting_methods_that_are_not_virtual_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        MethodInfo[] methods = type.Methods().ThatAreNotVirtual().ToArray();

        // Assert
        methods.Should()
            .NotBeEmpty()
            .And.NotContain(m => m.Name == "PublicVirtualVoidMethodWithAttribute")
            .And.NotContain(m => m.Name == "ProtectedVirtualVoidMethodWithAttribute");
    }

    [Fact]
    public void When_selecting_methods_that_are_static_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithStaticAndNonStaticMethod);

        // Act
        MethodInfo[] methods = type.Methods().ThatAreStatic().ToArray();

        // Assert
        methods.Should().ContainSingle()
            .Which.Name.Should().Be("PublicStaticMethod");
    }

    [Fact]
    public void When_selecting_methods_that_are_not_static_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithStaticAndNonStaticMethod);

        // Act
        MethodInfo[] methods = type.Methods().ThatAreNotStatic().ToArray();

        // Assert
        methods.Should().ContainSingle()
            .Which.Name.Should().Be("PublicNonStaticMethod");
    }

    [Fact]
    public void
        When_selecting_methods_not_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<MethodInfo> methods =
            type.Methods().ThatAreNotDecoratedWithOrInherit<DummyMethodNonInheritableAttributeAttribute>().ToArray();

        // Assert
        methods.Should().ContainSingle();
    }

    [Fact]
    public void When_selecting_methods_return_types_it_should_return_the_correct_types()
    {
        // Arrange
        Type type = typeof(TestClassForMethodReturnTypesSelector);

        // Act
        IEnumerable<Type> returnTypes = type.Methods().ReturnTypes().ToArray();

        // Assert
        returnTypes.Should()
            .HaveCount(3)
            .And.Contain(typeof(void))
            .And.Contain(typeof(int))
            .And.Contain(typeof(string));
    }

    [Fact]
    public void When_accidentally_using_equals_it_should_throw_a_helpful_error()
    {
        // Arrange
        Type type = typeof(TestClassForMethodSelector);

        // Act
        var action = () => type.Methods().Should().Equals(null);

        // Assert
        action.Should().Throw<NotSupportedException>()
            .WithMessage("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
    }
}

#region Internal classes used in unit tests

internal class TestClassForMethodSelector
{
#pragma warning disable 67, S3264 // "event is never used"
    public event EventHandler SomethingChanged = (_, _) => { };
#pragma warning restore 67, S3264

    public virtual void PublicVirtualVoidMethod()
    {
    }

    [DummyMethod]
    public virtual void PublicVirtualVoidMethodWithAttribute()
    {
    }

    internal virtual int InternalVirtualIntMethod()
    {
        return 0;
    }

    [DummyMethod]
    protected virtual void ProtectedVirtualVoidMethodWithAttribute()
    {
    }

    private void PrivateVoidDoNothing()
    {
    }

    protected virtual string ProtectedVirtualStringMethod()
    {
        return "";
    }

    private string PrivateStringMethod()
    {
        return "";
    }
}

internal class TestClassForMethodSelectorWithInheritableAttribute
{
    [DummyMethod]
    public virtual void PublicVirtualVoidMethodWithAttribute() { }
}

internal class TestClassForMethodSelectorWithNonInheritableAttribute
{
    [DummyMethodNonInheritableAttribute]
    public virtual void PublicVirtualVoidMethodWithAttribute() { }
}

internal class TestClassForMethodSelectorWithInheritableAttributeDerived : TestClassForMethodSelectorWithInheritableAttribute
{
    public override void PublicVirtualVoidMethodWithAttribute() { }
}

internal class TestClassForMethodSelectorWithNonInheritableAttributeDerived
    : TestClassForMethodSelectorWithNonInheritableAttribute
{
    public override void PublicVirtualVoidMethodWithAttribute() { }
}

internal class TestClassForMethodSelectorWithAsyncAndNonAsyncMethod
{
    public async Task PublicAsyncMethod() => await Task.Yield();

    public Task PublicNonAsyncMethod() => Task.CompletedTask;
}

internal class TestClassForMethodSelectorWithStaticAndNonStaticMethod
{
    public static void PublicStaticMethod() { }

    public void PublicNonStaticMethod() { }
}

internal class TestClassForMethodReturnTypesSelector
{
    public void SomeMethod() { }

    public int AnotherMethod() { return default; }

    public string OneMoreMethod() { return default; }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class DummyMethodNonInheritableAttributeAttribute : Attribute
{
    public bool Filter { get; set; }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DummyMethodAttribute : Attribute
{
    public bool Filter { get; set; }
}

internal abstract class TestClassForMethodSelectorWithAbstractAndVirtualMethods
{
    public abstract void PublicAbstractMethod();

    protected abstract void ProtectedAbstractMethod();

    internal abstract void InternalAbstractMethod();

    public static void PublicStaticMethod() { }

    protected static void ProtectedStaticMethod() { }

    internal static void InternalStaticMethod() { }

    public virtual void PublicVirtualMethod() { }

    protected virtual void ProptectedVirtualMethod() { }

    internal virtual void InternalVirtualMethod() { }

    public void PublicNotAbstractMethod() { }

    protected void ProtectedNotAbstractMethod() { }

    internal void InternalNotAbstractMethod() { }

    private void PrivateAbstractMethod() { }
}

#endregion
