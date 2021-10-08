using System;
using System.Collections.Generic;
using Xunit;

namespace FluentAssertions.Specs.Equivalency
{
    public class TypeEquivalencySpecs
    {
        [Fact]
        public void SameTypes_AndProperties_AreEquivalent()
        {
            State a1 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State a2 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));

            a1
                .Should()
                .BeSameTypeEquivalentTo(a2);
        }

        [Fact]
        public void EnumerableSameTypes_AndProperties_AreInOrderEquivalent()
        {
            State a1 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State b1 = new State.B("Green", 800, 590, new TestModel("testing", 589));

            State a2 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State b2 = new State.B("Green", 800, 590, new TestModel("testing", 589));

            new List<State> { a1, b1 }
                .Should()
                .BeInOrderSameTypeEquivalentTo(a2, b2);
        }

        [Fact]
        public void SameTypes_DifferentValueTypeProperty_AreNotEquivalent()
        {
            State a1 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State a2 = new State.A("Yellow", 56, 700, new TestModel("test", 1234));

            Assert.ThrowsAny<Exception>(
                () => a1.Should().BeSameTypeEquivalentTo(a2));
        }

        [Fact]
        public void SameTypes_DifferentReferenceTypeProperty_AreNotEquivalent()
        {
            State a1 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State a2 = new State.A("Green", 56, 58, new TestModel("test", 1234));

            Assert.ThrowsAny<Exception>(
                () => a1.Should().BeSameTypeEquivalentTo(a2));
        }

        [Fact]
        public void SameTypes_DifferentSubModelProperty_AreNotEquivalent()
        {
            State a1 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State a2 = new State.A("Green", 56, 58, new TestModel("hello", 1234));

            Assert.ThrowsAny<Exception>(
                () => a1.Should().BeSameTypeEquivalentTo(a2));
        }

        [Fact]
        public void EnumerableSameTypes_DifferentProperties_AreNotEquivalent()
        {
            State a1 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State b1 = new State.B("Green", 800, 590, new TestModel("testing", 589));

            State a2 = new State.A("Yellow", 56, 58, new TestModel("test", 1234));
            State b2 = new State.B("Blue", 801, 590, new TestModel("testing", 589));

            Assert.ThrowsAny<Exception>(() =>
                new List<State> { a1, b1 }
                    .Should()
                    .BeInOrderSameTypeEquivalentTo(a2, b2));
        }

        [Fact]
        public void DifferentTypes_NoProperties_AreNotEquivalent()
        {
            State stateC = new State.C();
            State stateD = new State.D();

            Assert.ThrowsAny<Exception>(
                () => stateC.Should().BeSameTypeEquivalentTo(stateD));
        }

        [Fact]
        public void SameTypes_NoProperties_AreEquivalent()
        {
            State stateC1 = new State.C();
            State stateC2 = new State.C();

            stateC1.Should().BeSameTypeEquivalentTo(stateC2);
        }

        private abstract class State
        {
            public class A : State
            {
                public string Color { get; }

                public int Count { get; }

                public double Height { get; }

                public TestModel TestModel { get; }

                public A(string color, int count, double height, TestModel testModel)
                {
                    Color = color;
                    Count = count;
                    Height = height;
                    TestModel = testModel;
                }
            }

            public class B : State
            {
                public string Color { get; }

                public int Count { get; }

                public double Height { get; }

                public TestModel TestModel { get; }

                public B(string color, int count, double height, TestModel testModel)
                {
                    Color = color;
                    Count = count;
                    Height = height;
                    TestModel = testModel;
                }
            }

            public class C : State { }

            public class D : State { }
        }

        private class TestModel
        {
            public string StringValue { get; }

            public int IntValue { get; }

            public TestModel(string stringValue, int intValue)
            {
                StringValue = stringValue;
                IntValue = intValue;
            }
        }
    }
}
