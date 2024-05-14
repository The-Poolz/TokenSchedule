# Token Schedule

A simple C# library for managing token distribution schedules.

## Features

- Define token distribution schedules with start and end times and ratios.
- Validate schedules to ensure correct distribution.
- Retrieve the Token Generation Event (TGE) and subsequent distribution events.

## Usage

### Creating a Schedule and Retrieving Information

```csharp
using TokenSchedule;

// Define schedule data
var scheduleData = new List<ScheduleItem>
{
    new ScheduleItem(0.4m, new DateTime(2024, 1, 1)),
    new ScheduleItem(0.1m, new DateTime(2024, 2, 1)),
    new ScheduleItem(0.2m, new DateTime(2024, 4, 1)),
    new ScheduleItem(0.1m, new DateTime(2024, 2, 1), new DateTime(2024, 3, 1)),
    new ScheduleItem(0.2m, new DateTime(2024, 4, 1), new DateTime(2024, 5, 1))
};

// Create a new schedule
var schedule = new ScheduleManager(scheduleData);

// Retrieve TGE information
var tge = schedule.TGE;
Console.WriteLine($"TGE Ratio: {tge.Ratio}, Start Time: {tge.StartDate}");

// Retrieve the rest of the schedule information
var restOfSchedule = schedule.Rest;
foreach (var row in restOfSchedule)
{
    Console.WriteLine($"Ratio: {row.Ratio}, Start Time: {row.StartDate}, End Time: {row.FinishDate}");
}

// Retrieve the linear vesting of the schedule information
var linear = schedule.LinearVesting;
foreach (var row in linear)
{
    Console.WriteLine($"Ratio: {row.Ratio}, Start Time: {row.StartDate}, End Time: {row.FinishDate}");
}

// Retrieve the monthly vesting of the schedule information
var monthly = schedule.MonthlyVesting;
foreach (var row in monthly)
{
    Console.WriteLine($"Ratio: {row.Ratio}, Start Time: {row.StartDate}, End Time: {row.FinishDate}");
}
```

### License
This project is licensed under the MIT License - see the LICENSE file for details.
