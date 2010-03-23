using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class DateTimeAssertionSpecs
    {
        private DateTime Today { get; set; }
        private DateTime Yesterday { get; set; }
        private DateTime Tomorrow { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            Today = DateTime.Today;
            Yesterday = Today.AddDays(-1);
            Tomorrow = Today.AddDays(1);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_value_is_equal_to_the_same_value()
        {
            Today.Should().Be(Today);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_value_is_equal_to_the_different_value()
        {
            Today.Should().Be(Tomorrow);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_value_is_equal_to_the_different_value()
        {
            var assertions = Today.Should();
            assertions.ShouldThrow(x => x.Be(Tomorrow, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(string.Format(
                                 "Expected <{0}> because we want to test the failure message, but found <{1}>.", Tomorrow, Today));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_before_later_datetime()
        {
            Today.Should().BeBefore(Tomorrow);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_is_before_earlier_datetime()
        {
            Today.Should().BeBefore(Yesterday);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_before_earlier_datetime()
        {
            var assertions = Today.Should();
            assertions.ShouldThrow(x => x.BeBefore(Yesterday, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(string.Format(
                                 "Expected a date/time before <{0}> because we want to test the failure message, but found <{1}>.",
                                 Yesterday, Today));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_before_later_datetime()
        {
            Today.Should().BeOnOrBefore(Tomorrow);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_before_the_same_datetime()
        {
            Today.Should().BeOnOrBefore(Today);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_is_on_or_before_earlier_datetime()
        {
            Today.Should().BeOnOrBefore(Yesterday);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_on_or_before_earlier_datetime()
        {
            var assertions = Today.Should();
            assertions.ShouldThrow(x => x.BeOnOrBefore(Yesterday, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(string.Format(
                                 "Expected a date/time on or before <{0}> because we want to test the failure message, but found <{1}>.",
                                 Yesterday, Today));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_after_earlier_datetime()
        {
            Today.Should().BeAfter(Yesterday);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_is_after_later_datetime()
        {
            Today.Should().BeAfter(Tomorrow);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_after_later_datetime()
        {
            var assertions = Today.Should();
            assertions.ShouldThrow(x => x.BeAfter(Tomorrow, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(string.Format(
                                 "Expected a date/time after <{0}> because we want to test the failure message, but found <{1}>.",
                                 Tomorrow, Today));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_after_earlier_datetime()
        {
            Today.Should().BeOnOrAfter(Yesterday);
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_is_on_or_after_the_same_datetime()
        {
            Today.Should().BeOnOrAfter(Today);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_is_on_or_after_later_datetime()
        {
            Today.Should().BeOnOrAfter(Tomorrow);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_is_on_or_after_later_datetime()
        {
            var assertions = Today.Should();
            assertions.ShouldThrow(x => x.BeOnOrAfter(Tomorrow, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage(string.Format(
                                 "Expected a date/time on or after <{0}> because we want to test the failure message, but found <{1}>.",
                                 Tomorrow, Today));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_year_with_the_same_value()
        {
            new DateTime(2009, 12, 31).Should().HaveYear(2009);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_has_a_year_with_a_different_value()
        {
            new DateTime(2009, 12, 31).Should().HaveYear(2008);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_year_with_a_different_value()
        {
            var assertions = new DateTime(2009, 12, 31).Should();
            assertions.ShouldThrow(x => x.HaveYear(2008, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected year <2008> because we want to test the failure message, but found <2009>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_month_with_the_same_value()
        {
            new DateTime(2009, 12, 31).Should().HaveMonth(12);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_has_a_month_with_a_different_value()
        {
            new DateTime(2009, 12, 31).Should().HaveMonth(11);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_month_with_a_different_value()
        {
            var assertions = new DateTime(2009, 12, 31).Should();
            assertions.ShouldThrow(x => x.HaveMonth(11, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected month <11> because we want to test the failure message, but found <12>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_a_day_with_the_same_value()
        {
            new DateTime(2009, 12, 31).Should().HaveDay(31);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_has_a_day_with_a_different_value()
        {
            new DateTime(2009, 12, 31).Should().HaveDay(30);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_a_day_with_a_different_value()
        {
            var assertions = new DateTime(2009, 12, 31).Should();
            assertions.ShouldThrow(x => x.HaveDay(30, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected day <30> because we want to test the failure message, but found <31>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_an_hour_with_the_same_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveHour(23);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_has_an_hour_with_different_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveHour(22);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_an_hour_with_different_value()
        {
            var assertions = new DateTime(2009, 12, 31, 23, 59, 00).Should();
            assertions.ShouldThrow(x => x.HaveHour(22, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected hour <22> because we want to test the failure message, but found <23>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_minutes_with_the_same_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveMinute(59);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_has_minutes_with_different_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveMinute(58);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_minutes_with_different_value()
        {
            var assertions = new DateTime(2009, 12, 31, 23, 59, 00).Should();
            assertions.ShouldThrow(x => x.HaveMinute(58, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected minute <58> because we want to test the failure message, but found <59>.");
        }

        [TestMethod]
        public void Should_succeed_when_asserting_datetime_has_seconds_with_the_same_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveSecond(0);
        }

        [TestMethod]
        [ExpectedException(typeof (SpecificationMismatchException))]
        public void Should_fail_when_asserting_datetime_has_seconds_with_different_value()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should().HaveSecond(1);
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_datetime_has_seconds_with_different_value()
        {
            var assertions = new DateTime(2009, 12, 31, 23, 59, 00).Should();
            assertions.ShouldThrow(x => x.HaveSecond(1, "because we want to test the failure {0}", "message"))
                .Exception<SpecificationMismatchException>()
                .WithMessage("Expected second <1> because we want to test the failure message, but found <0>.");
        }

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            new DateTime(2009, 12, 31, 23, 59, 00).Should()
                .HaveYear(2009)
                .And
                .HaveDay(31);
        }
    }
}