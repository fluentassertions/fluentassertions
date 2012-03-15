using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#if WINRT
using System.Reflection.RuntimeExtensions;
#endif

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{


    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    internal class PropertyEqualityValidator
    {
        #region Private Definitions

#if !WINRT
        private const BindingFlags PublicPropertiesFlag =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
#endif

        private UniqueObjectTracker uniqueObjectTracker;
        private string parentPropertyName = "";
        private readonly IList<string> includedProperties = new List<string>();
        private readonly IList<string> excludedProperties = new List<string>();

        #endregion

        public PropertyEqualityValidator()
        {
            PropertySelection = PropertySelection.None;
        }

        public PropertySelection PropertySelection { get; set; }

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

        public Type CompileTimeType { get; set; }

        public string Reason { get; set; }

        public object[] ReasonArgs { get; set; }

        public void ExcludeProperty(string propertyName)
        {
            excludedProperties.Add(propertyName);
        }

        public void IncludeProperty(string propertyName)
        {
            includedProperties.Add(propertyName);
        }

        public void AssertEquality(object subject, object expected, string parentPropertyName = "")
        {
            AssertEquality(subject, expected, new UniqueObjectTracker(), parentPropertyName);
        }

        private void AssertEquality(object subject, object expected, UniqueObjectTracker tracker,
            string parentPropertyName)
        {
            this.parentPropertyName = parentPropertyName;

            uniqueObjectTracker = tracker;
            uniqueObjectTracker.Track(subject);

            if (ReferenceEquals(expected, null))
            {
                throw new NullReferenceException("Cannot compare subject's properties with a <null> object.");
            }

            AssertSelectedPropertiesAreEqual(subject, expected);
        }

        private void AssertSelectedPropertiesAreEqual(object subject, object expected)
        {
            foreach (PropertyInfo propertyInfo in DeterminePropertiesToInclude(subject))
            {
                object actualValue = propertyInfo.GetValue(subject, null);

                PropertyInfo compareeProperty = FindPropertyFrom(expected, propertyInfo.Name);
                if (compareeProperty != null)
                {
                    object expectedValue = compareeProperty.GetValue(expected, null);

                    AssertPropertyEqualityUsingVerificationContext(expectedValue, actualValue, propertyInfo);
                }
            }
        }

        private IEnumerable<PropertyInfo> DeterminePropertiesToInclude(object subject)
        {
            IEnumerable<PropertyInfo> properties;

            if (PropertySelection == PropertySelection.AllRuntimePublic)
            {
                properties = GetNonPrivateProperties(subject.GetType());
            }
            else if ((PropertySelection == PropertySelection.AllCompileTimePublic) || (PropertySelection == PropertySelection.OnlyShared))
            {
                properties = GetNonPrivateProperties(CompileTimeType);
            }
            else if (PropertySelection == PropertySelection.None)
            {
                properties = GetNonPrivateProperties(subject.GetType(), includedProperties);
            }
            else
            {
                throw new InvalidOperationException("Unknown PropertySelection value " + PropertySelection);
            }

            properties = properties.Where(pi => !excludedProperties.Contains(pi.Name)).ToArray();
            if ((PropertySelection != PropertySelection.OnlyShared) && !properties.Any())
            {
                throw new InvalidOperationException("Please specify some properties to include in the comparison.");
            }

            return properties.ToArray();
        }

        private IEnumerable<PropertyInfo> GetNonPrivateProperties(Type typeToReflect, IEnumerable<string> explicitProperties = null)
        {
            var query =
#if !WINRT
                from propertyInfo in typeToReflect.GetProperties(PublicPropertiesFlag)
                let getMethod = propertyInfo.GetGetMethod(true)
                where (getMethod != null) && !getMethod.IsPrivate
#else
                from propertyInfo in typeToReflect.GetRuntimeProperties()
                let getMethod = propertyinfo.GetMethod
                where (getMethod != null) !getMethod.IsPrivate && !getMethod.IsStatic
#endif

                where (explicitProperties == null) || explicitProperties.Contains(propertyInfo.Name)
                select propertyInfo;

            return query.ToList();
        }

     

        private PropertyInfo FindPropertyFrom(object obj, string propertyName)
        {
            PropertyInfo compareeProperty =
#if !WINRT
                obj.GetType().GetProperties(PublicPropertiesFlag)
#else
                obj.GetType().GetRuntimeProperties().Where(p => !p.GetMethod.IsStatic)
#endif
                .SingleOrDefault(pi => pi.Name == propertyName);

            if ((PropertySelection != PropertySelection.OnlyShared) && (compareeProperty == null))
            {
                Execute.Verification.FailWith(
                    "Subject has property " + GetPropertyPath(propertyName) + " that the other object does not have.");
            }

            return compareeProperty;
        }

        private void AssertPropertyEqualityUsingVerificationContext(object expectedValue, object actualValue,
            PropertyInfo propertyInfo)
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
                        AssertNestedCollectionEquality(actualValue, (IEnumerable)expectedValue,
                            GetPropertyPath(propertyName));
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
            return (expectedValue != null) && expectedValue.GetType()
#if !WINRT
                .GetProperties(PublicPropertiesFlag)
#else
                .GetRuntimeProperties().Where(p => !p.GetMethod.IsStatic)
#endif
                .Any();
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
                PropertyEqualityValidator validator = CreateNestedValidatorFor(actualValue);
                validator.AssertEquality(actualValue, expectedValue, uniqueObjectTracker, propertyName);
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

        private PropertyEqualityValidator CreateNestedValidatorFor(object actualItem)
        {
            return new PropertyEqualityValidator
            {
                RecurseOnNestedObjects = true,
                CyclicReferenceHandling = CyclicReferenceHandling,
                PropertySelection = PropertySelection,
                CompileTimeType = actualItem.GetType()
            };
        }

        private string GetPropertyPath(string propertyName)
        {
            return (parentPropertyName.Length > 0) ? parentPropertyName + "." + propertyName : propertyName;
        }
    }

    public enum PropertySelection
    {
        AllCompileTimePublic,
        AllRuntimePublic,
        OnlyShared,
        None,
    }
}
