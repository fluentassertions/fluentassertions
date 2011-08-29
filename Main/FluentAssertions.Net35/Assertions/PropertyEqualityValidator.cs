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
    internal class PropertyEqualityValidator
    {
        private const BindingFlags InstancePropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        private const int RootLevel = 0;
        private readonly CyclicReferenceTracker cyclicReferenceTracker = new CyclicReferenceTracker();
        private int nestedPropertyLevel;

        public PropertyEqualityValidator(object subject)
        {
            Subject = subject;
            Properties = new List<PropertyInfo>();
        }

        internal object Subject { get; private set; }
        public object OtherObject { get; set; }
        public IList<PropertyInfo> Properties { get; private set; }

        public bool OnlySharedProperties { get; set; }
        public bool RecurseOnIncompatibleProperties { get; set; }

        public string Reason { get; set; }

        public object [] ReasonArgs { get; set; }

        public void Validate(int nestingLevel = RootLevel)
        {
            nestedPropertyLevel = nestingLevel;

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

                bool typesAreCompatible;
                actualValue = HarmonizeTypeDifferences(actualValue, expectedValue, out typesAreCompatible);

                if (ReferenceEquals(actualValue, expectedValue))
                {
                    return;
                }

                if (typesAreCompatible)
                {
                    CompareCompatibleProperties(actualValue, expectedValue, propertyInfo.Name);
                }
                else
                {
                    ValidateEqualityOfIncompatibleProperties(actualValue, expectedValue, propertyInfo);
                }
            }
        }

        private object HarmonizeTypeDifferences(object subjectValue, object expectedValue, out bool typesAreCompatible)
        {
            typesAreCompatible = true;

            if (!ReferenceEquals(subjectValue, null) && !ReferenceEquals(expectedValue, null) &&
                (subjectValue.GetType() != expectedValue.GetType()))
            {
                try
                {
                    subjectValue = Convert.ChangeType(subjectValue, expectedValue.GetType(), CultureInfo.CurrentCulture);
                }
                catch (FormatException)
                {
                    typesAreCompatible = false;
                }
                catch (InvalidCastException)
                {
                    typesAreCompatible = false;
                }
            }

            return subjectValue;
        }

        private void CompareCompatibleProperties(object actualValue, object expectedValue, string propertyName)
        {
            Verification.SubjectName = "property " + propertyName;

            try
            {
                VerifySemanticEquality(actualValue, expectedValue);
            }
            finally
            {
                Verification.SubjectName = null;
            }
        }

        private void ValidateEqualityOfIncompatibleProperties(object actualValue, object expectedValue, PropertyInfo propertyInfo)
        {
            if (RecurseOnIncompatibleProperties)
            {
                AssertNoCyclicReferenceFor(actualValue);

                CompareIncompatibleProperties(actualValue, expectedValue, propertyInfo.Name);
            }
            else
            {
                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith("Expected property " + propertyInfo.Name +
                        " to be {0}{reason}, but found {1} which is of an incompatible type.", expectedValue, actualValue);
            }
        }

        private void AssertNoCyclicReferenceFor(object actualValue)
        {
            if (nestedPropertyLevel == RootLevel)
            {
                cyclicReferenceTracker.Initialize();
            }

            cyclicReferenceTracker.AssertNoCyclicReferenceFor(actualValue);
        }

        private PropertyInfo FindPropertyFrom(string propertyName)
        {
            PropertyInfo compareeProperty =
                OtherObject.GetType().GetProperties(InstancePropertiesFlag).SingleOrDefault(pi => pi.Name == propertyName);

            if (!OnlySharedProperties && (compareeProperty == null))
            {
                Execute.Verification.FailWith(
                    "Subject has property " + propertyName + " that the other object does not have.");
            }

            return compareeProperty;
        }

        private void CompareIncompatibleProperties(object actualValue, object expectedValue, string propertyName)
        {
            try
            {
                var validator = new PropertyEqualityValidator(actualValue)
                {
                    RecurseOnIncompatibleProperties = true,
                    OtherObject = expectedValue
                };

                AddPublicPropertiesFor(actualValue, validator);

                validator.Validate(nestedPropertyLevel + 1);
            }
            catch (CyclicReferenceInRecursionException)
            {
                if (nestedPropertyLevel != RootLevel)
                {
                    // Keep throwing the CyclicReferenceInRecursionException untill it is caught at the root level
                    throw;
                }

                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith("Expected property " + propertyName +
                        " to be {0}{reason}, but found {1} which is of an incompatible type and has a cyclic reference.",
                        expectedValue.GetType(), actualValue.GetType());
            }
            catch
            {
                Execute.Verification
                    .BecauseOf(Reason, ReasonArgs)
                    .FailWith("Expected property " + propertyName +
                        " to have all properties equal to {0}{reason}, but found {1}.", expectedValue, actualValue);
            }
        }

        private static void AddPublicPropertiesFor(object actualValue, PropertyEqualityValidator validator)
        {
            foreach (var propertyInfo in actualValue.GetType().GetProperties(InstancePropertiesFlag))
            {
                if (!propertyInfo.GetGetMethod(true).IsPrivate)
                {
                    validator.Properties.Add(propertyInfo);
                }
            }
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