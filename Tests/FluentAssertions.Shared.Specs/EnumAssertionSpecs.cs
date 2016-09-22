using System;
using System.Collections.Generic;
using System.Text;
#if !OLD_MSTEST
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Reflection.Emit;
using System.Reflection;

#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    public enum EnumULong : ulong
    {
        UInt64Max = UInt64.MaxValue,
        UInt64LessOne = UInt64.MaxValue-1,
        Int64Max = Int64.MaxValue
    }

    public enum EnumLong : long
    {
        Int64Max = Int64.MaxValue,   
        Int64LessOne = Int64.MaxValue-1    
    }

    
    [TestClass]
    public class EnumAssertionSpecs
    {
        [TestMethod]
        public void When_both_enums_are_equal_and_greater_than_max_long_it_should_not_throw()
        {
            // Arrange
            var enumOne = EnumULong.UInt64Max;
            var enumTwo = EnumULong.UInt64Max;

            // Act
            Action act = () => enumOne.ShouldBeEquivalentTo(enumTwo);

            // Assert
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_both_enums_are_equal_and_of_different_underlying_types_it_should_not_throw()
        {     
            // Arrange
            var enumOne = EnumLong.Int64Max;
            var enumTwo = EnumULong.Int64Max;

            // Act
            Action act = () => enumOne.ShouldBeEquivalentTo(enumTwo);

            // Assert
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_both_enums_are_large_and_not_equal_it_should_throw()
        {   
            // Arrange
            var enumOne = EnumLong.Int64LessOne;
            var enumTwo = EnumULong.UInt64Max;

            // Act
            Action act = () => enumOne.ShouldBeEquivalentTo(enumTwo);

            // Assert
            act.ShouldThrow<AssertFailedException>();
        }

    }
}

