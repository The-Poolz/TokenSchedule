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
            var testCode = () => _ = new ScheduleItem(0.1m, DateTime.Now, DateTime.Now.AddDays(-1));

            testCode.Should().Throw<ValidationException>()
                .Which.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("End time must be greater than start time.");
            testCode.Should().Throw<ValidationException>()
                .WithMessage("*End time must be greater than start time.*");
        }

        [Fact]
        internal void WhenRatioIsNegative_ThrowException()
        {
            var testCode = () => _ = new ScheduleItem(-0.1m, DateTime.Now, DateTime.Now.AddDays(1));

            testCode.Should().Throw<ValidationException>()
                .Which.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Ratio must be greater than or equal to 1e-18.");
        }
    }
}