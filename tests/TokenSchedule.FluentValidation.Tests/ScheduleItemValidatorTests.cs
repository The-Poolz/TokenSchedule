using Xunit;
using FluentAssertions;
using FluentValidation;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation.Tests
{
    public class ScheduleItemValidatorTests
    {
        public class ValidateAndThrow
        {
            private readonly ScheduleItemValidator _validator = new();

            [Fact]
            internal void ValidItem_ShouldNotThrow()
            {
                var item = new TestScheduleItem(0.1m, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().NotThrow();
            }

            [Fact]
            internal void NullItem_ShouldThrowValidationException()
            {
                IValidatedScheduleItem item = null!;

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ArgumentNullException>()
                   .WithMessage("*Cannot pass null model to Validate.*");
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            internal void NonPositiveRatio_ShouldThrowValidationException(decimal ratio)
            {
                var item = new TestScheduleItem(ratio, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                   .WithMessage("*Ratio must be positive.*");
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-1)]
            [InlineData(-100)]
            internal void NonPositiveRatio_ShouldThrowValidationExceptionNoFinish(decimal ratio)
            {
                var item = new TestScheduleItem(ratio, DateTime.UtcNow);

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                   .WithMessage("*Ratio must be positive.*");
            }

            [Fact]
            internal void StartDateBiggerThanFinishDate_ShouldThrowValidationException()
            {
                var item = new TestScheduleItem(0.1m, DateTime.UtcNow.AddHours(2), DateTime.UtcNow);

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                   .WithMessage("*End time must be greater than start time.*");
            }

            [Fact]
            internal void StartDateEqualToFinishDate_ShouldThrowValidationException()
            {
                var now = DateTime.UtcNow;
                var item = new TestScheduleItem(0.1m, now, now);

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                   .WithMessage("*End time must be greater than start time.*");
            }

            [Fact]
            internal void SmallRato_ShouldThrowValidationException()
            {
                var item = new TestScheduleItem(0.0000000000000000001m, DateTime.UtcNow.AddHours(2));

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                   .WithMessage("*Ratio must be greater than or equal to 1e-18.*");
            }
        }
    }
}