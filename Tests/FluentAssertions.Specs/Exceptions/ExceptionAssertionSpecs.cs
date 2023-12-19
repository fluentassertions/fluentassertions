using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Exceptions;

public class ExceptionAssertionSpecs
{
    [Fact]
    public void When_method_throws_an_empty_AggregateException_it_should_fail()
    {
        // Arrange
        Action act = () => throw new AggregateException();

        // Act
        Action act2 = () => act.Should().NotThrow();

        // Assert
        act2.Should().Throw<XunitException>();
    }

#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    [Theory]
    [MemberData(nameof(AggregateExceptionTestData))]
    public void When_the_expected_exception_is_wrapped_it_should_succeed<T>(Action action, T _)
        where T : Exception
    {
        // Act/Assert
        action.Should().Throw<T>();
    }

    [Theory]
    [MemberData(nameof(AggregateExceptionTestData))]
    public void When_the_expected_exception_is_not_wrapped_it_should_fail<T>(Action action, T _)
        where T : Exception
    {
        // Act
        Action act2 = () => action.Should().NotThrow<T>();

        // Assert
        act2.Should().Throw<XunitException>();
    }
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters

    public static TheoryData<Action, Exception> AggregateExceptionTestData()
    {
        var tasks = new[]
        {
            AggregateExceptionWithLeftNestedException,
            AggregateExceptionWithRightNestedException
        };

        var types = new Exception[]
        {
            new AggregateException(),
            new ArgumentNullException(),
            new InvalidOperationException()
        };

        var data = new TheoryData<Action, Exception>();

        foreach (var task in tasks)
        {
            foreach (var type in types)
            {
                data.Add(task, type);
            }
        }

        data.Add(EmptyAggregateException, new AggregateException());

        return data;
    }

    private static void AggregateExceptionWithLeftNestedException()
    {
        var ex1 = new AggregateException(new InvalidOperationException());
        var ex2 = new ArgumentNullException();
        var wrapped = new AggregateException(ex1, ex2);

        throw wrapped;
    }

    private static void AggregateExceptionWithRightNestedException()
    {
        var ex1 = new ArgumentNullException();
        var ex2 = new AggregateException(new InvalidOperationException());
        var wrapped = new AggregateException(ex1, ex2);

        throw wrapped;
    }

    private static void EmptyAggregateException()
    {
        throw new AggregateException();
    }

    [Fact]
    public void ThrowExactly_when_subject_throws_subclass_of_expected_exception_it_should_throw()
    {
        // Arrange
        Action act = () => throw new ArgumentNullException();

        try
        {
            // Act
            act.Should().ThrowExactly<ArgumentException>("because {0} should do that", "Does.Do");

            throw new XunitException("This point should not be reached.");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should()
                .Match(
                    "Expected type to be System.ArgumentException because Does.Do should do that, but found System.ArgumentNullException.");
        }
    }

    [Fact]
    public void ThrowExactly_when_subject_throws_aggregate_exception_instead_of_expected_exception_it_should_throw()
    {
        // Arrange
        Action act = () => throw new AggregateException(new ArgumentException());

        try
        {
            // Act
            act.Should().ThrowExactly<ArgumentException>("because {0} should do that", "Does.Do");

            throw new XunitException("This point should not be reached.");
        }
        catch (XunitException ex)
        {
            // Assert
            ex.Message.Should()
                .Match(
                    "Expected type to be System.ArgumentException because Does.Do should do that, but found System.AggregateException.");
        }
    }

    [Fact]
    public void ThrowExactly_when_subject_throws_expected_exception_it_should_not_do_anything()
    {
        // Arrange
        Action act = () => throw new ArgumentNullException();

        // Act / Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}

public class SomeTestClass
{
    internal const string ExceptionMessage = "someMessage";

    public IList<string> Strings = new List<string>();

    public void Throw()
    {
        throw new ArgumentException(ExceptionMessage);
    }
}

public abstract class Does
{
    public abstract void Do();

    public abstract void Do(string someParam);

    public abstract int Return();

    public static Does Throw<TException>(TException exception)
        where TException : Exception
    {
        return new DoesThrow<TException>(exception);
    }

    public static Does Throw<TException>()
        where TException : Exception, new()
    {
        return Throw(new TException());
    }

    public static Does NotThrow() => new DoesNotThrow();

    private class DoesThrow<TException> : Does
        where TException : Exception
    {
        private readonly TException exception;

        public DoesThrow(TException exception)
        {
            this.exception = exception;
        }

        public override void Do() => throw exception;

        public override void Do(string someParam) => throw exception;

        public override int Return() => throw exception;
    }

    private class DoesNotThrow : Does
    {
        public override void Do() { }

        public override void Do(string someParam) { }

        public override int Return() => 42;
    }
}

internal class ExceptionWithProperties : Exception
{
    public ExceptionWithProperties(string propertyValue)
    {
        Property = propertyValue;
    }

    public string Property { get; set; }
}

internal class ExceptionWithEmptyToString : Exception
{
    public override string ToString()
    {
        return string.Empty;
    }
}
