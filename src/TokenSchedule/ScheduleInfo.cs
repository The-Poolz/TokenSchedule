using System;
using System.Linq;
using System.Collections.Generic;

namespace TokenSchedule
{
    public class ScheduleInfo
    {
        private readonly IList<SingleRow> data;

        public ScheduleInfo(IEnumerable<SingleRow> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var dataList = data.ToList();

            ValidateData(dataList);
            this.data = dataList;
        }

        public SingleRow GetTge() => data.First();

        public IEnumerable<SingleRow> GetRest() => IsOnlyTge() ? Enumerable.Empty<SingleRow>() : data.Skip(1);

        public bool IsOnlyTge() => data.Count == 1;

        private static void ValidateData(IList<SingleRow> data)
        {
            if (!data.Any())
            {
                throw new ArgumentException("Data must have elements.", nameof(data));
            }

            if (data.Sum(x => x.Ratio) != 1)
            {
                throw new ArgumentException("The sum of the ratios must be 1.", nameof(data));
            }

            if (data.First().StartTime != data.Min(x => x.StartTime))
            {
                throw new ArgumentException("The first element must be the TGE (Token Generation Event).", nameof(data));
            }
        }

    }
}
