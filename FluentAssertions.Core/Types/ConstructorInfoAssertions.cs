using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="MethodInfo"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ConstructorInfoAssertions :
        MethodBaseAssertions<ConstructorInfo, ConstructorInfoAssertions>
    {
        public ConstructorInfoAssertions(ConstructorInfo constructorInfo)
        {
            Subject = constructorInfo;
        }
        
        internal static string GetDescriptionFor(ConstructorInfo constructorInfo)
        {
            return String.Format("{0}({1})",
                constructorInfo.DeclaringType, GetParameterString(constructorInfo));
        }

        internal override string SubjectDescription
        {
            get { return GetDescriptionFor(Subject); }
        }

        protected override string Context
        {
            get { return "constructor"; }
        }
    }
}