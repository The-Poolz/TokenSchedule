using FluentValidation;
using Net.Utils.ErrorHandler.Extensions;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation
{
    public class ScheduleItemValidator : AbstractValidator<IValidatedScheduleItem>
    {
        public const decimal MinRatio = 0.000000000000000001m;
        public ScheduleItemValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(item => item)
                .NotNull();

            RuleFor(item => item)
                .Must(item => item.Ratio >= MinRatio)
                .WithErrorCode(Error.MINIMUM_RATIO_1E_MINUS_18.ToErrorCode())
                .WithMessage(Error.MINIMUM_RATIO_1E_MINUS_18.ToErrorMessage());

            RuleFor(item => item)
                .Must(item => item.StartDate < item.FinishDate!.Value)
                .When(item => item.FinishDate.HasValue, ApplyConditionTo.CurrentValidator)
                .WithErrorCode(Error.END_TIME_MUST_BE_GREATER_THAN_START_TIME.ToErrorCode())
                .WithMessage(Error.END_TIME_MUST_BE_GREATER_THAN_START_TIME.ToErrorMessage());
        }
    }
}
