using System;
using ExampleExtensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs;

public class ExtensibilitySpecs
{
    [Fact]
    public void Methods_marked_as_custom_assertion_are_ignored_during_caller_identification()
    {
        // Arrange
        var myClient = new MyCustomer
        {
            Active = false
        };

        // Act
        Action act = () => myClient.Should().BeActive("because we don't work with old clients");

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected myClient to be true because we don't work with old clients, but found False.");
    }

    [Fact]
    public void Methods_in_assemblies_marked_as_custom_assertion_are_ignored_during_caller_identification()
    {
        // Arrange
        string palindrome = "fluent";

        // Act
        Action act = () => palindrome.Should().BePalindromic();

        // Assert
        act.Should().Throw<XunitException>().WithMessage(
            "Expected palindrome to be*tneulf*");
    }
}

internal class MyCustomer
{
    public bool Active { get; set; }
}

internal static class MyCustomerExtensions
{
    public static MyCustomerAssertions Should(this MyCustomer customer)
    {
        return new MyCustomerAssertions(customer);
    }
}

internal class MyCustomerAssertions
{
    private readonly MyCustomer customer;

    public MyCustomerAssertions(MyCustomer customer)
    {
        this.customer = customer;
    }

    [CustomAssertion]
    public void BeActive(string because = "", params object[] becauseArgs)
    {
        customer.Active.Should().BeTrue(because, becauseArgs);
    }
}
