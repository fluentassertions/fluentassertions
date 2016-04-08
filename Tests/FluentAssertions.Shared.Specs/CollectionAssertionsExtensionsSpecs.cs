using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System;

namespace FluentAssetions.Shared.Specs
{
    [TestClass]
    public class CollectionAssertionsExtensionsTests
    {
        #region ShouldRaiseException

        [TestMethod]
        public void NotBeEquivallentTo_GivenTheSameArrayTwice_ShouldRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ComplexType[] subject = new ComplexType[3]
                { new ComplexType() { X = 1 }
                , new ComplexType() { X = 2 }
                , new ComplexType() { X = 3 } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void NotBeEquivallentTo_GivenTwoArraysWithSameElementsValues_ShouldRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------

            ComplexType[] subject = new ComplexType[2]
                { new ComplexType() { X = 1, Name = "Foo" }
                , new ComplexType() { X = 2, Name = "Bar" } };
            ComplexType[] subject2 = new ComplexType[2]
                { new ComplexType() { X = 1, Name = "Foo" }
                , new ComplexType() { X = 2, Name = "Bar" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject2, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void NotBeEquivallentTo_GivenTwoArraysWithSameElementsRefrences_ShouldRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------

            ComplexType[] subject = new ComplexType[2]
                { new ComplexType() { X = 1, Name = "Foo" }
                , new ComplexType() { X = 2, Name = "Bar" } };
            ComplexType[] subject2 = new ComplexType[2]
                { subject[0]
                , subject[1] };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject2, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        [TestMethod]
        public void NotBeEquivallentTo_GivenTwoArraysWithSameElementsByPropertyButDifferentInOtherProperties_ShouldRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------

            ComplexType[] subject = new ComplexType[2]
                { new ComplexType() { X = 1, Name = "Foo" }
                , new ComplexType() { X = 2, Name = "Bar" } };
            ComplexType[] subject2 = new ComplexType[2]
                { new ComplexType() { X = 1, Name = "Hello" }
                , new ComplexType() { X = 2, Name = "World" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject2, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>();
        }

        #endregion

        #region ShouldNotRaiseException

        [TestMethod]
        public void NotBeEquivallentTo_GivenDifferentArrays_ShouldNotRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ComplexType[] subject = new ComplexType[3]
            { new ComplexType() { X = 1 }
            , new ComplexType() { X = 2 }
            , new ComplexType() { X = 3 } };

            ComplexType[] subject2 = new ComplexType[3]
            { new ComplexType() { X = 2 }
            , new ComplexType() { X = 3 }
            , new ComplexType() { X = 4 } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject2, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow<AssertFailedException>();
        }

        [TestMethod]
        public void NotBeEquivallentTo_GivenArrayAndSubArray_ShouldNotRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ComplexType[] subject = new ComplexType[3]
            { new ComplexType() { X = 1 }
            , new ComplexType() { X = 2 }
            , new ComplexType() { X = 3 } };

            ComplexType[] subject2 = new ComplexType[2]
            { new ComplexType() { X = 1 }
            , new ComplexType() { X = 2 } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject2, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow<AssertFailedException>();
        }

        [TestMethod]
        public void NotBeEquivallentTo_GivenDifferentArrayInOnlyOneElement_ShouldNotRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------

            ComplexType[] subject = new ComplexType[3]
            { new ComplexType() { X = 1 }
            , new ComplexType() { X = 2 }
            , new ComplexType() { X = 3 } };

            ComplexType[] subject2 = new ComplexType[3]
            { new ComplexType() { X = 1 }
            , new ComplexType() { X = 2 }
            , new ComplexType() { X = 4 } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject2, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow<AssertFailedException>();
        }

        [TestMethod]
        public void NotBeEquivallentTo_GivenDifferentArrayOnOnlyTheRequestedProperty_ShouldNotRaiseException()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            ComplexType[] subject = new ComplexType[3]
            { new ComplexType() { X = 1, Name = "Foo" }
            , new ComplexType() { X = 2, Name = "Foo" }
            , new ComplexType() { X = 3, Name = "Foo" } };

            ComplexType[] subject2 = new ComplexType[3]
            { new ComplexType() { X = 1, Name = "Foo" }
            , new ComplexType() { X = 2, Name = "Foo" }
            , new ComplexType() { X = 4, Name = "Foo" } };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().NotBeEquivalentTo(subject2, (item) => item.X);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow<AssertFailedException>();
        }

        #endregion
    }
    #region Test Classes

    class ComplexType
    {
        public int X { get; set; }

        public string Name { get; set; }
    }

    #endregion
}
