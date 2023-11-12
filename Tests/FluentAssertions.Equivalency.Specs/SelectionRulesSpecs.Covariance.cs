#if NET5_0_OR_GREATER

using System;
using Xunit;

namespace FluentAssertions.Equivalency.Specs;

public partial class SelectionRulesSpecs
{
    public class Covariance
    {
        [Fact]
        public void Excluding_a_covariant_property_should_work()
        {
            // Arrange
            var actual = new DerivedWithCovariantOverride(new DerivedWithProperty
            {
                DerivedProperty = "a",
                BaseProperty = "a_base"
            })
            {
                OtherProp = "other"
            };

            var expectation = new DerivedWithCovariantOverride(new DerivedWithProperty
            {
                DerivedProperty = "b",
                BaseProperty =
                    "b_base"
            })
            {
                OtherProp = "other"
            };

            // Act / Assert
            actual.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(d => d.Property));
        }

        [Fact]
        public void Excluding_a_covariant_property_through_the_base_class_excludes_the_base_class_property()
        {
            // Arrange
            var actual = new DerivedWithCovariantOverride(new DerivedWithProperty
            {
                DerivedProperty = "a",
                BaseProperty = "a_base"
            })
            {
                OtherProp = "other"
            };

            BaseWithAbstractProperty expectation = new DerivedWithCovariantOverride(new DerivedWithProperty
            {
                DerivedProperty =
                    "b",
                BaseProperty = "b_base"
            })
            {
                OtherProp = "other"
            };

            // Act
            Action act = () => actual.Should().BeEquivalentTo(expectation, opts => opts
                .Excluding(d => d.Property));

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("No members*");
        }

        private class BaseWithProperty
        {
            public string BaseProperty { get; set; }
        }

        private class DerivedWithProperty : BaseWithProperty
        {
            public string DerivedProperty { get; set; }
        }

        private abstract class BaseWithAbstractProperty
        {
            public abstract BaseWithProperty Property { get; }
        }

        private sealed class DerivedWithCovariantOverride : BaseWithAbstractProperty
        {
            public override DerivedWithProperty Property { get; }

            public string OtherProp { get; set; }

            public DerivedWithCovariantOverride(DerivedWithProperty prop)
            {
                Property = prop;
            }
        }
    }
}

#endif
