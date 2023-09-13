---
title: Streams
permalink: /json/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## JSON ##
A lot (if not the vast majority) of applications use JSON (de)serialization.
By doing so, how the serialization is performed is part of the public contract/API.
To prevent unexpected changes, writing tests that ensure the behavior is important.

### JSON serialization options
To ensure that the assertions are done on the serialization options that are registered, it might be worth explicitly resolve those options:

```csharp
public class Serialization : Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory<Program>
{
    private JsonSerializerOptions SerializerOptions => Services.GetRequiredService<JsonSerializerOptions>();
}
```

Both a `Serialize()` and a `Deserialize()` can be performed:

```csharp
JsonSerializerOptions options = GetOptions();

options.Should().Serialize(someObject);
options.Should().Deserialize(someJson);
```

And because we specially interested in the results of these actions, we want to assert the outcomes to:

```csharp
public class Serializes
{
    [Fact]
    public void DateOnly_as_string_value()
    {
        JsonSerializerOptions options = GetOptions();
        
        options.Should().Serialize(new DateOnly(2017, 06, 11))
            .Which.Should().BeString("2017-06-11");
    }
    
    [Fact]
    public void Enum_as_number()
    {
    
        JsonSerializerOptions options = GetOptions();
        
        options.Should().Serialize(SomeEnum.SomeValue)
            .Which.Should().BeNumber(42);
    }
    
    [Fact]
    pulic void Default_of_custom_struct_as_null()
    {
        JsonSerializerOptions options = GetOptions();
        
        options.Should().Serialize(default(CustomStruct))
            .Which.Should().BeNull();
    }
}
```

And

```csharp
public class Deserializes
{
    [Fact]
    public void DateOnly_from_JSON_string()
    {
        JsonSerializerOptions options = GetOptions();
        
        options.Should().Deserialize<DateOnly>("2017-06-11")
            .Which.Should().Be(new DateOnly(2017-06-11));
    }
    
    [Fact]
    public void Enum_from_number()
    {
        JsonSerializerOptions options = GetOptions();
        
        options.Should().Deserialize<SomeEnum>("42")
            .Which.Should().BeNumber(SomeEnum.SomeValue);
    }
    
    [Fact]
    public void Enum_from_string()
    {
    
        JsonSerializerOptions options = GetOptions();
        
        options.Should().Deserialize<SomeEnum>("\"SomeValue\"")
            .Which.Should().BeNumber(SomeEnum.SomeValue);
    }
}
```