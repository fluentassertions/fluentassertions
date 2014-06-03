The following new features and updates will be included in Version 3.1:

## Assembly References ##

New in version 3.1 are methods to assert an assembly does / does not reference another assembly.  These are typically used to enforce layers within an application.  For example, asserting the web layer does not reference the data layer.

There is a static helper class to get assemblies to assert:

```` csharp
var assembly = FindAssembly.Containing<SomeClass>();
````

To assert the references, use the the following syntax:

```` csharp
assembly.Should().Reference(otherAssembly);
assembly.Should().NotReference(otherAssembly);
````

These assertions are only avaliable in the .NET 4 and 4.5 versions of Fluent Assertions as the reflection methods used are not available in Silverlight and Windows Phone runtimes.
