---
title: Basic Assertions
permalink: /basicassertions/
layout: single
class: wide
sidebar:
  nav: "sidebar"
---

The following assertions are available to all types of objects.

```csharp
object theObject = null;
theObject.Should().BeNull("because the value is null");
theObject.Should().NotBeNull();

theObject = "whatever";
theObject.Should().BeOfType<string>("because a {0} is set", typeof(string));
theObject.Should().BeOfType(typeof(string), "because a {0} is set", typeof(string));
```

Sometimes you might like to first assert that an object is of a certain type using `BeOfType` and then continue with additional assertions on the result of casting that object to the specified type.
You can do that by chaining those assertions onto the `Which` property like this.

```csharp
someObject.Should().BeOfType<Exception>()
  .Which.Message.Should().Be("Other Message");
```

To assert that two objects are equal (through their implementation of `Object.Equals`), use

```csharp
string otherObject = "whatever";
theObject.Should().Be(otherObject, "because they have the same values");
theObject.Should().NotBe(otherObject);
```

To assert that an object is equal to one of the provided objects, you can use

```csharp
theObject.Should().BeOneOf(obj1, obj2, obj3);
```

To assert object equality using a custom equality comparer, you can use

```csharp
theObject.Should().Be(otherObject, new ObjEqualityComparer());
theObject.Should().NotBe(otherObject, new ObjEqualityComparer());
theObject.Should().BeOneOf(new[] { obj1, obj2, obj3 }, new ObjEqualityComparer());
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
ex.Should().NotBeAssignableTo<DateTime>("because it is an exception");

var dummy = new Object();
dummy.Should().Match(d => (d.ToString() == "System.Object"));
dummy.Should().Match<string>(d => (d == "System.Object"));
dummy.Should().Match((string d) => (d == "System.Object"));
```

As an alternative to using predicate matching, it is also possible to use element inspectors to do nested assertions in a fluent way.

```csharp
var productDto = new ProductDto
    {
        Name = "Some product name",
        Price = 19.95,
        SKU = "ABC12345",
        Store = new Store
            {
                Country = "Germany",
                Quantity = 42
            }
    };
productDto.Should().Satisfy<ProductDto>(dto =>
    {
        dto.Name.Should().Be("Some product name");
        dto.Price.Should().Be(19.95);
        dto.SKU.Should().EndWith("12345");
        dto.Store.Should().Satisfy<Store>(store =>
            {
                store.Country.Should().Be("Germany");
                store.Quantity.Should().BeGreaterThan(40);
            }
        );
    }
);
```

Some users requested the ability to easily downcast an object to one of its derived classes in a fluent way.

```csharp
customer.Animals.First().As<Human>().Height.Should().Be(178);
```

We’ve also added the possibility to assert that an object can be serialized and deserialized using the XML or data contract formatters.

```csharp
theObject.Should().BeXmlSerializable();
theObject.Should().BeDataContractSerializable();
```
