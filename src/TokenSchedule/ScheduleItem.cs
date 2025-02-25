using System;
using System.Numerics;
using FluentValidation;
using TokenSchedule.FluentValidation;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule
{
    public class ScheduleItem : IValidatedScheduleItem
    {
        public BigInteger Ratio { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }

        public ScheduleItem(BigInteger ratio, DateTime startDate, DateTime? finishDate = null)
        {
            Ratio = ratio;
            StartDate = startDate;
            FinishDate = finishDate;

            new ScheduleItemValidator().ValidateAndThrow(this);
        }
    }
}
