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
                var item = new TestScheduleItem(1L, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

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
            [MemberData(nameof(InvalidRatios))]
            internal void InvalidRatio_ShouldThrowValidationException(TestScheduleItem item)
            {
                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                    .Which.Errors.Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new
                    {
                        ErrorCode = "RATIO_MUST_BE_POSITIVE",
                        ErrorMessage = "Ratio must be a positive number."
                    });
            }

            [Fact]
            internal void StartDateBiggerThanFinishDate_ShouldThrowValidationException()
            {
                var item = new TestScheduleItem(1L, DateTime.UtcNow.AddHours(2), DateTime.UtcNow);

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                    .Which.Errors.Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new
                    {
                        ErrorCode = "END_TIME_MUST_BE_GREATER_THAN_START_TIME",
                        ErrorMessage = "End time must be greater than start time."
                    });
            }

            [Fact]
            internal void StartDateEqualToFinishDate_ShouldThrowValidationException()
            {
                var now = DateTime.UtcNow;
                var item = new TestScheduleItem(1L, now, now);

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                    .Which.Errors.Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new
                    {
                        ErrorCode = "END_TIME_MUST_BE_GREATER_THAN_START_TIME",
                        ErrorMessage = "End time must be greater than start time."
                    });
            }

            public static List<object[]> InvalidRatios =>
            [
                new object[] { new TestScheduleItem(0L, DateTime.UtcNow) },
                new object[] { new TestScheduleItem(-1L, DateTime.UtcNow) },
                new object[] { new TestScheduleItem(-100L, DateTime.UtcNow) },
                new object[] { new TestScheduleItem(0L, DateTime.UtcNow, DateTime.UtcNow.AddHours(1)) },
                new object[] { new TestScheduleItem(-1L, DateTime.UtcNow, DateTime.UtcNow.AddHours(1)) },
                new object[] { new TestScheduleItem(-100L, DateTime.UtcNow, DateTime.UtcNow.AddHours(1)) },
            ];
        }
    }
}