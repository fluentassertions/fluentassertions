using System;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Extensions;
#if NET47
using FluentAssertionsAsync.Specs.Common;
#endif
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Exceptions;

public class AsyncFunctionExceptionAssertionSpecs
{
    [Fact]
    public void When_getting_the_subject_it_should_remain_unchanged()
    {
        // Arrange
        Func<Task> subject = () => Task.FromResult(42);

        // Act
        Action action = () => subject.Should().Subject.As<object>().Should().BeSameAs(subject);

        // Assert
        action.Should().NotThrow("the Subject should remain the same");
    }

    [Fact]
    public async Task When_subject_is_null_when_expecting_an_exception_it_should_throw()
    {
        // Arrange
        Func<Task> action = null;

        // Act
        Func<Task> testAction = async () =>
        {
            using var _ = new AssertionScope();
            await action.Should().ThrowAsync<ArgumentException>("because we want to test the failure {0}", "message");
        };

        // Assert
        await testAction.Should().ThrowAsync<XunitException>()
            .WithMessage("*because we want to test the failure message*found <null>*")
            .Where(e => !e.Message.Contains("NullReferenceException"));
    }

    [Fact]
    public async Task When_subject_is_null_when_not_expecting_a_generic_exception_it_should_throw()
    {
        // Arrange
        Func<Task> action = null;

        // Act
        Func<Task> testAction = async () =>
        {
            using var _ = new AssertionScope();
            await action.Should().NotThrowAsync<ArgumentException>("because we want to test the failure {0}", "message");
        };

        // Assert
        await testAction.Should().ThrowAsync<XunitException>()
            .WithMessage("*because we want to test the failure message*found <null>*")
            .Where(e => !e.Message.Contains("NullReferenceException"));
    }

    [Fact]
    public async Task When_subject_is_null_when_not_expecting_an_exception_it_should_throw()
    {
        // Arrange
        Func<Task> action = null;

        // Act
        Func<Task> testAction = async () =>
        {
            using var _ = new AssertionScope();
            await action.Should().NotThrowAsync("because we want to test the failure {0}", "message");
        };

        // Assert
        await testAction.Should().ThrowAsync<XunitException>()
            .WithMessage("*because we want to test the failure message*found <null>*")
            .Where(e => !e.Message.Contains("NullReferenceException"));
    }

    [Fact]
    public async Task When_async_method_throws_an_empty_AggregateException_it_should_fail()
    {
        // Arrange
        Func<Task> act = () => throw new AggregateException();

        // Act
        Func<Task> act2 = () => act.Should().NotThrowAsync();

        // Assert
        await act2.Should().ThrowAsync<XunitException>();
    }

    [Collection("UIFacts")]
    public partial class UIFacts
    {
        [UIFact]
        public async Task When_async_method_throws_an_empty_AggregateException_on_UI_thread_it_should_fail()
        {
            // Arrange
            Func<Task> act = () => throw new AggregateException();

            // Act
            Func<Task> act2 = () => act.Should().NotThrowAsync();

            // Assert
            await act2.Should().ThrowAsync<XunitException>();
        }
    }

    [Fact]
    public async Task When_async_method_throws_a_nested_AggregateException_it_should_provide_the_message()
    {
        // Arrange
        Func<Task> act = () => throw new AggregateException(new ArgumentException("That was wrong."));

        // Act & Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("That was wrong.");
    }

    public partial class UIFacts
    {
        [UIFact]
        public async Task When_async_method_throws_a_nested_AggregateException_on_UI_thread_it_should_provide_the_message()
        {
            // Arrange
            Func<Task> act = () => throw new AggregateException(new ArgumentException("That was wrong."));

            // Act & Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("That was wrong.");
        }
    }

    [Fact]
    public async Task When_async_method_throws_a_flat_AggregateException_it_should_provide_the_message()
    {
        // Arrange
        Func<Task> act = () => throw new AggregateException("That was wrong as well.");

        // Act & Assert
        await act.Should().ThrowAsync<AggregateException>().WithMessage("That was wrong as well.");
    }

    [Fact]
    public async Task When_async_method_throws_a_nested_AggregateException_it_should_provide_unwrapped_exception_to_predicate()
    {
        // Arrange
        Func<Task> act = () => throw new AggregateException(new ArgumentException("That was wrong."));

        // Act & Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .Where(i => i.Message == "That was wrong.");
    }

    [Fact]
    public async Task When_async_method_throws_a_flat_AggregateException_it_should_provide_it_to_predicate()
    {
        // Arrange
        Func<Task> act = () => throw new AggregateException("That was wrong as well.");

        // Act & Assert
        await act.Should().ThrowAsync<AggregateException>()
            .Where(i => i.Message == "That was wrong as well.");
    }

#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    [Theory]
    [MemberData(nameof(AggregateExceptionTestData))]
    public async Task When_the_expected_exception_is_wrapped_async_it_should_succeed<T>(Func<Task> action, T _)
        where T : Exception
    {
        // Act/Assert
        await action.Should().ThrowAsync<T>();
    }

    [UITheory]
    [MemberData(nameof(AggregateExceptionTestData))]
    public async Task When_the_expected_exception_is_wrapped_on_UI_thread_async_it_should_succeed<T>(Func<Task> action, T _)
        where T : Exception
    {
        // Act/Assert
        await action.Should().ThrowAsync<T>();
    }

    [Theory]
    [MemberData(nameof(AggregateExceptionTestData))]
    public async Task When_the_expected_exception_is_not_wrapped_async_it_should_fail<T>(Func<Task> action, T _)
        where T : Exception
    {
        // Act
        Func<Task> act2 = () => action.Should().NotThrowAsync<T>();

        // Assert
        await act2.Should().ThrowAsync<XunitException>();
    }

    [UITheory]
    [MemberData(nameof(AggregateExceptionTestData))]
    public async Task When_the_expected_exception_is_not_wrapped_on_UI_thread_async_it_should_fail<T>(Func<Task> action, T _)
        where T : Exception
    {
        // Act
        Func<Task> act2 = () => action.Should().NotThrowAsync<T>();

        // Assert
        await act2.Should().ThrowAsync<XunitException>();
    }
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters

    public static TheoryData<Func<Task>, Exception> AggregateExceptionTestData()
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

        var data = new TheoryData<Func<Task>, Exception>();

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

    private static Task AggregateExceptionWithLeftNestedException()
    {
        var ex1 = new AggregateException(new InvalidOperationException());
        var ex2 = new ArgumentNullException();
        var wrapped = new AggregateException(ex1, ex2);

        return FromException(wrapped);
    }

    private static Task AggregateExceptionWithRightNestedException()
    {
        var ex1 = new ArgumentNullException();
        var ex2 = new AggregateException(new InvalidOperationException());
        var wrapped = new AggregateException(ex1, ex2);

        return FromException(wrapped);
    }

    private static Task EmptyAggregateException()
    {
        return FromException(new AggregateException());
    }

    private static Task FromException(AggregateException exception)
    {
        return Task.FromException(exception);
    }

    [Fact]
    public async Task When_subject_throws_subclass_of_expected_exact_exception_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsync<ArgumentNullException>())
            .Should().ThrowExactlyAsync<ArgumentException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.ArgumentNullException.");
    }

    [Fact]
    public async Task When_subject_ValueTask_throws_subclass_of_expected_exact_exception_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsyncValueTask<ArgumentNullException>())
            .Should().ThrowExactlyAsync<ArgumentException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.ArgumentNullException.");
    }

    [Fact]
    public async Task When_subject_throws_aggregate_exception_and_not_expected_exact_exception_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAggregateExceptionAsync<ArgumentException>())
            .Should().ThrowExactlyAsync<ArgumentException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.AggregateException.");
    }

    [Fact]
    public async Task When_subject_throws_aggregate_exception_and_not_expected_exact_exception_through_ValueTask_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAggregateExceptionAsyncValueTask<ArgumentException>())
            .Should().ThrowExactlyAsync<ArgumentException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.AggregateException.");
    }

    [Fact]
    public async Task When_subject_throws_the_expected_exact_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act / Assert
        await asyncObject
            .Awaiting(x => x.ThrowAsync<ArgumentNullException>())
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task When_subject_throws_the_expected_exact_exception_through_ValueTask_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act / Assert
        await asyncObject
            .Awaiting(x => x.ThrowAsyncValueTask<ArgumentNullException>())
            .Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task When_async_method_throws_expected_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsync<ArgumentException>())
            .Should().ThrowAsync<ArgumentException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_expected_exception_through_ValueTask_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsyncValueTask<ArgumentException>())
            .Should().ThrowAsync<ArgumentException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_subject_is_null_it_should_be_null()
    {
        // Arrange
        Func<Task> subject = null;

        // Act
        Func<Task> action = () => subject.Should().NotThrowAsync();

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("*found <null>*");
    }

    [Fact]
    public async Task When_async_method_throws_async_expected_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject.ThrowAsync<ArgumentException>();

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task When_async_method_does_not_throw_expected_exception_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.SucceedAsync())
            .Should().ThrowAsync<InvalidOperationException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown because IFoo.Do should do that, but no exception was thrown.");
    }

    [Fact]
    public async Task When_async_method_does_not_throw_expected_exception_through_ValueTask_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.SucceedAsyncValueTask())
            .Should().ThrowAsync<InvalidOperationException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown because IFoo.Do should do that, but no exception was thrown.");
    }

    [Fact]
    public async Task When_async_method_throws_unexpected_exception_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsync<ArgumentException>())
            .Should().ThrowAsync<InvalidOperationException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown because IFoo.Do should do that, but found <System.ArgumentException>*");
    }

    [Fact]
    public async Task When_async_method_throws_unexpected_exception_through_ValueTask_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsyncValueTask<ArgumentException>())
            .Should().ThrowAsync<InvalidOperationException>("because {0} should do that", "IFoo.Do");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage(
                "Expected a <System.InvalidOperationException> to be thrown because IFoo.Do should do that, but found <System.ArgumentException>*");
    }

    [Fact]
    public async Task When_async_method_does_not_throw_exception_and_that_was_expected_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.SucceedAsync())
            .Should().NotThrowAsync();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_does_not_throw_exception_through_ValueTask_and_that_was_expected_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.SucceedAsyncValueTask())
            .Should().NotThrowAsync();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_does_not_throw_async_exception_and_that_was_expected_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject.SucceedAsync();

        // Assert
        await action.Should().NotThrowAsync();
    }

    public partial class UIFacts
    {
        [UIFact]
        public async Task When_async_method_does_not_throw_async_exception_on_UI_thread_and_that_was_expected_it_should_succeed()
        {
            // Arrange
            var asyncObject = new AsyncClass();

            // Act
            Func<Task> action = () => asyncObject.SucceedAsync();

            // Assert
            await action.Should().NotThrowAsync();
        }
    }

    [Fact]
    public async Task When_subject_throws_subclass_of_expected_async_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject.ThrowAsync<ArgumentNullException>();

        // Assert
        await action.Should().ThrowAsync<ArgumentException>("because {0} should do that", "IFoo.Do");
    }

    [Fact]
    public async Task When_function_of_task_int_in_async_method_throws_the_expected_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();
        Func<Task<int>> f = () => asyncObject.ThrowTaskIntAsync<ArgumentNullException>(true);

        // Act / Assert
        await f.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task When_function_of_task_int_in_async_method_throws_not_excepted_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();
        Func<Task<int>> f = () => asyncObject.ThrowTaskIntAsync<InvalidOperationException>(true);

        // Act / Assert
        await f.Should().NotThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task When_subject_is_null_when_expecting_an_exact_exception_it_should_throw()
    {
        // Arrange
        Func<Task> action = null;

        // Act
        Func<Task> testAction = async () =>
        {
            using var _ = new AssertionScope();
            await action.Should().ThrowExactlyAsync<ArgumentException>("because we want to test the failure {0}", "message");
        };

        // Assert
        await testAction.Should().ThrowAsync<XunitException>()
            .WithMessage("*because we want to test the failure message*found <null>*")
            .Where(e => !e.Message.Contains("NullReferenceException"));
    }

    [Fact]
    public async Task When_subject_throws_subclass_of_expected_async_exact_exception_it_should_throw()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject.ThrowAsync<ArgumentNullException>();
        Func<Task> testAction = () => action.Should().ThrowExactlyAsync<ArgumentException>("ABCDE");

        // Assert
        await testAction.Should().ThrowAsync<XunitException>()
            .WithMessage("*ArgumentException*ABCDE*ArgumentNullException*");
    }

    [Fact]
    public async Task When_subject_throws_aggregate_exception_instead_of_exact_exception_it_should_throw()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject.ThrowAggregateExceptionAsync<ArgumentException>();
        Func<Task> testAction = () => action.Should().ThrowExactlyAsync<ArgumentException>("ABCDE");

        // Assert
        await testAction.Should().ThrowAsync<XunitException>()
            .WithMessage("*ArgumentException*ABCDE*AggregateException*");
    }

    [Fact]
    public async Task When_subject_throws_expected_async_exact_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject.ThrowAsync<ArgumentException>();

        // Assert
        await action.Should().ThrowExactlyAsync<ArgumentException>("because {0} should do that", "IFoo.Do");
    }

    public partial class UIFacts
    {
        [UIFact]
        public async Task When_subject_throws_on_UI_thread_expected_async_exact_exception_it_should_succeed()
        {
            // Arrange
            var asyncObject = new AsyncClass();

            // Act
            Func<Task> action = () => asyncObject.ThrowAsync<ArgumentException>();

            // Assert
            await action.Should().ThrowExactlyAsync<ArgumentException>("because {0} should do that", "IFoo.Do");
        }
    }

    [Fact]
    public async Task When_async_method_throws_exception_and_no_exception_was_expected_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsync<ArgumentException>())
            .Should().NotThrowAsync();

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Did not expect any exception, but found System.ArgumentException*");
    }

    [Fact]
    public async Task When_async_method_throws_exception_through_ValueTask_and_no_exception_was_expected_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsyncValueTask<ArgumentException>())
            .Should().NotThrowAsync();

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Did not expect any exception, but found System.ArgumentException*");
    }

    [Fact]
    public async Task When_async_method_throws_exception_and_expected_not_to_throw_another_one_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsync<ArgumentException>())
            .Should().NotThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task
        When_async_method_throws_exception_through_ValueTask_and_expected_not_to_throw_another_one_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsyncValueTask<ArgumentException>())
            .Should().NotThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_exception_and_expected_not_to_throw_async_another_one_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject.ThrowAsync<ArgumentException>();

        // Assert
        await action.Should().NotThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task When_async_method_succeeds_and_expected_not_to_throw_particular_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(_ => asyncObject.SucceedAsync())
            .Should().NotThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task
        When_async_method_succeeds_and_expected_not_to_throw_particular_exception_through_ValueTask_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(_ => asyncObject.SucceedAsyncValueTask())
            .Should().NotThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_exception_expected_not_to_be_thrown_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsync<ArgumentException>())
            .Should().NotThrowAsync<ArgumentException>();

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Did not expect System.ArgumentException, but found*");
    }

    [Fact]
    public async Task When_async_method_throws_exception_expected_through_ValueTask_not_to_be_thrown_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowAsyncValueTask<ArgumentException>())
            .Should().NotThrowAsync<ArgumentException>();

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Did not expect System.ArgumentException, but found*");
    }

    [Fact]
    public async Task When_async_method_of_T_succeeds_and_expected_not_to_throw_particular_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(_ => asyncObject.ReturnTaskInt())
            .Should().NotThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_ValueTask_async_method_of_T_succeeds_and_expected_not_to_throw_particular_exception_it_should_succeed()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(_ => asyncObject.ReturnValueTaskInt())
            .Should().NotThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_of_T_throws_exception_expected_not_to_be_thrown_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowTaskIntAsync<ArgumentException>(true))
            .Should().NotThrowAsync<ArgumentException>();

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Did not expect System.ArgumentException, but found System.ArgumentException*");
    }

    [Fact]
    public async Task When_ValueTask_async_method_of_T_throws_exception_expected_not_to_be_thrown_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();

        // Act
        Func<Task> action = () => asyncObject
            .Awaiting(x => x.ThrowValueTaskIntAsync<ArgumentException>(true))
            .Should().NotThrowAsync<ArgumentException>();

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Did not expect System.ArgumentException, but found System.ArgumentException*");
    }

    [Fact]
    public async Task When_async_method_throws_the_expected_inner_exception_it_should_succeed()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new InvalidOperationException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerException<AggregateException, InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_the_expected_inner_exception_from_argument_it_should_succeed()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new InvalidOperationException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerException(typeof(InvalidOperationException));

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_the_expected_inner_exception_exactly_it_should_succeed()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new ArgumentException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerExceptionExactly<AggregateException, ArgumentException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_the_expected_inner_exception_exactly_defined_in_arguments_it_should_succeed()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new ArgumentException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerExceptionExactly(typeof(ArgumentException));

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_aggregate_exception_containing_expected_exception_it_should_succeed()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new InvalidOperationException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_throws_the_expected_exception_it_should_succeed()
    {
        // Arrange
        Func<Task> task = () => Throw.Async<InvalidOperationException>();

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_async_method_does_not_throw_the_expected_inner_exception_it_should_fail()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new ArgumentException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerException<AggregateException, InvalidOperationException>();

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage("*InvalidOperation*Argument*");
    }

    [Fact]
    public async Task When_async_method_does_not_throw_the_expected_inner_exception_from_argument_it_should_fail()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new ArgumentException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerException(typeof(InvalidOperationException));

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage("*InvalidOperation*Argument*");
    }

    [Fact]
    public async Task When_async_method_does_not_throw_the_expected_inner_exception_exactly_it_should_fail()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new ArgumentNullException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerExceptionExactly<AggregateException, ArgumentException>();

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage("*ArgumentException*ArgumentNullException*");
    }

    [Fact]
    public async Task
        When_async_method_does_not_throw_the_expected_inner_exception_exactly_defined_in_arguments_it_should_fail()
    {
        // Arrange
        Func<Task> task = () => Throw.Async(new AggregateException(new ArgumentNullException()));

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<AggregateException>()
            .WithInnerExceptionExactly(typeof(ArgumentException));

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage("*ArgumentException*ArgumentNullException*");
    }

    [Fact]
    public async Task When_async_method_does_not_throw_the_expected_exception_it_should_fail()
    {
        // Arrange
        Func<Task> task = () => Throw.Async<ArgumentException>();

        // Act
        Func<Task> action = () => task
            .Should().ThrowAsync<InvalidOperationException>();

        // Assert
        await action.Should().ThrowAsync<XunitException>().WithMessage("*InvalidOperation*Argument*");
    }

    [Fact]
    public void When_asserting_async_void_method_should_throw_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();
        Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

        // Act
        Action action = () => asyncVoidMethod.Should().Throw<ArgumentException>();

        // Assert
        action.Should().Throw<InvalidOperationException>("*async*void*");
    }

    [Fact]
    public void When_asserting_async_void_method_should_throw_exactly_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();
        Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

        // Act
        Action action = () => asyncVoidMethod.Should().ThrowExactly<ArgumentException>();

        // Assert
        action.Should().Throw<InvalidOperationException>("*async*void*");
    }

    [Fact]
    public void When_asserting_async_void_method_should_not_throw_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();
        Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

        // Act
        Action action = () => asyncVoidMethod.Should().NotThrow();

        // Assert
        action.Should().Throw<InvalidOperationException>("*async*void*");
    }

    [Fact]
    public void When_asserting_async_void_method_should_not_throw_specific_exception_it_should_fail()
    {
        // Arrange
        var asyncObject = new AsyncClass();
        Action asyncVoidMethod = async () => await asyncObject.IncompleteTask();

        // Act
        Action action = () => asyncVoidMethod.Should().NotThrow<ArgumentException>();

        // Assert
        action.Should().Throw<InvalidOperationException>("*async*void*");
    }

    [Fact]
    public async Task When_a_method_throws_with_a_matching_parameter_name_it_should_succeed()
    {
        // Arrange
        Func<Task> task = () => new AsyncClass().ThrowAsync(new ArgumentNullException("someParameter"));

        // Act
        Func<Task> act = () =>
            task.Should().ThrowAsync<ArgumentException>()
                .WithParameterName("someParameter");

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_a_method_throws_with_a_non_matching_parameter_name_it_should_fail_with_a_descriptive_message()
    {
        // Arrange
        Func<Task> task = () => new AsyncClass().ThrowAsync(new ArgumentNullException("someOtherParameter"));

        // Act
        Func<Task> act = () =>
            task.Should().ThrowAsync<ArgumentException>()
                .WithParameterName("someParameter", "we want to test the failure {0}", "message");

        // Assert
        await act.Should().ThrowAsync<XunitException>()
            .WithMessage("*with parameter name \"someParameter\"*we want to test the failure message*\"someOtherParameter\"*");
    }

    #region NotThrowAfterAsync

    [Fact]
    public async Task When_wait_time_is_zero_for_async_func_executed_with_wait_it_should_not_throw()
    {
        // Arrange
        var waitTime = 0.Milliseconds();
        var pollInterval = 10.Milliseconds();

        var clock = new FakeClock();
        var asyncObject = new AsyncClass();
        Func<Task> someFunc = () => asyncObject.SucceedAsync();

        // Act
        Func<Task> act = () => someFunc.Should(clock).NotThrowAfterAsync(waitTime, pollInterval);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_poll_interval_is_zero_for_async_func_executed_with_wait_it_should_not_throw()
    {
        // Arrange
        var waitTime = 10.Milliseconds();
        var pollInterval = 0.Milliseconds();

        var clock = new FakeClock();
        var asyncObject = new AsyncClass();
        Func<Task> someFunc = () => asyncObject.SucceedAsync();

        // Act
        Func<Task> act = () => someFunc.Should(clock).NotThrowAfterAsync(waitTime, pollInterval);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task When_subject_is_null_for_async_func_it_should_throw()
    {
        // Arrange
        var waitTime = 0.Milliseconds();
        var pollInterval = 0.Milliseconds();
        Func<Task> action = null;

        // Act
        Func<Task> testAction = async () =>
        {
            using var _ = new AssertionScope();

            await action.Should().NotThrowAfterAsync(waitTime, pollInterval,
                "because we want to test the failure {0}", "message");
        };

        // Assert
        await testAction.Should().ThrowAsync<XunitException>()
            .WithMessage("*because we want to test the failure message*found <null>*")
            .Where(e => !e.Message.Contains("NullReferenceException"));
    }

    [Fact]
    public async Task When_wait_time_is_negative_for_async_func_it_should_throw()
    {
        // Arrange
        var waitTime = -1.Milliseconds();
        var pollInterval = 10.Milliseconds();

        var asyncObject = new AsyncClass();
        Func<Task> someFunc = () => asyncObject.SucceedAsync();

        // Act
        Func<Task> act = () => someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithParameterName("waitTime")
            .WithMessage("*must be non-negative*");
    }

    [Fact]
    public async Task When_poll_interval_is_negative_for_async_func_it_should_throw()
    {
        // Arrange
        var waitTime = 10.Milliseconds();
        var pollInterval = -1.Milliseconds();

        var asyncObject = new AsyncClass();
        Func<Task> someFunc = () => asyncObject.SucceedAsync();

        // Act
        Func<Task> act = () => someFunc.Should().NotThrowAfterAsync(waitTime, pollInterval);

        // Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithParameterName("pollInterval")
            .WithMessage("*must be non-negative*");
    }

    [Fact]
    public async Task When_no_exception_should_be_thrown_for_null_async_func_after_wait_time_it_should_throw()
    {
        // Arrange
        var waitTime = 2.Seconds();
        var pollInterval = 10.Milliseconds();

        Func<Task> func = null;

        // Act
        Func<Task> action = () => func.Should()
            .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("*but found <null>*");
    }

    [Fact]
    public async Task When_no_exception_should_be_thrown_for_async_func_after_wait_time_but_it_was_it_should_throw()
    {
        // Arrange
        var waitTime = 2.Seconds();
        var pollInterval = 10.Milliseconds();

        var clock = new FakeClock();
        var timer = clock.StartTimer();
        clock.CompleteAfter(waitTime);

        Func<Task> throwLongerThanWaitTime = async () =>
        {
            if (timer.Elapsed <= waitTime.Multiply(1.5))
            {
                throw new ArgumentException("An exception was forced");
            }

            await Task.Yield();
        };

        // Act
        Func<Task> action = () => throwLongerThanWaitTime.Should(clock)
            .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

        // Assert
        await action.Should().ThrowAsync<XunitException>()
            .WithMessage("Did not expect any exceptions after 2s because we passed valid arguments*");
    }

    public partial class UIFacts
    {
        [UIFact]
        public async Task
            When_no_exception_should_be_thrown_on_UI_thread_for_async_func_after_wait_time_but_it_was_it_should_throw()
        {
            // Arrange
            var waitTime = 2.Seconds();
            var pollInterval = 10.Milliseconds();

            var clock = new FakeClock();
            var timer = clock.StartTimer();
            clock.CompleteAfter(waitTime);

            Func<Task> throwLongerThanWaitTime = async () =>
            {
                if (timer.Elapsed <= waitTime.Multiply(1.5))
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Yield();
            };

            // Act
            Func<Task> action = () => throwLongerThanWaitTime.Should(clock)
                .NotThrowAfterAsync(waitTime, pollInterval, "we passed valid arguments");

            // Assert
            await action.Should().ThrowAsync<XunitException>()
                .WithMessage("Did not expect any exceptions after 2s because we passed valid arguments*");
        }
    }

    [Fact]
    public async Task When_no_exception_should_be_thrown_for_async_func_after_wait_time_and_none_was_it_should_not_throw()
    {
        // Arrange
        var waitTime = 6.Seconds();
        var pollInterval = 10.Milliseconds();

        var clock = new FakeClock();
        var timer = clock.StartTimer();
        clock.Delay(waitTime);

        Func<Task> throwShorterThanWaitTime = async () =>
        {
            if (timer.Elapsed <= waitTime.Divide(12))
            {
                throw new ArgumentException("An exception was forced");
            }

            await Task.Yield();
        };

        // Act
        Func<Task> act = () => throwShorterThanWaitTime.Should(clock).NotThrowAfterAsync(waitTime, pollInterval);

        // Assert
        await act.Should().NotThrowAsync();
    }

    public partial class UIFacts
    {
        [UIFact]
        public async Task
            When_no_exception_should_be_thrown_on_UI_thread_for_async_func_after_wait_time_and_none_was_it_should_not_throw()
        {
            // Arrange
            var waitTime = 6.Seconds();
            var pollInterval = 10.Milliseconds();

            var clock = new FakeClock();
            var timer = clock.StartTimer();
            clock.Delay(waitTime);

            Func<Task> throwShorterThanWaitTime = async () =>
            {
                if (timer.Elapsed <= waitTime.Divide(12))
                {
                    throw new ArgumentException("An exception was forced");
                }

                await Task.Yield();
            };

            // Act
            Func<Task> act = () => throwShorterThanWaitTime.Should(clock).NotThrowAfterAsync(waitTime, pollInterval);

            // Assert
            await act.Should().NotThrowAsync();
        }
    }

    #endregion
}

internal class AsyncClass
{
    public Task ThrowAsync<TException>()
        where TException : Exception, new() =>
        ThrowAsync(new TException());

    public Task ThrowAsync(Exception exception) =>
        Throw.Async(exception);

    public ValueTask ThrowAsyncValueTask<TException>()
        where TException : Exception, new() =>
        Throw.AsyncValueTask(new TException());

    public Task ThrowAggregateExceptionAsync<TException>()
        where TException : Exception, new() =>
        Throw.Async(new AggregateException(new TException()));

    public ValueTask ThrowAggregateExceptionAsyncValueTask<TException>()
        where TException : Exception, new() =>
        Throw.AsyncValueTask(new AggregateException(new TException()));

    public async Task SucceedAsync()
    {
        await Task.FromResult(0);
    }

    public async ValueTask SucceedAsyncValueTask()
    {
        await Task.FromResult(0);
    }

    public Task<int> ReturnTaskInt()
    {
        return Task.FromResult(0);
    }

    public ValueTask<int> ReturnValueTaskInt()
    {
        return new ValueTask<int>(0);
    }

    public Task IncompleteTask()
    {
        return new TaskCompletionSource<bool>().Task;
    }

    public async Task<int> ThrowTaskIntAsync<TException>(bool throwException)
        where TException : Exception, new()
    {
        await Task.Yield();

        if (throwException)
        {
            throw new TException();
        }

        return 123;
    }

    public async ValueTask<int> ThrowValueTaskIntAsync<TException>(bool throwException)
        where TException : Exception, new()
    {
        await Task.Yield();

        if (throwException)
        {
            throw new TException();
        }

        return 123;
    }
}

internal static class Throw
{
    public static Task Async<TException>()
        where TException : Exception, new() =>
        Async(new TException());

    public static async Task Async(Exception expcetion)
    {
        await Task.Yield();
        throw expcetion;
    }

    public static async ValueTask AsyncValueTask(Exception expcetion)
    {
        await Task.Yield();
        throw expcetion;
    }
}
