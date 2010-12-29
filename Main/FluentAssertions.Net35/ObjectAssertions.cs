using System;
using System.Diagnostics;

using FluentAssertions.Common;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class ObjectAssertions : Assertions<object, ObjectAssertions>
    {
        protected internal ObjectAssertions(object value)
        {
            Subject = value;
        }

        /// <summary>
        /// Verifies that the value of an object equals another object when using it's <see cref="object.Equals(object)"/> method.
        /// </summary>
        public AndConstraint<ObjectAssertions> Be(object expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Verifies that an object equals another object using it's <see cref="object.Equals(object)"/> method.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> Be(object expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(Subject.IsEqualTo(expected),
                "Expected {0}{2}, but found {1}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Verifies that an object does not equal another object using it's <see cref="object.Equals(object)"/> method.
        /// </summary>
        public AndConstraint<ObjectAssertions> NotBe(object expected)
        {
            return NotBe(expected, String.Empty);
        }

        /// <summary>
        /// Verifies that an object does not equal another object using it's <see cref="object.Equals(object)"/> method.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBe(object expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(!Subject.IsEqualTo(expected),
                "Did not expect object to be equal to {0}{2}.", expected, null, reason, reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Verifies that an object reference refers to the exact same object as another object reference.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeSameAs(object expected)
        {
            return BeSameAs(expected, String.Empty);
        }

        /// <summary>
        /// Verifies that an object reference refers to the exact same object as another object reference.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> BeSameAs(object expected, string reason, params object[] reasonParameters)
        {
            Verification.Verify(() => ReferenceEquals(Subject, expected),
                "Expected the exact same objects{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Verifies that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        public AndConstraint<ObjectAssertions> NotBeSameAs(object expected)
        {
            return NotBeSameAs(expected, String.Empty);
        }

        /// <summary>
        /// Verifies that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonParameters">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])"/> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBeSameAs(object expected, string reason, params object[] reasonParameters)
        {
            if (ReferenceEquals(Subject, expected))
            {
                Verification.Fail("Expected different objects{2}.", expected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> BeNull()
        {
            return BeNull(String.Empty);
        }

        public AndConstraint<ObjectAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            Verification.Verify(ReferenceEquals(Subject, null),
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
            Verification.Verify(!ReferenceEquals(Subject, null),
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
            Verification.Verify(() => typeof(T).IsAssignableFrom(Subject.GetType()),
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
            Verification.Verify(() => typeof(T).IsAssignableFrom(Subject.GetType()),
                "Expected to be assignable to {0}{2}, but {1} does not implement {0}", typeof(T),
                Subject.GetType(), reason, reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }
    }
}
