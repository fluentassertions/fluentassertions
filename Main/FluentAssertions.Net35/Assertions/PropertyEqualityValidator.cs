using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    internal class PropertyEqualityValidator
    {
        #region Private Definitions

        private const BindingFlags InstancePropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        private const int RootLevel = 0;
        private readonly ObjectTracker objectTracker = new ObjectTracker();
        private int nestedPropertyLevel;
        private string parentPropertyName = "";

        #endregion

        public PropertyEqualityValidator(object subject)
        {
            Subject = subject;
            Properties = new List<PropertyInfo>();
        }

        internal object Subject { get; private set; }

        public object OtherObject { get; set; }

        public IList<PropertyInfo> Properties { get; private set; }

        public bool OnlySharedProperties { get; set; }

        public bool RecurseOnNestedObjects { get; set; }

        public string Reason { get; set; }

        public object[] ReasonArgs { get; set; }

        public void Validate(int nestingLevel = RootLevel, string parentPropertyName = "")
        {
            nestedPropertyLevel = nestingLevel;
            this.parentPropertyName = parentPropertyName;

            if (ReferenceEquals(OtherObject, null))
            {
                throw new NullReferenceException("Cannot compare subject's properties with a <null> object.");
            }

            if (Properties.Count == 0)
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            AssertSelectedPropertiesAreEqual(Subject, OtherObject);
        }

        private void AssertSelectedPropertiesAreEqual(object subject, object expected)
        {
            foreach (var propertyInfo in Properties)
            {
                object actualValue = propertyInfo.GetValue(subject, null);

                PropertyInfo compareeProperty = FindPropertyFrom(expected, propertyInfo.Name);
                if (compareeProperty != null)
                {
                    object expectedValue = compareeProperty.GetValue(OtherObject, null);

                    AssertPropertyEqualityUsingVerificationContext(expectedValue, actualValue, propertyInfo);
                }
            }
        }

        private PropertyInfo FindPropertyFrom(object obj, string propertyName)
        {
            PropertyInfo compareeProperty =
                obj.GetType().GetProperties(InstancePropertiesFlag).SingleOrDefault(pi => pi.Name == propertyName);

            if (!OnlySharedProperties && (compareeProperty == null))
            {
                Execute.Verification.FailWith(
                    "Subject has property " + GetPropertyPath(propertyName) + " that the other object does not have.");
            }

            return compareeProperty;
        }

        private void AssertPropertyEqualityUsingVerificationContext(object expectedValue, object actualValue, PropertyInfo propertyInfo)
        {
            try
            {
                Verification.SubjectName = "property " + GetPropertyPath(propertyInfo.Name);
                AssertSinglePropertyEquality(propertyInfo.Name, actualValue, expectedValue);
            }
            finally
            {
                Verification.SubjectName = null;
            }
        }

        private void AssertSinglePropertyEquality(string propertyName, object actualValue, object expectedValue)
        {
            actualValue = TryConvertTo(expectedValue, actualValue);

            if (!actualValue.IsSameOrEqualTo(expectedValue))
            {
                if (expectedValue is string)
                {
                    ((string)actualValue).Should().Be(expectedValue.ToString(), Reason, ReasonArgs);
                }
                else if (expectedValue is IEnumerable)
                {
                    ((IEnumerable)actualValue).Should().Equal(((IEnumerable)expectedValue), Reason, ReasonArgs);
                }
                else if (IsComplexType(expectedValue) & RecurseOnNestedObjects)
                {
                    DetectCyclicReference(actualValue);

                    AssertNestedEquality(actualValue, expectedValue, GetPropertyPath(propertyName));
                }
                else
                {
                    actualValue.Should().Be(expectedValue, Reason, ReasonArgs);
                }
            }
        }

        private object TryConvertTo(object expectedValue, object subjectValue)
        {
            if (!ReferenceEquals(expectedValue, null) && !ReferenceEquals(subjectValue, null)
                && !subjectValue.GetType().IsSameOrInherits(expectedValue.GetType()))
            {
                try
                {
                    subjectValue = Convert.ChangeType(subjectValue, expectedValue.GetType(), CultureInfo.CurrentCulture);
                }
                catch (FormatException)
                {
                }
                catch (InvalidCastException)
                {
                }
            }

            return subjectValue;
        }

        private static bool IsComplexType(object expectedValue)
        {
            return (expectedValue != null) && expectedValue.GetType().GetProperties(InstancePropertiesFlag).Any();
        }

        private void DetectCyclicReference(object actualValue)
        {
            if (nestedPropertyLevel == RootLevel)
            {
                objectTracker.Reset();
            }

            objectTracker.Add(actualValue);
        }

        private void AssertNestedEquality(object actualValue, object expectedValue, string propertyName)
        {
            try
            {
                var validator = CreateNestedValidatorFor(actualValue, expectedValue);
                validator.Validate(nestedPropertyLevel + 1, propertyName);
            }
            catch (ObjectAlreadyTrackedException)
            {
                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith("Expected property " + propertyName + " to be {0}{reason}, but it contains a cyclic reference.",
                        expectedValue);
            }
        }

        private static PropertyEqualityValidator CreateNestedValidatorFor(object actualValue, object expectedValue)
        {
            var validator = new PropertyEqualityValidator(actualValue)
            {
                RecurseOnNestedObjects = true,
                OtherObject = expectedValue
            };

            foreach (var propertyInfo in actualValue.GetType().GetProperties(InstancePropertiesFlag))
            {
                if (!propertyInfo.GetGetMethod(true).IsPrivate)
                {
                    validator.Properties.Add(propertyInfo);
                }
            }

            return validator;
        }

        private string GetPropertyPath(string propertyName)
        {
            return (parentPropertyName.Length > 0) ? parentPropertyName + "." + propertyName : propertyName;
        }
    }
}