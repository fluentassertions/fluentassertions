---
title: Object graph comparison
permalink: /objectgraphs/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

Consider the class `Order` and its wire-transfer equivalent `OrderDto` (a so-called [DTO](http://en.wikipedia.org/wiki/Data_transfer_object)).
Suppose also that an order has one or more `Product`s and an associated `Customer`.
Coincidentally, the `OrderDto` will have one or more `ProductDto`s and a corresponding `CustomerDto`.
You may want to make sure that all exposed members of all the objects in the `OrderDto` object graph match the equally named members of the `Order` object graph.

You may assert the structural equality of two object graphs with `Should().BeEquivalentTo()`:
```csharp
orderDto.Should().BeEquivalentTo(order);
```

Additionally you can check the inequality of two objects with `Should().NotBeEquivalentTo()`:
```csharp
orderDto.Should().NotBeEquivalentTo(order);
```

All options described in the following sections are available for both `BeEquivalentTo` and `NotBeEquivalentTo`.

### Recursion ###

The comparison is recursive by default.
To avoid infinite recursion, Fluent Assertions will recurse up to 10 levels deep by default, but if you want to force it to go as deep as possible, use the `AllowingInfiniteRecursion` option.
On the other hand, if you want to disable recursion, just use this option:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.ExcludingNestedObjects());
```

### Value Types ###

To determine whether Fluent Assertions should recurs into an object's properties or fields, it needs to understand what types have value semantics and what types should be treated as reference types. The default behavior is to treat every type that overrides `Object.Equals` as an object that was designed to have value semantics. Unfortunately, anonymous types and tuples also override this method, but because we tend to use them quite often in equivalency comparison, we always compare them by their properties.

You can easily override this by using the `ComparingByValue<T>` or `ComparingByMembers<T>` options for individual assertions:

```csharp
subject.Should().BeEquivalentTo(expected,
   options => options.ComparingByValue<IPAddress>());
```

Or  do the same using the global options:

```csharp
AssertionOptions.AssertEquivalencyUsing(options => options
    .ComparingByValue<DirectoryInfo>());
```

### Auto-Conversion ###
In the past, Fluent Assertions would attempt to convert the value of a property of the subject-under-test to the type of the corresponding property on the expectation. But a lot of people complained about this behavior where a string property representing a date and time would magically match a `DateTime` property. As of 5.0, this conversion will no longer happen. However, you can still adjust the assertion by using the `WithAutoConversion` or `WithAutoConversionFor` options:

```csharp
subject.Should().BeEquivalentTo(expectation, options => options
    .WithAutoConversionFor(x => x.SelectedMemberPath.Contains("Birthdate")));
```

### Compile-time types vs. run-time types ###

By default, Fluent Assertions respects an object's or member's declared (compile-time) type when selecting members to process during a recursive comparison.
That is to say if the subject is a `OrderDto` but the variable it is assigned to has type `Dto` only the members defined by the latter class would be considered when comparing the object to the `order` variable.
This behavior can be configured and you can choose to use run-time types if you prefer:

```csharp
Dto orderDto = new OrderDto();

// Use runtime type information of orderDto
orderDto.Should().BeEquivalentTo(order, options => 
    options.RespectingRuntimeTypes());

// Use declared type information of orderDto
orderDto.Should().BeEquivalentTo(order, options => 
    options.RespectingDeclaredTypes());
```

One exception to this rule is when the declared type is `object`.
Since `object` doesn't expose any properties, it makes no sense to respect the declared type.
So if the subject or member's type is `object`, it will use the run-time type for that node in the graph. This will also work better with (multidimensional) arrays.

### Matching Members ###

All public members of the `Order` object must be available on the `OrderDto` having the same name. If any members are missing, an exception will be thrown.
However, you may customize this behavior.
For instance, if you want to include only the members both object graphs have:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.ExcludingMissingMembers());
```

### Selecting Members ###

If you want to exclude certain (potentially deeply nested) individual members using the `Excluding()` method:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.Excluding(o => o.Customer.Name));
```

The `Excluding()` method on the options object also takes a lambda expression that offers a bit more flexibility for deciding what member to exclude:

```csharp
orderDto.Should().BeEquivalentTo(order, options => options 
    .Excluding(ctx => ctx.SelectedMemberPath == "Level.Level.Text"));
```

Maybe far-fetched, but you may even decide to exclude a member on a particular nested object by its index.

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.Excluding(o => o.Products[1].Status));
```

Of course, `Excluding()` and `ExcludingMissingMembers()` can be combined.

You can also take a different approach and explicitly tell Fluent Assertions which members to include. You can directly specify a property expression or use a predicate that acts on the provided `ISubjectInfo`.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options
    .Including(o => o.OrderNumber)
    .Including(pi => pi.SelectedMemberPath.EndsWith("Date"));
```

### Including properties and/or fields ###

You may also configure member inclusion more broadly.
Barring other configuration, Fluent Assertions will include all `public` properties and fields.
This behavior can be changed:

```csharp
// Include Fields
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingFields();

// Include Properties
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingProperties();

// Exclude Fields
orderDto.Should().BeEquivalentTo(order, options => options
    .ExcludingFields();

// Exclude Properties
orderDto.Should().BeEquivalentTo(order, options => options
    .ExcludingProperties();
```

This configuration affects the initial inclusion of members and happens before any `Exclude`s or other `IMemberSelectionRule`s.
This configuration also affects matching.
For example, that if properties are excluded, properties will not be inspected when looking for a match on the expected object.

### Equivalency Comparison Behavior ###
In addition to influencing the members that are including in the comparison, you can also override the actual assertion operation that is executed on a particular member.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
    .When(info => info.SelectedMemberPath.EndsWith("Date")));
```

If you want to do this for all members of a certain type, you can shorten the above call like this.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options 
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
    .WhenTypeIs<DateTime>());
```

### Enums ###

By default, `Should().BeEquivalentTo()` compares `Enum` members by the enum's underlying numeric value.
An option to compare an `Enum` only by name is also available, using the following configuration :

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.ComparingEnumsByName());
```

### Collections and Dictionaries ###
Considering our running example, you could use the following against a collection of `OrderDto`s: 

```csharp
orderDtos.Should().BeEquivalentTo(orders, options => options.Excluding(o => o.Customer.Name));
```

You can also assert that all instances of `OrderDto` are structurally equal to a single object:

```csharp
orderDtos.Should().AllBeEquivalentTo(singleOrder);
```

### Ordering ###

Fluent Assertions will, by default, ignore the order of the items in the collections, regardless of whether the collection is at the root of the object graph or tucked away in a nested property or field.
If the order is important, you can override the default behavior with the following option:

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.WithStrictOrdering());
```

You can even tell FA to use strict ordering only for a particular collection or dictionary member, similar to how you exclude certain members:

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.WithStrictOrderingFor(s => s.Products));
```

And you can tell FA to generally use strict ordering but ignore it for a particular collection or dictionary member:

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.WithStrictOrdering().WithoutStrictOrderingFor(s => s.Products));
```

In case you chose to use strict ordering by default you can still configure non-strict ordering in specific tests:
```csharp
AssertionOptions.AssertEquivalencyUsing(options => options.WithStrictOrdering());

orderDto.Should().BeEquivalentTo(expectation, options => options.WithoutStrictOrdering());
```

**Notice:** For performance reasons, collections of bytes are compared in exact order. This is even true when applying `WithoutStrictOrdering()`.

### Diagnostics
`Should().BeEquivalentTo` is a very powerful feature, and one of the unique selling points of Fluent Assertions. But sometimes it can be a bit overwhelming, especially if some assertion fails under unexpected conditions. To help you understand how Fluent Assertions compared two (collections of) object graphs, the failure message will always include the relevant configuration settings:

```
Xunit.Sdk.XunitException
Expected item[0] to be 0x06, but found 0x01.
Expected item[1] to be 0x05, but found 0x02.
Expected item[2] to be 0x04, but found 0x03.
Expected item[3] to be 0x03, but found 0x04.
Expected item[4] to be 0x02, but found 0x05.
Expected item[5] to be 0x01, but found 0x06.

With configuration:
- Use declared types and members
- Compare enums by value
- Include all non-private properties
- Include all non-private fields
- Match member by name (or throw)
- Be strict about the order of items in byte arrays
```

However, sometimes that's not enough. For those scenarios where you need to understand a but more, you can add the `WithTracing` option. When added to the assertion call, it would extend the above output with something like this:

```
With trace:
  Structurally comparing System.Object[] and expectation System.Byte[] at root
  {
    Strictly comparing expectation 6 at root to item with index 0 in System.Object[]
    {
      Treating item[0] as a value type
    }
    Strictly comparing expectation 5 at root to item with index 1 in System.Object[]
    {
      Treating item[1] as a value type
    }
    Strictly comparing expectation 4 at root to item with index 2 in System.Object[]
    {
      Treating item[2] as a value type
    }
    Strictly comparing expectation 3 at root to item with index 3 in System.Object[]
    {
      Treating item[3] as a value type
    }
    Strictly comparing expectation 2 at root to item with index 4 in System.Object[]
    {
      Treating item[4] as a value type
    }
    Strictly comparing expectation 1 at root to item with index 5 in System.Object[]
    {
      Treating item[5] as a value type
    }
  }
```
  
By default, the trace is displayed only when an assertion fails as part of the message.
If the trace is needed for the successful assertion you can catch it like this:

```csharp
object object1 = [...];
object object2 = [...];
var traceWriter = new StringBuilderTraceWriter();            
object1.Should().BeEquivalentTo(object2, opts => opts.WithTracing(traceWriter));             
string trace = traceWriter.ToString();
```

Alternatively, you could write your own implementation of `ITraceWriter` for special purposes e.g. writing to a file.

### Global Configuration ###
Even though the structural equivalency API is pretty flexible, you might want to change some of these options on a global scale.
This is where the static class `AssertionOptions` comes into play.
For instance, to always compare enumerations by name, use the following statement:

```csharp
AssertionOptions.AssertEquivalencyUsing(options => 
   options.ComparingEnumsByValue);
``` 

All the options available to an individual call to `Should().BeEquivalentTo` are supported, with the exception of some of the overloads that are specific to the type of the subject (for obvious reasons).
