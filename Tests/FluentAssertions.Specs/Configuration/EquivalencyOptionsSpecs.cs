using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Equivalency;
using FluentAssertions.Equivalency.Steps;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Configuration;

public class EquivalencyOptionsSpecs
{
    [Fact]
    public void When_injecting_a_null_configurer_it_should_throw()
    {
        // Arrange / Act
        var action = () => AssertionConfiguration.Current.Equivalency.Modify(configureOptions: null);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("configureOptions");
    }

    [Fact]
    public void When_concurrently_getting_equality_strategy_it_should_not_throw()
    {
        // Arrange / Act
        IEquivalencyOptions equivalencyOptions = new EquivalencyOptions();

        var action = () => Parallel.For(0, 10_000, new ParallelOptions { MaxDegreeOfParallelism = 8 },
            _ => equivalencyOptions.GetEqualityStrategy(typeof(IEnumerable))
        );

        // Assert
        action.Should().NotThrow();
    }

    [Collection("ConfigurationSpecs")]
    public sealed class Given_temporary_global_assertion_options : IDisposable
    {
        [Fact]
        public void When_modifying_global_reference_type_settings_a_previous_assertion_should_not_have_any_effect_it_should_try_to_compare_the_classes_by_member_semantics_and_thus_throw()
        {
            // Arrange
            // Trigger a first equivalency check using the default global settings
            new MyValueType { Value = 1 }.Should().BeEquivalentTo(new MyValueType { Value = 2 });

            AssertionConfiguration.Current.Equivalency.Modify(o => o.ComparingByMembers<MyValueType>());

            // Act
            Action act = () => new MyValueType { Value = 1 }.Should().BeEquivalentTo(new MyValueType { Value = 2 });

            // Assert
            act.Should().Throw<XunitException>();
        }

        internal class MyValueType
        {
            [UsedImplicitly]
            public int Value { get; set; }

            public override bool Equals(object obj) => true;

            public override int GetHashCode() => 0;
        }

        [Fact]
        public void When_modifying_global_value_type_settings_a_previous_assertion_should_not_have_any_effect_it_should_try_to_compare_the_classes_by_value_semantics_and_thus_throw()
        {
            // Arrange
            // Trigger a first equivalency check using the default global settings
            new MyClass { Value = 1 }.Should().BeEquivalentTo(new MyClass { Value = 1 });

            AssertionConfiguration.Current.Equivalency.Modify(o => o.ComparingByValue<MyClass>());

            // Act
            Action act = () => new MyClass() { Value = 1 }.Should().BeEquivalentTo(new MyClass { Value = 1 });

            // Assert
            act.Should().Throw<XunitException>();
        }

        internal class MyClass
        {
            [UsedImplicitly]
            public int Value { get; set; }
        }

        [Fact]
        public void When_modifying_record_settings_globally_it_should_use_the_global_settings_for_comparing_records()
        {
            // Arrange
            AssertionConfiguration.Current.Equivalency.Modify(o => o.ComparingByValue(typeof(Position)));

            // Act / Assert
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

        [Fact]
        public void When_assertion_doubles_should_always_allow_small_deviations_then_it_should_ignore_small_differences_without_the_need_of_local_options()
        {
            // Arrange
            AssertionConfiguration.Current.Equivalency.Modify(options => options
                .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01))
                .WhenTypeIs<double>());

            var actual = new
            {
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_local_similar_options_are_used_then_they_should_override_the_global_options()
        {
            // Arrange
            AssertionConfiguration.Current.Equivalency.Modify(options => options
                .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01))
                .WhenTypeIs<double>());

            var actual = new
            {
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected, options => options
                .Using<double>(ctx => ctx.Subject.Should().Be(ctx.Expectation))
                .WhenTypeIs<double>());

            // Assert
            act.Should().Throw<XunitException>().WithMessage("Expected*");
        }

        [Fact]
        public void When_local_similar_options_are_used_then_they_should_not_affect_any_other_assertions()
        {
            // Arrange
            AssertionConfiguration.Current.Equivalency.Modify(options => options
                .Using<double>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 0.01))
                .WhenTypeIs<double>());

            var actual = new
            {
                Value = 1D / 3D
            };

            var expected = new
            {
                Value = 0.33D
            };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().NotThrow();
        }

        public void Dispose() =>
            AssertionConfiguration.Current.Equivalency.Modify(_ => new EquivalencyOptions());
    }

    [Collection("ConfigurationSpecs")]
    public sealed class Given_self_resetting_equivalency_plan : IDisposable
    {
        private static EquivalencyPlan Plan => AssertionConfiguration.Current.Equivalency.Plan;

        [Fact]
        public void When_inserting_a_step_then_it_should_precede_all_other_steps()
        {
            // Arrange / Act
            Plan.Insert<MyEquivalencyStep>();

            // Assert
            var addedStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);

            Plan.Should().StartWith(addedStep);
        }

        [Fact]
        public void When_inserting_a_step_before_another_then_it_should_precede_that_particular_step()
        {
            // Arrange / Act
            Plan.InsertBefore<DictionaryEquivalencyStep, MyEquivalencyStep>();

            // Assert
            var addedStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);
            var successor = Plan.LastOrDefault(s => s is DictionaryEquivalencyStep);

            Plan.Should().HaveElementPreceding(successor, addedStep);
        }

        [Fact]
        public void When_appending_a_step_then_it_should_precede_the_final_builtin_step()
        {
            // Arrange / Act
            Plan.Add<MyEquivalencyStep>();

            // Assert
            var equivalencyStep = Plan.LastOrDefault(s => s is SimpleEqualityEquivalencyStep);
            var subjectStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);

            Plan.Should().HaveElementPreceding(equivalencyStep, subjectStep);
        }

        [Fact]
        public void When_appending_a_step_after_another_then_it_should_precede_the_final_builtin_step()
        {
            // Arrange / Act
            Plan.AddAfter<DictionaryEquivalencyStep, MyEquivalencyStep>();

            // Assert
            var addedStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);
            var predecessor = Plan.LastOrDefault(s => s is DictionaryEquivalencyStep);

            Plan.Should().HaveElementSucceeding(predecessor, addedStep);
        }

        [Fact]
        public void When_appending_a_step_and_no_builtin_steps_are_there_then_it_should_precede_the_simple_equality_step()
        {
            // Arrange / Act
            Plan.Clear();
            Plan.Add<MyEquivalencyStep>();

            // Assert
            var subjectStep = Plan.LastOrDefault(s => s is MyEquivalencyStep);
            Plan.Should().EndWith(subjectStep);
        }

        [Fact]
        public void When_removing_a_specific_step_then_it_should_precede_the_simple_equality_step()
        {
            // Arrange / Act
            Plan.Remove<SimpleEqualityEquivalencyStep>();

            // Assert
            Plan.Should().NotContain(s => s is SimpleEqualityEquivalencyStep);
        }

        [Fact]
        public void When_removing_a_specific_step_that_doesnt_exist_Then_it_should_precede_the_simple_equality_step()
        {
            // Arrange / Act
            var action = () => Plan.Remove<MyEquivalencyStep>();

            // Assert
            action.Should().NotThrow();
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

        public void Dispose() => Plan.Reset();
    }
}
