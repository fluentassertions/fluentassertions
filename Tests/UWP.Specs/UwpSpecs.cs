using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UWP.Specs
{
    [TestClass]
    public class UwpSpecs
    {
        [TestMethod]
        public void Determining_caller_identity_should_not_throw_for_native_programs()
        {
            // Arrange
            Action someAction = () => throw new Exception();

            // Act
            Action act = () => someAction.Should().NotThrow();

            // Assert
            act.Should().Throw<AssertFailedException>();
        }
    }
}
