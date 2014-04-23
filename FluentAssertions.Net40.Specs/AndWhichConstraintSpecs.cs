using System;

using FluentAssertions.Collections;

#if WINRT || WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class AndWhichConstraintSpecs
    {
        [TestMethod]
        public void When_many_objects_are_provided_accesing_which_should_throw_a_descriptive_exception()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var continuation = new AndWhichConstraint<StringCollectionAssertions, string>(null, new[] {"hello", "world"});

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
            {
                var item = continuation.Which;
            };

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<InvalidOperationException>()
                .WithMessage(
                    "More than one object found.  FluentAssertions cannot determine which object is meant.*")
                .WithMessage("*Found objects:\r\n\t\"hello\"\r\n\t\"world\"");
        }
    }
}
