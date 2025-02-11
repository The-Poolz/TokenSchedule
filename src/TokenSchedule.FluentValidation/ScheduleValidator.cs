using System.Linq;
using FluentValidation;
using System.Collections.Generic;
using Net.Utils.ErrorHandler.Extensions;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation
{
    public class ScheduleValidator : AbstractValidator<IEnumerable<IValidatedScheduleItem>>
    {
        public ScheduleValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(schedule => schedule)
                .NotNull();

            RuleFor(schedule => schedule)
                .NotEmpty()
                .WithError(Error.SCHEDULE_IS_EMPTY);

            RuleFor(schedule => schedule)
                .Must(schedule => schedule.Sum(item => item.Ratio) == 1.0m)
                .WithError(Error.SUM_OF_RATIOS_MUST_BE_ONE);

            RuleFor(schedule => schedule.ToArray())
                .Must(schedule => schedule[0].StartDate == schedule.Min(x => x.StartDate))
                .WithError(Error.FIRST_ELEMENT_MUST_BE_TGE);

            RuleFor(schedule => schedule.ToArray())
                .ForEach(item => item.SetValidator(new ScheduleItemValidator()));
        }
    }
}
