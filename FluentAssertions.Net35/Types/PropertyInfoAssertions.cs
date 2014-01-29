using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types
{
    [DebuggerNonUserCode]
    public class PropertyInfoAssertions :
        ReferenceTypeAssertions<PropertyInfo, PropertyInfoAssertions>
    {
        public PropertyInfoAssertions(PropertyInfo propertyInfo)
        {
            this.Subject = propertyInfo;
        }

        protected override string Context
        {
            get { return "property info"; }
        }

        public AndConstraint<PropertyInfoAssertions> BeVirtual(
            string reason = "", params object[] reasonArgs)
        {
            string failureMessage = "Expected property " +
                                    GetDescriptionFor(Subject) +
                                    " to be virtual{reason}, but it is not virtual.";

            Execute.Assertion
                .ForCondition(!IsGetterNonVirtual(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        public AndConstraint<PropertyInfoAssertions> BeWritable(
            string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CanWrite)
                .BecauseOf(reason, reasonArgs)
                .FailWith(
                    "Expected {context:property} {0} to have a setter{reason}.",
                    Subject);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        public AndConstraint<PropertyInfoAssertions> BeDecoratedWith
            <TAttribute>(string reason = "", params object[] reasonArgs)
        {
            string failureMessage = "Expected property " +
                                    GetDescriptionFor(Subject) +
                                    " to be decorated with {0}{reason}, but that attribute was not found.";

            Execute.Assertion
                .ForCondition(IsDecoratedWith<TAttribute>(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith(failureMessage, typeof (TAttribute));

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        internal static bool IsDecoratedWith<TAttribute>(PropertyInfo property)
        {
            return property.GetCustomAttributes(false).OfType<TAttribute>().Any();
        }

        internal static bool IsGetterNonVirtual(PropertyInfo property)
        {
            MethodInfo getter = property.GetGetMethod(true);
            return MethodInfoAssertions.IsNonVirtual(getter);
        }

        internal static string GetDescriptionFor(PropertyInfo property)
        {
            string propTypeName = property.PropertyType.Name;
            return String.Format("{0} {1}.{2}", propTypeName,
                property.DeclaringType, property.Name);
        }
    }
}