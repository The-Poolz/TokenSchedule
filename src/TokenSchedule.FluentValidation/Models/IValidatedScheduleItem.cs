using System;
using System.Numerics;

namespace TokenSchedule.FluentValidation.Models
{
    public interface IValidatedScheduleItem
    {
        public BigInteger Ratio { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }
    }
}
