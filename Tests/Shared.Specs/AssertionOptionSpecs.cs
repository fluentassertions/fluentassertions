using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Chill;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs
{
    namespace AssertionOptionsSpecs
    {
        public class AssertionOptionsSpecs
        {
            [Fact]
            public void When_injecting_a_null_configurer_it_should_throw()
            {
                // Arrange
                Action act = () => AssertionOptions.AssertEquivalencyUsing(defaultsConfigurer: null);

                // Assert
                act.Should().ThrowExactly<ArgumentNullException>()
                    .Which.ParamName.Should().Be("defaultsConfigurer");
            }
        }

        // Due to tests that call AssertionOptions
        [CollectionDefinition("AssertionOptionsSpecs", DisableParallelization = true)]
        public class AssertionOptionsSpecsDefinition { }

        [Collection("AssertionOptionsSpecs")]
        public class When_concurrently_getting_equality_strategy : GivenSubject<EquivalencyAssertionOptions, Action>
        {
            public When_concurrently_getting_equality_strategy()
            {
                When(() =>
                {
                    IEquivalencyAssertionOptions equivalencyAssertionOptions = new EquivalencyAssertionOptions();
                    return () => Parallel.For(0, 10_000, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                        e => equivalencyAssertionOptions.GetEqualityStrategy(typeof(IEnumerable))
                    );
                });
            }

            [Fact]
            public void It_should_not_throw()
            {
                Result.Should().NotThrow();
            }
        }

        public abstract class Given_temporary_global_assertion_options : GivenWhenThen
        {
            protected override void Dispose(bool disposing)
            {
                AssertionOptions.AssertEquivalencyUsing(options => new EquivalencyAssertionOptions());

                base.Dispose(disposing);
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_assertion_doubles_should_always_allow_small_deviations :
            Given_temporary_global_assertion_options
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

            [Fact]
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

                Action act = () => actual.Should().BeEquivalentTo(expected);

                act.Should().NotThrow();
            }
        }

        [Collection("AssertionOptionsSpecs")]
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

            [Fact]
            public void Then_they_should_override_the_global_options()
            {
                var actual = new
                {
                    Value = (1D / 3D)
                };

                var expected = new
                {
                    Value = 0.33D
                };

                Action act = () => actual.Should().BeEquivalentTo(expected, options => options
                    .Using<double>(ctx => ctx.Subject.Should().Be(ctx.Expectation))
                    .WhenTypeIs<double>());

                act.Should().Throw<XunitException>().WithMessage("Expected*");
            }

            [Fact]
            public void Then_they_should_not_affect_any_other_assertions()
            {
                var actual = new
                {
                    Value = (1D / 3D)
                };

                var expected = new
                {
                    Value = 0.33D
                };

                Action act = () => actual.Should().BeEquivalentTo(expected);

                act.Should().NotThrow();
            }
        }

        public class Given_temporary_equivalency_steps : GivenWhenThen
        {
            protected override void Dispose(bool disposing)
            {
                Steps.Reset();
                base.Dispose(disposing);
            }

            protected static EquivalencyStepCollection Steps
            {
                get { return AssertionOptions.EquivalencySteps; }
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_inserting_a_step : Given_temporary_equivalency_steps
        {
            public When_inserting_a_step()
            {
                When(() => Steps.Insert<MyEquivalencyStep>());
            }

            [Fact]
            public void Then_it_should_precede_all_other_steps()
            {
                var addedStep = Steps.LastOrDefault(s => s is MyEquivalencyStep);

                Steps.Should().StartWith(addedStep);
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_inserting_a_step_before_another : Given_temporary_equivalency_steps
        {
            public When_inserting_a_step_before_another()
            {
                When(() => Steps.InsertBefore<DictionaryEquivalencyStep, MyEquivalencyStep>());
            }

            [Fact]
            public void Then_it_should_precede_that_particular_step()
            {
                var addedStep = Steps.LastOrDefault(s => s is MyEquivalencyStep);
                var successor = Steps.LastOrDefault(s => s is DictionaryEquivalencyStep);

                Steps.Should().HaveElementPreceding(successor, addedStep);
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_appending_a_step : Given_temporary_equivalency_steps
        {
            public When_appending_a_step()
            {
                When(() => Steps.Add<MyEquivalencyStep>());
            }

            [Fact]
            public void Then_it_should_precede_the_final_builtin_step()
            {
                var equivalencyStep = Steps.LastOrDefault(s => s is SimpleEqualityEquivalencyStep);
                var subjectStep = Steps.LastOrDefault(s => s is MyEquivalencyStep);

                Steps.Should().HaveElementPreceding(equivalencyStep, subjectStep);
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_appending_a_step_after_another : Given_temporary_equivalency_steps
        {
            public When_appending_a_step_after_another()
            {
                When(() => Steps.AddAfter<DictionaryEquivalencyStep, MyEquivalencyStep>());
            }

            [Fact]
            public void Then_it_should_precede_the_final_builtin_step()
            {
                var addedStep = Steps.LastOrDefault(s => s is MyEquivalencyStep);
                var predecessor = Steps.LastOrDefault(s => s is DictionaryEquivalencyStep);

                Steps.Should().HaveElementSucceeding(predecessor, addedStep);
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_appending_a_step_and_no_builtin_steps_are_there : Given_temporary_equivalency_steps
        {
            public When_appending_a_step_and_no_builtin_steps_are_there()
            {
                When(() =>
                {
                    Steps.Clear();
                    Steps.Add<MyEquivalencyStep>();
                });
            }

            [Fact]
            public void Then_it_should_precede_the_simple_equality_step()
            {
                var subjectStep = Steps.LastOrDefault(s => s is MyEquivalencyStep);

                Steps.Should().EndWith(subjectStep);
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_removing_a_specific_step : Given_temporary_equivalency_steps
        {
            public When_removing_a_specific_step()
            {
                When(() => Steps.Remove<SimpleEqualityEquivalencyStep>());
            }

            [Fact]
            public void Then_it_should_precede_the_simple_equality_step()
            {
                Steps.Should().NotContain(s => s is SimpleEqualityEquivalencyStep);
            }
        }

        [Collection("AssertionOptionsSpecs")]
        public class When_removing_a_specific_step_that_doesnt_exist : Given_temporary_equivalency_steps
        {
            public When_removing_a_specific_step_that_doesnt_exist()
            {
                WhenAction = () => Steps.Remove<MyEquivalencyStep>();
            }

            [Fact]
            public void Then_it_should_precede_the_simple_equality_step()
            {
                WhenAction.Should().NotThrow();
            }
        }

        internal class MyEquivalencyStep : IEquivalencyStep
        {
            public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
            {
                return true;
            }

            public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
                IEquivalencyAssertionOptions config)
            {
                Execute.Assertion.FailWith(GetType().FullName);

                return true;
            }
        }
    }
}
