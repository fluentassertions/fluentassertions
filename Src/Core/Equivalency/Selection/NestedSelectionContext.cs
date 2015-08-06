using System;
using System.Reflection;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency.Selection
{
    /// <summary>
    /// Represents a selection context of a nested property
    /// </summary>
    internal class NestedSelectionContext : ISubjectInfo
    {
        public NestedSelectionContext(ISubjectInfo context, SelectedMemberInfo selectedMemberInfo)
        {
            SelectedMemberPath = context.SelectedMemberPath.Combine(selectedMemberInfo.Name);
            SelectedMemberDescription = context.SelectedMemberDescription.Combine(selectedMemberInfo.Name);
            CompileTimeType = selectedMemberInfo.MemberType;
            RuntimeType = selectedMemberInfo.MemberType;
            SelectedMemberInfo = selectedMemberInfo;
        }

        public SelectedMemberInfo SelectedMemberInfo { get; private set; }

        public string SelectedMemberPath { get; private set; }

        public string SelectedMemberDescription { get; private set; }

        /// <summary>
        /// Gets the compile-time type of the current object. If the current object is not the root object, then it returns the 
        /// same <see cref="System.Type"/> as the <see cref="ISubjectInfo.RuntimeType"/> property does.
        /// </summary>
        public Type CompileTimeType { get; private set; }

        /// <summary>
        /// Gets the run-time type of the current object.
        /// </summary>
        public Type RuntimeType { get; private set; }
    }
}