using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;
#if FAKES

using FakeItEasy;

using FluentAssertions.Primitives;

namespace FluentAssertions.Specs
{
    
    public class ExceptionAssertionSpecs
    {
        [Fact]
        public void ShouldThrowExactly_when_subject_throws_subclass_of_expected_exception_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action a = () => { throw new ArgumentNullException(); };

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                a.ShouldThrowExactly<ArgumentException>("because {0} should do that", "IFoo.Do");

                throw new XunitException("This point should not be reached.");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Match("Expected type to be System.ArgumentException because IFoo.Do should do that, but found System.ArgumentNullException.");
            }
        }

        [Fact]
        public void ShouldThrowExactly_when_subject_throws_expected_exception_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action a = () => { throw new ArgumentNullException(); };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            a.ShouldThrowExactly<ArgumentNullException>();
        }

        #region Outer Exceptions

        [Fact]
        public void When_subject_throws_expected_exception_with_an_expected_message_it_should_not_do_anything()
        {
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException("some message"));

            testSubject.Invoking(x => x.Do()).ShouldThrow<InvalidOperationException>().WithMessage("some message");
        }

        [Fact]
        public void When_subject_throws_expected_exception_but_with_unexpected_message_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException("some"));

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                testSubject
                    .Invoking(x => x.Do())
                    .ShouldThrow<InvalidOperationException>()
                    .WithMessage("some message");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().StartWith(
                    "Expected exception message to match the equivalent of \r\n\"some message\", but \r\n\"some\" does not.");
            }
        }

        [Fact]
        public void When_subject_throws_expected_exception_with_message_starting_with_expected_message_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException("expected message"));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("expected mes*");
        }

        [Fact]
        public void When_subject_throws_expected_exception_with_message_that_does_not_start_with_expected_message_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException("OxpectOd message"));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => testSubject
                .Invoking(s => s.Do())
                .ShouldThrow<InvalidOperationException>()
                .WithMessage("Expected mes");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>()
                .WithMessage("Expected exception message to match the equivalent of \r\n\"Expected mes*\", but \r\n\"OxpectOd message\" does not*");
        }

        [Fact]
        public void When_subject_throws_expected_exception_with_message_starting_with_expected_equivalent_message_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException("Expected Message"));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("expected mes*");
        }

        [Fact]
        public void When_subject_throws_expected_exception_with_message_that_does_not_start_with_equivalent_message_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException("OxpectOd message"));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => testSubject
                    .Invoking(s => s.Do())
                    .ShouldThrow<InvalidOperationException>()
                    .WithMessage("expected mes");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>()
                .WithMessage("Expected exception message to match the equivalent of \r\n\"expected mes*\", but \r\n\"OxpectOd message\" does not*");
        }

        [Fact]
        public void When_subject_throws_some_exception_with_unexpected_message_it_should_throw_with_clear_description()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo subjectThatThrows = A.Fake<IFoo>();
            A.CallTo(() => subjectThatThrows.Do()).Throws(new InvalidOperationException("message1"));

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                subjectThatThrows
                    .Invoking(x => x.Do())
                    .ShouldThrow<InvalidOperationException>()
                    .WithMessage("message2", "because we want to test the failure {0}", "message");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Match(
                    "Expected exception message to match the equivalent of \"message2\" because we want to test the failure message, but \"message1\" does not*");
            }
        }

        [Fact]
        public void When_subject_throws_some_exception_with_an_empty_message_it_should_throw_with_clear_description()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo subjectThatThrows = A.Fake<IFoo>();
            A.CallTo(() => subjectThatThrows.Do()).Throws(new InvalidOperationException(""));

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                subjectThatThrows
                    .Invoking(x => x.Do())
                    .ShouldThrow<InvalidOperationException>()
                    .WithMessage("message2");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Match(
                    "Expected exception message to match the equivalent of \"message2\"*, but \"\"*");
            }
        }

        [Fact]
        public void When_subject_throws_some_exception_with_message_which_contains_complete_expected_exception_and_more_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo subjectThatThrows = A.Fake<IFoo>();
            A.CallTo(() => subjectThatThrows.Do(A<string>.Ignored))
                .Throws(new ArgumentNullException("someParam", "message2"));

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                subjectThatThrows
                    .Invoking(x => x.Do("something"))
                    .ShouldThrow<ArgumentNullException>()
                    .WithMessage("message2");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Match(
                    "Expected exception message to match the equivalent of \r\n\"message2\"*, but \r\n\"message2\\r\\nParameter name: someParam\"*");
            }
        }

        [Fact]
        public void When_no_exception_was_thrown_but_one_was_expected_it_should_clearly_report_that()
        {
            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Arrange
                //-----------------------------------------------------------------------------------------------------------
                IFoo testSubject = A.Fake<IFoo>();

                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>("because {0} should do that", "IFoo.Do");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Be(
                    "Expected a <System.Exception> to be thrown because IFoo.Do should do that, but no exception was thrown.");
            }
        }

        [Fact]
        public void When_subject_throws_another_exception_than_expected_it_should_include_details_of_that_exception()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var actualException = new ArgumentException();

            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(actualException);

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                testSubject
                    .Invoking(x => x.Do())
                    .ShouldThrow<InvalidOperationException>("because {0} should throw that one", "IFoo.Do");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().StartWith(
                    "Expected a <System.InvalidOperationException> to be thrown because IFoo.Do should throw that one, but found a <System.ArgumentException>:");

                ex.Message.Should().Contain(actualException.Message);
            }
        }

        [Fact]
        public void When_asserting_with_an_aggregate_exception_type_the_asserts_should_occur_against_the_aggregate_exception()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do())
                .Throws(new AggregateException("Outer Message",
                    new Exception("Inner Message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------            
            act.ShouldThrow<AggregateException>()
                .WithMessage("Outer Message")
                .WithInnerException<Exception>()
                .WithInnerMessage("Inner Message");
        }

        #endregion

        #region Inner Exceptions

        [Fact]
        public void When_subject_throws_an_exception_with_the_expected_inner_exception_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new ArgumentException()));

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            testSubject
                .Invoking(x => x.Do())
                .ShouldThrow<Exception>()
                .WithInnerException<ArgumentException>();
        }

        [Fact]
        public void WithInnerExceptionExactly_no_paramters_when_subject_throws_subclass_of_expected_inner_exception_it_should_throw_with_clear_description()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var innerException = new ArgumentNullException();

            Action a = () => { throw new BadImageFormatException("", innerException); };

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                a.ShouldThrow<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentException>();

                throw new XunitException("This point should not be reached.");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                var expectedMessage = BuildExpectedMessageForWithInnerExceptionExactly("Expected inner System.ArgumentException, but found System.ArgumentNullException with message", innerException.Message);

                ex.Message.Should().Be(expectedMessage);
            }
        }

        [Fact]
        public void WithInnerExceptionExactly_no_paramters_when_subject_throws_expected_inner_exception_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action a = () => { throw new BadImageFormatException("", new ArgumentNullException()); };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            a.ShouldThrow<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentNullException>();
        }

        [Fact]
        public void WithInnerExceptionExactly_when_subject_throws_subclass_of_expected_inner_exception_it_should_throw_with_clear_description()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var innerException = new ArgumentNullException();

            Action a = () => { throw new BadImageFormatException("", innerException); };

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                a.ShouldThrow<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentException>("because {0} should do just that", "the action");

                throw new XunitException("This point should not be reached.");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                var expectedMessage = BuildExpectedMessageForWithInnerExceptionExactly("Expected inner System.ArgumentException because the action should do just that, but found System.ArgumentNullException with message", innerException.Message);

                ex.Message.Should().Be(expectedMessage);
            }
        }

        [Fact]
        public void WithInnerExceptionExactly_when_subject_throws_expected_inner_exception_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action a = () => { throw new BadImageFormatException("", new ArgumentNullException()); };

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            a.ShouldThrow<BadImageFormatException>()
                    .WithInnerExceptionExactly<ArgumentNullException>("because {0} should do just that", "the action");
        }

        private static string BuildExpectedMessageForWithInnerExceptionExactly(string because, string innerExceptionMessage)
        {
            var expectedMessage = string.Format("{0} \"{1}\"\n.", because, innerExceptionMessage);

            return expectedMessage;
        }

        [Fact]
        public void When_subject_throws_an_exception_with_an_unexpected_inner_exception_it_should_throw_with_clear_description()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var innerException = new NullReferenceException();

            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", innerException));

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                testSubject
                    .Invoking(x => x.Do())
                    .ShouldThrow<Exception>()
                    .WithInnerException<ArgumentException>("because {0} should do just that", "IFoo.Do");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException exc)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                exc.Message.Should().StartWith(
                    "Expected inner System.ArgumentException because IFoo.Do should do just that, but found System.NullReferenceException");

                exc.Message.Should().Contain(innerException.Message);
            }
        }

        [Fact]
        public void When_subject_throws_an_exception_without_expected_inner_exception_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = A.Fake<IFoo>();
                A.CallTo(() => testSubject.Do()).Throws(new Exception(""));

                testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>()
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
        public void
            When_subject_throws_an_exception_without_expected_inner_exception_and_has_reason_it_should_throw_with_clear_description
            ()
        {
            try
            {
                IFoo testSubject = A.Fake<IFoo>();
                A.CallTo(() => testSubject.Do()).Throws(new Exception(""));

                testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>()
                    .WithInnerException<InvalidOperationException>("because {0} should do that", "IFoo.Do");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner System.InvalidOperationException because IFoo.Do should do that, but the thrown exception has no inner exception.");
            }
        }

        [Fact]
        public void When_subject_throws_inner_exception_with_expected_message_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("expected message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>()
                .WithInnerMessage("*xpected messag*");
        }
        
        [Fact]
        public void When_subject_throws_inner_exception_with_message_starting_with_expected_message_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("expected message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>()
                .WithInnerMessage("expected mes*");
        }
        
        [Fact]
        public void When_subject_throws_inner_exception_with_message_that_does_not_start_with_expected_message_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("OxpectOd message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => testSubject
                    .Invoking(s => s.Do())
                    .ShouldThrow<Exception>()
                    .WithInnerMessage("Expected mes*");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>()
                .WithMessage("Expected inner exception message to match the equivalent of \r\n\"Expected mes*\", but \r\n\"OxpectOd message\" does not*");
        }

        [Fact]
        public void When_subject_throws_inner_exception_with_message_starting_with_expected_equivalent_message_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("Expected Message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>()
                .WithInnerMessage("expected mes*");
        }

        [Fact]
        public void When_subject_throws_inner_exception_with_message_that_does_not_start_with_equivalent_message_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("OxpectOd message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => testSubject
                    .Invoking(s => s.Do())
                    .ShouldThrow<Exception>()
                    .WithInnerMessage("expected mes");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>()
                .WithMessage("Expected inner exception message to match the equivalent of \r\n\"expected mes*\", but \r\n\"OxpectOd message\" does not*");
        }

        [Fact]
        public void When_subject_throws_inner_exception_without_an_equivalent_message_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("OxpectOd message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => testSubject
                    .Invoking(s => s.Do())
                    .ShouldThrow<Exception>()
                    .WithInnerMessage("Expected message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>()
                .WithMessage("Expected inner exception message to match the equivalent of \r\n\"Expected message\", but \r\n\"OxpectOd message\" does not*");
        }

        [Fact]
        public void When_subject_throws_inner_exception_with_a_matching_message_with_different_casing_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("Expected Message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<Exception>()
                .WithInnerMessage("EXPECTED*");
        }

        [Fact]
        public void When_subject_throws_inner_exception_with_a_matching_message_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("expected message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<Exception>()
                .WithInnerMessage("*ted*mes*");
        }

        [Fact]
        public void When_subject_throws_inner_exception_with_unexpected_message_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("unexpected message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            try
            {
                testSubject
                    .Invoking(x => x.Do())
                    .ShouldThrow<Exception>()
                    .WithInnerMessage("expected message", "because {0} should do just that", "IFoo.Do");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Match(
                    "Expected inner*\r\n\"expected message\"*because IFoo.Do should do just that, but*");
            }
        }

        #endregion

        #region Miscellaneous

        [Fact]
        public void When_getting_value_of_property_of_thrown_exception_it_should_return_value_of_property()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------            
            const string SomeParamNameValue = "param";
            var target = A.Fake<IFoo>();
            A.CallTo(() => target.Do()).Throws(new ExceptionWithProperties(SomeParamNameValue));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = target.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------            
            act.ShouldThrow<ExceptionWithProperties>().And.Property.Should().Be(SomeParamNameValue);
        }

        [Fact]
        public void When_validating_a_subject_against_multiple_conditions_it_should_support_chaining()
        {
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(
                new InvalidOperationException("message", new ArgumentException("inner message")));

            testSubject
                .Invoking(x => x.Do())
                .ShouldThrow<InvalidOperationException>()
                .WithInnerMessage("inner message")
                .WithInnerException<ArgumentException>()
                .WithInnerMessage("inner message");
        }

        [Fact]
        public void When_a_yielding_enumerable_throws_an_expected_exception_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Func<IEnumerable<char>> act = () => MethodThatUsesYield("aaa!aaa");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Enumerating().ShouldThrow<Exception>();
        }

        private static IEnumerable<char> MethodThatUsesYield(string bar)
        {
            foreach (var character in bar)
            {
                if (character.Equals('!'))
                {
                    throw new Exception("No exclamation marks allowed.");
                }

                yield return char.ToUpper(character);
            }
        }

        [Fact]
        public void When_custom_condition_is_not_met_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => { throw new ArgumentException(""); };

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                act
                    .ShouldThrow<ArgumentException>("")
                    .Where(e => e.Message.Length > 0, "an exception must have a message");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException exc)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                exc.Message.Should().StartWith(
                    "Expected exception where (e.Message.Length > 0) because an exception must have a message, but the condition was not met");
            }
        }

        [Fact]
        public void When_a_2nd_condition_is_not_met_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => { throw new ArgumentException("Fail"); };

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                act
                    .ShouldThrow<ArgumentException>("")
                    .Where(e => e.Message.Length > 0)
                    .Where(e => e.Message == "Error");

                throw new XunitException("This point should not be reached");
            }
            catch (XunitException exc)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                exc.Message.Should().StartWith(
                    "Expected exception where (e.Message == \"Error\"), but the condition was not met");
            }
            catch (Exception exc)
            {
                exc.Message.Should().StartWith(
                    "Expected exception where (e.Message == \"Error\"), but the condition was not met");

            }
        }

        [Fact]
        public void When_custom_condition_is_met_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => { throw new ArgumentException(""); };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<ArgumentException>()
                .Where(e => e.Message.Length == 0);
        }

        [Fact]
        public void
            When_two_exceptions_are_thrown_and_the_assertion_assumes_there_can_only_be_one_it_should_fail
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do())
                .Throws(new AggregateException(new Exception(), new Exception()));
            Action throwingMethod = testSubject.Do;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => throwingMethod.ShouldThrow<Exception>().And.Message.Should();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action.ShouldThrow<Exception>();
        }

        [Fact]
        public void When_an_exception_of_a_different_type_is_thrown_it_should_include_the_type_of_the_thrown_exception()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Action throwException = () => { throw new ExceptionWithEmptyToString(); };

            //-----------------------------------------------------------------------------------------------------------
            // Arrange / Act
            //-----------------------------------------------------------------------------------------------------------
            Action act =
                () => throwException.ShouldThrow<NullReferenceException>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<XunitException>()
                .WithMessage(
                    string.Format("*System.NullReferenceException*{0}*",
                        typeof (ExceptionWithEmptyToString)));
        }

        #endregion

        #region Not Throw

        [Fact]
        public void When_a_specific_exception_should_not_be_thrown_but_it_was_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var foo = A.Fake<IFoo>();
            A.CallTo(() => foo.Do()).Throws(new ArgumentException("An exception was forced"));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action =
                () => foo.Invoking(f => f.Do()).ShouldNotThrow<ArgumentException>("we passed valid arguments");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>().WithMessage(
                    "Did not expect System.ArgumentException because we passed valid arguments, " +
                        "but found*with message \"An exception was forced\"*");
        }

        [Fact]
        public void When_a_specific_exception_should_not_be_thrown_but_another_was_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var foo = A.Fake<IFoo>();
            A.CallTo(() => foo.Do()).Throws(new ArgumentException());

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            foo.Invoking(f => f.Do()).ShouldNotThrow<InvalidOperationException>();
        }

        [Fact]
        public void When_no_exception_should_be_thrown_but_it_was_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var foo = A.Fake<IFoo>();
            A.CallTo(() => foo.Do()).Throws(new ArgumentException("An exception was forced"));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action action = () => foo.Invoking(f => f.Do()).ShouldNotThrow("we passed valid arguments");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            action
                .ShouldThrow<XunitException>().WithMessage(
                    "Did not expect any exception because we passed valid arguments, " +
                        "but found System.ArgumentException with message \"An exception was forced\"*");
        }

        [Fact]
        public void When_no_exception_should_be_thrown_and_none_was_it_should_not_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var foo = A.Fake<IFoo>();

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            foo.Invoking(f => f.Do()).ShouldNotThrow();
        }
    }

    #endregion

    public class SomeTestClass
    {
        internal const string ExceptionMessage = "someMessage";

        public IList<string> Strings = new List<string>();

        public void Throw()
        {
            throw new ArgumentException(ExceptionMessage);
        }
    }

    public interface IFoo
    {
        void Do();

        void Do(string someParam);
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
}


#endif
