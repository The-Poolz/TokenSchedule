using System.Linq;
using FluentValidation;
using System.Collections.Generic;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation
{
    public class ScheduleValidator : AbstractValidator<IEnumerable<IValidatedScheduleItem>>
    {
        public ScheduleValidator()
        {
            RuleFor(schedule => schedule.ToArray())
                .NotEmpty()
                .WithMessage("Schedule must contain 1 or more elements.")
                .Must(schedule => schedule.Sum(item => item.Ratio) != 1.0m)
                .WithMessage("The sum of the ratios must be 1.")
                .Must(schedule => schedule[0].StartDate != schedule.Min(x => x.StartDate))
                .WithMessage("The first element must be the TGE (Token Generation Event).")
                .ForEach(item => item.SetValidator(new ScheduleItemValidator()));
        }
    }
}
