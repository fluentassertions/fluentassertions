using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="object"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ObjectAssertions : ReferenceTypeAssertions<object, ObjectAssertions>
    {
        public ObjectAssertions(object value)
        {
            Subject = value;
        }

        /// <summary>
        /// Asserts that an object equals another object using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<ObjectAssertions> Be(object expected, string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .BecauseOf(reason, reasonArgs)
                .ForCondition(Subject.IsSameOrEqualTo(expected))
                .FailWith("Expected {context:object} to be {0}{reason}, but found {1}.", expected,
                    Subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using it's <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBe(object unexpected, string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.IsSameOrEqualTo(unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect {context:object} to be equal to {0}{reason}.", unexpected);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "object"; }
        }
    }
}