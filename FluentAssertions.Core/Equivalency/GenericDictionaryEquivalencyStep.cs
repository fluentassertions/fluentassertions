using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    internal class GenericDictionaryEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type subjectType = EnumerableEquivalencyStep.GetSubjectType(context, config);

            return context.Subject != null
                   && GetIDictionaryInterfaces(subjectType).Any();
        }

        private static Type[] GetIDictionaryInterfaces(Type type)
        {
            return GenericEnumerableEquivalencyStep.GetClosedGenericInterfaces(
                type,
                typeof(IDictionary<,>));
        }

        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Type[] interfaces = GetIDictionaryInterfaces(context.Subject.GetType());
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

            if (PreconditionsAreMet(context.Expectation))
            {
            }

            return false;
        }

        private static bool PreconditionsAreMet(object expectation)
        {
            return AssertIsDictionary(expectation);
        }

        private static bool AssertIsDictionary(object expectation)
        {
            Type dictionaryInterface = GetIDictionaryInterfaces(expectation.GetType()).SingleOrDefault();

            return
                AssertionScope.Current.ForCondition(dictionaryInterface != null)
                    .FailWith("{context:subject} is a dictionary and cannot be compared with a non-dictionary type.");
        }
    }
}