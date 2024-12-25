using System;

namespace TokenSchedule.FluentValidation.Models
{
    public interface IValidatedScheduleItem
    {
        public decimal Ratio { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }
    }
}
