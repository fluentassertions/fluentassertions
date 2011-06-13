using System;
using System.Collections.Generic;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class ExceptionAssertionSpecs
    {
        #region Outer Exceptions

        [TestMethod]
        public void When_subject_throws_expected_exception_with_an_expected_message_it_should_not_do_anything()
        {
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException("some message"));

            testSubject.Invoking(x => x.Do()).ShouldThrow<InvalidOperationException>().WithMessage("some message");
        }

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Be(
                    "Expected exception with message \r\n\"some message\", but \r\n\"some\" is too short.");
            }
        }

        [TestMethod]
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
                    .WithMessage("message2");

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                Assert.AreEqual(
                    "Expected exception with message \r\n\"message2\", but \r\n\"message1\" differs near \"1\" (index 7).",
                    ex.Message);
            }
        }

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                Assert.AreEqual(
                    "Expected exception with message \r\n\"message2\", but message was empty.",
                    ex.Message);
            }
        }

        [TestMethod]
        public void
            When_subject_throws_some_exception_with_message_which_contains_complete_expected_exception_and_more_it_should_throw
            ()
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                Assert.AreEqual(
                    "Expected exception with message \r\n\"message2\", but \r\n\"message2\\r\\nParameter name: someParam\" differs near \"\\r\\nP\" (index 8).",
                    ex.Message);
            }
        }

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Be(
                    "Expected System.Exception because IFoo.Do should do that, but no exception was thrown.");
            }
        }

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().StartWith(
                    "Expected System.InvalidOperationException because IFoo.Do should throw that one, but found System.ArgumentException");

                ex.Message.Should().Contain(actualException.Message);
                ex.Message.Should().Contain(actualException.StackTrace);
            }
        }

        #endregion

        #region Inner Exceptions

        [TestMethod]
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

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException exc)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                exc.Message.Should().StartWith(
                    "Expected inner System.ArgumentException because IFoo.Do should do just that, but found System.NullReferenceException");

                exc.Message.Should().Contain(innerException.Message);
            }
        }

        [TestMethod]
        public void When_subject_throws_an_exception_without_expected_inner_exception_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = A.Fake<IFoo>();
                A.CallTo(() => testSubject.Do()).Throws(new Exception(""));

                testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>()
                    .WithInnerException<InvalidOperationException>();

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner System.InvalidOperationException, but the thrown exception has no inner exception.");
            }
        }

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Be(
                    "Expected inner System.InvalidOperationException because IFoo.Do should do that, but the thrown exception has no inner exception.");
            }
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_expected_message_it_should_not_do_anything()
        {
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("", new InvalidOperationException("expected message")));

            testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>()
                .WithInnerMessage("expected message");
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_unexpected_message_it_should_throw_with_clear_description()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("",
                new InvalidOperationException("unexpected message")));

            try
            {
                //-----------------------------------------------------------------------------------------------------------
                // Act
                //-----------------------------------------------------------------------------------------------------------
                testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>()
                    .WithInnerMessage("expected message");

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Be(
                    "Expected inner exception with message \r\n\"expected message\", but \r\n\"unexpected message\" differs near \"une\" (index 0).");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_inner_exception_with_unexpected_message_it_should_throw_with_clear_description_and_reason
            ()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new Exception("",
                new InvalidOperationException("unexpected message")));

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            try
            {
                testSubject
                    .Invoking(x => x.Do())
                    .ShouldThrow<Exception>()
                    .WithInnerMessage("expected message", "because {0} should do just that", "IFoo.Do");

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException ex)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                ex.Message.Should().Be(
                    "Expected inner exception with message \r\n\"expected message\" because IFoo.Do should do just that, but \r\n\"unexpected message\" differs near \"une\" (index 0).");
            }
        }

        #endregion

        #region Miscellaneous

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException exc)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                exc.Message.Should().StartWith(
                    "Expected exception where (e.Message.Length > 0) because an exception must have a message, but the condition was not met");
            }
        }

        [TestMethod]
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

                Assert.Fail("This point should not be reached");
            }
            catch (AssertFailedException exc)
            {
                //-----------------------------------------------------------------------------------------------------------
                // Assert
                //-----------------------------------------------------------------------------------------------------------
                exc.Message.Should().StartWith(
                    "Expected exception where (e.Message == \"Error\"), but the condition was not met");
            }
        }

        [TestMethod]
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

        #endregion

        #region Not Throw

        [TestMethod]
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
                .ShouldThrow<AssertFailedException>().WithMessage(
                    "Did not expect System.ArgumentException because we passed valid arguments, " +
                        "but found one with message \"An exception was forced\".");
        }

        [TestMethod]
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

        [TestMethod]
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
                .ShouldThrow<AssertFailedException>().WithMessage(
                    "Did not expect any exception because we passed valid arguments, " +
                        "but found a System.ArgumentException with message \"An exception was forced\".");
        }

        [TestMethod]
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
}