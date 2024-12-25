using System;
using System.Linq;
using FluentValidation;
using System.Collections.Generic;
using TokenSchedule.FluentValidation;

namespace TokenSchedule
{
    public class ScheduleManager : IScheduleManager
    {
        public IEnumerable<ScheduleItem> Schedule { get; }
        public ScheduleItem TGE { get; }
        public bool IsOnlyTGE { get; }
        public IEnumerable<ScheduleItem> Rest { get; }
        public IEnumerable<ScheduleItem> MonthlyVesting { get; }
        public IEnumerable<ScheduleItem> LinearVesting { get; }

        public ScheduleManager(IEnumerable<ScheduleItem> schedule) : this(schedule.ToArray()) { }

        public ScheduleManager(params ScheduleItem[] schedule)
        {
            new ScheduleValidator().ValidateAndThrow(schedule);

            Schedule = schedule;
            TGE = schedule[0];
            IsOnlyTGE = schedule.Length == 1;
            Rest = IsOnlyTGE ? Array.Empty<ScheduleItem>() : schedule.Skip(1).ToArray();
            MonthlyVesting = Rest.Where(x => x.FinishDate == null);
            LinearVesting = Rest.Where(x => x.FinishDate != null);
        }
    }
}
