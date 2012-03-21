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
        private UniqueObjectTracker uniqueObjectTracker;
        private string parentPropertyName = "";
        private readonly object subject;

        #endregion

        public PropertyEqualityValidator(object subject)
        {
            this.subject = subject;
            Properties = new List<PropertyInfo>();
        }

        public object OtherObject { get; set; }

        /// <summary>
        /// Contains the properties that should be included when comparing two objects.
        /// </summary>
        public IList<PropertyInfo> Properties { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the validator will ignore properties from the <see cref="Properties"/>
        /// collection that the <see cref="Other"/> object doesn't have.
        /// </summary>
        public bool OnlySharedProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it should continue comparing (collections of objects) that
        /// the <see cref="OtherObject"/> refers to.
        /// </summary>
        public bool RecurseOnNestedObjects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how cyclic references that are encountered while comparing (collections of)
        /// objects should be handled.
        /// </summary>
        public CyclicReferenceHandling CyclicReferenceHandling { get; set; }

        public string Reason { get; set; }

        public object[] ReasonArgs { get; set; }

        public void Validate()
        {
            Validate(new UniqueObjectTracker(), "");
        }

        private void Validate(UniqueObjectTracker tracker, string parentPropertyName)
        {
            this.parentPropertyName = parentPropertyName;

            uniqueObjectTracker = tracker;
            uniqueObjectTracker.Track(subject);

            if (ReferenceEquals(OtherObject, null))
            {
                throw new NullReferenceException("Cannot compare subject's properties with a <null> object.");
            }

            if (Properties.Count == 0)
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
        }

            AssertSelectedPropertiesAreEqual(subject, OtherObject);
        }

        private void AssertSelectedPropertiesAreEqual(object subject, object expected)
        {
            foreach (PropertyInfo propertyInfo in Properties)
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
                else if (expectedValue is DateTime)
                {
                    ((DateTime)actualValue).Should().Be((DateTime)expectedValue, Reason, ReasonArgs);
                }
                else if (IsCollection(expectedValue))
                {
                    if (RecurseOnNestedObjects)
                    {
                        AssertNestedCollectionEquality(actualValue, (IEnumerable)expectedValue, GetPropertyPath(propertyName));
                    }
                    else
                    {
                        ((IEnumerable)actualValue).Should().Equal(((IEnumerable)expectedValue), Reason, ReasonArgs);
                    }
                }
                else if (IsComplexType(expectedValue) & RecurseOnNestedObjects)
                {
                    AssertNestedEquality(actualValue, expectedValue, GetPropertyPath(propertyName));
                }
                else
                {
                    actualValue.Should().Be(expectedValue, Reason, ReasonArgs);
                }
            }
        }

        private void AssertNestedCollectionEquality(object actualValue, IEnumerable expectedValue, string propertyPath)
        {
            if (!IsCollection(actualValue))
            {
                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith("Expected {0} property to be a collection{reason}, but {1} is a {2}.",
                        propertyPath, actualValue, actualValue.GetType().FullName);
            }

            var actualItems = ((IEnumerable)actualValue).Cast<object>().ToArray();
            var expectedItems = expectedValue.Cast<object>().ToArray();

            if (actualItems.Length != expectedItems.Length)
            {
                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith("Expected {0} property to be a collection with {1} item(s){reason}, but found {2}.",
                        propertyPath, expectedItems.Length, actualItems.Length);
            }

            for (int index = 0; index < actualItems.Length; index++)
            {
                AssertNestedEquality(actualItems[index], expectedItems[index], propertyPath + "[index " + index + "]");
            }
        }

        private static bool IsCollection(object value)
        {
            return (!(value is string) && (value is IEnumerable));
        }

        private static bool IsComplexType(object expectedValue)
        {
            return (expectedValue != null) && expectedValue.GetType().GetProperties(InstancePropertiesFlag).Any();
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

        private void AssertNestedEquality(object actualValue, object expectedValue, string propertyName)
        {
            try
            {
                Execute.Verification
                    .ForCondition(!ReferenceEquals(actualValue, null))
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith("Expected property " + propertyName + " to be {0}{reason}, but it is <null>.",
                        expectedValue);

                PropertyEqualityValidator validator = CreateNestedValidatorFor(actualValue, expectedValue);
                validator.Validate(uniqueObjectTracker, propertyName);
            }
            catch (ObjectAlreadyTrackedException)
            {
                if (CyclicReferenceHandling == CyclicReferenceHandling.ThrowException)
                {
                    Execute.Verification
                        .BecauseOf(Reason, ReasonArgs)
                        .FailWith("Expected property " + propertyName + " to be {0}{reason}, but it contains a cyclic reference.",
                            expectedValue);
                }
                else
                {
                // Ignore cyclic references
            }
        }
        }

        private PropertyEqualityValidator CreateNestedValidatorFor(object actualValue, object expectedValue)
        {
            var validator = new PropertyEqualityValidator(actualValue)
            {
                RecurseOnNestedObjects = true,
                CyclicReferenceHandling = CyclicReferenceHandling,
                OtherObject = expectedValue,
                OnlySharedProperties = OnlySharedProperties
            };

            foreach (var propertyInfo in actualValue.GetType().GetProperties(InstancePropertiesFlag))
            {
                var getter = propertyInfo.GetGetMethod(true);
                if ((getter != null) && !getter.IsPrivate)
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
