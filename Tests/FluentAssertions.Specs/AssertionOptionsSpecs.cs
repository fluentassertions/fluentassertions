using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Chill;
using FluentAssertionsAsync.Equivalency;
using FluentAssertionsAsync.Equivalency.Steps;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Formatting;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs;

public class AssertionOptionsSpecs
{
    // Due to tests that call AssertionOptions
    [CollectionDefinition("AssertionOptionsSpecs", DisableParallelization = true)]
    public class AssertionOptionsSpecsDefinition
    {
    }

    public abstract class Given_temporary_global_assertion_options : GivenWhenThen
    {
        protected override void Dispose(bool disposing)
        {
            AssertionOptions.AssertEquivalencyUsing(_ => new EquivalencyOptions());

            base.Dispose(disposing);
        }
    }

    [Collection("AssertionOptionsSpecs")]
    public class When_injecting_a_null_configurer : GivenSubject<EquivalencyOptions, Func<Task>>
    {
        public When_injecting_a_null_configurer()
        {
            When(() => () =>
            {
                AssertionOptions.AssertEquivalencyUsing(defaultsConfigurer: null);
                return Task.CompletedTask;
            });
        }

        [Fact]
        public async Task It_should_throw()
        {
            (await Result.Should().ThrowExactlyAsync<ArgumentNullException>())
                .WithParameterName("defaultsConfigurer");
        }
    }

    [Collection("AssertionOptionsSpecs")]
    public class When_concurrently_getting_equality_strategy : GivenSubject<EquivalencyOptions, Func<Task>>
    {
        public When_concurrently_getting_equality_strategy()
        {
            When(() =>
            {
#pragma warning disable CA1859 // https://github.com/dotnet/roslyn-analyzers/issues/6704
                IEquivalencyOptions equivalencyOptions = new EquivalencyOptions();
#pragma warning restore CA1859

                return () => Task.FromResult(Parallel.For(0, 10_000, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                    _ => equivalencyOptions.GetEqualityStrategy(typeof(IEnumerable)))
                );
            });
        }

        [Fact]
        public async Task It_should_not_throw()
        {
            await Result.Should().NotThrowAsync();
        }
    }

    [Collection("AssertionOptionsSpecs")]
    public class When_modifying_global_reference_type_settings_a_previous_assertion_should_not_have_any_effect
        : Given_temporary_global_assertion_options
    {
        public When_modifying_global_reference_type_settings_a_previous_assertion_should_not_have_any_effect()
        {
            Given(async () =>
            {
                // Trigger a first equivalency check using the default global settings
                await new MyValueType { Value = 1 }.Should().BeEquivalentToAsync(new MyValueType { Value = 2 });
            });

            When(() => AssertionOptions.AssertEquivalencyUsing(o => o.ComparingByMembers<MyValueType>()));
        }

        [Fact]
        public async Task It_should_try_to_compare_the_classes_by_member_semantics_and_thus_throw()
        {
            Func<Task> act = () => new MyValueType { Value = 1 }.Should().BeEquivalentToAsync(new MyValueType { Value = 2 });

            await act.Should().ThrowAsync<XunitException>();
        }

        internal class MyValueType
        {
            public int Value { get; set; }

            public override bool Equals(object obj) => true;

            public override int GetHashCode() => 0;
        }
    }

    [Collection("AssertionOptionsSpecs")]
    public class When_modifying_global_value_type_settings_a_previous_assertion_should_not_have_any_effect
        : Given_temporary_global_assertion_options
    {
        public When_modifying_global_value_type_settings_a_previous_assertion_should_not_have_any_effect()
        {
            Given(async () =>
            {
                // Trigger a first equivalency check using the default global settings
                await new MyClass { Value = 1 }.Should().BeEquivalentToAsync(new MyClass { Value = 1 });
            });

            When(() => AssertionOptions.AssertEquivalencyUsing(o => o.ComparingByValue<MyClass>()));
        }

        [Fact]
        public async Task It_should_try_to_compare_the_classes_by_value_semantics_and_thus_throw()
        {
            MyClass myClass = new() { Value = 1 };

            Func<Task> act = () => myClass.Should().BeEquivalentToAsync(new MyClass { Value = 1 });

            await act.Should().ThrowAsync<XunitException>();
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
                AssertionOptions.AssertEquivalencyUsing(
                    options => options.ComparingByValue(typeof(Position)));
            });
        }

        [Fact]
        public async Task It_should_use_the_global_settings_for_comparing_records()
        {
            await new Position(123).Should().BeEquivalentToAsync(new Position(123));
        }

        private record Position
        {
            private readonly int value;

            public Position(int value)
            {
                this.value = value;
            }
        }
    }

    [Collection("AssertionOptionsSpecs")]
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

        [Fact]
        public async Task Then_it_should_ignore_small_differences_without_the_need_of_local_options()
        {
            var actual = new
            {
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            Func<Task> act = () => actual.Should().BeEquivalentToAsync(expected);

            await act.Should().NotThrowAsync();
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
        public async Task Then_they_should_override_the_global_options()
        {
            var actual = new
            {
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            Func<Task> act = () => actual.Should().BeEquivalentToAsync(expected, options => options
                .Using<double>(ctx => ctx.Subject.Should().Be(ctx.Expectation))
                .WhenTypeIs<double>());

            await act.Should().ThrowAsync<XunitException>().WithMessage("Expected*");
        }

        [Fact]
        public async Task Then_they_should_not_affect_any_other_assertions()
        {
            var actual = new
            {
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            Func<Task> act = () => actual.Should().BeEquivalentToAsync(expected);

            await act.Should().NotThrowAsync();
        }
    }

    [Collection("AssertionOptionsSpecs")]
    public class Given_self_resetting_equivalency_plan : GivenWhenThen
    {
        protected override void Dispose(bool disposing)
        {
            Plan.Reset();
            base.Dispose(disposing);
        }

        protected static EquivalencyPlan Plan
        {
            get { return AssertionOptions.EquivalencyPlan; }
        }
    }

    [Collection("AssertionOptionsSpecs")]
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

    [Collection("AssertionOptionsSpecs")]
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

    [Collection("AssertionOptionsSpecs")]
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

    [Collection("AssertionOptionsSpecs")]
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

    [Collection("AssertionOptionsSpecs")]
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

    [Collection("AssertionOptionsSpecs")]
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

    [Collection("AssertionOptionsSpecs")]
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

    [Collection("AssertionOptionsSpecs")]
    public class When_global_formatting_settings_are_modified : GivenWhenThen
    {
        private FormattingOptions oldSettings;

        public When_global_formatting_settings_are_modified()
        {
            Given(() => oldSettings = AssertionOptions.FormattingOptions.Clone());

            When(() =>
            {
                AssertionOptions.FormattingOptions.UseLineBreaks = true;
                AssertionOptions.FormattingOptions.MaxDepth = 123;
                AssertionOptions.FormattingOptions.MaxLines = 33;
            });
        }

        [Fact]
        public void Then_the_current_assertion_scope_should_use_these_settings()
        {
            AssertionScope.Current.FormattingOptions.UseLineBreaks.Should().BeTrue();
            AssertionScope.Current.FormattingOptions.MaxDepth.Should().Be(123);
            AssertionScope.Current.FormattingOptions.MaxLines.Should().Be(33);
        }

        protected override void Dispose(bool disposing)
        {
            AssertionOptions.FormattingOptions.MaxDepth = oldSettings.MaxDepth;
            AssertionOptions.FormattingOptions.UseLineBreaks = oldSettings.UseLineBreaks;
            AssertionOptions.FormattingOptions.MaxLines = oldSettings.MaxLines;

            base.Dispose(disposing);
        }
    }

    internal class MyEquivalencyStep : IEquivalencyStep
    {
        public Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator)
        {
            Execute.Assertion.FailWith(GetType().FullName);

            return Task.FromResult(EquivalencyResult.AssertionCompleted);
        }
    }
}
