using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions
{
    public static partial class CustomAssertionExtensions
    {
        [DebuggerNonUserCode]
        public class ObjectAssertions : Assertions<object, ObjectAssertions>
        {
            protected internal ObjectAssertions(object value)
            {
                ActualValue = value;
            }

            public AndConstraint<ObjectAssertions> Equal(object expected)
            {
                return Equal(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> Equal(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreEqual(expected, ActualValue),
                           "Expected <{0}>{2}, but found <{1}>.", expected, ActualValue, reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> NotEqual(object expected)
            {
                return NotEqual(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> NotEqual(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreNotEqual(expected, ActualValue),
                           "Did not expect <{0}>{2}.", expected, ActualValue, reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> BeSameAs(object expected)
            {
                return BeSameAs(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> BeSameAs(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreSame(expected, ActualValue),
                           "Expected the exact same objects{2}.", expected, ActualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> NotBeSameAs(object expected)
            {
                return NotBeSameAs(expected, string.Empty);
            }

            public AndConstraint<ObjectAssertions> NotBeSameAs(object expected, string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.AreNotSame(expected, ActualValue),
                           "Expected different objects{2}.", expected, ActualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> BeNull()
            {
                return BeNull(string.Empty);
            }

            public AndConstraint<ObjectAssertions> BeNull(string reason, params object[] reasonParameters)
            {
                AssertThat(() => (ActualValue == null),
                           "Expected <null>{2}, but found <{1}>.", null, ActualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> NotBeNull()
            {
                return NotBeNull(string.Empty);
            }

            public AndConstraint<ObjectAssertions> NotBeNull(string reason, params object[] reasonParameters)
            {
                AssertThat(() => (ActualValue != null),
                           "Expected non-null value{2}, but found <null>.", null, ActualValue, reason,
                           reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            public AndConstraint<ObjectAssertions> BeOfType<T>()
            {
                return BeOfType<T>(string.Empty);
            }

            public AndConstraint<ObjectAssertions> BeOfType<T>(string reason, params object[] reasonParameters)
            {
                AssertThat(() => Assert.IsInstanceOfType(ActualValue, typeof(T)),
                           "Expected type <{0}>{2}, but found <{1}>.", typeof(T), ActualValue.GetType(), reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }

            /// <summary>
            /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
            /// </summary>
            /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
            /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
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
            /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
            public AndConstraint<ObjectAssertions> BeAssignableTo<T>(string reason, params object[] reasonParameters)
            {
                AssertThat(() => typeof(T).IsAssignableFrom(ActualValue.GetType()),
                           "Expected to be assignable to <{0}>{2}, but <{1}> does not implement <{0}>", typeof(T),
                           ActualValue.GetType(), reason, reasonParameters);

                return new AndConstraint<ObjectAssertions>(this);
            }
        }
    }
}
