using Xunit;
using FluentAssertions;

namespace TokenSchedule.Tests;

public class ScheduleItemTests
{
    public class Constructor
    {
        [Fact]
        internal void WhenFinishTimeLowerThenStartTime_ThrowException()
        {
            var testCode = () => _ = new ScheduleItem(0.1m, DateTime.Now, DateTime.Now.AddDays(-1));

            testCode.Should().Throw<ArgumentException>()
                .WithMessage("End time must be greater than start time. (Parameter 'startDate')");
        }

        [Fact]
        internal void WhenRatioIsNegative_ThrowException()
        {
            var testCode = () => _ = new ScheduleItem(-0.1m, DateTime.Now, DateTime.Now.AddDays(1));

            testCode.Should().Throw<ArgumentException>()
                .WithMessage("Ratio must be positive. (Parameter 'ratio')");
        }
    }
}