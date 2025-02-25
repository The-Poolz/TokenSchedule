using FluentValidation;
using Net.Utils.ErrorHandler.Extensions;
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
                .Must(item => item.Ratio >= 1)
                .WithError(Error.RATIO_MUST_BE_POSITIVE)
                .Must(item => item.StartDate < item.FinishDate!.Value)
                .When(item => item.FinishDate.HasValue, ApplyConditionTo.CurrentValidator)
                .WithError(Error.END_TIME_MUST_BE_GREATER_THAN_START_TIME);
        }
    }
}
