using System;

namespace TokenSchedule
{
    public class ScheduleItem
    {
        public decimal Ratio { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }

        public ScheduleItem(decimal ratio, DateTime startTime, DateTime? endTime = null)
        {
            if (endTime.HasValue && startTime >= endTime.Value)
            {
                throw new ArgumentException("End time must be greater than start time.", nameof(startTime));
            }
            if (ratio <= 0)
            {
                throw new ArgumentException("Ratio must be positive.", nameof(ratio));
            }

            Ratio = ratio;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
