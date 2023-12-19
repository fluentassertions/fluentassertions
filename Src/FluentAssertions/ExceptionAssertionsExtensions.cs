using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Specialized;

namespace FluentAssertionsAsync;

public static class ExceptionAssertionsExtensions
{
#pragma warning disable AV1755 // "Name of async method ... should end with Async"; Async suffix is too noisy in fluent API

    /// <summary>
    /// Asserts that the thrown exception has a message that matches <paramref name="expectedWildcardPattern" />.
    /// </summary>
    /// <param name="task">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="expectedWildcardPattern">
    /// The wildcard pattern with which the exception message is matched, where * and ? have special meanings.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public static async Task<ExceptionAssertions<TException>> WithMessage<TException>(
        this Task<ExceptionAssertions<TException>> task,
        string expectedWildcardPattern,
        string because = "",
        params object[] becauseArgs)
        where TException : Exception
    {
        return (await task).WithMessage(expectedWildcardPattern, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the exception matches a particular condition.
    /// </summary>
    /// <param name="task">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="exceptionExpression">
    /// The condition that the exception must match.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public static async Task<ExceptionAssertions<TException>> Where<TException>(
        this Task<ExceptionAssertions<TException>> task,
        Expression<Func<TException, bool>> exceptionExpression,
        string because = "", params object[] becauseArgs)
        where TException : Exception
    {
        return (await task).Where(exceptionExpression, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of type <typeparamref name="TInnerException" />.
    /// </summary>
    /// <typeparam name="TException">The expected type of the exception.</typeparam>
    /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
    /// <param name="task">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static async Task<ExceptionAssertions<TInnerException>> WithInnerException<TException, TInnerException>(
        this Task<ExceptionAssertions<TException>> task,
        string because = "",
        params object[] becauseArgs)
        where TException : Exception
        where TInnerException : Exception
    {
        return (await task).WithInnerException<TInnerException>(because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of type <param name="innerException" />.
    /// </summary>
    /// <typeparam name="TException">The expected type of the exception.</typeparam>
    /// <param name="task">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static async Task<ExceptionAssertions<Exception>> WithInnerException<TException>(
        this Task<ExceptionAssertions<TException>> task,
        Type innerException,
        string because = "",
        params object[] becauseArgs)
        where TException : Exception
    {
        return (await task).WithInnerException(innerException, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of the exact type <typeparamref name="TInnerException" /> (and not a derived exception type).
    /// </summary>
    /// <typeparam name="TException">The expected type of the exception.</typeparam>
    /// <typeparam name="TInnerException">The expected type of the inner exception.</typeparam>
    /// <param name="task">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static async Task<ExceptionAssertions<TInnerException>> WithInnerExceptionExactly<TException, TInnerException>(
        this Task<ExceptionAssertions<TException>> task,
        string because = "",
        params object[] becauseArgs)
        where TException : Exception
        where TInnerException : Exception
    {
        return (await task).WithInnerExceptionExactly<TInnerException>(because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the thrown exception contains an inner exception of the exact type <param name="innerException" /> (and not a derived exception type).
    /// </summary>
    /// <typeparam name="TException">The expected type of the exception.</typeparam>
    /// <param name="task">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static async Task<ExceptionAssertions<Exception>> WithInnerExceptionExactly<TException>(
        this Task<ExceptionAssertions<TException>> task,
        Type innerException,
        string because = "",
        params object[] becauseArgs)
        where TException : Exception
    {
        return (await task).WithInnerExceptionExactly(innerException, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that the thrown exception has a parameter which name matches <paramref name="paramName" />.
    /// </summary>
    /// <param name="parent">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="paramName">The expected name of the parameter</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public static ExceptionAssertions<TException> WithParameterName<TException>(
        this ExceptionAssertions<TException> parent,
        string paramName,
        string because = "",
        params object[] becauseArgs)
        where TException : ArgumentException
    {
        Execute.Assertion
            .ForCondition(parent.Which.ParamName == paramName)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected exception with parameter name {0}{reason}, but found {1}.", paramName, parent.Which.ParamName);

        return parent;
    }

    /// <summary>
    /// Asserts that the thrown exception has a parameter which name matches <paramref name="paramName" />.
    /// </summary>
    /// <param name="task">The <see cref="ExceptionAssertions{TException}"/> containing the thrown exception.</param>
    /// <param name="paramName">The expected name of the parameter</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public static async Task<ExceptionAssertions<TException>> WithParameterName<TException>(
        this Task<ExceptionAssertions<TException>> task,
        string paramName,
        string because = "",
        params object[] becauseArgs)
        where TException : ArgumentException
    {
        return (await task).WithParameterName(paramName, because, becauseArgs);
    }

#pragma warning restore AV1755
}
