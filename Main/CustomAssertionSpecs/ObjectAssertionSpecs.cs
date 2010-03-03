using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.specs
{
    [TestClass]
    public class ObjectAssertionSpecs
    {
        [TestMethod]
        public void Should_succeed_when_asserting_object_to_be_equal_to_an_equal_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);
            someObject.Should().Equal(equalObject);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_object_to_be_equal_to_non_equal_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);
            someObject.Should().Equal(nonEqualObject);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_object_to_be_equal_to_non_equal_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);
            var assertions = someObject.Should();
            assertions.ShouldThrow(x => x.Equal(nonEqualObject, "because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage(
                "Expected <ClassWithCustomEqualMethod(2)> because we want to test the failure message, but found <ClassWithCustomEqualMethod(1)>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_not_to_be_equal_to_non_equal_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);
            someObject.Should().NotEqual(nonEqualObject);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_object_not_to_be_equal_to_equal_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);
            someObject.Should().NotEqual(equalObject);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_object_not_to_be_equal_to_equal_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);
            var assertions = someObject.Should();
            assertions.ShouldThrow(x => x.NotEqual(equalObject, "because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage("Did not expect <ClassWithCustomEqualMethod(1)> because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_to_be_the_same_as_the_same_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var sameObject = someObject;
            someObject.Should().BeSameAs(sameObject);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_object_to_be_the_same_as_a_different_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var notSameObject = new ClassWithCustomEqualMethod(1);
            someObject.Should().BeSameAs(notSameObject);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_object_to_be_the_same_as_a_different_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var notSameObject = new ClassWithCustomEqualMethod(1);
            var assertions = someObject.Should();
            assertions.ShouldThrow(x => x.BeSameAs(notSameObject, "because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage("Expected the exact same objects because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_to_not_be_the_same_as_a_different_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            var notSameObject = new ClassWithCustomEqualMethod(1);
            someObject.Should().NotBeSameAs(notSameObject);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_object_to_not_be_the_same_as_the_same_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            ClassWithCustomEqualMethod sameObject = someObject;
            someObject.Should().NotBeSameAs(sameObject);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_object_to_not_be_the_same_as_the_same_object()
        {
            var someObject = new ClassWithCustomEqualMethod(1);
            ClassWithCustomEqualMethod sameObject = someObject;
            var assertions = someObject.Should();
            assertions.ShouldThrow(x => x.NotBeSameAs(sameObject, "because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage("Expected different objects because we want to test the failure message.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_null_object_to_be_null()
        {
            object someObject = null;
            someObject.Should().BeNull();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_non_null_object_to_be_null()
        {
            var someObject = new object();
            someObject.Should().BeNull();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_non_null_object_to_be_null()
        {
            var someObject = new object();
            var assertions = someObject.Should();
            assertions.ShouldThrow(x => x.BeNull("because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage("Expected <null> because we want to test the failure message, but found <System.Object>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_non_null_object_not_to_be_null()
        {
            var someObject = new object();
            someObject.Should().NotBeNull();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_null_object_not_to_be_null()
        {
            object someObject = null;
            someObject.Should().NotBeNull();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_null_object_not_to_be_null()
        {
            object someObject = null;
            var assertions = someObject.Should();
            assertions.ShouldThrow(x => x.NotBeNull("because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage("Expected non-null value because we want to test the failure message, but found <null>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_type_to_be_equal_to_the_same_type()
        {
            var someObject = new Exception();
            someObject.Should().BeOfType<Exception>();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void Should_fail_when_asserting_object_type_to_be_equal_to_a_different_type()
        {
            var someObject = new object();
            someObject.Should().BeOfType<Exception>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_object_type_to_be_equal_to_a_different_type()
        {
            var someObject = new object();
            var assertions = someObject.Should();
            assertions.ShouldThrow(x => x.BeOfType<Exception>("because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage(
                "Expected type <System.Exception> because we want to test the failure message, but found <System.Object>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_assignable_to_for_same_type()
        {
            var someObject = new DummyImplementingClass();
            someObject.Should().BeAssignableTo<DummyImplementingClass>();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_assignable_to_base_type()
        {
            var someObject = new DummyImplementingClass();
            someObject.Should().BeAssignableTo<DummyBaseClass>();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_assignable_to_implemented_interface_type()
        {
            var someObject = new DummyImplementingClass();
            someObject.Should().BeAssignableTo<IDisposable>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_object_assignable_to_not_implemented_type()
        {
            var someObject = new DummyImplementingClass();

            someObject.ShouldThrow(x => x.Should().BeAssignableTo<DateTime>("because we want to test the failure {0}", "message"))
                .Exception<AssertFailedException>()
                .And.WithMessage(string.Format(
                "Expected to be assignable to <{1}> because we want to test the failure message, but <{0}> does not implement <{1}>",
                typeof(DummyImplementingClass), typeof(DateTime)));
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            var someObject = new Exception();
            someObject.Should()
                .BeOfType<Exception>()
                .And
                .NotBeNull();
        }

        #region Nested type: ClassWithCustomEqualMethod

        private class ClassWithCustomEqualMethod
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            public ClassWithCustomEqualMethod(int key)
            {
                Key = key;
            }

            private int Key { get; set; }

            private bool Equals(ClassWithCustomEqualMethod other)
            {
                if (ReferenceEquals(null, other))
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                return other.Key == Key;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != typeof(ClassWithCustomEqualMethod))
                    return false;
                return Equals((ClassWithCustomEqualMethod)obj);
            }

            public override int GetHashCode()
            {
                return Key;
            }

            public static bool operator ==(ClassWithCustomEqualMethod left, ClassWithCustomEqualMethod right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ClassWithCustomEqualMethod left, ClassWithCustomEqualMethod right)
            {
                return !Equals(left, right);
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override string ToString()
            {
                return string.Format("ClassWithCustomEqualMethod({0})", Key);
            }
        }

        #endregion

        private class DummyBaseClass { }

        private class DummyImplementingClass : DummyBaseClass, IDisposable
        {
            public void Dispose()
            {
                // Ignore
            }
        }
    }
}