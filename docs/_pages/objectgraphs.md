---
title: Object graphs
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

### Recursion

The comparison is recursive by default.
To avoid infinite recursion, Fluent Assertions will recurse up to 10 levels deep by default, but if you want to force it to go as deep as possible, use the `AllowingInfiniteRecursion` option.
On the other hand, if you want to disable recursion, just use this option:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.ExcludingNestedObjects());
```

### Strict typing (or not)

By default, `BeEquivalentTo` will consider objects equivalent as long as their members match, regardless of whether the types are exactly the same. This means that objects of different types can be considered equivalent if they have the same structure and values.

However, sometimes you may want to ensure that not only the values match, but also the types are exactly the same. For such scenarios, you can use the strict typing options:

```csharp
// Ensure all types must match exactly throughout the entire object graph
orderDto.Should().BeEquivalentTo(order, options => 
    options.WithStrictTyping());
```

If you only want to enforce strict typing for specific members or paths, you can use `WithStrictTypingFor`:

```csharp
// Only enforce strict typing for properties named "Nested"
orderDto.Should().BeEquivalentTo(order, options => 
    options.WithStrictTypingFor(info => info.Path.EndsWith("Nested")));

// Only enforce strict typing for the root object
orderDto.Should().BeEquivalentTo(order, options => 
    options.WithStrictTypingFor(info => info.Path.Length == 0));
```

The predicate in `WithStrictTypingFor` receives an `IObjectInfo` parameter that provides information about the current member being compared, including its path in the object graph. This allows you to precisely control where strict typing should be applied.

You can also disable strict typing that was previously enabled (for example, through global configuration):

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.WithoutStrictTyping());
```

**Note:** When using strict typing with runtime types (via `PreferringRuntimeMemberTypes()`), the comparison will use the actual runtime type rather than the declared type for the type equality check.

### Value Types

To determine whether Fluent Assertions should recurs into an object's properties or fields, it needs to understand what types have value semantics and what types should be treated as reference types. The default behavior is to treat every type that overrides `Object.Equals` as an object that was designed to have value semantics. Anonymous types, `record`s, `record struct`s and tuples also override this method, but because the community proved us that they use them quite often in equivalency comparisons, we decided to always compare them by their members.

You can easily override this by using the `ComparingByValue<T>`, `ComparingByMembers<T>`, `ComparingRecordsByValue` and `ComparingRecordsByMembers` options for individual assertions:

```csharp
subject.Should().BeEquivalentTo(expected,
   options => options.ComparingByValue<IPAddress>());
```

For `record`s and `record struct`s this works like this:

```csharp
actual.Should().BeEquivalentTo(expected, options => options
    .ComparingRecordsByValue()
    .ComparingByMembers<MyRecord>());
```

Or  do the same using the global options:

```csharp
AssertionConfiguration.Current.Equivalency.Modify(options => options
    .ComparingByValue<DirectoryInfo>());
```

Note that primitive types are never compared by their members and trying to call e.g. `ComparingByMembers<int>` will throw an `InvalidOperationException`.

### Auto-Conversion

In the past, Fluent Assertions would attempt to convert the value of a property of the subject-under-test to the type of the corresponding property on the expectation. But a lot of people complained about this behavior where a string property representing a date and time would magically match a `DateTime` property. As of 5.0, this conversion will no longer happen. However, you can still adjust the assertion by using the `WithAutoConversion` or `WithAutoConversionFor` options:

```csharp
subject.Should().BeEquivalentTo(expectation, options => options
    .WithAutoConversionFor(x => x.Path.Contains("Birthdate")));
```

### Compile-time types vs. run-time types

By default, Fluent Assertions respects an object's or member's declared (compile-time) type when selecting members to process during a recursive comparison.
That is to say if the subject is a `OrderDto` but the variable it is assigned to has type `Dto` only the members defined by the latter class would be considered when comparing the object to the `order` variable.
This behavior can be configured and you can choose to use run-time types if you prefer:

```csharp
Dto orderDto = new OrderDto();

// Use the runtime type of the members of orderDto
orderDto.Should().BeEquivalentTo(order, options => 
    options.PreferringRuntimeMemberTypes());

// Use the declared type information of the members of orderDto
orderDto.Should().BeEquivalentTo(order, options => 
    options.PreferringDeclaredMemberTypes());
```

One exception to this rule is when the declared type is `object`.
Since `object` doesn't expose any properties, it makes no sense to respect the declared type.
So if the subject or member's type is `object`, it will use the run-time type for that node in the graph. This will also work better with (multidimensional) arrays.

### Matching Members

All public members of the `Order` object must be available on the `OrderDto` having the same name. If any members are missing, an exception will be thrown.
However, you may customize this behavior.
For instance, if you want to include only the members both object graphs have:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.ExcludingMissingMembers());
```

### Selecting Members

If you want to exclude certain (potentially deeply nested) individual members using the `Excluding()` method:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.Excluding(o => o.Customer.Name));
```

The `Excluding()` method on the options object also takes a lambda expression that offers a bit more flexibility for deciding what member to exclude:

```csharp
orderDto.Should().BeEquivalentTo(order, options => options 
    .Excluding(ctx => ctx.Path == "Level.Level.Text"));
```

And if you want to exclude one or more specific properties everywhere in the object graph just by their names, use `ExcludingMembersNamed` like this:

```csharp
orderDto.Should().BeEquivalentTo(order, options => options 
    .ExcludingMembersNamed("ID", "Version"));
```

Maybe far-fetched, but you may even decide to exclude a member on a particular nested object by its index.

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.Excluding(o => o.Products[1].Status));
```

You can use `For` and `Exclude` if you want to exclude a member on each nested object regardless of its index.

```csharp
orderDto.Should().BeEquivalentTo(order, options =>
    options.For(o => o.Products)
           .Exclude(o => o.Status));
```

Using `For` you can navigate arbitrarily deep. Consider a `Product` has a collection of `Part`s and a `Part` has a name. Using `For` your can also exclude the `Name` of all `Part`s of all `Product`s.

```csharp
orderDto.Should().BeEquivalentTo(order, options =>
    options.For(o => o.Products)
           .For(o => o.Parts)
           .Exclude(o => o.Name));
```

You can also use an anonymous object to exclude members. This can be useful if you want to exclude multiple (maybe nested) fields and avoid writing 
`Excluding().Excuding().Excluding()...`.

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.Excluding(o => new { o.Customer.Name, o.Customer.LastName, o.Vat }));
```

This is also possible after `.For()` like

```csharp
orderDto.Should().BeEquivalentTo(order, options =>
    options.For(o => o.Products)
           .Exclude(o => new { o.Name, o.Price }));
```

Of course, `Excluding()` and `ExcludingMissingMembers()` can be combined.

You can also take a different approach and explicitly tell Fluent Assertions which members to include. You can directly specify a property expression or use a predicate that acts on the provided `ISubjectInfo`.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options
    .Including(o => o.OrderNumber)
    .Including(pi => pi.Name == "Date"));
```

### Including properties and/or fields

You may also configure member inclusion more broadly.
Barring other configuration, Fluent Assertions will include all `public` properties and fields.
This behavior can be changed:

```csharp
// Include properties (which is the default)
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingProperties());

// Include fields
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingFields());

// Include internal properties as well
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingInternalProperties());

// And the internal fields
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingInternalFields());

// Exclude Fields
orderDto.Should().BeEquivalentTo(order, options => options
    .ExcludingFields());

// Exclude Properties
orderDto.Should().BeEquivalentTo(order, options => options
    .ExcludingProperties());
```

This configuration affects the initial inclusion of members and happens before any `Exclude`s or other `IMemberSelectionRule`s. This configuration also affects matching. For example, that if properties are excluded, properties will not be inspected when looking for a match on the expected object.

The behavior with anonymous objects for selection also applies to `Include` as it does to `Exclude`.

### Comparing members with different names

Imagine you want to compare an `Order` and an `OrderDto` using `BeEquivalentTo`, but the first type has a `Name` property and the second has a `OrderName` property. You can map those using the following option:

```csharp
// Using names with the expectation member name first. Then the subject's member name.
orderDto.Should().BeEquivalentTo(order, options => options
    .WithMapping("Name", "OrderName"));

// Using expressions, but again, with expectation first, subject last.
orderDto.Should().BeEquivalentTo(order, options => options
    .WithMapping<OrderDto>(e => e.Name, s => s.OrderName));
```

Another option is to map two deeply nested members to each other. In that case, your path must start at the root:

```csharp
// Using dotted property paths 
rootSubject.Should().BeEquivalentTo(rootExpectation, options => options
    .WithMapping("Parent.Collection[].Member", "Parent.Collection[].Member"));

// Using expressions
rootSubject.Should().BeEquivalentTo(rootExpectation, options => options
    .WithMapping<SubjectType>(e => e.Parent.Collection[0].Member, s => s.Parent.Collection[0].Member));
```

Note that collection indices in string-based paths are not allowed. Within expressions, you must use an index to make it a valid property path, but it'll be ignored. So both the examples are equivalent. Also, such nested paths must have the same parent. So mapping properties or fields at different levels is not (yet) supported.

Now imagine those types appear somewhere in the object graph. Then you can use this overload:

```csharp
// Using names
orderDto.Should().BeEquivalentTo(order, options => options
    .WithMapping<Order, OrderDto>("Name", "OrderName"));

// Using expressions
orderDto.Should().BeEquivalentTo(order, options => options
    .WithMapping<Order, OrderDto>(e => e.Name, s => s.OrderName));
```

Notice that you can also map properties to fields and vice-versa.

### Hidden Members

Sometimes types have members out of necessity, to satisfy a contract, but they aren't logically a part of the type. In this case, they are often marked with the attribute `[EditorBrowsable(EditorBrowsableState.Never)]`, so that the object can satisfy the contract but the members don't show up in IntelliSense when writing code that uses the type.

If you want to compare objects that have such fields, but you want to exclude the non-browsable "hidden" members (for instance, their implementations often simply throw `NotImplementedException`), you can call `ExcludingNonBrowsableMembers` on the options object:

```csharp
class DataType
{
    public int X { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public int Y => throw new NotImplementedException();
}

DataType original, derived;

derived.Should().BeEquivalentTo(original, options => options
    .ExcludingNonBrowsableMembers());
```

### Custom assertions

In addition to influencing the members that are including in the comparison, you can also override the actual assertion operation that is executed on a particular member.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
    .When(info => info.Path.EndsWith("Date")));
```

If you want to do this for all members of a certain type, you can shorten the above call like this.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options 
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
    .WhenTypeIs<DateTime>());
```

### Inline assertions

The `Using`/`When` construct is one way to customize the assertion logic, but often does not feel very "fluent". Instead, `Value.ThatMatches` and `Value.ThatSatisfies` provides a much more flexible syntax for inline assertions. Considering the following subject:

```csharp
var actual = new
{
    Name = "John",
    Age = 30
};
```

To ensure that `Age` is less than 40, you can use either one of the following constructs.

```csharp
actual.Should().BeEquivalentTo(new
{
    Name = "John",
    Age = Value.ThatMatches<int>(age => age < 40)
});

actual.Should().BeEquivalentTo(new
{
    Name = "John",
    Age = Value.ThatSatisfies<int>(age => age.Should().BeLessThan(40))
});
```

### Enums

By default, `Should().BeEquivalentTo()` compares `Enum` members by the enum's underlying numeric value.
An option to compare an `Enum` only by name is also available, using the following configuration :

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.ComparingEnumsByName());
```

Note that even though an enum's underlying value equals a numeric value or the enum's name equals some string value, we do not consider those to be equivalent.
In other words, enums are only considered to be equivalent to enums of the same or another type, but you can control whether they should equal by name or by value.

### Collections and Dictionaries

Considering our running example, you could use the following against a collection of `OrderDto`s:

```csharp
orderDtos.Should().BeEquivalentTo(orders, options => options.Excluding(o => o.Customer.Name));
```

You can also assert that all instances of `OrderDto` are structurally equal to a single object:

```csharp
orderDtos.Should().AllBeEquivalentTo(singleOrder);
```

### JSON

For projects targeting .NET 6 or later, you can also compare a `JsonNode` from the `System.Text.Json` namespace representing a deeply nested JSON block against another object such as an anonymous type. You can even use inline assertions like `Value.ThatSatisfies()` and `Value.ThatMatches()`.

```csharp
var node = JsonNode.Parse(
    """
    {
        "name": "Product",
        "price": 99.99,
        "isAvailable": true,
        "firstIntroducedOn": "2025-09-11T21:17:00",
        "tags": ["electronics", "gadget"],
        "metadata": {
            "settings": {
                "visible": true,
                "priority": 1
            }
        }
    }
    """);

node.Should().BeEquivalentTo(new
{
    name = "Product",
    price = 99.99,
    isAvailable = true,
    firstIntroducedOn = 11.September(2025).At(21, 17),
    tags = new[]{"electronics", "gadget"},
    metadata = new
    {
        settings = new
        {
            visible = true,
            priority = Value.ThatMatches<int>(x => x > 0)
        }
    },
});
```

By default, the casing of the properties of the expectation must match those of the JSON properties, but you can influence that using the `IgnoringJsonPropertyCasing()` option. Next to that, as you can see in the example above, JSON properties representing local or UTC-based ISO 8601 compliant dates and times, can be compared to a `DateTime` property. 

### Ordering

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
AssertionConfiguration.Current.Equivalency.Modify(options => options.WithStrictOrdering());

orderDto.Should().BeEquivalentTo(expectation, options => options.WithoutStrictOrdering());
```

**Notice:** For performance reasons, collections of bytes are compared in exact order. This is even true when applying `WithoutStrictOrdering()`.

### Diagnostics

`Should().BeEquivalentTo` is a very powerful feature, and one of the unique selling points of Fluent Assertions. But sometimes it can be a bit overwhelming, especially if some assertion fails under unexpected conditions. To help you understand how Fluent Assertions compared two (collections of) object graphs, the failure message will always include the relevant configuration settings:

```csharp
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

However, sometimes that's not enough. For those scenarios where you need to understand a bit more, you can add the `WithTracing` option. When added to the assertion call, it would extend the above output with something like this:

```text
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

### Global Configuration

Even though the structural equivalency API is pretty flexible, you might want to change some of these options on a global scale.
This is where the static class `AssertionConfiguration` comes into play.
For instance, to always compare enumerations by name, use the following statement:

```csharp
AssertionConfiguration.Current.Equivalency.Modify(options => 
   options.ComparingEnumsByName);
```

All the options available to an individual call to `Should().BeEquivalentTo` are supported, with the exception of some of the overloads that are specific to the type of the expectation (for obvious reasons).
