using System;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Net45.Specs
{
    public class ExtensibilitySpecs
    {
        [Fact]
        public void When_a_method_is_marked_as_custom_assertion_it_should_be_ignored_during_caller_identification()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var myClient = new Customer
            {
               Active = false
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => myClient.Should().BeActive("because we don't work with old clients");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<XunitException>().WithMessage(
#if NETCOREAPP1_1
                "Expected boolean to be true because we don't work with old clients, but found False.");
#else
                "Expected myClient to be true because we don't work with old clients, but found False.");
#endif
        }
    }

    public class Customer
    {
        public bool Active { get; set; }
    }

    public static class CustomerExtensions
    {
        public static CustomerAssertions Should(this Customer customer)
        {
            return new CustomerAssertions(customer);
        }
    }

    public class CustomerAssertions
    {
        private readonly Customer customer;

        public CustomerAssertions(Customer customer)
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
