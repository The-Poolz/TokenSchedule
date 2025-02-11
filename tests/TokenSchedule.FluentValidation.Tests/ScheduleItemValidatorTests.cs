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
            [MemberData(nameof(InvalidRatios))]
            internal void InvalidRatio_ShouldThrowValidationException(TestScheduleItem item)
            {
                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                    .Which.Errors.Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new
                    {
                        ErrorCode = "MINIMUM_RATIO_1E_MINUS_18",
                        ErrorMessage = "Ratio must be greater than or equal to 1e-18."
                    });
            }

            [Fact]
            internal void StartDateBiggerThanFinishDate_ShouldThrowValidationException()
            {
                var item = new TestScheduleItem(0.1m, DateTime.UtcNow.AddHours(2), DateTime.UtcNow);

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
                var item = new TestScheduleItem(0.1m, now, now);

                var testCode = () => _validator.ValidateAndThrow(item);

                testCode.Should().Throw<ValidationException>()
                    .Which.Errors.Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new
                    {
                        ErrorCode = "END_TIME_MUST_BE_GREATER_THAN_START_TIME",
                        ErrorMessage = "End time must be greater than start time."
                    });
            }

            public static List<object[]> InvalidRatios => new()
            {
                new object[] { new TestScheduleItem(0m, DateTime.UtcNow) },
                new object[] { new TestScheduleItem(-1m, DateTime.UtcNow) },
                new object[] { new TestScheduleItem(-100m, DateTime.UtcNow) },
                new object[] { new TestScheduleItem(0.0000000000000000001m, DateTime.UtcNow) },
                new object[] { new TestScheduleItem(0m, DateTime.UtcNow, DateTime.UtcNow.AddHours(1)) },
                new object[] { new TestScheduleItem(-1m, DateTime.UtcNow, DateTime.UtcNow.AddHours(1)) },
                new object[] { new TestScheduleItem(-100m, DateTime.UtcNow, DateTime.UtcNow.AddHours(1)) },
                new object[] { new TestScheduleItem(0.0000000000000000001m, DateTime.UtcNow, DateTime.UtcNow.AddHours(1)) }
            };
        }
    }
}