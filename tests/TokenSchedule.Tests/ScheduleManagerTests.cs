using Xunit;
using FluentAssertions;
using FluentValidation;

namespace TokenSchedule.Tests;

public class ScheduleManagerTests
{
    private static readonly ScheduleItem tge = new(0.5m, DateTime.Now.AddMinutes(-10));

    private static readonly ScheduleItem[] rest = {
        new(0.3m, DateTime.Now.AddMinutes(10)),
        new(0.1m, DateTime.Now.AddMinutes(20)),
        new(0.1m, DateTime.Now.AddMinutes(30), DateTime.Now.AddMinutes(40)),
    };

    private static readonly IEnumerable<ScheduleItem> schedule = new[]
    {
        tge,
        rest[0],
        rest[1],
        rest[2],
    };

    public class Properties
    {
        private readonly IScheduleManager manager = new ScheduleManager(schedule);

        [Fact]
        public void Schedule()
        {
            manager.Schedule.Should().BeEquivalentTo(schedule);
        }

        [Fact]
        public void TGE()
        {
            manager.TGE.Should().BeEquivalentTo(tge);
        }

        [Fact]
        public void IsOnlyTGE()
        {
            manager.IsOnlyTGE.Should().BeFalse();
        }

        [Fact]
        public void Rest()
        {
            manager.Rest.Should().BeEquivalentTo(rest);
        }

        [Fact]
        public void MonthlyVesting()
        {
            manager.MonthlyVesting.Should().BeEquivalentTo(new[] { rest[0], rest[1] });
        }

        [Fact]
        public void LinearVesting()
        {
            manager.LinearVesting.Should().BeEquivalentTo(new[] { rest[2] });
        }
    }

    public class Constructors
    {
        [Fact]
        public void WhenScheduleIsNull_ThrowException()
        {
            ScheduleItem[] schedule = null!;

            var testCode = () => _ = new ScheduleManager(schedule);

            testCode.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WhenScheduleContainsZeroElements_ThrowException()
        {
            var testCode = () => _ = new ScheduleManager(
                Array.Empty<ScheduleItem>()
            );

            testCode.Should().Throw<ValidationException>()
                .WithMessage("*Schedule must contain 1 or more elements.*");
        }

        [Fact]
        public void WhenSumOfRatiosNotEqualOne_ThrowException()
        {
            var testCode = () => _ = new ScheduleManager(
                new ScheduleItem(0.8m, DateTime.Now),
                new ScheduleItem(0.1m, DateTime.Now.AddMinutes(5))
            );

            testCode.Should().Throw<ValidationException>()
                .WithMessage("*The sum of the ratios must be 1.*");
        }

        [Fact]
        public void WhenFirstElementIsNotTGE_ThrowException()
        {
            var testCode = () => _ = new ScheduleManager(
                new ScheduleItem(0.9m, DateTime.Now.AddDays(1)),
                new ScheduleItem(0.1m, DateTime.Now)
            );

            testCode.Should().Throw<ValidationException>()
                .WithMessage("*The first element must be the TGE (Token Generation Event).*");
        }
    }
}