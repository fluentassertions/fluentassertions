using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    public class EquivalencyValidator : IEquivalencyValidator
    {
        #region Private Definitions

        private readonly IEquivalencyAssertionOptions config;

        private readonly List<IEquivalencyStep> steps = new List<IEquivalencyStep>
        {
            new TryConversionEquivalencyStep(),
            new ReferenceEqualityEquivalencyStep(),
            new ApplyAssertionRulesEquivalencyStep(),
            new DictionaryEquivalencyStep(),
            new EnumerableEquivalencyStep(),
            new ComplexTypeEquivalencyStep(),
            new SimpleEqualityEquivalencyStep()
        };

        #endregion

        public EquivalencyValidator(IEquivalencyAssertionOptions config)
        {
            this.config = config;
        }

        /// <summary>
        /// Provides access the list of steps that are executed in the order of appearance during an equivalency test.
        /// </summary>
        public IList<IEquivalencyStep> Steps
        {
            get { return steps; }
        }

        public void AssertEquality(EquivalencyValidationContext context)
        {
            using (var scope = new AssertionScope())
            {
                scope.AddReportable("configuration", config.ToString());
                scope.AddNonReportable("objects", new ObjectTracker(config.CyclicReferenceHandling));

                scope.BecauseOf(context.Reason, context.ReasonArgs);

                AssertEqualityUsing(context);
            }
        }

        public void AssertEqualityUsing(EquivalencyValidationContext context)
        {
            AssertionScope scope = AssertionScope.Current;
            scope.AddNonReportable("context", context.IsRoot ? "subject" : context.PropertyDescription);
            scope.AddNonReportable("subject", context.Subject);
            scope.AddNonReportable("expectation", context.Expectation);

            var objectTracker = scope.Get<ObjectTracker>("objects");

            if (!objectTracker.IsCyclicReference(new ObjectReference(context.Subject, context.PropertyPath)))
            {
                foreach (IEquivalencyStep strategy in steps.Where(s => s.CanHandle(context, config)))
                {
                    if (strategy.Handle(context, this, config))
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Keeps track of objects and their location within an object graph so that cyclic references can be detected
    /// and handled upon.
    /// </summary>
    internal class ObjectTracker : ICloneable2
    {
        #region Private Definitions

        private readonly CyclicReferenceHandling handling;
        private List<ObjectReference> references = new List<ObjectReference>();

        #endregion

        public ObjectTracker(CyclicReferenceHandling handling)
        {
            this.handling = handling;
        }

        /// <summary>
        /// Determines whether the specified object reference is a cyclic reference to the same object earlier in the 
        /// equivalency validation.
        /// </summary>
        /// <remarks>
        /// The behavior of a cyclic reference is determined byt he <see cref="CyclicReferenceHandling"/> constructor
        /// parameter.
        /// </remarks>
        public bool IsCyclicReference(ObjectReference reference)
        {
            bool isCyclic = false;

            if (reference.IsReference)
            {
                if (references.Contains(reference))
                {
                    isCyclic = true;
                    if (handling == CyclicReferenceHandling.ThrowException)
                    {
                        AssertionScope.Current.FailWith(
                            "Expected {context:subject} to be {expectation}{reason}, but it contains a cyclic reference.");
                    }
                }
                else
                {
                    references.Add(reference);
                }
            }

            return isCyclic;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// 
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new ObjectTracker(handling)
            {
                references = references.ToList()
            };
        }
    }

    /// <summary>
    /// Represents  an object tracked by the <see cref="ObjectTracker"/> including it's location within an object graph.
    /// </summary>
    internal class ObjectReference
    {
        private readonly object @object;
        private readonly string propertyPath;

        public ObjectReference(object @object, string propertyPath)
        {
            this.@object = @object;
            this.propertyPath = propertyPath;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var other = (ObjectReference)obj;

            return ReferenceEquals(@object, other.@object) &&
                   !string.Equals(propertyPath, other.propertyPath);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (@object.GetHashCode() * 397) ^ propertyPath.GetHashCode();
            }
        }

        public bool IsReference
        {
            get { return !ReferenceEquals(@object, null) && @object.GetType().IsComplexType(); }
        }
    }
}