using TokenSchedule.FluentValidation.Models;

namespace TokenSchedule.FluentValidation.Tests;

public class TestScheduleItem : IValidatedScheduleItem
{
    public TestScheduleItem(decimal ratio, DateTime startDate, DateTime? finishDate)
    {
        Ratio = ratio;
        StartDate = startDate;
        FinishDate = finishDate;
    }

    public decimal Ratio { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
}