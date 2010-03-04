using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class AssertionsSpecs
    {
        [TestMethod]
        public void When_reason_starts_with_because_it_should_not_do_anything()
        {
            var assertions = new AssertionsTestSubClass();

            assertions
                .ShouldThrow(x => x.AssertFail("because {0} should always fail.", typeof(AssertionsTestSubClass).Name))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }        
        
        [TestMethod]
        public void When_reason_includes_no_because_it_should_be_added()
        {
            var assertions = new AssertionsTestSubClass();

            assertions
                .ShouldThrow(x => x.AssertFail("{0} should always fail.", typeof(AssertionsTestSubClass).Name))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_satisfies_predicate_which_is_satisfied()
        {
            var someObject = new object();

            someObject.Should().Satisfy(o => (o != null));
        }

        [TestMethod]
        public void Should_fail_when_asserting_object_satisfies_predicate_which_is_not_statisfied()
        {
            var someObject = new object();
            var assertions = someObject.Should();

            assertions.ShouldThrow(x => x.Satisfy(y => (y == null), "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .And.WithMessage("Expected to satisfy predicate because we want to test the failure message, " +
                             "but predicate not satisfied by System.Object");
        }

        internal class AssertionsTestSubClass : FluentAssertionExtensions.Assertions<object,AssertionsTestSubClass>
        {
            public void AssertFail(string reason, params object[] reasonParameters)
            {
                VerifyThat(false, "Expected it to fail{2}", null, null, reason, reasonParameters);
            }
        }
    }
}