using System;
using System.Diagnostics;

using FluentAssertions.Common;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class ObjectAssertions : ReferenceTypeAssertions<object, ObjectAssertions>
    {
        protected internal ObjectAssertions(object value)
        {
            Subject = value;
        }

        /// <summary>
        /// Asserts that the value of an object equals another object when using it's <see cref="object.Equals(object)"/> method.
        /// </summary>
        public AndConstraint<ObjectAssertions> Be(object expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object equals another object using it's <see cref="object.Equals(object)"/> method.
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
            Execute.Verify(Subject.IsEqualTo(expected),
                "Expected {0}{2}, but found {1}.", expected, Subject, reason, reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using it's <see cref="object.Equals(object)"/> method.
        /// </summary>
        public AndConstraint<ObjectAssertions> NotBe(object expected)
        {
            return NotBe(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using it's <see cref="object.Equals(object)"/> method.
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
            Execute.Verify(!Subject.IsEqualTo(expected),
                "Did not expect object to be equal to {0}{2}.", expected, null, reason, reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeSameAs(object expected)
        {
            return BeSameAs(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object reference refers to the exact same object as another object reference.
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
            Execute.Verify(() => ReferenceEquals(Subject, expected),
                "Expected the exact same objects{2}.", expected, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        public AndConstraint<ObjectAssertions> NotBeSameAs(object expected)
        {
            return NotBeSameAs(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object reference refers to a different object than another object reference refers to.
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
                Execute.Fail("Expected different objects{2}.", expected, Subject, reason, reasonParameters);
            }

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> BeNull()
        {
            return BeNull(String.Empty);
        }

        public AndConstraint<ObjectAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            Execute.Verify(ReferenceEquals(Subject, null),
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
            Execute.Verify(!ReferenceEquals(Subject, null),
                "Expected non-null value{2}, but found <null>.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }
    }
}
