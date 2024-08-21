using System;
using FluentAssertions;
using Machine.Specifications;

namespace MSpec.Specs;

[Subject("FrameworkSpecs")]
public class When_mspec_is_used
{
    Because of = () => Exception = Catch.Exception(() => 0.Should().Be(1));

    It should_fail = () => Exception.Should().BeAssignableTo<Exception>();

    // Don't reference the exception type explicitly like this: Exception.Should().BeAssignableTo<SpecificationException>()
    // It could cause this specs project to load the assembly containing the exception (this actually happens for xUnit)
    It should_fail_with_a_specification_exception = () => Exception.GetType().FullName.Should().Be("Machine.Specifications.SpecificationException");

    private static Exception Exception;
}
