# TokenSchedule.FluentValidation

**TokenSchedule.FluentValidation** is a small library that provides validators for schedule items used in token distribution.
It is built on top of [FluentValidation](https://github.com/FluentValidation/FluentValidation) to handle validation dependencies separately.

## Features

- **`ScheduleItemValidator`**: Validates a single schedule item (`IValidatedScheduleItem`):
  - Ensures `Ratio` is positive (`> 0`).
  - Validates that `FinishDate` is either not set or later than `StartDate`.
- **`ScheduleValidator`**: Validates an entire schedule (`IEnumerable<IValidatedScheduleItem>`):
  - Schedule cannot be null or empty.
  - The sum of all `Ratio` values must be `1e18`.
  - The first item in the schedule must have the earliest `StartDate` (treated as the Token Generation Event).
  - Each individual item is validated by the `ScheduleItemValidator`.

## Installation

You can install this package via the .NET CLI:

```powershell
dotnet add package TokenSchedule.FluentValidation
```

Or via the NuGet Package Manager console:

```powershell
Install-Package TokenSchedule.FluentValidation
```

## Getting Started

Below is a simple example showing how to use `TokenSchedule.FluentValidation`:

```csharp
using System;
using System.Numerics;
using FluentValidation;
using System.Collections.Generic;
using TokenSchedule.FluentValidation;
using TokenSchedule.FluentValidation.Models;

public class MyScheduleItem : IValidatedScheduleItem
{
    public BigInteger Ratio { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }
}

class Program
{
    static void Main()
    {
        // Create some schedule items
        var scheduleItems = new List<IValidatedScheduleItem>
        {
            new MyScheduleItem
            {
                Ratio = 500000000000000000,
                StartDate = new DateTime(2024, 1, 1),
                FinishDate = new DateTime(2024, 6, 1)
            },
            new MyScheduleItem
            {
                Ratio = 400000000000000000,
                StartDate = new DateTime(2024, 6, 2),
                FinishDate = new DateTime(2024, 12, 31)
            }
        };

        // Create an instance of the ScheduleValidator
        var validator = new ScheduleValidator();

        // Validate the schedule
        var result = validator.Validate(scheduleItems);

        if (result.IsValid)
        {
            Console.WriteLine("Schedule is valid!");
        }
        else
        {
            Console.WriteLine("Schedule is invalid. Errors:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
        }
    }
}
```

### Explanation
1. **Define your schedule items** by implementing the `IValidatedScheduleItem` interface.  
2. **Create a list of schedule items** that you want to validate.  
3. **Use the `ScheduleValidator`** to validate the entire collection.  
4. **Handle the validation results**, e.g. by displaying errors or by throwing exceptions.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
