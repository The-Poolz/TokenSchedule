using System.Linq;
using System.Numerics;
using FluentValidation;
using System.Collections.Generic;
using Net.Utils.ErrorHandler.Extensions;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation
{
    public class ScheduleValidator : AbstractValidator<IEnumerable<IValidatedScheduleItem>>
    {
        public static BigInteger OneEth => 1000000000000000000;

        public ScheduleValidator()
        {
            RuleFor(schedule => schedule.ToArray())
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithError(Error.SCHEDULE_IS_EMPTY)
                .Must(schedule => schedule.Aggregate(BigInteger.Zero, (acc, item) => acc + item.Ratio) == OneEth)
                .WithError(Error.SUM_OF_RATIOS_MUST_BE_ONE)
                .Must(schedule => schedule[0].StartDate == schedule.Min(x => x.StartDate))
                .WithError(Error.FIRST_ELEMENT_MUST_BE_TGE)
                .ForEach(item => item.SetValidator(new ScheduleItemValidator()));
        }
    }
}
