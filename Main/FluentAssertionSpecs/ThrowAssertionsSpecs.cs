using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace FluentAssertions.specs
{
    [TestClass]
    public class ThrowAssertionsSpecs
    {
        [TestMethod]
        public void When_subject_throws_expected_exception_it_should_not_do_anything()
        {
            IFoo testSubject = MockRepository.GenerateStub<IFoo>();
            testSubject.Stub(x => x.Do()).Throw(new InvalidOperationException());

            testSubject.ShouldThrow(x => x.Do()).Exception<InvalidOperationException>();
        }

        [TestMethod]
        public void When_action_throws_expected_exception_it_should_not_do_anything()
        {
            var act = new Action(() => { throw new InvalidOperationException("Some exception"); });

            act.ShouldThrow().Exception<InvalidOperationException>();
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
            catch (SpecificationMismatchException ex)
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

                act.ShouldThrow().Exception<Exception>();

                Assert.Fail("ShouldThrow() dit not throw");
            }
            catch (SpecificationMismatchException ex)
            {
                ex.Message.Should().Be(
                    "Expected <System.Exception>, but no exception was thrown.");
            }
        }
    }
}
