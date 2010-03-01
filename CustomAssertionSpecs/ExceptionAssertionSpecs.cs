using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FluentAssertions.specs
{
    [TestClass]
    public class ExceptionAssertionSpecs
    {
        [TestMethod]
        public void When_subject_throws_expected_exception_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new InvalidOperationException());

            testSubject.ShouldThrow(x => x.Do()).Exception<InvalidOperationException>();
        }

        [TestMethod]
        public void When_subject_throws_expected_exception_with_an_expected_message_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new InvalidOperationException("some message"));

            testSubject.ShouldThrow(x => x.Do()).WithMessage("some message");
        }

        [TestMethod]
        [ExpectedException(typeof (AssertFailedException))]
        public void When_subject_throws_expected_exception_but_with_unexpected_message_it_should_throw()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new InvalidOperationException("unexpected message"));

            testSubject.ShouldThrow(x => x.Do()).WithMessage("expected message");
        }

        [TestMethod]
        public void When_subject_throws_some_exception_with_unexpected_message_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new InvalidOperationException("unexpected message"));

                testSubject.ShouldThrow(x => x.Do()).WithMessage("expected message");

                Assert.Fail("ShouldThrow() did not detect the wrong exception message");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected exception with message <expected message>, but found <unexpected message>.");
            }
        }

        [TestMethod]
        public void When_subject_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>();

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected exception <System.Exception>, but no exception was thrown.");
            }
        }

        [TestMethod]
        public void
            When_subject_does_not_throw_exception_but_one_was_expected_with_reason_it_should_throw_with_clear_description_and_reason
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();

                testSubject.ShouldThrow(x => x.Do()).Exception<Exception>("because {0} should do that", "IFoo.Do");

                Assert.Fail("An exception should have been thrown");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected exception <System.Exception> because IFoo.Do should do that, but no exception was thrown.");
            }
        }

        [TestMethod]
        public void
            When_subject_does_not_throw_exception_but_one_was_expected_with_specific_message_it_should_throw_with_clear_description
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithMessage("expected message", "because we expected {0} to throw", "IFoo.Do");

                Assert.Fail("An exception should have been thrown");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected exception with message <expected message> because we expected IFoo.Do to throw, but no exception was thrown.");
            }
        }

        [TestMethod]
        public void When_subject_throws_another_exception_than_expected_it_should_throw_with_clear_description()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new ArgumentException());

            try
            {
                testSubject
                    .ShouldThrow(x => x.Do())
                    .Exception<InvalidOperationException>();

                Assert.Fail("ShouldThrow() dit not detect the wrong exception type");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected exception <System.InvalidOperationException>, but found <System.ArgumentException>.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_another_exception_than_expected_it_should_throw_with_clear_description_and_reason()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new ArgumentException());

            try
            {
                testSubject
                    .ShouldThrow(x => x.Do())
                    .Exception<InvalidOperationException>("because {0} should throw that one", "IFoo.Do");

                Assert.Fail("ShouldThrow() dit not detect the wrong exception type");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected exception <System.InvalidOperationException> because IFoo.Do should throw that one, but found <System.ArgumentException>.");
            }
        }

        [TestMethod]
        public void When_subject_throws_an_exception_with_the_expected_innerexception_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new Exception("", new ArgumentException()));

            testSubject
                .ShouldThrow(x => x.Do())
                .WithInnerException<ArgumentException>();
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_with_an_unexpected_innerexception_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new NullReferenceException()));

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerException<ArgumentException>();

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception <System.ArgumentException>, but found <System.NullReferenceException>.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_with_an_unexpected_innerexception_it_should_throw_with_clear_description_reason
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new NullReferenceException()));

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerException<ArgumentException>("because {0} should do just that", "IFoo.Do");

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception <System.ArgumentException> because IFoo.Do should do just that, but found <System.NullReferenceException>.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_without_expected_inner_exception_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception(""));

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerException<InvalidOperationException>();

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception <System.InvalidOperationException>, but the thrown exception has no inner exception.");
            }
        }

        [TestMethod]
        public void
            When_subject_throws_an_exception_without_expected_inner_exception_and_has_reason_it_should_throw_with_clear_description
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception(""));

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerException<InvalidOperationException>("because {0} should do that", "IFoo.Do");

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception <System.InvalidOperationException> because IFoo.Do should do that, but the thrown exception has no inner exception.");
            }
        }

        [TestMethod]
        public void
            When_subject_does_not_throw_at_all_and_inner_exception_is_expected_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerException<ArgumentException>();

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception <System.ArgumentException>, but no exception was thrown.");
            }
        }

        [TestMethod]
        public void
            When_subject_does_not_throw_at_all_and_inner_exception_is_expected_with_reason_it_should_throw_with_clear_description
            ()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerException<ArgumentException>("because {0} should do that", "IFoo.Do");

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception <System.ArgumentException> because IFoo.Do should do that, but no exception was thrown.");
            }
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_expected_message_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new Exception("", new InvalidOperationException("expected message")));

            testSubject
                .ShouldThrow(x => x.Do())
                .WithInnerMessage("expected message");
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_unexpected_message_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new InvalidOperationException("unexpected message")));

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerMessage("expected message");

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception with message <expected message>, but found <unexpected message>.");
            }
        }

        [TestMethod]
        public void When_subject_throws_inner_exception_with_unexpected_message_it_should_throw_with_clear_description_and_reason()
        {
            try
            {
                IFoo testSubject = MockRepository.GenerateStub<IFoo>();
                testSubject.Stub(x => x.Do()).Throw(new Exception("", new InvalidOperationException("unexpected message")));

                testSubject
                    .ShouldThrow(x => x.Do())
                    .WithInnerMessage("expected message", "because {0} should do just that", "IFoo.Do");

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Equal(
                    "Expected inner exception with message <expected message> because IFoo.Do should do just that, but found <unexpected message>.");
            }
        }

        [TestMethod]
        public void When_validating_a_subject_against_multiple_conditions_it_should_support_chaining()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(
                new InvalidOperationException("message", new ArgumentException("inner message")));

            testSubject
                .ShouldThrow(x => x.Do())
                .Exception<InvalidOperationException>().And
                .WithInnerMessage("inner message").And
                .WithInnerException<ArgumentException>().And
                .WithInnerMessage("inner message");
        }
    }

    public interface IFoo
    {
        void Do();
    }
}