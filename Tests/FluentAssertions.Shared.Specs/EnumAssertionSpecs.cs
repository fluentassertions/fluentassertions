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
        public void Should_succeed_when_asserting_large_enum_equals_large_enum()
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
        public void Should_succeed_when_asserting_large_enum_equals_large_enum_of_different_underlying_types()
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
        public void Sshould_fail_when_asserting_large_enum_equals_different_large_enum_of_different_underlying_types()
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

