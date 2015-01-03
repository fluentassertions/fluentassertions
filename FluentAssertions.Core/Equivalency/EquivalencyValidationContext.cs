using System;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    public class EquivalencyValidationContext : IEquivalencyValidationContext
    {
        public EquivalencyValidationContext()
        {
            SelectedMemberDescription = "";
            SelectedMemberPath = "";
        }

        public ISelectedMemberInfo SelectedMemberInfo { get; set; }

        public string SelectedMemberPath { get; set; }

        public string SelectedMemberDescription { get; set; }

        [Obsolete]
        public PropertyInfo PropertyInfo
        {
            get
            {
                var propertySelectedMemberInfo = SelectedMemberInfo as PropertySelectedMemberInfo;

                if (propertySelectedMemberInfo != null)
                {
                    return propertySelectedMemberInfo.PropertyInfo;
                }

                return null;
            }
        }

        [Obsolete]
        public string PropertyPath
        {
            get { return SelectedMemberPath; }
        }

        [Obsolete]
        public string PropertyDescription
        {
            get { return SelectedMemberDescription; }
        }

        /// <summary>
        /// Gets the value of the <see cref="ISubjectInfo.SelectedMemberInfo" />
        /// </summary>
        public object Subject { get; set; }

        /// <summary>
        /// Gets the value of the <see cref="IEquivalencyValidationContext.MatchingExpectationProperty" />.
        /// </summary>
        public object Expectation { get; set; }

        /// <summary>
        ///   A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        ///   Zero or more objects to format using the placeholders in <see cref="IEquivalencyValidationContext.Reason" />.
        /// </summary>
        public object[] ReasonArgs { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the current context represents the root of the object graph.
        /// </summary>
        public bool IsRoot
        {
            get { return (SelectedMemberDescription.Length == 0); }
        }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object, then it returns the 
        /// same <see cref="Type"/> as the <see cref="ISubjectInfo.RuntimeType"/> property does.
        /// </summary>
        public Type CompileTimeType { get; set; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        public Type RuntimeType
        {
            get
            {
                if (Subject != null)
                {
                    return Subject.GetType();
                }

                if (SelectedMemberInfo != null)
                {
                    return SelectedMemberInfo.MemberType;
                }

                return CompileTimeType;
            }
        }
    }
}