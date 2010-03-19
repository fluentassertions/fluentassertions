using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class ObjectAssertions : Assertions<object, ObjectAssertions>
    {
        protected internal ObjectAssertions(object value)
        {
            Subject = value;
        }

        public AndConstraint<ObjectAssertions> Be(object expected)
        {
            return Be(expected, String.Empty);
        }

        public AndConstraint<ObjectAssertions> Be(object expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => Subject.Equals(expected),
                "Expected {0}{2}, but found {1}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> NotBe(object expected)
        {
            return NotBe(expected, String.Empty);
        }

        public AndConstraint<ObjectAssertions> NotBe(object expected, string reason, params object[] reasonParameters)
        {
            if (Subject.Equals(expected))
            {
                FailWith("Did not expect objects {0} and {1} to be equal{2}.", expected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> BeSameAs(object expected)
        {
            return BeSameAs(expected, String.Empty);
        }

        public AndConstraint<ObjectAssertions> BeSameAs(object expected, string reason, params object[] reasonParameters)
        {
            VerifyThat(() => ReferenceEquals(Subject, expected),
                "Expected the exact same objects{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> NotBeSameAs(object expected)
        {
            return NotBeSameAs(expected, String.Empty);
        }

        public AndConstraint<ObjectAssertions> NotBeSameAs(object expected, string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, expected))
            {
                FailWith("Expected different objects{2}.", expected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> BeNull()
        {
            return BeNull(String.Empty);
        }

        public AndConstraint<ObjectAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject == null),
                "Expected <null>{2}, but found {1}.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> NotBeNull()
        {
            return NotBeNull(String.Empty);
        }

        public AndConstraint<ObjectAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => (Subject != null),
                "Expected non-null value{2}, but found <null>.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> BeOfType<T>()
        {
            return BeOfType<T>(String.Empty);
        }

        public AndConstraint<ObjectAssertions> BeOfType<T>(string reason, params object[] reasonParameters)
        {
            VerifyThat(() => typeof(T).IsAssignableFrom(Subject.GetType()),
                "Expected type {0}{2}, but found {1}.", typeof(T), Subject.GetType(), reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
        /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
        public AndConstraint<ObjectAssertions> BeAssignableTo<T>()
        {
            return BeAssignableTo<T>(String.Empty);
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
            VerifyThat(() => typeof(T).IsAssignableFrom(Subject.GetType()),
                "Expected to be assignable to {0}{2}, but {1} does not implement {0}", typeof(T),
                Subject.GetType(), reason, reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }
    }
}
