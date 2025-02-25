using Xunit;
using FluentAssertions;
using FluentValidation;

namespace TokenSchedule.Tests;

public class ScheduleItemTests
{
    public class Constructor
    {
        [Fact]
        internal void WhenFinishTimeLowerThenStartTime_ThrowException()
        {
            var testCode = () => _ = new ScheduleItem(1000000000000000000, DateTime.Now, DateTime.Now.AddDays(-1));

            testCode.Should().Throw<ValidationException>()
                .Which.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("End time must be greater than start time.");
        }

        [Fact]
        internal void WhenRatioIsNegative_ThrowException()
        {
            var testCode = () => _ = new ScheduleItem(-1L, DateTime.Now, DateTime.Now.AddDays(1));

            testCode.Should().Throw<ValidationException>()
                .Which.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Ratio must be a positive number.");
        }
    }
}