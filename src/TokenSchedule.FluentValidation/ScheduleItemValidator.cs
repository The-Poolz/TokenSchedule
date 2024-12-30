using FluentValidation;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation
{
    public class ScheduleItemValidator : AbstractValidator<IValidatedScheduleItem>
    {
        public const decimal MinRatio = 0.000000000000000001m;
        public ScheduleItemValidator()
        {
            RuleFor(item => item)
                .NotNull()
                .Must(item => item.Ratio >= MinRatio)
                .WithMessage("Ratio must be greater than or equal to 1e-18.")
                .Must(item => item.StartDate < item.FinishDate!.Value)
                .When(item => item.FinishDate.HasValue, ApplyConditionTo.CurrentValidator)
                .WithMessage("End time must be greater than start time.");
        }
    }
}
