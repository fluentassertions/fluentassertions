using System;
using Chill;
using FluentAssertions.Equivalency;
#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endif

namespace FluentAssertions.Specs
{
    namespace AssertionOptionsSpecs
    {
        [TestClass]
        public class When_assertion_doubles_should_always_allow_small_deviations : Given_temporary_global_assertion_options
        {
            public When_assertion_doubles_should_always_allow_small_deviations()
            {
                When(() =>
                {
                    AssertionOptions.AssertEquivalencyUsing(options => options
                        .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01))
                        .WhenTypeIs<double>());
                });
            }

            [TestMethod]
            public void Then_it_should_ignore_small_differences_without_the_need_of_local_options()
            {
                var actual = new
                {
                    Value = (1D / 3D)
                };

                var expected = new
                {
                    Value = 0.33D
                };

                Action act = () => actual.ShouldBeEquivalentTo(expected);

                act.ShouldNotThrow();
            }


        }

        [TestClass]
        public class When_local_similar_options_are_used : Given_temporary_global_assertion_options
        {
            public When_local_similar_options_are_used()
            {
                When(() =>
                {
                    AssertionOptions.AssertEquivalencyUsing(options => options
                        .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01))
                        .WhenTypeIs<double>());
                });
            }

            [TestMethod]
            public void Then_they_should_override_the_global_options()
            {
                var actual = new
                {
                    Value = (1D/3D)
                };

                var expected = new
                {
                    Value = 0.33D
                };

                Action act = () => actual.ShouldBeEquivalentTo(expected, options => options
                    .Using<double>(ctx => ctx.Subject.Should().Be(ctx.Expectation))
                    .WhenTypeIs<double>());

                act.ShouldThrow<AssertFailedException>().WithMessage("Expected*");
            }
            
            [TestMethod]
            public void Then_they_should_not_affect_any_other_assertions()
            {
                var actual = new
                {
                    Value = (1D/3D)
                };

                var expected = new
                {
                    Value = 0.33D
                };

                Action act = () => actual.ShouldBeEquivalentTo(expected);

                act.ShouldNotThrow();
            }
        }

        public class Given_temporary_global_assertion_options : GivenWhenThen
        {
            protected override void Dispose(bool disposing)
            {
                AssertionOptions.AssertEquivalencyUsing(options => new EquivalencyAssertionOptions());
                base.Dispose(disposing);
            }
        }
    }
}
