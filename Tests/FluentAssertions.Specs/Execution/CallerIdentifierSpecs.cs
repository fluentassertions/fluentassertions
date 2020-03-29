
using Xunit;
using Xunit.Sdk;
using FluentAssertions;
using System;

namespace FluentAssertions.Specs
{
    public class CallerIdentifierSpecs
    {
        [Fact]
        public void When_namespace_is_exactly_System_caller_should_be_unknown()
        {
            // Act
            Action act = () => System.SystemNamespaceClass.DetermineCallerIdentityInNamespace();

            //Assert
            act.Should().Throw<XunitException>().WithMessage("Expected function to be*");
        }

        [Fact]
        public void When_namespace_is_nested_under_System_caller_should_be_unknown()
        {
            // Act
            Action act = () => System.Data.NestedSystemNamespaceClass.DetermineCallerIdentityInNamespace();

            //Assert
            act.Should().Throw<XunitException>().WithMessage("Expected function to be*");
        }

        [Fact]
        public void When_namespace_is_prefixed_with_System_caller_should_be_known()
        {
            // Act
            Action act = () => SystemPrefixed.SystemPrefixedNamespaceClass.DetermineCallerIdentityInNamespace();

            //Assert
            act.Should().Throw<XunitException>().WithMessage("Expected actualCaller to be*");
        }
    }
}

namespace System
{
    public class SystemNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}

namespace SystemPrefixed
{
    public class SystemPrefixedNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}

namespace System.Data
{
    public class NestedSystemNamespaceClass
    {
        public static void DetermineCallerIdentityInNamespace()
        {
            Func<string> actualCaller = () => CallerIdentifier.DetermineCallerIdentity();
            actualCaller.Should().BeNull("we want this check to fail for the test");
        }
    }
}

