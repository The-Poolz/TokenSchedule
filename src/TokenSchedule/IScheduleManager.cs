﻿using System.Collections.Generic;

namespace TokenSchedule
{
    public interface IScheduleManager
    {
        public IEnumerable<ScheduleItem> Schedule { get; }
        public ScheduleItem TGE { get; }
        public bool IsOnlyTGE { get; }
        public IEnumerable<ScheduleItem> Rest { get; }
        public IEnumerable<ScheduleItem> MonthlyVesting { get; }
        public IEnumerable<ScheduleItem> LinearVesting { get; }
    }
}
