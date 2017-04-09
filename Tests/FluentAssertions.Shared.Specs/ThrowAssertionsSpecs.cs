using System;
using Xunit;
using Xunit.Sdk;
#if FAKES
using FakeItEasy;
#endif

namespace FluentAssertions.Specs
{
    
    public class ThrowAssertionsSpecs
    {
#if FAKES
        [Fact]
        public void When_subject_throws_expected_exception_it_should_not_do_anything()
        {
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException());

            testSubject.Invoking(x => x.Do()).ShouldThrow<InvalidOperationException>();
        }
#endif

        [Fact]
        public void When_action_throws_expected_exception_it_should_not_do_anything()
        {
            var act = new Action(() => { throw new InvalidOperationException("Some exception"); });

            act.ShouldThrow<InvalidOperationException>();
        }

#if FAKES
        [Fact]
        public void When_subject_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = A.Fake<IFoo>();

                testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>();

                throw new XunitException("ShouldThrow() dit not throw");
            }
            catch (XunitException ex)
            {
                ex.Message.Should().Be(
                    "Expected a <System.Exception> to be thrown, but no exception was thrown.");
            }
        }
#endif

        [Fact]
        public void When_action_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
        {
            try
            {
                var act = new Action(() => { });

                act.ShouldThrow<Exception>();

                throw new XunitException("ShouldThrow() dit not throw");
            }
            catch (XunitException ex)
            {
                ex.Message.Should().Be(
                    "Expected a <System.Exception> to be thrown, but no exception was thrown.");
            }
        }
    }
}
