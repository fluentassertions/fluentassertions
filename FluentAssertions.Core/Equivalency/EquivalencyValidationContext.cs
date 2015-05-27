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

        public SelectedMemberInfo SelectedMemberInfo { get; set; }

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
            get
            {
                // SMELL: That prefix should be obtained from some kind of constant
                return (SelectedMemberDescription.Length == 0) ||
                       (RootIsCollection && SelectedMemberDescription.StartsWith("item[") && !SelectedMemberDescription.Contains("."));
            }
        }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object, then it returns the 
        /// same <see cref="System.Type"/> as the <see cref="ISubjectInfo.RuntimeType"/> property does.
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

        /// <summary>
        /// Gets or sets a value indicating that the root of the graph is a collection so all type-specific options apply on 
        /// the collection type and not on the root itself.
        /// </summary>
        public bool RootIsCollection { get; set; }

        public override string ToString()
        {
            return string.Format("{{Path=\"{0}\", Subject={1}, Expectation={2}}}", SelectedMemberDescription, Subject, Expectation);
        }
    }
}