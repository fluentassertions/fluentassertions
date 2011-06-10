using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    internal class PropertyEqualityValidator<T>
    {
        private const BindingFlags InstancePropertiesFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        public PropertyEqualityValidator(T subject)
        {
            Subject = subject;
            Properties = new List<PropertyInfo>();
        }

        internal T Subject { get; private set; }
        public object OtherObject { get; set; }
        public IList<PropertyInfo> Properties { get; private set; }

        public bool OnlySharedProperties { get; set; }

        public string Reason { get; set; }

        public object[] ReasonArgs { get; set; }

        public void Validate()
        {
            if (ReferenceEquals(OtherObject, null))
            {
                throw new NullReferenceException("Cannot compare subject's properties with a <null> object.");
            }

            if (Properties.Count == 0)
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            foreach (var propertyInfo in Properties)
            {
                CompareProperty(propertyInfo);
            }
        }

        private void CompareProperty(PropertyInfo propertyInfo)
        {
            object actualValue = propertyInfo.GetValue(Subject, null);

            PropertyInfo compareeProperty = FindPropertyFrom(propertyInfo.Name);
            if (compareeProperty != null)
            {
                object expectedValue = compareeProperty.GetValue(OtherObject, null);

                actualValue = HarmonizeTypeDifferences(propertyInfo.Name, actualValue, expectedValue);

                if (!ReferenceEquals(actualValue, expectedValue))
                {
                    Verification.SubjectName = "property " + propertyInfo.Name;

                    try
                    {
                        VerifySemanticEquality(actualValue, expectedValue);
                    }
                    finally
                    {
                        Verification.SubjectName = null;
                    }
                }
            }
        }

        private PropertyInfo FindPropertyFrom(string propertyName)
        {
            PropertyInfo compareeProperty =
                OtherObject.GetType().GetProperties(InstancePropertiesFlag).SingleOrDefault(pi => pi.Name == propertyName);

            if (!OnlySharedProperties&& (compareeProperty == null))
            {
                Execute.Verification.FailWith(
                    "Subject has property " + propertyName + " that the other object does not have.");
            }

            return compareeProperty;
        }

        private object HarmonizeTypeDifferences(string propertyName, object subjectValue, object expectedValue)
        {
            if (!ReferenceEquals(subjectValue, null) && !ReferenceEquals(expectedValue, null) &&
                (subjectValue.GetType() != expectedValue.GetType()))
            {
                try
                {
                    subjectValue = Convert.ChangeType(subjectValue, expectedValue.GetType(), CultureInfo.CurrentCulture);
                }
                catch (FormatException)
                {
                    Execute.Verification.BecauseOf(Reason, ReasonArgs).FailWith(
                            "Expected property " + propertyName + " to be {1}{0}, but {2} is of an incompatible type.",
                            expectedValue, subjectValue);
                }
            }

            return subjectValue;
        }

        private void VerifySemanticEquality(object subjectValue, object expectedValue)
        {
            if (subjectValue is string)
            {
                ((string) subjectValue).Should().Be((string) expectedValue, Reason, ReasonArgs);
            }
            else if (subjectValue is IEnumerable)
            {
                ((IEnumerable) subjectValue).Should().Equal(((IEnumerable) expectedValue), Reason, ReasonArgs);
            }
            else
            {
                subjectValue.Should().Be(expectedValue, Reason, ReasonArgs);
            }
        }
    }
}