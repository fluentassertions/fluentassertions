using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Exceptions
{
    public class InnerExceptionSpecs
    {
        [Fact]
        public void When_subject_throws_an_exception_with_the_expected_inner_exception_it_should_not_do_anything()
        {
            // Arrange
            Does testSubject = Does.Throw(new Exception("", new ArgumentException()));

            // Act / Assert
            testSubject
                .Invoking(x => x.Do())
                .Should().Throw<Exception>()
                .WithInnerException<ArgumentException>();
        }

        [Fact]
        public void When_subject_throws_an_exception_with_the_expected_inner_base_exception_it_should_not_do_anything()
        {
            // Arrange
            Does testSubject = Does.Throw(new Exception("", new ArgumentNullException()));

            // Act / Assert
            testSubject
                .Invoking(x => x.Do())
                .Should().Throw<Exception>()
                .WithInnerException<ArgumentException>();
        }

        [Fact]
        public void When_subject_throws_an_exception_with_the_expected_inner_exception_from_argument_it_should_not_do_anything()
        {
            // Arrange
            Does testSubject = Does.Throw(new Exception("", new ArgumentException()));

            // Act / Assert
            testSubject
                .Invoking(x => x.Do())
                .Should().Throw<Exception>()
                .WithInnerException(typeof(ArgumentException));
        }

        [Fact]
        public void WithInnerExceptionExactly_no_parameters_when_subject_throws_subclass_of_expected_inner_exception_it_should_throw_with_clear_description()
        {
            // Arrange
            var innerException = new ArgumentNullException();

            Action act = () => throw new BadImageFormatException("", innerException);

            try
            {
                // Act
                act.Should().Throw<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentException>();

                throw new XunitException("This point should not be reached.");
            }
            catch (XunitException ex)
            {
                // Assert
                var expectedMessage = BuildExpectedMessageForWithInnerExceptionExactly("Expected inner System.ArgumentException, but found System.ArgumentNullException with message", innerException.Message);

                ex.Message.Should().Be(expectedMessage);
            }
        }

        [Fact]
        public void WithInnerExceptionExactly_no_parameters_when_subject_throws_expected_inner_exception_it_should_not_do_anything()
        {
            // Arrange
            Action act = () => throw new BadImageFormatException("", new ArgumentNullException());

            // Act / Assert
            act.Should().Throw<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentNullException>();
        }

        [Fact]
        public void WithInnerExceptionExactly_when_subject_throws_subclass_of_expected_inner_exception_it_should_throw_with_clear_description()
        {
            // Arrange
            var innerException = new ArgumentNullException();

            Action act = () => throw new BadImageFormatException("", innerException);

            try
            {
                // Act
                act.Should().Throw<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentException>("because {0} should do just that", "the action");

                throw new XunitException("This point should not be reached.");
            }
            catch (XunitException ex)
            {
                // Assert
                var expectedMessage = BuildExpectedMessageForWithInnerExceptionExactly("Expected inner System.ArgumentException because the action should do just that, but found System.ArgumentNullException with message", innerException.Message);

                ex.Message.Should().Be(expectedMessage);
            }
        }

        [Fact]
        public void WithInnerExceptionExactly_with_type_exception_when_subject_throws_expected_inner_exception_it_should_not_do_anything()
        {
            // Arrange
            Action act = () => throw new BadImageFormatException("", new ArgumentNullException());

            // Act / Assert
            act.Should().Throw<BadImageFormatException>()
                .WithInnerExceptionExactly(typeof(ArgumentNullException), "because {0} should do just that", "the action");
        }

        [Fact]
        public void WithInnerExceptionExactly_with_type_exception_no_parameters_when_subject_throws_expected_inner_exception_it_should_not_do_anything()
        {
            // Arrange
            Action act = () => throw new BadImageFormatException("", new ArgumentNullException());

            // Act / Assert
            act.Should().Throw<BadImageFormatException>()
                .WithInnerExceptionExactly(typeof(ArgumentNullException));
        }

        [Fact]
        public void WithInnerExceptionExactly_with_type_exception_when_subject_throws_subclass_of_expected_inner_exception_it_should_throw_with_clear_description()
        {
            // Arrange
            var innerException = new ArgumentNullException();

            Action act = () => throw new BadImageFormatException("", innerException);

            try
            {
                // Act
                act.Should().Throw<BadImageFormatException>()
                    .WithInnerExceptionExactly(typeof(ArgumentException), "because {0} should do just that", "the action");

                throw new XunitException("This point should not be reached.");
            }
            catch (XunitException ex)
            {
                // Assert
                var expectedMessage = BuildExpectedMessageForWithInnerExceptionExactly("Expected inner System.ArgumentException because the action should do just that, but found System.ArgumentNullException with message", innerException.Message);

                ex.Message.Should().Be(expectedMessage);
            }
        }

        [Fact]
        public void WithInnerExceptionExactly_when_subject_throws_expected_inner_exception_it_should_not_do_anything()
        {
            // Arrange
            Action act = () => throw new BadImageFormatException("", new ArgumentNullException());

            // Act / Assert
            act.Should().Throw<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentNullException>("because {0} should do just that", "the action");
        }

        private static string BuildExpectedMessageForWithInnerExceptionExactly(string because, string innerExceptionMessage)
        {
            var expectedMessage = $"{because} \"{innerExceptionMessage}\".";

            return expectedMessage;
        }

        [Fact]
        public void When_subject_throws_an_exception_with_an_unexpected_inner_exception_it_should_throw_with_clear_description()
        {
            // Arrange
            var innerException = new NullReferenceException();

            Does testSubject = Does.Throw(new Exception("", innerException));

            try
            {
                // Act
                testSubject
                    .Invoking(x => x.Do())
                    .Should().Throw<Exception>()
                    .WithInnerException<ArgumentException>("because {0} should do just that", "Does.Do");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException exc)
            {
                // Assert
                exc.Message.Should().StartWith(
                    "Expected inner System.ArgumentException because Does.Do should do just that, but found System.NullReferenceException");

                exc.Message.Should().Contain(innerException.Message);
            }
        }

        [Fact]
        public void When_subject_throws_an_exception_without_expected_inner_exception_it_should_throw_with_clear_description()
        {
            try
            {
                Does testSubject = Does.Throw<Exception>();

                testSubject.Invoking(x => x.Do()).Should().Throw<Exception>()
                    .WithInnerException<InvalidOperationException>();

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner System.InvalidOperationException, but the thrown exception has no inner exception.");
            }
        }

        [Fact]
        public void When_subject_throws_an_exception_without_expected_inner_exception_and_has_reason_it_should_throw_with_clear_description()
        {
            try
            {
                Does testSubject = Does.Throw<Exception>();

                testSubject.Invoking(x => x.Do()).Should().Throw<Exception>()
                    .WithInnerException<InvalidOperationException>("because {0} should do that", "Does.Do");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner System.InvalidOperationException because Does.Do should do that, but the thrown exception has no inner exception.");
            }
        }

        [Fact]
        public void When_an_inner_exception_matches_exactly_it_should_allow_chaining_more_asserts_on_that_exception_type()
        {
            // Act
            Action act = () =>
                throw new ArgumentException("OuterMessage", new InvalidOperationException("InnerMessage"));

            // Assert
            act
                .Should().ThrowExactly<ArgumentException>()
                .WithInnerExceptionExactly<InvalidOperationException>()
                .Where(i => i.Message == "InnerMessage");
        }

        [Fact]
        public void When_an_inner_exception_matches_exactly_it_should_allow_chaining_more_asserts_on_that_exception_type_from_argument()
        {
            // Act
            Action act = () =>
                throw new ArgumentException("OuterMessage", new InvalidOperationException("InnerMessage"));

            // Assert
            act
                .Should().ThrowExactly<ArgumentException>()
                .WithInnerExceptionExactly(typeof(InvalidOperationException))
                .Where(i => i.Message == "InnerMessage");
        }

        [Fact]
        public void When_injecting_a_null_predicate_it_should_throw()
        {
            // Arrange
            Action act = () => throw new Exception();

            // Act
            Action act2 = () => act.Should().Throw<Exception>()
                .Where(exceptionExpression: null);

            // Act
            act2.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("exceptionExpression");
        }
    }
}
