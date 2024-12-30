using Xunit;
using FluentAssertions;
using FluentValidation;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation.Tests;

public class ScheduleValidatorTests
{
    public class ValidateAndThrow
    {
        private readonly ScheduleValidator _validator = new();

        [Fact]
        public void NullSchedule_ShouldThrowValidationException()
        {
            IEnumerable<IValidatedScheduleItem> schedule = null!;

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ArgumentNullException>()
                .WithMessage("*Cannot pass null model to Validate.*");
        }

        [Fact]
        public void EmptySchedule_ShouldThrowValidationException()
        {
            var schedule = Enumerable.Empty<IValidatedScheduleItem>();

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ValidationException>()
                .WithMessage("*Schedule must contain 1 or more elements.*");
        }

        [Fact]
        public void SumOfRatiosNotEqualToOne_ShouldThrowValidationException()
        {
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(0.5m, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(0.4m, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ValidationException>()
                .WithMessage("*The sum of the ratios must be 1.*");
        }

        [Fact]
        public void FirstItemNotEarliest_ShouldThrowValidationException()
        {
            var now = DateTime.UtcNow;
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(0.5m, DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(0.5m, DateTime.UtcNow, DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ValidationException>()
                .WithMessage("*The first element must be the TGE (Token Generation Event).*");
        }

        [Fact]
        public void InvalidItemInSchedule_ShouldThrowValidationException()
        {
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(1.0m, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ValidationException>()
                .WithMessage("*Ratio must be greater than*");
        }

        [Fact]
        public void ValidSchedule_ShouldNotThrow()
        {
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(0.4m, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(0.6m, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().NotThrow();
        }
    }
}