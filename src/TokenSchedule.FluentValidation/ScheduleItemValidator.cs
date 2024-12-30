using FluentValidation;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation
{
    public class ScheduleItemValidator : AbstractValidator<IValidatedScheduleItem>
    {
        public ScheduleItemValidator()
        {
            RuleFor(item => item)
                .NotNull()
                .Must(item => item.Ratio > 0)
                .WithMessage("Ratio must be positive.");
            RuleFor(item => item)
                .Must(item => item.StartDate < item.FinishDate!.Value)
                .When(item => item.FinishDate.HasValue)
                .WithMessage("End time must be greater than start time.");
            RuleFor(item => item)
                .Must(item => item.Ratio >= 0.000000000000000001m)
                .WithMessage("Ratio must be greater than or equal to 1e-18.");
        }
    }
}
