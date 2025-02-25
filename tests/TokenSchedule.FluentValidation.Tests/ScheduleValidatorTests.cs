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
                .Which.Errors.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    ErrorCode = "SCHEDULE_IS_EMPTY",
                    ErrorMessage = "Schedule must contain 1 or more elements."
                });
        }

        [Fact]
        public void SumOfRatiosNotEqualToOne_ShouldThrowValidationException()
        {
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(500000000000000000, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(400000000000000000, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ValidationException>()
                .Which.Errors.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    ErrorCode = "SUM_OF_RATIOS_MUST_BE_ONE",
                    ErrorMessage = "The sum of the ratios must be 1."
                });
        }

        [Fact]
        public void FirstItemNotEarliest_ShouldThrowValidationException()
        {
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(500000000000000000, DateTime.UtcNow.AddHours(1), DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(500000000000000000, DateTime.UtcNow, DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ValidationException>()
                .Which.Errors.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    ErrorCode = "FIRST_ELEMENT_MUST_BE_TGE",
                    ErrorMessage = "The first element must be the TGE (Token Generation Event)."
                });
        }

        [Fact]
        public void InvalidItemInSchedule_ShouldThrowValidationException()
        {
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(1000000000000000000, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(0, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().Throw<ValidationException>()
                .Which.Errors.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new
                {
                    ErrorCode = "RATIO_MUST_BE_POSITIVE",
                    ErrorMessage = "Ratio must be a positive number."
                });
        }

        [Fact]
        public void ValidSchedule_ShouldNotThrow()
        {
            var schedule = new List<IValidatedScheduleItem>
            {
                new TestScheduleItem(600000000000000000, DateTime.UtcNow, DateTime.UtcNow.AddDays(1)),
                new TestScheduleItem(400000000000000000, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2))
            };

            var testCode = () => _validator.ValidateAndThrow(schedule);

            testCode.Should().NotThrow();
        }
    }
}