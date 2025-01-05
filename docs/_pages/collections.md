---
title: Collections
permalink: /collections/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

A collection object in .NET is so versatile that the number of assertions on them require the same level of versatility.
Most, if not all, are so self-explanatory that we'll just list them here.

```csharp
IEnumerable<int> collection = new[] { 1, 2, 5, 8 };

collection.Should().NotBeEmpty()
    .And.HaveCount(4)
    .And.ContainInOrder(new[] { 2, 5 })
    .And.ContainItemsAssignableTo<int>();

collection.Should().Equal(new List<int> { 1, 2, 5, 8 });
collection.Should().Equal(1, 2, 5, 8);
collection.Should().NotEqual(8, 2, 3, 5);
collection.Should().BeEquivalentTo(new[] {8, 2, 1, 5});
collection.Should().NotBeEquivalentTo(new[] {8, 2, 3, 5});

collection.Should().HaveCount(c => c > 3)
  .And.OnlyHaveUniqueItems();

collection.Should().HaveCountGreaterThan(3);
collection.Should().HaveCountGreaterThanOrEqualTo(4);
collection.Should().HaveCountLessThanOrEqualTo(4);
collection.Should().HaveCountLessThan(5);
collection.Should().NotHaveCount(3);
collection.Should().HaveSameCount(new[] { 6, 2, 0, 5 });
collection.Should().NotHaveSameCount(new[] { 6, 2, 0 });

collection.Should().StartWith(1);
collection.Should().StartWith(new[] { 1, 2 });
collection.Should().EndWith(8);
collection.Should().EndWith(new[] { 5, 8 });

collection.Should().BeSubsetOf(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, });

collection.Should().ContainSingle();
collection.Should().ContainSingle(x => x > 3);
collection.Should().Contain(8)
  .And.HaveElementAt(2, 5)
  .And.NotBeSubsetOf(new[] {11, 56});

collection.Should().Contain(x => x > 3);
collection.Should().Contain(collection, "", 5, 6); // It should contain the original items, plus 5 and 6.

collection.Should().OnlyContain(x => x < 10);
collection.Should().ContainItemsAssignableTo<int>();
collection.Should().NotContainItemsAssignableTo<string>();

collection.Should().ContainInOrder(new[] { 1, 5, 8 });
collection.Should().NotContainInOrder(new[] { 5, 1, 2 });

collection.Should().ContainInConsecutiveOrder(new[] { 2, 5, 8 });
collection.Should().NotContainInConsecutiveOrder(new[] { 1, 5, 8});

collection.Should().NotContain(82);
collection.Should().NotContain(new[] { 82, 83 });
collection.Should().NotContainNulls();
collection.Should().NotContain(x => x > 10);

object boxedValue = 2;
collection.Should().ContainEquivalentOf(boxedValue); // Compared by object equivalence
object unexpectedBoxedValue = 82;
collection.Should().NotContainEquivalentOf(unexpectedBoxedValue); // Compared by object equivalence

const int successor = 5;
const int predecessor = 5;
collection.Should().HaveElementPreceding(successor, element);
collection.Should().HaveElementSucceeding(predecessor, element);

collection.Should().BeEmpty();
collection.Should().BeNullOrEmpty();
collection.Should().NotBeNullOrEmpty();

IEnumerable<int> otherCollection = new[] { 1, 2, 5, 8, 1 };
IEnumerable<int> anotherCollection = new[] { 10, 20, 50, 80, 10 };
collection.Should().IntersectWith(otherCollection);
collection.Should().NotIntersectWith(anotherCollection);

var singleEquivalent = new[] { new { Size = 42 } };
singleEquivalent.Should().ContainSingle()
    .Which.Should().BeEquivalentTo(new { Size = 42 });
```

Asserting that a collection contains items in a certain order is as easy as using one of the several overloads of `BeInAscendingOrder` or `BeInDescendingOrder`. The default overload will use the default `Comparer` for the specified type, but overloads also exist that take an `IComparer<T>`, a property expression to sort by an object's property, or a lambda expression to avoid the need for `IComparer<T>` implementations.

```csharp
collection.Should().BeInAscendingOrder();
collection.Should().BeInDescendingOrder();
collection.Should().NotBeInAscendingOrder();
collection.Should().NotBeInDescendingOrder();
```

Since **6.9.0** there is a slight change in how culture is taken into comparison. Now by default `StringComparer.Ordinal` is used to compare strings.
If you want to overrule this behavior use the `IComparer<T>` overload. The use case is e.g. if you get data from a database ordered by culture aware sort order.

```csharp
collection.Should().BeInAscendingOrder(item => item.SomeProp, StringComparer.CurrentCulture);
collection.Should().BeInDescendingOrder(item => item.SomeProp, StringComparer.CurrentCulture);
collection.Should().NotBeInAscendingOrder(item => item.SomeProp, StringComparer.CurrentCulture);
collection.Should().NotBeInDescendingOrder(item => item.SomeProp, StringComparer.CurrentCulture);
```
 
For `String` collections there are specific methods to assert the items. For the `ContainMatch` and `NotContainMatch` methods we support wildcards.

The pattern can be a combination of literal and wildcard characters, but it doesn't support regular expressions.

The following wildcard specifiers are permitted in the pattern:

| Wildcard specifier | Matches                                   |
| ----------------- | ----------------------------------------- |
| * (asterisk)      | Zero or more characters in that position. |
| ? (question mark) | Exactly one character in that position.   |

```csharp
IEnumerable<string> stringCollection = new[] { "build succeeded", "test failed" };
stringCollection.Should().AllBe("build succeeded");
stringCollection.Should().ContainMatch("* failed");
```

In order to assert presence of an equivalent item in a collection applying [Object graph comparison](objectgraphs.md) rules, use this:

```csharp
collection.Should().ContainEquivalentOf(boxedValue);
collection.Should().NotContainEquivalentOf(unexpectedBoxedValue)
```

Those last two methods can be used to assert a collection contains items in ascending or descending order.
For simple types that might be fine, but for more complex types, it requires you to implement `IComparable`, something that doesn't make a whole lot of sense in all cases.
That's why we offer overloads that take an expression.

```csharp
collection.Should().BeInAscendingOrder(x => x.SomeProperty);
collection.Should().BeInDescendingOrder(x => x.SomeProperty);
collection.Should().NotBeInAscendingOrder(x => x.SomeProperty);
collection.Should().NotBeInDescendingOrder(x => x.SomeProperty);
```

When asserting on a projection of a collection the failure message will be less descriptive as it only knows about the projected value and not object containing that property.

```csharp
collection.Select(x => x.SomeProperty).Should().OnlyHaveUniqueItems();
```

Therefore we offer two overloads that takes an expression to select the property.

```csharp
collection.Should().OnlyHaveUniqueItems(x => x.SomeProperty);
collection.Should().NotContainNulls(x => x.SomeProperty);
```

Special overloads of `Equal()`, `StartWith` and `EndWith` take a lambda that is used for checking the two collections without relying on the type’s Equals() method.
Consider for instance two collections that contain some kind of domain entity persisted to a database and then reloaded.
Since the actual object instance is different, if you want to make sure a particular property was properly persisted, you usually do something like this:

```csharp
persistedCustomers.Select(c => c.Name).Should().Equal(customers.Select(c => c.Name));
persistedCustomers.Select(c => c.Name).Should().StartWith(customers.Select(c => c.Name));
persistedCustomers.Select(c => c.Name).Should().EndWith(customers.Select(c => c.Name));
```

With these new overloads, you can rewrite them into:

```csharp
persistedCustomers.Should().Equal(customers, (c1, c2) => c1.Name == c2.Name);
persistedCustomers.Should().StartWith(customers, (c1, c2) => c1.Name == c2.Name);
persistedCustomers.Should().EndWith(customers, (c1, c2) => c1.Name == c2.Name);
```

You can also perform assertions on all elements of a collection:

```csharp
IEnumerable<BaseType> collection = new BaseType[] { new DerivedType() };

collection.Should().AllBeAssignableTo<DerivedType>();
collection.Should().AllBeOfType<DerivedType>();
collection.Should().AllBeEquivalentTo(referenceObject);
```

In case if you need to perform individual assertions on all elements of a collection, you can assert each element separately in the following manner:

```csharp
var collection = new []
{
    new { Id = 1, Name = "John", Attributes = new string[] { } },
    new { Id = 2, Name = "Jane", Attributes = new string[] { "attr" } }
};
collection.Should().SatisfyRespectively(
    first =>
    {
        first.Id.Should().Be(1);
        first.Name.Should().StartWith("J");
        first.Attributes.Should().NotBeNull();
    },
    second =>
    {
        second.Id.Should().Be(2);
        second.Name.Should().EndWith("e");
        second.Attributes.Should().NotBeEmpty();
    });
```

If you need to perform the same assertion on all elements of a collection:

```csharp
var collection = new []
{
    new { Id = 1, Name = "John", Attributes = new string[] { } },
    new { Id = 2, Name = "Jane", Attributes = new string[] { "attr" } }
};
collection.Should().AllSatisfy(x =>
{
    x.Id.Should().BePositive();
    x.Name.Should().StartWith("J");
    x.Attributes.Should().NotBeNull();
});
```

If you need to perform individual assertions on all elements of a collection without setting expectation about the order of elements:

```csharp
var collection = new []
{
    new { Id = 1, Name = "John", Attributes = new string[] { } },
    new { Id = 2, Name = "Jane", Attributes = new string[] { "attr" } }
};

collection.Should().Satisfy(
    e => e.Id == 2 && e.Name == "Jane" && e.Attributes == null,
    e => e.Id == 1 && e.Name == "John" && e.Attributes != null && e.Attributes.Length > 0);
```
