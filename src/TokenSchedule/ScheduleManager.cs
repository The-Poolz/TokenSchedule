﻿using System;
using System.Linq;
using System.Collections.Generic;

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
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            if (!schedule.Any())
            {
                throw new ArgumentException("Schedule must contain 1 or more elements.", nameof(schedule));
            }

            if (schedule.Sum(x => x.Ratio) != 1)
            {
                throw new ArgumentException("The sum of the ratios must be 1.", nameof(schedule));
            }

            if (schedule[0].StartDate != schedule.Min(x => x.StartDate))
            {
                throw new ArgumentException("The first element must be the TGE (Token Generation Event).", nameof(schedule));
            }

            Schedule = schedule;
            TGE = schedule[0];
            IsOnlyTGE = schedule.Length == 1;
            Rest = IsOnlyTGE ? Array.Empty<ScheduleItem>() : schedule.Skip(1).ToArray();
            MonthlyVesting = Rest.Where(x => x.FinishDate == null);
            LinearVesting = Rest.Where(x => x.FinishDate != null);
        }
    }
}
