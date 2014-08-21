namespace FluentAssertions.Equivalency
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions.Execution;

    internal class GenericDictionaryEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return context.Subject != null
                   && GetIDictionaryInterfaces(context).Any();
        }

        private static Type[] GetIDictionaryInterfaces(EquivalencyValidationContext context)
        {
            return GenericEnumerableEquivalencyStep.GetClosedGenericInterfaces(
                context.Subject.GetType(),
                typeof(IDictionary<,>));
        }

        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Type[] interfaces = GetIDictionaryInterfaces(context);
            bool multipleInterfaces = (interfaces.Count() > 1);

            if (multipleInterfaces)
            {
                AssertionScope.Current.FailWith(
                    string.Format(
                        "{{context:Subject}} is enumerable for more than one type.  "
                        + "It is not known which type should be use for equivalence.{0}"
                        + "The following IDictionary interfaces are implemented: {1}",
                        Environment.NewLine,
                        String.Join(", ", (IEnumerable<Type>)interfaces)));
            }

            return false;
        }
    }
}