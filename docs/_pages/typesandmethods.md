---
title: Type, Method, and Property assertions
permalink: /typesandmethods/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

We have added a number of assertions on types and on methods and properties of types.
These are rather technical assertions and, although we like our unit tests to read as functional specifications for the application, we still see a use for assertions on the members of a class.
For example when you use policy injection on your classes and require its methods to be virtual.
Forgetting to make a method virtual will avoid the policy injection mechanism from creating a proxy for it, but you will only notice the consequences at runtime.
Therefore it can be useful to create a unit test that asserts such requirements on your classes.
Some examples.

```csharp
typeof(MyPresentationModel).Should().BeDecoratedWith<SomeAttribute>();

typeof(MyPresentationModel)
  .Should().BeDecoratedWithOrInherit<SomeInheritedOrDirectlyDecoratedAttribute>();

typeof(MyPresentationModel).Should().NotBeDecoratedWith<SomeAttribute>();
typeof(MyPresentationModel)
  .Should().NotBeDecoratedWithOrInherit<SomeInheritedOrDirectlyDecoratedAttribute>();

typeof(MyBaseClass).Should().BeAbstract();
typeof(InjectedClass).Should().NotBeStatic();

MethodInfo method = GetMethod();
method.Should().BeVirtual();

PropertyInfo property = GetSomeProperty();
property.Should().BeVirtual()
  .And.BeDecoratedWith<SomeAttribute>();
```

You can also perform assertions on multiple methods or properties in a certain type by using the `Methods()` or `Properties()` extension methods and some optional filtering methods.
Like this:

```csharp
typeof(MyPresentationModel).Methods()
  .ThatArePublicOrInternal 
  .ThatReturnVoid
  .Should()
  .BeVirtual("because this is required to intercept exceptions")
    .And.BeWritable()
    .And.BeAsync();

typeof(MyController).Methods()
  .ThatDoNotReturn<ActionResult>()
  .ThatAreNotDecoratedWith<HttpPostAttribute>()
  .Should().NotBeVirtual()
    .And.NotBeAsync()
    .And.NotReturnVoid()
    .And.NotReturn<ActionResult>();

typeof(MyController).Methods()
  .ThatReturn<ActionResult>()
  .ThatAreDecoratedWith<HttpPostAttribute>()
  .Should()
  .BeDecoratedWith<ValidateAntiForgeryTokenAttribute>(
    "because all Actions with HttpPost require ValidateAntiForgeryToken");
```

You can also perform assertions on all of methods return types to check class contract.
Like this:

```csharp
typeof(MyDataReader).Methods()
  .ReturnTypes()
  .Properties()
  .Should()
  .NotBeWritable("all the return types should be immutable");
```

If the methods return types are `IEnumerable<T>` or `Task<T>` you can unwrap underlying types to with `UnwrapTaskTypes` and `UnwrapEnumerableTypes` methods.
Like this:

```csharp
typeof(MyDataReader).Methods()
  .ReturnTypes()
  .UnwrapTaskTypes()
  .UnwrapEnumerableTypes()
  .Properties()
  .Should()
  .NotBeWritable("all the return types should be immutable");
```

If you also want to assert that an attribute has a specific property value, use this syntax.

```csharp
typeWithAttribute.Should()
  .BeDecoratedWith<DummyClassAttribute>(a => ((a.Name == "Unexpected") && a.IsEnabled));
```

You can assert methods or properties from all types in an assembly that apply to certain filters, like this:

```csharp
var types = typeof(ClassWithSomeAttribute).Assembly.Types()
  .ThatAreDecoratedWith<SomeAttribute>()
  .ThatImplement<ISomeInterface>()
  .ThatAreInNamespace("Internal.Main.Test");

var properties = types.Properties().ThatArePublicOrInternal;
properties.Should().BeVirtual();
```

Alternatively you can use this more fluent syntax instead.

```csharp
AllTypes.From(assembly)
  .ThatAreDecoratedWith<SomeAttribute>()
  .ThatImplement<ISomeInterface>()
  .ThatDeriveFrom<IDisposable>()
  .ThatAreUnderNamespace("Internal.Main.Test");

AllTypes.From(assembly)
  .ThatAreNotDecoratedWith<SomeAttribute>()
  .ThatDoNotImplement<ISomeInterface>()
  .ThatDoNotDeriveFrom<IDisposable>()
  .ThatAreNotUnderNamespace("Internal.Main")
  .ThatAreNotInNamespace("Internal.Main.Test");
```

There are so many possibilities and specialized methods that none of these examples do them good. Check out the [TypeAssertionSpecs.cs](https://github.com/fluentassertions/fluentassertions/blob/master/Tests/Shared.Specs/TypeAssertionSpecs.cs#L12) from the source for more examples. 
