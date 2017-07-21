using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Collections
{
    public class StringCollectionAssertions :
        SelfReferencingCollectionAssertions<string, StringCollectionAssertions>
    {
        public StringCollectionAssertions(IEnumerable<string> actualValue)
            : base(actualValue)
        {
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public new AndConstraint<StringCollectionAssertions> Equal(params string[] expected)
        {
            return base.Equal(expected.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain all the same elements in the same order as the collection identified by 
        /// <paramref name="expected" />. Elements are compared using their <see cref="object.Equals(object)" />.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<StringCollectionAssertions> Equal(IEnumerable<string> expected)
        {
            return base.Equal(expected);
        }
        
        /// <summary>
        /// Asserts that a collection of objects is equivalent to another collection of objects. 
        /// </summary>
        /// <remarks>
        /// Objects within the collections are equivalent when both object graphs have equally named properties with the same 
        /// value, irrespective of the type of those objects. Two properties are also equal if one type can be converted to another 
        /// and the result is equal. 
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
        /// </remarks>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public void BeEquivalentTo(IEnumerable<string> expectation, string because = "", params object[] becauseArgs)
        {
            BeEquivalentTo(expectation, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a collection of objects is equivalent to another collection of objects. 
        /// </summary>
        /// <remarks>
        /// Objects within the collections are equivalent when both object graphs have equally named properties with the same 
        /// value,  irrespective of the type of those objects. Two properties are also equal if one type can be converted to another 
        /// and the result is equal. 
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable"/> and all
        /// items in the collection are structurally equal. 
        /// </remarks>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used 
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the 
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the 
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the 
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public void BeEquivalentTo(IEnumerable<string> expectation,
            Func<EquivalencyAssertionOptions<string>, EquivalencyAssertionOptions<string>> config, string because = "",
            params object[] becauseArgs)
        {
            EquivalencyAssertionOptions<IEnumerable<string>> options = config(AssertionOptions.CloneDefaults<string>()).AsCollection();

            var context = new EquivalencyValidationContext
            {
                Subject = Subject,
                Expectation = expectation,
                RootIsCollection = true,
                CompileTimeType = typeof(IEnumerable<string>),
                Because = because,
                BecauseArgs = becauseArgs,
                Tracer = options.TraceWriter
            };

            new EquivalencyValidator(options).AssertEquality(context);
        }


        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<StringCollectionAssertions> ContainInOrder(params string[] expected)
        {
            return base.ContainInOrder(expected.AsEnumerable());
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in the exact same order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> ContainInOrder(IEnumerable<string> expected, string because = "",
            params object[] becauseArgs)
        {
            return base.ContainInOrder(expected, because, becauseArgs);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expected)
        {
            return base.Contain(expected);
        }

        /// <summary>
        /// Expects the current collection to contain the specified elements in any order. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">An <see cref="IEnumerable"/> with the expected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> Contain(IEnumerable<string> expected, string because = null,
            object becauseArg = null,
            params object[] becauseArgs)
        {
            var args = new List<object> {becauseArg};
            args.AddRange(becauseArgs);
            return base.Contain(expected, because, args.ToArray());
        }

        /// <summary>
        /// Asserts that the current collection does not contain the supplied items. Elements are compared
        /// using their <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">An <see cref="IEnumerable"/> with the unexpected elements.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<StringCollectionAssertions> NotContain(IEnumerable<string> unexpected, string because = null,
            object becauseArg = null,
            params object[] becauseArgs)
        {
            var args = new List<object> { becauseArg };
            args.AddRange(becauseArgs);
            return base.NotContain(unexpected, because, args.ToArray());
        }
    }
}