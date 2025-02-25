using System.Numerics;
using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation.Tests;

public class TestScheduleItem(BigInteger ratio, DateTime startDate, DateTime? finishDate = null) : IValidatedScheduleItem
{
    public BigInteger Ratio { get; set; } = ratio;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime? FinishDate { get; set; } = finishDate;
}