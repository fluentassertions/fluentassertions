using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif NUNIT
using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
using TestMethodAttribute = NUnit.Framework.TestCaseAttribute;
using AssertFailedException = NUnit.Framework.AssertionException;
using TestInitializeAttribute = NUnit.Framework.SetUpAttribute;
using Assert = NUnit.Framework.Assert;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace FluentAssertions.Specs
{
    [TestClass]
    public class ObjectCastingSpecs
    {
        [TestMethod]
        public void When_casting_an_object_using_the_as_operator_it_should_return_the_expected_type()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            SomeBaseClass baseInstance = new SomeDerivedClass
            {
                DerivedProperty = "hello"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            SomeDerivedClass derivedInstance = baseInstance.As<SomeDerivedClass>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            derivedInstance.DerivedProperty.Should().Be("hello");
        }
        
        private class SomeBaseClass
        {
            
        }

        private class SomeDerivedClass : SomeBaseClass
        {
            public string DerivedProperty { get; set; }
        }
    }
}
