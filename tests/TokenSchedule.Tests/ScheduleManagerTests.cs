using Xunit;

namespace TokenSchedule.Tests;

public class ScheduleManagerTests
{
    [Fact]
    public void ScheduleInfo_WithValidData_ShouldNotThrow()
    {
        // Arrange
        var scheduleData = new List<ScheduleItem>
        {
            new ScheduleItem(0.5m, DateTime.UtcNow),
            new ScheduleItem(0.5m, DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(2))
        };

        // Act & Assert
        var exception = Record.Exception(() => new ScheduleManager(scheduleData));
        Assert.Null(exception);
    }

    [Fact]
    public void ScheduleInfo_WithInvalidData_ShouldThrow()
    {
        // Arrange
        var scheduleData = new List<ScheduleItem>
        {
            new ScheduleItem(0.5m, DateTime.UtcNow),
            new ScheduleItem(0.6m, DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(2)) // Sum of ratios != 1
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ScheduleManager(scheduleData));
    }

    [Fact]
    public void SingleRow_WithNegativeRatio_ShouldThrow()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ScheduleItem(-0.1m, DateTime.UtcNow));
    }

    [Fact]
    public void SingleRow_WithEndTimeBeforeStartTime_ShouldThrow()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ScheduleItem(0.1m, DateTime.UtcNow, DateTime.UtcNow.AddDays(-1)));
    }

    [Fact]
    public void GetTge_ShouldReturnFirstElement()
    {
        // Arrange
        var expectedTge = new ScheduleItem(0.5m, DateTime.UtcNow);
        var scheduleData = new List<ScheduleItem>
        {
            expectedTge,
            new ScheduleItem(0.5m, DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(2))
        };
        var scheduleInfo = new ScheduleManager(scheduleData);

        // Act
        var tge = scheduleInfo.TGE;

        // Assert
        Assert.Equal(expectedTge, tge);
    }

    [Fact]
    public void GetRest_ShouldReturnAllElementsExceptFirst()
    {
        // Arrange
        var tge = new ScheduleItem(0.5m, DateTime.UtcNow);
        var restOfScheduleData = new ScheduleItem(0.5m, DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(2));
        var scheduleData = new List<ScheduleItem> { tge, restOfScheduleData };
        var scheduleInfo = new ScheduleManager(scheduleData);

        // Act
        var rest = scheduleInfo.Rest;

        // Assert
        Assert.Single(rest);
        Assert.Contains(restOfScheduleData, rest);
    }

    [Fact]
    public void IsOnlyTge_WithSingleElement_ShouldReturnTrue()
    {
        // Arrange
        var scheduleData = new List<ScheduleItem>
        {
            new ScheduleItem(1.0m, DateTime.UtcNow)
        };
        var scheduleInfo = new ScheduleManager(scheduleData);

        // Act
        var result = scheduleInfo.IsOnlyTGE;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsOnlyTge_WithMultipleElements_ShouldReturnFalse()
    {
        // Arrange
        var scheduleData = new List<ScheduleItem>
        {
            new ScheduleItem(0.5m, DateTime.UtcNow),
            new ScheduleItem(0.5m, DateTime.UtcNow.AddMonths(1), DateTime.UtcNow.AddMonths(2))
        };
        var scheduleInfo = new ScheduleManager(scheduleData);

        // Act
        var result = scheduleInfo.IsOnlyTGE;

        // Assert
        Assert.False(result);
    }
}