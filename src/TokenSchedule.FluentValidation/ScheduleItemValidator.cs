using FluentValidation;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation
{
    public class ScheduleItemValidator : AbstractValidator<IValidatedScheduleItem>
    {
        public ScheduleItemValidator()
        {
            RuleFor(item => item)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .Must(item => item.Ratio >= 0.000000000000000001)
                .WithMessage("Ratio must be greater than or equal to 1e-18.")
                .Must(item => item.StartDate < item.FinishDate!.Value)
                .When(item => item.FinishDate.HasValue)
                .WithMessage("End time must be greater than start time.");
        }
    }
}
