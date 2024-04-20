using System;

namespace TokenSchedule
{
    public class SingleRow
    {
        public SingleRow(decimal ratio, DateTime startTime, DateTime? endTime = null)
        {
            Ratio = ratio;
            StartTime = startTime;
            EndTime = endTime;

            if (EndTime.HasValue && StartTime >= EndTime.Value)
            {
                throw new ArgumentException("End time must be greater than start time.", nameof(startTime));
            }
            if (Ratio <= 0)
            {
                throw new ArgumentException("Ratio must be positive.", nameof(ratio));
            }
        }

        public decimal Ratio { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
    }
}
