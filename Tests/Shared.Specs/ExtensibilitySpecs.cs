using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    public class ExtensibilitySpecs
    {
        [Fact]
        public void When_a_method_is_marked_as_custom_assertion_it_should_be_ignored_during_caller_identification()
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
#if NETCOREAPP1_1
                "Expected boolean to be true because we don't work with old clients, but found False.");
#else
                "Expected myClient to be true because we don't work with old clients, but found False.");
#endif
        }
    }

    public class MyCustomer
    {
        public bool Active { get; set; }
    }

    public static class MyCustomerExtensions
    {
        public static MyCustomerAssertions Should(this MyCustomer customer)
        {
            return new MyCustomerAssertions(customer);
        }
    }

    public class MyCustomerAssertions
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
}
