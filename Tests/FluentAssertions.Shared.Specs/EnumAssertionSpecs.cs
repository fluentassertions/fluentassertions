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
        public void ShouldBeEquivalentTo_should_succeed_when_asserting_large_enum_equals_large_enum()
        {
            Action act = () => EnumULong.UInt64Max.ShouldBeEquivalentTo(EnumULong.UInt64Max);
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldBeEquivalentTo_should_succeed_when_asserting_large_enum_equals_large_enum_of_different_underlying_types()
        {
            Action act = () => EnumLong.Int64Max.ShouldBeEquivalentTo(EnumULong.Int64Max);
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void ShouldBeEquivalentTo_should_fail_when_asserting_large_enum_equals_different_large_enum_of_different_underlying_types()
        {
            Action act = () => EnumLong.Int64LessOne.ShouldBeEquivalentTo(EnumULong.UInt64Max);
            act.ShouldThrow<AssertFailedException>();
        }

    }
}

