---
title: Upgrading to version 8.0
permalink: /upgradingtov8
layout: single
toc: true
sidebar:
  nav: "sidebar"
---

## Dropping support for .NET Core 2.x and .NET Core 3.x.

As of v7, we've decided to no longer directly target .NET Core. All versions of .NET Core, including .NET 5 are [already out of support by Microsoft](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core). But we still support .NET Standard 2.0 and 2.1, so Fluent Assertions will still work with those deprecated frameworks.

## From `Execute.Assertion` to `AssertionChain`

We've made quite some changes to the API that you use to build your own assertions. For example, the `BooleanAssertions` class was instantiated in `AssertionExtensions` like this:

```csharp
public static BooleanAssertions Should(this bool actualValue)
{
    return new BooleanAssertions(actualValue);
}
```

On turn, the `BooleanAssertions` would expose a `BeTrue` method

```csharp
public AndConstraint<TAssertions> BeTrue(string because = "", params object[] becauseArgs)
{
    Execute.Assertion
        .ForCondition(Subject == true)
        .BecauseOf(because, becauseArgs)
        .FailWith("Expected {context:boolean} to be {0}{reason}, but found {1}.", true, Subject);

    return new AndConstraint<TAssertions>((TAssertions)this);
}
```

To be able to support chaining multiple assertions where the chained assertion can extend the caller identification, we introduced an `AssertionChain` class which instance can flow from one assertion to another. Because of that, the above code changed to:

```csharp
public static BooleanAssertions Should(this bool actualValue)
{
    return new BooleanAssertions(actualValue, AssertionChain.GetOrCreate());
}
```

Notice how we pass the call to `AssertionChain.GetOrCreate` to the assertions class? By default `GetOrCreate` will create a new instance of  `AssertionChain`. But if the previous assertion method uses `AssertionChain.ReuseOnce`, `GetOrCreate` will return that reused instance only once.

The new `BeTrue` now looks like:

```csharp
public AndConstraint<TAssertions> BeTrue(string because = "", params object[] becauseArgs)
{
    assertionChain
        .ForCondition(Subject == true)
        .BecauseOf(because, becauseArgs)
        .FailWith("Expected {context:boolean} to be {0}{reason}, but found {1}.", true, Subject);

    return new AndConstraint<TAssertions>((TAssertions)this);
}
```

So all of the methods to build an assertion that used to live on the `AssertionScope` (which is what `Execute.Assertion` returned), have now moved to `AssertionChain`. This is great because it allows the second assertion to get access to the state of the first assertion. For instance, if the first assertion failed, any successive attempts to call `FailWith` will not do anything.

## No more `ClearExpectation`

If you wanted to reuse the first part of the failure message across multiple failures, you could use the following construct (example taken from `TimeOnlyAssertions.BeCloseTo`):

```csharp
Execute.Assertion
    .BecauseOf(because, becauseArgs)
    .WithExpectation("Expected {context:the time} to be within {0} from {1}{reason}, ", precision, nearbyTime)
    .ForCondition(Subject is not null)
    .FailWith("but found <null>.")
    .Then
    .ForCondition(Subject?.IsCloseTo(nearbyTime, precision) == true)
    .FailWith("but {0} was off by {1}.", Subject, difference)
    .Then
    .ClearExpectation();
```

When using an `using new AssertionScope()` construct to wrap multiple assertions, all assertions executed within that scope will reuse the same instance of `AssertionScope` (which is what `Execute.Assertion` returned). The problem was that you had to explicitly call `ClearExpectation` to prevent the failure message passed to `WithExpectation` to leak into the next assertion within that scope. People often forgot that.

We solved this in v7, by making `WithExpectation` use a nested construct. This is what it now looks like:

```csharp
assertionChain
    .BecauseOf(because, becauseArgs)
    .WithExpectation("Expected {context:the time} to be within {0} from {1}{reason}, ", precision, nearbyTime, chain => chain
      .ForCondition(Subject is not null)
      .FailWith("but found <null>.")
      .Then
      .ForCondition(Subject?.IsCloseTo(nearbyTime, precision) == true)
      .FailWith("but {0} was off by {1}.", Subject, difference)
    );
```

All the code nested within the `WithExpectation` will share the first part of the failure message, and there's no need to explicitly clear it anymore.

## Amending caller identifiers with `WithPostfix`

Imagine the following chained assertion

```csharp
var element = XElement.Parse(
                """
                <parent>
                    <child />
                    <child />
                </parent>
                """);


element.Should().HaveElement("child", AtLeast.Twice()).Which.Should().HaveCount(1);
```

Prior to version 7, if the `HaveElement` assertion succeeded, but the `NotBeNull` failed, you would get the following exception:

    Expected element to contain 1 item(s), but found 3: {<child />, <child />, <child />}.

Now, in v7, it'll will return the following:

    Expected element/child to contain 1 item(s), but found 3: {<child />, <child />, <child />}.

This is possible because `HaveElement` will pass the `AssertionChain` through `ReuseOnce` to the succeeding `HaveCount()` _and_ amend the automatically detected caller identifier `element` (the part on which the first `Should` is invoked) with `"/child"` using `WithCallerPostfix`. Since this is a common thing in v7, the `AndWhichConstraint` has a constructor that does most of that automatically.

This is what `HaveElement` looks like (with some details left out):

```csharp
public AndWhichConstraint<XElementAssertions, XElement> HaveElement(XName expected,
    string because = "", params object[] becauseArgs)
{
    xElement = Subject!.Element(expected);

    assertionChain
        .ForCondition(xElement is not null)
        .BecauseOf(because, becauseArgs)
        .FailWith(
            "Expected {context:subject} to have child element {0}{reason}, but no such child element was found.",
            expected.ToString().EscapePlaceholders());

    return new AndWhichConstraint<XElementAssertions, XElement>(this, xElement, assertionChain, "/" + expected);
}
```

Notice the last argument to the `AndWhichConstraint` constructor.

## Drop direct support for assertions on `HttpResponseMessage`

If you need to do so, please refer to [FluentAssertions.Web](https://github.com/adrianiftode/FluentAssertions.Web) which
offers a bunch of extensions on the HTTP specific types.

## Other breaking changes

Check out the [release notes](releases.md) for other changes that might affect the upgrade to v8.
