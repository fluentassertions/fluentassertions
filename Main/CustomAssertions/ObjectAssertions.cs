using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        [DebuggerNonUserCode]
        public class ObjectAssertions : Assertions
        {
            private readonly object actualValue;

            protected internal ObjectAssertions(object value)
            {
                actualValue = value;
            }

            public AndConstraint<ObjectAssertions> Equal(object expected)
            {
                return Equal(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> Equal(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreEqual(expected, actualValue),
                           "Expected <{0}>{2}, but found <{1}>.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> NotEqual(object expected)
            {
                return NotEqual(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> NotEqual(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreNotEqual(expected, actualValue),
                           "Did not expect <{0}>{2}.", expected, actualValue, reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> BeSameAs(object expected)
            {
                return BeSameAs(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> BeSameAs(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreSame(expected, actualValue),
                           "Expected the exact same objects{2}.", expected, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> NotBeSameAs(object expected)
            {
                return NotBeSameAs(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> NotBeSameAs(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreNotSame(expected, actualValue),
                           "Expected different objects{2}.", expected, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> BeNull()
            {
                return BeNull(string.Empty);
            }

            public AndConstraint<ObjectAssertions> BeNull(string reason, params object[] reasonParameters)
            {
                AssertThat(() => (actualValue == null),
                           "Expected <null>{2}, but found <{1}>.", null, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> NotBeNull()
            {
                return NotBeNull(string.Empty);
            }

            public AndConstraint<ObjectAssertions> NotBeNull(string reason, params object[] reasonParameters)
            {
                AssertThat(() => (actualValue != null),
                           "Expected non-null value{2}, but found <null>.", null, actualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> BeOfType<T>()
            {
                return BeOfType<T>(string.Empty);
            }

            public AndConstraint<ObjectAssertions> BeOfType<T>(string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.IsInstanceOfType(actualValue, typeof(T)),
                           "Expected type <{0}>{2}, but found <{1}>.", typeof(T), actualValue.GetType(), reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            /// <summary>
            /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
            /// <returns>An <see cref="AndConstraint"/> which can be use to chain assertions.</returns>
            public AndConstraint<ObjectAssertions> BeAssignableTo<T>()
            {
                return BeAssignableTo<T>(string.Empty);
            }

            /// <summary>
            /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
            /// <param name="reason">The reason why the object should be assignable to the type.</param>
            /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
            /// <returns>An <see cref="AndConstraint"/> which can be use to chain assertions.</returns>
            public AndConstraint<ObjectAssertions> BeAssignableTo<T>(string reason, params object[] reasonParameters)
            {
                AssertThat(() => typeof(T).IsAssignableFrom(actualValue.GetType()),
                           "Expected to be assignable to <{0}>{2}, but <{1}> does not implement <{0}>", typeof(T),
                           actualValue.GetType(), reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            /// <summary>
            /// Asserts that the <paramref name="predicate"/> is statisfied.
            /// </summary>
            /// <param name="predicate">The predicate which must be satisfied by the object.</param>
            /// <returns>An <see cref="AndConstraint"/> which can be use to chain assertions.</returns>
            public AndConstraint<ObjectAssertions> Satisfy(Predicate<object> predicate)
            {
                return Satisfy(predicate, string.Empty);
            }

            /// <summary>
            /// Asserts that the <paramref name="predicate" /> is satisfied.
            /// </summary>
            /// <param name="predicate">The predicate which must be statisfied by the object.</param>
            /// <param name="reason">The reason why the predicate should be satisfied.</param>
            /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
            /// <returns>An <see cref="AndConstraint"/> which can be use to chain assertions.</returns>
            public AndConstraint<ObjectAssertions> Satisfy(Predicate<object> predicate, string reason, params object[] reasonParameters)
            {
                AssertThat(() => predicate(actualValue),
                           "Expected to satisfy predicate{2}, but predicate not satisfied by {1}",
                           predicate, actualValue, reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }
        }
    }
}