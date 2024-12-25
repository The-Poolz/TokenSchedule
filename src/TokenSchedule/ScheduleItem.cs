using System;
using FluentValidation;
using TokenSchedule.FluentValidation;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule
{
    public class ScheduleItem : IValidatedScheduleItem
    {
        public decimal Ratio { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }

        public ScheduleItem(decimal ratio, DateTime startDate, DateTime? finishDate = null)
        {
            Ratio = ratio;
            StartDate = startDate;
            FinishDate = finishDate;

            new ScheduleItemValidator().ValidateAndThrow(this);
        }
    }
}
