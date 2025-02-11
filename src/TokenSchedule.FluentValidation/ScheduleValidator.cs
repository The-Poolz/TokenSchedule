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
                .WithErrorCode(Error.SCHEDULE_IS_EMPTY.ToErrorCode())
                .WithMessage(Error.SCHEDULE_IS_EMPTY.ToErrorMessage());

            RuleFor(schedule => schedule)
                .Must(schedule => schedule.Sum(item => item.Ratio) == 1.0m)
                .WithErrorCode(Error.SUM_OF_RATIOS_MUST_BE_ONE.ToErrorCode())
                .WithMessage(Error.SUM_OF_RATIOS_MUST_BE_ONE.ToErrorMessage());

            RuleFor(schedule => schedule.ToArray())
                .Must(schedule => schedule[0].StartDate == schedule.Min(x => x.StartDate))
                .WithErrorCode(Error.FIRST_ELEMENT_MUST_BE_TGE.ToErrorCode())
                .WithMessage(Error.FIRST_ELEMENT_MUST_BE_TGE.ToErrorMessage());

            RuleFor(schedule => schedule.ToArray())
                .ForEach(item => item.SetValidator(new ScheduleItemValidator()));
        }
    }
}
