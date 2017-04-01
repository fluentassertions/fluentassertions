---
title: Documentation
---

**This is the documentation for v3.0 and higher. You can find the v2.2 documentation [here](v2/documentation.md).**

## Coding by Example ##
As you may have noticed the purpose of this open-source project is to not only be the best assertion framework in the .NET realm, but to also demonstrate high-quality code. We heavily practice Test Driven Development and one of the promises TDD makes is that unit tests can be treated as your API's documentation. So although you are free to go through the many examples here, please consider to analyze the many [unit tests](https://github.com/fluentassertions/fluentassertions/tree/master/Tests/FluentAssertions.Shared.Specs).

## Table of Contents ##
* TOC
{:toc}


## Supported Test Frameworks ##

Fluent Assertions supports MSTest, MSTest2, NUnit, xUnit, xUnit2, MSpec, NSpec, MBUnit and the Gallio Framework as well as the Windows Store and Windows Phone unit testing frameworks. You can simply add a reference to the corresponding test framework assembly to the unit test project. Fluent Assertions will automatically find the corresponding assembly and use it for throwing the framework-specific exceptions. 

If, for some unknown reason, Fluent Assertions fails to find the assembly, try specifying the framework explicitly using a configuration setting in the project’s app.config. If it cannot find any of the supported frameworks, it will fall back to using a custom `AssertFailedException` exception class.

```xml
<configuration>
  <appSettings>
    <!-- Supported values: nunit, xunit, mstest, mspec, mbunit and gallio -->
    <add key="FluentAssertions.TestFramework" value="nunit"/>
  </appSettings>
</configuration>
```
Just add nuget package "fluentassertions" to your test project.

## Basic assertions ##
The following assertions are available to all types of objects. 

```csharp
object theObject = null;
theObject.Should().BeNull("because the value is null");
theObject.Should().NotBeNull();

theObject = "whatever";
theObject.Should().BeOfType<string>("because a {0} is set", typeof(string));
theObject.Should().BeOfType(typeof(string), "because a {0} is set", typeof(string));	
```

Sometimes you might like to first assert that an object is of a certain type using `BeOfType` and then continue with additional assertions on the result of casting that object to the specified type. You can do that by chaining those assertions onto the `Which` property like this. 

```csharp
someObject.Should().BeOfType<Exception>().Which.Message.Should().Be("Other Message");
```

To assert that two objects are equal (through their implementation of `Object.Equals`), use 

```csharp
string otherObject = "whatever";
theObject.Should().Be(otherObject, "because they have the same values");
theObject.Should().NotBe(otherObject);
```
 
If you want to make sure two objects are not just functionally equal but refer to the exact same object in memory, use the following two methods. 

```csharp
theObject = otherObject; 
theObject.Should().BeSameAs(otherObject);
theObject.Should().NotBeSameAs(otherObject);
```

Other examples of some general purpose assertions include

```csharp
var ex = new ArgumentException();
ex.Should().BeAssignableTo<Exception>("because it is an exception");

var dummy = new Object();
dummy.Should().Match(d => (d.ToString() == "System.Object"));
dummy.Should().Match<string>(d => (d == "System.Object"));
dummy.Should().Match((string d) => (d == "System.Object"));
```

Some users requested the ability to easily downcast an object to one of its derived classes in a fluent way.

```csharp	
customer.Animals.First().As<Human>().Height.Should().Be(178);
```

We’ve also added the possibility to assert that an object can be serialized and deserialized using the XML or binary formatters.

```csharp
theObject.Should().BeXmlSerializable();
theObject.Should().BeBinarySerializable();
```

Internally, `BeBinarySerializable` uses the [Object graph comparison](#object-graph-comparison) API, so if you are in need of excluding certain properties from the comparison (for instance, because its backing field is `[NonSerializable]`, you can do this:

```csharp
theObject.Should().BeBinarySerializable<MyClass>(
	options => options.Excluding(s => s.SomeNonSerializableProperty));
```

Fluent Assertions has special support for `[Flags]` based enumerations, which allow you to do something like this:

```csharp
regexOptions.Should().HaveFlag(RegexOptions.Global);
regexOptions.Should().NotHaveFlag(RegexOptions.CaseInsensitive);
```

## Nullable types ##

```csharp
short? theShort = null;
theShort.Should().NotHaveValue();
theShort.Should().BeNull();

int? theInt = 3;
theInt.Should().HaveValue();
theInt.Should().NotBeNull();

DateTime? theDate = null;
theDate.Should().NotHaveValue();
theDate.Should().BeNull();
```
## Booleans ##

```csharp
bool theBoolean = false;
theBoolean.Should().BeFalse("it's set to false");

theBoolean = true;
theBoolean.Should().BeTrue();
theBoolean.Should().Be(otherBoolean);
```

Obviously the above assertions also work for nullable booleans, but if you really want to be make sure a boolean is either `true` or `false` and not `null`, you can use these methods.

```csharp
theBoolean.Should().NotBeFalse();
theBoolean.Should().NotBeTrue();
```

## Strings ##

For asserting whether a string is null, empty or contains whitespace only, you have a wide range of methods to your disposal.

```csharp
string theString = "";
theString.Should().NotBeNull();
theString.Should().BeNull();
theString.Should().BeEmpty();
theString.Should().NotBeEmpty("because the string is not empty"); 
theString.Should().HaveLength(0);
theString.Should().BeNullOrWhiteSpace(); // either null, empty or whitespace only
theString.Should().NotBeNullOrWhiteSpace();
```

Obviously you’ll find all the methods you would expect for string assertions.

```csharp
theString = "This is a String";
theString.Should().Be("This is a String");
theString.Should().NotBe("This is another String");
theString.Should().BeEquivalentTo("THIS IS A STRING");

theString.Should().Contain("is a");
theString.Should().NotContain("is a");
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING");
theString.Should().NotContainEquivalentOf("HeRe ThE CaSiNg Is IgNoReD As WeLl");

theString.Should().StartWith("This");
theString.Should().NotStartWith("This");
theString.Should().StartWithEquivalent("this");
theString.Should().NotStartWithEquivalentOf("this");

theString.Should().EndWith("a String");
theString.Should().NotEndWith("a String");
theString.Should().EndWithEquivalent("a string");
theString.Should().NotEndWithEquivalentOf("a string");
```

We even support wildcards. For instance, if you would like to assert that some email address is correct, use this:

```csharp
emailAddress.Should().Match("*@*.com");
```

If the casing of the input string is irrelevant, use this:

```csharp	
emailAddress.Should().MatchEquivalentOf("*@*.COM");
```

And if wildcards aren't enough for you, you can always use some regular expression magic:

```csharp
someString.Should().MatchRegex("h.*\\sworld.$");
subject.Should().NotMatchRegex(".*earth.*");
```

## Numeric types and everything else that implements IComparable<T\> ##

```csharp
int theInt = 5;
theInt.Should().BeGreaterOrEqualTo(5);
theInt.Should().BeGreaterOrEqualTo(3);
theInt.Should().BeGreaterThan(4);     
theInt.Should().BeLessOrEqualTo(5);
theInt.Should().BeLessThan(6);
theInt.Should().BePositive();
theInt.Should().Be(5);
theInt.Should().NotBe(10);
theInt.Should().BeInRange(1,10);

theInt = 0;
//theInt.Should().BePositive(); => Expected positive value, but found 0
//theInt.Should().BeNegative(); => Expected negative value, but found 0

theInt = -8;
theInt.Should().BeNegative();
int? nullableInt = 3;
nullableInt.Should().Be(3);

double theDouble = 5.1;     
theDouble.Should().BeGreaterThan(5);
byte theByte = 2;
theByte.Should().Be(2);
```

Notice that `Should().Be()` and `Should().NotBe()` are not available for floats and  doubles. As explained in  this article, floating point variables are inheritably inaccurate and should never be compared for equality. Instead, either use the Should().BeInRange() method or the following method specifically designed for floating point or `decimal` variables.

```csharp
float value = 3.1415927F;
value.Should().BeApproximately(3.14F, 0.01F);
```

This will verify that the value of the float is between 3.139 and 3.141.

To assert that a value matches one of the provided values, you can do this.

```csharp
value.Should().BeOneOf(new[] { 3, 6});
```

## Dates and times ##

For asserting a `DateTime` or a `DateTimeOffset` against various constraints, FA offers a bunch of methods that, provided that you use the extension methods for representinging dates and times, really help to keep your assertions readable.

```csharp
var theDatetime = 1.March(2010).At(22,15);

theDatetime.Should().BeAfter(1.February(2010));
theDatetime.Should().BeBefore(2.March(2010));     
theDatetime.Should().BeOnOrAfter(1.March(2010));

theDatetime.Should().Be(1.March(2010).At(22, 15));
theDatetime.Should().NotBe(1.March(2010).At(22, 16)); 

theDatetime.Should().HaveDay(1);
theDatetime.Should().HaveMonth(3);
theDatetime.Should().HaveYear(2010);
theDatetime.Should().HaveHour(22);
theDatetime.Should().HaveMinute(15);
theDatetime.Should().HaveSecond(0);

theDatetime.Should().BeSameDateAs(1.March(2010));
```

We've added a whole set of methods for asserting that the difference between two DateTime objects match a certain time frame. All five methods support a Before and  After extension method.

```csharp
theDatetime.Should().BeLessThan(10.Minutes()).Before(otherDatetime); // Equivalent to <
theDatetime.Should().BeWithin(2.Hours()).After(otherDatetime);       // Equivalent to <=
theDatetime.Should().BeMoreThan(1.Days()).Before(deadline);          // Equivalent to >
theDatetime.Should().BeAtLeast(2.Days()).Before(deliveryDate);       // Equivalent to >=
theDatetime.Should().BeExactly(24.Hours()).Before(appointement);     // Equivalent to ==
```

To assert that a date/time is within a specified number of milliseconds from another date/time value you can use this method.

```csharp
theDatetime.Should().BeCloseTo(March(2010).At(22,15), 2000); // 2000 milliseconds
theDatetime.Should().BeCloseTo(March(2010).At(22,15));       // default is 20 milliseconds
```

This can be particularly useful if your database truncates date/time values. 

## TimeSpans ##

FA also support a few dedicated methods that apply to (nullable) TimeSpans directly:

```csharp
var timeSpan = new TimeSpan(12, 59, 59); 
timeSpan.Should().BePositive(); 
timeSpan.Should().BeNegative(); 
timeSpan.Should().Be(12.Hours()); 
timeSpan.Should().NotBe(1.Days()); 
timeSpan.Should().BeLessThan(someOtherTimeSpan); 
timeSpan.Should().BeLessOrEqualTO(someOtherTimeSpan); 
timeSpan.Should().BeGreaterThan(someOtherTimeSpan); 
timeSpan.Should().BeGreaterOrEqualTO(someOtherTimeSpan);
```

Similarly to the [date and time assertions](#dates-and-times), `BeCloseTo` is also available for time spans:

```csharp
timeSpan.Should().BeCloseTo(new TimeSpan(13, 0, 0), 10.Ticks());
```

## Collections ##

A collection object in .NET is so versatile that the number of assertions on them require the same level of versatility. Most, if not all, are so self-explanatory that we'll just list them here.   

```csharp
IEnumerable collection = new[] { 1, 2, 5, 8 };

collection.Should().NotBeEmpty()
     .And.HaveCount(4)
     .And.ContainInOrder(new[] { 2, 5 })
     .And.ContainItemsAssignableTo<int>();

collection.Should().Equal(new List<int> { 1, 2, 5, 8 });
collection.Should().Equal(1, 2, 5, 8);
collection.Should().BeEquivalentTo(8, 2, 1, 5);
collection.Should().NotBeEquivalentTo(8, 2, 3, 5);

collection.Should().HaveCount(c => c > 3).And.OnlyHaveUniqueItems();
collection.Should().HaveSameCount(new[] {6, 2, 0, 5});

collection.Should().StartWith(element);
collection.Should().EndWith(element);

collection.Should().BeSubsetOf(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, });

collection.Should().ContainSingle();
collection.Should().ContainSingle(x => x > 3);
collection.Should().Contain(8).And.HaveElementAt(2, 5).And.NotBeSubsetOf(new[] {11, 56});
collection.Should().Contain(x => x > 3); 
collection.Should().Contain(collection, "", 5, 6); // It should contain the original items, plus 5 and 6.

collection.Should().OnlyContain(x => x < 10);
collection.Should().ContainItemsAssignableTo<int>();

collection.Should().ContainInOrder(new[] { 1, 5, 8 });

collection.Should().NotContain(82);
collection.Should().NotContainNulls();
collection.Should().NotContain(x => x > 10);

const int successor = 5;
const int predecessor = 5;
collection.Should().HaveElementPreceding(successor, element);
collection.Should().HaveElementSucceeding(predecessor, element);

collection.Should().BeEmpty();
collection.Should().BeNullOrEmpty();
collection.Should().NotBeNullOrEmpty();

IEnumerable otherCollection = new[] { 1, 2, 5, 8, 1 };
IEnumerable anotherCollection = new[] { 10, 20, 50, 80, 10 };
collection.Should().IntersectWith(otherCollection);
collection.Should().NotIntersectWith(anotherCollection);

collection.Should().BeInAscendingOrder();
collection.Should().NotBeAscendingOrder();
```

Those last two methods can be used to assert a collection contains items in ascending or descending order.  For simple types that might be fine, but for more complex types, it requires you to implement `IComparable`, something that doesn't make a whole lot of sense in all cases. That's why we offer overloads that take an expression.

```csharp
collection.Should().BeInAscendingOrder(x => x.SomeProperty);
```

A special overload of `Equal()` takes a lambda that is used for checking the equality of two collections without relying on the type’s Equals() method. Consider for instance two collections that contain some kind of domain entity persisted to a database and then reloaded. Since the actual object instance is different, if you want to make sure a particular property was properly persisted, you usually do something like this:

```csharp
persistedCustomers.Select(c => c.Name).Should().Equal(customers.Select(c => c.Name);
```

With this new overload, you can rewrite it into:

```csharp
persistedCustomers.Should().Equal(customers, (c1, c2) => c1.Name == c2.Name);
```

## Dictionaries ##

You can apply Fluent Assertions to your generic dictionaries as well. Of course you can assert any dictionary to be null or not null, and empty or not empty. Like this:

```csharp
Dictionary<int, string> dictionary;
dictionary.Should().BeNull();
dictionary = new Dictionary<int, string>();
dictionary.Should().NotBeNull();
dictionary.Should().BeEmpty();
dictionary.Add(1, "first element");
dictionary.Should().NotBeEmpty();
```

You can also assert the equality of the entire dictionary, where the equality of the keys and values will be validated using their Equals implementation. Like this:

```csharp
var dictionary1 = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" }
};

var dictionary2 = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" }
};

var dictionary3 = new Dictionary<int, string>
{
    { 3, "Three" },
};

dictionary1.Should().Equal(dictionary2);
dictionary1.Should().NotEqual(dictionary3);
```

Or you can assert that the dictionary contains a certain key or value:

```csharp
dictionary.Should().ContainKey(1);
dictionary.Should().NotContainKey(9);
dictionary.Should().ContainValue("One");
dictionary.Should().NotContainValue("Nine");
```

You can also assert that the dictionary has a certain number of items:

```csharp
dictionary.Should().HaveCount(2);
```

And finally you can assert that the dictionary contains a specific key/value pair or not:

```csharp
KeyValuePair<int, string> item = new KeyValuePair<int, string>(1, "One");

dictionary.Should().Contain(item);
dictionary.Should().Contain(2, "Two");
dictionary.Should().NotContain(9, "Nine");
```

Chaining additional assertion is supported as well.

```csharp
dictionary.Should().ContainValue(myClass).Which.SomeProperty.Should().BeGreaterThan(0);
```

## Guids ##

The assertions you can do on Guids are simple. You can assert their equality to another Guid, or you can assert that a Guid is empty.

```csharp
Guid theGuid = Guid.NewGuid();
Guid sameGuid = theGuid;
Guid otherGuid = Guid.NewGuid();

theGuid.Should().Be(sameGuid);
theGuid.Should().NotBe(otherGuid);
theGuid.Should().NotBeEmpty();

Guid.Empty.Should().BeEmpty();
```

## Enums ##
With the standard `Should().Be()` method, Enums are compared using .NET's `Enum.Equals()` implementation. This means that the Enums must be of the same type, and have the same underlying value.

## Exceptions ##

The following example verifies that the `Foo()` method throws an `InvalidOperationException` which `Message` property has a specific value.

```csharp
subject.Invoking(y => y.Foo("Hello"))
     .ShouldThrow<InvalidOperationException>()
     .WithMessage("Hello is not allowed at this moment");
```

But if you, like me, prefer the arrange-act-assert syntax, you can also use an action in your act part.

```csharp
Action act = () => subject.Foo2("Hello");

act.ShouldThrow<InvalidOperationException>()
     .WithInnerException<ArgumentException>()
     .WithInnerMessage("whatever");
```

Notice that the example also verifies that the exception has a particular inner exception with a specific message. in fact, you can even check the individual properties of the exception instance using the And property.

```csharp
Action act = () => subject.Foo(null));

act.ShouldThrow<ArgumentNullException>()
 .And.ParamName.Should().Equal("message");
```

An alternative syntax for doing the same is by chaining one or more calls to the `Where()` method:

```csharp
Action act = () => subject.Foo(null)); 
act.ShouldThrow<ArgumentNullException>().Where(e => e.Message.StartsWith("did"));
```

However, we discovered that testing the exception message for a substring is so common, that we changed the default behavior of `WithMessage` to support wildcard expressions and match in a case-insensitive way.

```csharp
Action act = () => subject.Foo(null)); 
act
  .ShouldThrow<ArgumentNullException>()
  .WithMessage("?did*");
```

On the other hand, you may want to verify that no exceptions were thrown.

```csharp
Action act = () => subject.Foo("Hello"));
act.ShouldNotThrow();
```

I know that a unit test will fail anyhow if an exception was thrown, but this syntax returns a clearer description of the exception that was thrown and fits better to the AAA syntax. 

If you want to verify that a specific exception is not thrown, and want to ignore others, you can do that using an overload:

```csharp
Action act = () => subject.Foo("Hello"));
act.ShouldNotThrow<InvalidOperationException>();
```

If the method you are testing returns an `IEnumerable` or `IEnumerable<T>` and it uses the `yield` keyword to construct that collection, just calling the method will not cause the effect you expected. Because the real work is not done until you actually iterate over that collection. you can use the Enumerating() extension method to force enumerating the collection like this.

```csharp
Func<IEnumerable<char>> func = () => obj.SomeMethodThatUsesYield("blah");
func.Enumerating().ShouldThrow<ArgumentException>();
```

You do have to use the `Func<T>` type instead of `Action<T>` then.

The exception throwing API follows the same rules as the `try`...`catch`...construction does. In other words, if you're expecting a certain exception to be (not) thrown, and a more specific exception is thrown instead, it would still satisfy the assertion. So throwing an `ApplicationException` when an `Exception` was expected will not fail the assertion. However, if you really want to be explicit about the exact type of exception, you can use `ShouldThrowExactly` and `WithInnerExceptionExactly`.  

.NET 4.0 and later includes the `AggregateException` which is typically thrown by code that runs through the Parallel Task Library or using the new `async` keyword. All of the above also works for exceptions that are aggregated, whether or not you are asserting on the actual `AggregateException` or any of its (nested) aggregated exceptions.

Talking about the `async` keyword, you can also verify that an asynchronously executed method throws or doesn't throw an exception:

```csharp
Func<Task> act = async () => { await asyncObject.ThrowAsync<ArgumentException>(); };
act.ShouldThrow<InvalidOperationException>();
act.ShouldNotThrow();
```

Alternatively, you can use the `Awaiting` method like this:

```csharp
Func<Task> act = () => asyncObject.Awaiting(async x => await x.ThrowAsync<ArgumentException>())
act.ShouldThrow<ArgumentException>();
```

Both give you the same results, so it's just a matter of personal preference. 

## Object graph comparison ##

Consider the class `Order` and its wire-transfer equivalent `OrderDto` (a so-called [DTO](http://en.wikipedia.org/wiki/Data_transfer_object)). Suppose also that an order has one or more `Product`s and an associated `Customer`. Coincidentally, the `OrderDto` will have one or more `ProductDto`s and a corresponding `CustomerDto`. You may want to make sure that all exposed members of all the objects in the `OrderDto` object graph match the equally named members of the `Order` object graph.

You may assert the structural equality of two object graphs with `ShouldbeEquivalentTo`:
```csharp
orderDto.ShouldBeEquivalentTo(order);
```

### Recursion ###

The comparison is recursive by default. To avoid infinite recursion, Fluent Assertions will recurse up to 10 levels deep by default, but if you want to force it to go as deep as possible, use the `AllowingInfiniteRecursion` option.
On the other hand, if you want to disable recursion, just use this option:

```csharp
orderDto.ShouldBeEquivalentTo(order, options => 
    options.ExcludingNestedObjects());
```

### Compile-time types vs. run-time types ###

By default, Fluent Assertions respects an object's or member's declared (compile-time) type when selecting members to process during a recursive comparison. That is to say if the subject is a `OrderDto` but the variable it is assigned to has type `Dto` only the members defined by the latter class would be included. This behavior can be configured and you can choose to use run-time types if you prefer:

```csharp
// Use runtime type information
orderDto.ShouldBeEquivalentTo(order, options => 
    options.RespectingRuntimeTypes());
	
// Use declared type information
orderDto.ShouldBeEquivalentTo(order, options => 
    options.RespectingDeclaredTypes());	
```

One exception to this rule is when the declared type is `object`. Since `object` doesn't expose any properties, it makes no sense to respect the declared type. So if the subject or member's type is `object`, it will use the run-time type for that node in the graph. This will also work better with (multidimensional) arrays.

### Matching Members ###

All public members of the `OrderDto` must be available on the `Order` having the same name.  If any members are missing, an exception will be thrown. However, you may customize this behavior. For instance, if you want to include only the members both object graphs have:

```csharp
orderDto.ShouldBeEquivalentTo(order, options => 
    options.ExcludingMissingMembers());
```

### Selecting Members ###

If you want to exclude certain (potentially deeply nested) individual members using the `Excluding()` method:

```csharp
orderDto.ShouldBeEquivalentTo(order, options => 
    options.Excluding(o => o.Customer.Name));
```

The `Excluding()` method on the options object also takes a lambda expression that offers a bit more flexibility for deciding what member to exclude:

```csharp
orderDto.ShouldBeEquivalentTo(order, options => options 
    .Excluding(ctx => ctx.SelectedMemberPath == "Level.Level.Text")); 
```

Maybe far-fetched, but you may even decide to exclude a member on a particular nested object by its index. 

```csharp
orderDto.ShouldBeEquivalentTo(order, options => 
    options.Excluding(o => o.Products[1].Status)); 
```

Of course, `Excluding()` and `ExcludingMissingMembers()` can be combined.

You can also take a different approach and explicitly tell Fluent Assertions which members to include. You can directly specify a property expression or use a predicate that acts on the provided `ISubjectInfo`.  

```csharp
orderDto.ShouldBeEquivalentTo(order, options => options
    .Including(o => o.OrderNumber)
	.Including(pi => pi.PropertyPath.EndsWidth("Date")); 
```

### Including properties and/or fields ###

You may also configure member inclusion more broadly. Barring other configuration, Fluent Assertions will include all `public` properties and fields. This behavior can be changed:

```csharp
// Include Fields
orderDto.ShouldBeEquivalentTo(order, options => options
	.IncludingFields();

// Include Properties
orderDto.ShouldBeEquivalentTo(order, options => options
	.IncludingProperties();
	
// Exclude Fields
orderDto.ShouldBeEquivalentTo(order, options => options
	.ExcludingFields();
	
// Exclude Properties
orderDto.ShouldBeEquivalentTo(order, options => options
	.ExcludingProperties();
```

This configuration affects the initial inclusion of members and happens before any `Exclude`s or other `IMemberSelectionRule`s.  This configuration also affects matching.  For example, that if properties are excluded, properties will not be inspected when looking for a match on the expected object.


### Equivalency Comparison Behavior ###
In addition to influencing the members that are including in the comparison, you can also override the actual assertion operation that is executed on a particular member. 

```csharp
orderDto.ShouldBeEquivalentTo(order, options => options
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
    .When(info => info.SelectedMemberPath.EndsWith("Date"))); 
```

If you want to do this for all members of a certain type, you can shorten the above call like this. 

```csharp
orderDto.ShouldBeEquivalentTo(order, options => options 
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000)) 
    .WhenTypeIs<DateTime>(); 
```

### Enums ###

By default, ``ShouldBeEquivalentTo()`` compares Enum members by the Enum's underlying numeric value. An option to compare Enums only by name is also available, using the following configuration :

```csharp
orderDto.ShouldBeEquivalentTo(expectation, options => options.ComparingEnumsByName());
```

### Collections and Dictionaries ###
The original `ShouldAllBeEquivalentTo()` extension method does support collections, but it doesn’t allow you to influence the comparison based on the actual collection type, nor does it support (nested) dictionaries. The new extension method `ShouldAllBeEquivalentTo()` supports this so you can configure comparisons on the type of items in the collection.

Considering our running example, you could use the following against a collection of `OrderDto`s: 

```csharp
orderDtos.ShouldAllBeEquivalentTo(orders, options => options.Excluding(o => o.Customer.Name)); 
```

### Ordering ###

Fluent Assertions will, by default, ignore the order of the items in the collections, regardless of whether the collection is at the root of the object graph or tucked away in a nested property or field. If the order is important, you can override the default behavior with the following option:

```csharp
orderDto.ShouldBeEquivalentTo(expectation, options => options.WithStrictOrdering());
```

You can even tell FA to use strict ordering only for a particular collection or dictionary member, similar to how you exclude certain members:

```csharp
orderDto.ShouldBeEquivalentTo(expectation, options => options.WithStrictOrderingFor(s => s.Products));
```

**Notice:** For performance reasons, collections of bytes are compared in exact order.

### Global Configuration ###
Even though the structural equivalency API is pretty flexible, you might want to change some of these options on a global scale. This is where the static class `AssertionOptions` comes into play. For instance, to always compare enumerations by name, use the following statement:

```csharp
AssertionOptions.AssertEquivalencyUsing(options => options.ComparingEnumsByValue);
``` 

All the options available to an individual call to `ShouldBeEquivalenTo` are supported, with the exception of some of the overloads that are specific to the type of the subject (for obvious reasons). You can even change the algorithm that Fluent Assertions uses to determine if an object should be treated as a value type. Simply replace the `AssertionOptions.IsValueType` predicate with your own:

```csharp
AssertionOptions.IsValueType = type => // a custom algorithm 
```  

### Extensibility ###
Internally the structural comparison process consists of three phases which repeat as the comparison recurses:

1. Select the members of the subject object to include in the comparison.  
2. Find a matching member on the expectation object and decide what to do if it can’t find any.  
3. Select the appropriate assertion method for the member’s type and execute it.  

Three main extension points exist: `IEquivalencyStep`, `IMemberSelectionRule`, `IMemberMatchingRule`.  You may add your own implementations using the `Using` method overloads on `EquivalencyAssertionOptions`, or if you want to make it a global change, to the `AssertionOptions.EquivalencySteps` collection. 

Fluent Assertions uses these same interfaces to provide its  built-in functionality.  Internally, for example, `ExcludeMemberByPredicateSelectionRule`, is added to the collection of selection rules when you use the `Excluding(expression)` method on the options parameter of `ShouldBeEquivalentTo()`. Even the `Using().When()` construct in the previous section is doing nothing more than inserting an `IEquivalencyStep` in to the list of equivalency steps. Creating your own rules is quite straightforward.
  
1. Choose the appropriate phase that the rule should influence 
2. Select the corresponding interface 
3. Create a class that implements this interface 
4. Add it to the `ShouldBeEquivalentTo()` call using the `Using()` method on the options parameters. 

```csharp
subject.ShouldBeEquivalentTo(expected, options => options.Using(new ExcludeForeignKeysSelectionRule()));
```

## Event Monitoring ##

Version 1.3.0 introduced a new set of extensions that allow you to verify that an object raised a particular event. Before you can invoke the assertion extensions, you must first tell Fluent Assertions that you want to monitor the object:

```csharp
var subject = new EditCustomerViewModel();
subject.MonitorEvents();
```

Assuming that we’re dealing with a MVVM implementation, you might want to verify that it raised its `PropertyChanged` event for a particular property:

```csharp
subject
  .ShouldRaise("PropertyChanged")
  .WithSender(subject)
  .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SomeProperty");
```

Notice that `WithSender()` verifies that all occurrences had its sender argument set to the specified object. `WithArgs()` just verifies that at least one occurrence had a matching `EventArgs` object. In other words, event monitoring only works for events that comply with the standard two-argument sender/args .NET pattern.

Since verifying for `PropertyChanged` events is so common, I’ve included a specialized shortcut to the example above:

```csharp
subject.ShouldRaisePropertyChangeFor(x => x.SomeProperty);
```

In version 1.4 you can also do the opposite; asserting that a particular event was not raised.

```csharp
subject.ShouldNotRaisePropertyChangeFor(x => x.SomeProperty);
```

Or, if your project is .NET 3.5 or 4.0 based:
	
```csharp
subject.ShouldNotRaise("SomeOtherEvent");
```

In version 4.1.2 we added a new generic version of `MonitorEvents()`. It is used to limit which events you want to listen to. You do that by providing a type which defines the events. 

```csharp
var subject = new ClassWithManyEvents();
subject.MonitorEvents<IInterfaceWithFewEvents>();
```

This generic version of `MonitorEvents()` is also very useful if you wish to monitor events of a dynamically generated class using `System.Reflection.Emit`. Since events are dynamically generated and are not present in parent class non-generic version of `MonitorEvents()` will not find the events. This way you can tell the event monitor which interface was implemented in the generated class.

```csharp
POCOClass subject = EmitViewModelFromPOCOClass();
subject.MonitorEvents<INotifyPropertyChanged>();  // POCO class doesn't have INotifyPropertyChanged implemented
subject.ShouldRaisePropertyChangeFor(x => x.SomeProperty);
```

**Important Limitation:** Due to limitations in Silverlight, Windows Phone and .NET for Windows Store Apps, only the `ShouldRaisePropertyChangeFor` and `ShouldNotRaisePropertyChangeFor` methods are supported in those versions.

## Type, Method, and Property assertions ##

Recently, we have added a number of assertions on types and on methods and properties of types. These are rather technical assertions and, although we like our unit tests to read as functional specifications for the application, we still see a use for assertions on the members of a class. For example when you use policy injection on your classes and require its methods to be virtual. Forgetting to make a method virtual will avoid the policy injection mechanism from creating a proxy for it, but you will only notice the consequences at runtime. Therefore it can be useful to create a unit test that asserts such requirements on your classes. Some examples.

```csharp
typeof(MyPresentationModel).Should().BeDecoratedWith<SomeAttribute>();

MethodInfo method = GetMethod();
method.Should().BeVirtual();

PropertyInfo property = GetSomeProperty();
property.Should().BeVirtual().And.BeDecoratedWith<SomeAttribute>();
```

You can also perform assertions on multiple methods or properties in a certain type by using the `Methods()` or `Properties()` extension methods and some optional filtering methods. Like this:

```csharp
typeof(MyPresentationModel).Methods()
  .ThatArePublicOrInternal 
  .ThatReturnVoid
  .Should()
  .BeVirtual("because this is required to intercept exceptions")
  .BeWritable();

typeof(MyController).Methods()
  .ThatReturn<ActionResult>()
  .ThatAreDecoratedWith<HttpPostAttribute>()
  .Should()
  .BeDecoratedWith<ValidateAntiForgeryTokenAttribute>(
    "because all Actions with HttpPost require ValidateAntiForgeryToken");
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
```
## Assembly References ##
New in version 3.1 are methods to assert an assembly does or does not reference another assembly. These are typically used to enforce layers within an application, such as for example, asserting the web layer does not reference the data layer. To assert the references, use the the following syntax:

```csharp 
assembly.Should().Reference(otherAssembly); 
assembly.Should().NotReference(otherAssembly);
```

These assertions are only available in the .NET 4 and 4.5 versions of Fluent Assertions as the reflection methods used are not available in Silverlight and Windows Phone and Windows 8 run-times.

## XML classes ##

Fluent Assertions has support for assertions on several of the LINQ-to-XML classes:

```csharp
xDocument.Should().HaveRoot("configuration");
xDocument.Should().HaveElement("settings");

xElement.Should().HaveValue("36");
xElement.Should().HaveAttribute("age", "36");
xElement.Should().HaveElement("address");
```

Those two last assertions also support `XName` parameters:

```csharp
xElement.Should().HaveAttribute(XName.Get("age", "http://www.example.com/2012/test"), "36");
xElement.Should().HaveElement(XName.Get("address", "http://www.example.com/2012/test"));

xAttribute.Should().HaveValue("Amsterdam");
```

You can also perform a deep comparison between two elements like this.

```csharp
xDocument.Should().BeEquivalentTo(XDocument.Parse("<configuration><item>value</item></configuration>"));
xElement.Should().BeEquivalentTo(XElement.Parse("<item>value</item>"));	
```

Chaining additional assertions on top of a particular (root) element is possible through this syntax. 

```csharp
xDocument.Should().HaveElement("child").Which.Should().BeOfType<XElement>().And.HaveAttribute("attr", "1");
```

## Execution Time ##

New in version 1.4 is a method to assert that the execution time of particular method or action does not exceed a predefined value. To verify the execution time of a method, use the following syntax:

```csharp
public class SomePotentiallyVerySlowClass
    {
      public void ExpensiveMethod()
      {
        for (short i = 0; i < short.MaxValue; i++)
        {
          string tmp = " ";
          if (!string.IsNullOrEmpty(tmp))
          {
            tmp += " ";
          }
        }
      }
    }
var subject = new SomePotentiallyVerySlowClass();
subject.ExecutionTimeOf(s => s.ExpensiveMethod()).ShouldNotExceed(500.Milliseconds());
```

Alternatively, to verify the execution time of an arbitrary action, use this syntax:

```csharp
Action someAction = () => Thread.Sleep(510);
someAction.ExecutionTime().ShouldNotExceed(100.Milliseconds());
```

Since it doesn’t make sense to do something like that in Silverlight, it is only available in the .NET 3.5 and .NET 4.0 versions of Fluent Assertions.

## Extensibility ##

**Custom assertions**  
Adding your own assertion extensions is quite straightforward and happens in my projects quite often. You have a few options though.

* Extend one of the built-in classes such as `CollectionAssertions<T>` or `ReferenceTypeAssertions<T>` and expose them through a custom static class with extension methods named `Should()`. 

* Create extension methods that extend an assertion class: 

```csharp
	public static void BeWhatever<T>(this GenericCollectionAssertions<T> assertions, string because, params object[] becauseArgs)
	{ 
    	Execute.Assertion
    	   .ForCondition(somecondition)
    	   .BecauseOf(reason, reasonArgs)
    	   .FailWith("Expected object not to be {0}{reason}", null);
	}
```

* Create a custom assertions class and use the `Assertion` class to verify conditions and create comprehensive failure messages using the built-in formatters. Notice that error messages of custom assertion extensions can specify the `{context}` tag that is used to inject the property path in object graph comparisons. For instance, in the date/time assertions, this is used to display date and time. But when this assertion is used as part of a recursive object graph comparison, it will display the property path instead. You can use this special tag in your own extensions like this:

```csharp
Execute.Assertion
    .ForCondition(Subject.Value == expected)
    .BecauseOf(reason, reasonArgs)
    .FailWith("Expected {context:date and time} to be {0}{reason}, but found {1}.", expected, subject.Value);
```


**Formatters**  
In addition to writing your own extensions, you can influence the way data is formatted with these two techniques.

* You can alter the list of `IValueFormatter` objects on the `Formatter` class with your own implementation of that interface using its methods `AddFormatter` and `RemoveFormatter`. 

* You can override the way Fluent Assertions formats objects in an error message by annotating a static method with the `[ValueFormatter]` attribute. If a class doesn’t override `ToString()`, the built-in `DefaultValueFormatter` will render an object graph of that object. But you can now override that using a construct like this:

```csharp
public static class CustomFormatter
{    
    [ValueFormatter]
    public static string Foo(SomeClassvalue value)    
    {
        return "Property = " + value.Property;   
    }
}
```

Since scanning for value formatters incurs a significant performance hit, you need to explicitly enable that using the `<appSetting>`  with key `valueFormatters`. Valid values include `Disabled` (the default), `Scan` and `Specific`, where `Scan` will scan all assemblies in the `AppDomain`. Option `Specific` also requires you to set the `valueFormattersAssembly` setting key with the (partial) name of an assembly FA should scan. Since Silverlight and Windows Phone apps do not support an `app.config` file, you'll need to set those settings through the `ValueFormatterDetectionMode` and `ValueFormatterAssembly` properties of the static `Configuration.Current` object.

## Assertion Scope
See [Assertion Scope](/AssertionScope.html)