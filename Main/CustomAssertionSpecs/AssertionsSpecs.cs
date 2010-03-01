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
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }        
        
        [TestMethod]
        public void When_reason_includes_no_because_it_should_be_added()
        {
            var assertions = new AssertionsTestSubClass();

            assertions
                .ShouldThrow(x => x.AssertFail("{0} should always fail.", typeof(AssertionsTestSubClass).Name))
                .WithMessage("Expected it to fail because AssertionsTestSubClass should always fail.");
        }

        internal class AssertionsTestSubClass : CustomAssertionExtensions.Assertions
        {
            public void AssertFail(string reason, params object[] reasonParameters)
            {
                AssertThat(false, "Expected it to fail{2}", null, null, reason, reasonParameters);
            }
        }
    }
}


