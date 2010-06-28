using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class GenericCollectionAssertionsSpecs
    {
        [TestMethod]
        public void When_asserting_collection_contains_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            List<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().Contain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Expected collection to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }

        [TestMethod]
        public void When_asserting_collection__doesnt_contain_values_according_to_predicate_but_collection_is_null_it_should_throw()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            List<string> strings = null;

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => strings.Should().NotContain(x => x == "xxx", "because we're checking how it reacts to a null subject");

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<SpecificationMismatchException>().WithMessage(
                "Expected collection not to contain (x == \"xxx\") because we're checking how it reacts to a null subject, but found <null>.");
        }
    }
}
