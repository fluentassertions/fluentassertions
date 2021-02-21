using System.Threading.Tasks;
using FluentAssertions.Specialized;

namespace FluentAssertions
{
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
}
