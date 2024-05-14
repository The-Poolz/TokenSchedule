using System;

namespace TokenSchedule
{
    public class ScheduleItem
    {
        public decimal Ratio { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }

        public ScheduleItem(decimal ratio, DateTime startDate, DateTime? finishDate = null)
        {
            if (finishDate.HasValue && startDate >= finishDate.Value)
            {
                throw new ArgumentException("End time must be greater than start time.", nameof(startDate));
            }
            if (ratio <= 0)
            {
                throw new ArgumentException("Ratio must be positive.", nameof(ratio));
            }

            Ratio = ratio;
            StartDate = startDate;
            FinishDate = finishDate;
        }
    }
}
