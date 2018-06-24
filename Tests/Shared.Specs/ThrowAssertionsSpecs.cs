using System;
using Xunit;
using Xunit.Sdk;

using FakeItEasy;

namespace FluentAssertions.Specs
{
    public class ThrowAssertionsSpecs
    {
        [Fact]
        public void When_subject_throws_expected_exception_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException());

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            testSubject.Invoking(x => x.Do()).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_action_throws_expected_exception_it_should_not_do_anything()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var act = new Action(() => throw new InvalidOperationException("Some exception"));

            //-----------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-----------------------------------------------------------------------------------------------------------
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_subject_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = A.Fake<IFoo>();

                testSubject.Invoking(x => x.Do()).Should().Throw<Exception>();

                throw new XunitException("Should().Throw() did not throw");
            }
            catch (XunitException ex)
            {
                ex.Message.Should().Be(
                    "Expected a <System.Exception> to be thrown, but no exception was thrown.");
            }
        }

        [Fact]
        public void When_action_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
        {
            try
            {
                var act = new Action(() => { });

                act.Should().Throw<Exception>();

                throw new XunitException("Should().Throw() did not throw");
            }
            catch (XunitException ex)
            {
                ex.Message.Should().Be(
                    "Expected a <System.Exception> to be thrown, but no exception was thrown.");
            }
        }
    }
}
