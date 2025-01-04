using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Chill;
using FluentAssertions.Equivalency;
using FluentAssertions.Equivalency.Steps;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Configuration;

public class EquivalencyOptionsSpecs
{
    [Collection("ConfigurationSpecs")]
    public abstract class Given_temporary_global_assertion_options : GivenWhenThen
    {
        protected override void Dispose(bool disposing)
        {
            AssertionConfiguration.Current.Equivalency.Modify(_ => new EquivalencyOptions());

            base.Dispose(disposing);
        }
    }

    [Collection("ConfigurationSpecs")]
    public class When_injecting_a_null_configurer : GivenSubject<EquivalencyOptions, Action>
    {
        public When_injecting_a_null_configurer()
        {
            When(() => () => AssertionConfiguration.Current.Equivalency.Modify(configureOptions: null));
        }

        [Fact]
        public void It_should_throw()
        {
            Result.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("configureOptions");
        }
    }

    [Collection("ConfigurationSpecs")]

    public class When_concurrently_getting_equality_strategy : GivenSubject<EquivalencyOptions, Action>
    {
        public When_concurrently_getting_equality_strategy()
        {
            When(() =>
            {
#pragma warning disable CA1859 // https://github.com/dotnet/roslyn-analyzers/issues/6704
                IEquivalencyOptions equivalencyOptions = new EquivalencyOptions();
#pragma warning restore CA1859

                return () => Parallel.For(0, 10_000, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                    _ => equivalencyOptions.GetEqualityStrategy(typeof(IEnumerable))
                );
            });
        }

        [Fact]
        public void It_should_not_throw()
        {
            Result.Should().NotThrow();
        }
    }

    public class When_modifying_global_reference_type_settings_a_previous_assertion_should_not_have_any_effect
        : Given_temporary_global_assertion_options
    {
        public When_modifying_global_reference_type_settings_a_previous_assertion_should_not_have_any_effect()
        {
            Given(() =>
            {
                // Trigger a first equivalency check using the default global settings
                new MyValueType { Value = 1 }.Should().BeEquivalentTo(new MyValueType { Value = 2 });
            });

            When(() => AssertionConfiguration.Current.Equivalency.Modify(o => o.ComparingByMembers<MyValueType>()));
        }

        [Fact]
        public void It_should_try_to_compare_the_classes_by_member_semantics_and_thus_throw()
        {
            Action act = () => new MyValueType { Value = 1 }.Should().BeEquivalentTo(new MyValueType { Value = 2 });

            act.Should().Throw<XunitException>();
        }

        internal class MyValueType
        {
            public int Value { get; set; }

            public override bool Equals(object obj) => true;

            public override int GetHashCode() => 0;
        }
    }

    public class When_modifying_global_value_type_settings_a_previous_assertion_should_not_have_any_effect
        : Given_temporary_global_assertion_options
    {
        public When_modifying_global_value_type_settings_a_previous_assertion_should_not_have_any_effect()
        {
            Given(() =>
            {
                // Trigger a first equivalency check using the default global settings
                new MyClass { Value = 1 }.Should().BeEquivalentTo(new MyClass { Value = 1 });
            });

            When(() => AssertionConfiguration.Current.Equivalency.Modify(o => o.ComparingByValue<MyClass>()));
        }

        [Fact]
        public void It_should_try_to_compare_the_classes_by_value_semantics_and_thus_throw()
        {
            MyClass myClass = new() { Value = 1 };

            Action act = () => myClass.Should().BeEquivalentTo(new MyClass { Value = 1 });

            act.Should().Throw<XunitException>();
        }

        internal class MyClass
        {
            public int Value { get; set; }
        }
    }

    public class When_modifying_record_settings_globally : Given_temporary_global_assertion_options
    {
        public When_modifying_record_settings_globally()
        {
            When(() =>
            {
                AssertionConfiguration.Current.Equivalency.Modify(
                    options => options.ComparingByValue(typeof(Position)));
            });
        }

        [Fact]
        public void It_should_use_the_global_settings_for_comparing_records()
        {
            new Position(123).Should().BeEquivalentTo(new Position(123));
        }

        private record Position
        {
            [UsedImplicitly]
            private readonly int value;

            public Position(int value)
            {
                this.value = value;
            }
        }
    }

    public class When_assertion_doubles_should_always_allow_small_deviations : Given_temporary_global_assertion_options
    {
        public When_assertion_doubles_should_always_allow_small_deviations()
        {
            When(() =>
            {
                AssertionConfiguration.Current.Equivalency.Modify(options => options
                    .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01))
                    .WhenTypeIs<double>());
            });
        }

        [Fact]
        public void Then_it_should_ignore_small_differences_without_the_need_of_local_options()
        {
            var actual = new
            {
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            Action act = () => actual.Should().BeEquivalentTo(expected);

            act.Should().NotThrow();
        }
    }

    public class When_local_similar_options_are_used : Given_temporary_global_assertion_options
    {
        public When_local_similar_options_are_used()
        {
            When(() =>
            {
                AssertionConfiguration.Current.Equivalency.Modify(options => options
                    .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01))
                    .WhenTypeIs<double>());
            });
        }

        [Fact]
        public void Then_they_should_override_the_global_options()
        {
            var actual = new
            {
                Value = 1D / 3D
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
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            Action act = () => actual.Should().BeEquivalentTo(expected);

            act.Should().NotThrow();
        }
    }

    [Collection("ConfigurationSpecs")]
    public class Given_self_resetting_equivalency_plan : GivenWhenThen
    {
        protected override void Dispose(bool disposing)
        {
            Plan.Reset();
            base.Dispose(disposing);
        }

        protected static EquivalencyPlan Plan
        {
            get { return AssertionConfiguration.Current.Equivalency.Plan; }
        }
    }

    public class When_inserting_a_step : Given_self_resetting_equivalency_plan
    {
        public When_inserting_a_step()
        {
            When(() => Plan.Insert<MyEquivalencyStep>());
        }

        [Fact]
        public void Then_it_should_precede_all_other_steps()
        {
            var addedStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);

            Plan.Should().StartWith(addedStep);
        }
    }

    public class When_inserting_a_step_before_another : Given_self_resetting_equivalency_plan
    {
        public When_inserting_a_step_before_another()
        {
            When(() => Plan.InsertBefore<DictionaryEquivalencyStep, MyEquivalencyStep>());
        }

        [Fact]
        public void Then_it_should_precede_that_particular_step()
        {
            var addedStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);
            var successor = Plan.LastOrDefault(s => s is DictionaryEquivalencyStep);

            Plan.Should().HaveElementPreceding(successor, addedStep);
        }
    }

    public class When_appending_a_step : Given_self_resetting_equivalency_plan
    {
        public When_appending_a_step()
        {
            When(() => Plan.Add<MyEquivalencyStep>());
        }

        [Fact]
        public void Then_it_should_precede_the_final_builtin_step()
        {
            var equivalencyStep = Plan.LastOrDefault(s => s is SimpleEqualityEquivalencyStep);
            var subjectStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);

            Plan.Should().HaveElementPreceding(equivalencyStep, subjectStep);
        }
    }

    public class When_appending_a_step_after_another : Given_self_resetting_equivalency_plan
    {
        public When_appending_a_step_after_another()
        {
            When(() => Plan.AddAfter<DictionaryEquivalencyStep, MyEquivalencyStep>());
        }

        [Fact]
        public void Then_it_should_precede_the_final_builtin_step()
        {
            var addedStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);
            var predecessor = Plan.LastOrDefault(s => s is DictionaryEquivalencyStep);

            Plan.Should().HaveElementSucceeding(predecessor, addedStep);
        }
    }

    public class When_appending_a_step_and_no_builtin_steps_are_there : Given_self_resetting_equivalency_plan
    {
        public When_appending_a_step_and_no_builtin_steps_are_there()
        {
            When(() =>
            {
                Plan.Clear();
                Plan.Add<MyEquivalencyStep>();
            });
        }

        [Fact]
        public void Then_it_should_precede_the_simple_equality_step()
        {
            var subjectStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);

            Plan.Should().EndWith(subjectStep);
        }
    }

    public class When_removing_a_specific_step : Given_self_resetting_equivalency_plan
    {
        public When_removing_a_specific_step()
        {
            When(() => Plan.Remove<SimpleEqualityEquivalencyStep>());
        }

        [Fact]
        public void Then_it_should_precede_the_simple_equality_step()
        {
            Plan.Should().NotContain(s => s is SimpleEqualityEquivalencyStep);
        }
    }

    public class When_removing_a_specific_step_that_doesnt_exist : Given_self_resetting_equivalency_plan
    {
        public When_removing_a_specific_step_that_doesnt_exist()
        {
            WhenAction = () => Plan.Remove<MyEquivalencyStep>();
        }

        [Fact]
        public void Then_it_should_precede_the_simple_equality_step()
        {
            WhenAction.Should().NotThrow();
        }
    }

    private class MyEquivalencyStep : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
            IValidateChildNodeEquivalency valueChildNodes)
        {
            AssertionChain.GetOrCreate().For(context).FailWith(GetType().FullName);

            return EquivalencyResult.EquivalencyProven;
        }
    }
}
