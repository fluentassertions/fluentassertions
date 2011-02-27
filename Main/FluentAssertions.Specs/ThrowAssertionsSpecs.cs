using System;

using FakeItEasy;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class ThrowAssertionsSpecs
    {
        [TestMethod]
        public void When_subject_throws_expected_exception_it_should_not_do_anything()
        {
            IFoo testSubject = A.Fake<IFoo>();
            A.CallTo(() => testSubject.Do()).Throws(new InvalidOperationException());

            testSubject.Invoking(x => x.Do()).ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void When_action_throws_expected_exception_it_should_not_do_anything()
        {
            var act = new Action(() => { throw new InvalidOperationException("Some exception"); });

            act.ShouldThrow<InvalidOperationException>();
        }

        [TestMethod]
        public void When_subject_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
        {
            try
            {
                IFoo testSubject = A.Fake<IFoo>();

                testSubject.Invoking(x => x.Do()).ShouldThrow<Exception>();

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Be(
                    "Expected <System.Exception>, but no exception was thrown.");
            }
        }

        [TestMethod]
        public void When_action_does_not_throw_exception_but_one_was_expected_it_should_throw_with_clear_description()
        {
            try
            {
                var act = new Action(() => { });

                act.ShouldThrow<Exception>();

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (AssertFailedException ex)
            {
                ex.Message.Should().Be(
                    "Expected <System.Exception>, but no exception was thrown.");
            }
        }
    }
}
