using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Specialized;

namespace FluentAssertionsAsync;

public static class AsyncAssertionsExtensions
{
#pragma warning disable AV1755 // "Name of async method ... should end with Async"; Async suffix is too noisy in fluent API
    /// <summary>
    /// Asserts that the completed <see cref="Task{TResult}"/> provides the specified result.
    /// </summary>
    /// <param name="task">The <see cref="GenericAsyncFunctionAssertions{T}"/> containing the <see cref="Task{TResult}"/>.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <remarks>
    /// Please note that this assertion cannot identify whether the previous assertion was successful or not.
    /// In case it was not successful and it is running within an active <see cref="AssertionScope"/>
    /// there is no current result to compare with.
    /// So, this extension will compare with the default value.
    /// </remarks>
    public static async Task<AndWhichConstraint<GenericAsyncFunctionAssertions<T>, T>> WithResult<T>(
        this Task<AndWhichConstraint<GenericAsyncFunctionAssertions<T>, T>> task,
        T expected, string because = "", params object[] becauseArgs)
    {
        var andWhichConstraint = await task;
        var subject = andWhichConstraint.Subject;
        subject.Should().Be(expected, because, becauseArgs);

        return andWhichConstraint;
    }

    /// <summary>
    /// Asserts that the completed <see cref="TaskCompletionSource{TResult}"/> provides the specified result.
    /// </summary>
    /// <param name="task">The <see cref="TaskCompletionSourceAssertions{T}"/> containing the <see cref="TaskCompletionSource{TResult}"/>.</param>
    /// <param name="expected">The expected value.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public static async Task<AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>> WithResult<T>(
        this Task<AndWhichConstraint<TaskCompletionSourceAssertions<T>, T>> task,
        T expected, string because = "", params object[] becauseArgs)
    {
        var andWhichConstraint = await task;
        var subject = andWhichConstraint.Subject;
        subject.Should().Be(expected, because, becauseArgs);

        return andWhichConstraint;
    }
}
