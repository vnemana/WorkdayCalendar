namespace UnitTests;

public class WorkdayUnitTests
{
    private Calendar.Workday _workday;
    [SetUp]
    public void Setup() {
        _workday = new Calendar.Workday();
    }

    [Test]
    public void Workday_SetInvalidStartTime_ReturnsFalse() {
        var result = _workday.SetStartTime("25:00");
        Assert.IsFalse(result);
    }

    [Test]
    public void Workday_SetValidStartTime_ReturnsTrue() {
        var result = _workday.SetStartTime("08:00");
        Assert.IsTrue(result);
    }

    [Test]
    public void Workday_SetInvalidStopTime_ReturnsFalse() {
        var result = _workday.SetStopTime("24:01");
        Assert.IsFalse(result);
    }
    [Test]
    public void Workday_SetValidStopTime_ReturnsTrue() {
        var result = _workday.SetStopTime("08:00");
        Assert.IsTrue(result);
    }

    [Test]
    public void Workday_GetMilliseconds_ReturnsCorrectValue() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        Assert.That(_workday.GetMilliseconds, Is.EqualTo(28800000));
    }

    [Test]
    public void Workday_GetMillisecondsForFourHourDay_ReturnsCorrectValue() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("12:00");
        Assert.That(_workday.GetMilliseconds, Is.EqualTo(14400000));
    }

    [Test]
    public void Workday_GetEndTime_WhenStartTimeIsBeforeDayBegins_ReturnsEndTimeFromStartOfDay() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        var inputDate = new DateTime(2023, 10, 31, 7, 0, 0);
        var remainingTime = 0.25;
        var result = _workday.GetEndTime(inputDate, remainingTime);
        Assert.That(result.EndingWorkdayType, Is.EqualTo(Calendar.EndingWorkdayType.CurrentDay));
        Assert.That(result.EndTime, Is.EqualTo(new TimeOnly(10, 0)));
    }

    [Test]
    public void Workday_GetEndTime_WhenEndTimeIsAfterDayEnds_ReturnsEndTimeFromEndOfDay() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        var inputDate = new DateTime(2023, 10, 31, 18, 0, 0);
        var remainingTime = -0.25;
        var result = _workday.GetEndTime(inputDate, remainingTime);
        Assert.That(result.EndingWorkdayType, Is.EqualTo(Calendar.EndingWorkdayType.CurrentDay));
        Assert.That(result.EndTime, Is.EqualTo(new TimeOnly(14, 0)));
    }


    [Test]
    public void Workday_GetEndTime_WhenStartTimeIsAfterDayBegins_AndEndTimeIsBeforeDayEnds_ReturnsCurrentDay() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        var inputDate = new DateTime(2023, 10, 31, 9, 0, 0);
        var remainingTime = 0.25;
        var result = _workday.GetEndTime(inputDate, remainingTime);
        Assert.That(result.EndingWorkdayType, Is.EqualTo(Calendar.EndingWorkdayType.CurrentDay));
        Assert.That(result.EndTime, Is.EqualTo(new TimeOnly(11, 0)));
    }

    [Test]
    public void Workday_GetEndTime_WhenEndTimeIsAfterDayEnds_ReturnsNextDay() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        var inputDate = new DateTime(2023, 10, 31, 15, 0, 0);
        var remainingTime = 0.5;
        var result = _workday.GetEndTime(inputDate, remainingTime);
        Assert.That(result.EndingWorkdayType, Is.EqualTo(Calendar.EndingWorkdayType.NextDay));
        Assert.That(result.EndTime, Is.EqualTo(new TimeOnly(11, 0)));
    }

    [Test]
    public void Workday_GetEndTime_WhenStartTimeIsBeforeDayEnds_AndEndTimeIsAfterDayStarts_ReturnsCurrentDay() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        var inputDate = new DateTime(2023, 10, 31, 15, 0, 0);
        var remainingTime = -0.5;
        var result = _workday.GetEndTime(inputDate, remainingTime);
        Assert.That(result.EndingWorkdayType, Is.EqualTo(Calendar.EndingWorkdayType.CurrentDay));
        Assert.That(result.EndTime, Is.EqualTo(new TimeOnly(11, 0)));
    }

    [Test]
    public void Workday_GetEndTime_WhenEndTimeIsBeforeDayEnds_ReturnsPreviousDay() {
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        var inputDate = new DateTime(2023, 10, 31, 10, 0, 0);
        var remainingTime = -0.5;
        var result = _workday.GetEndTime(inputDate, remainingTime);
        Assert.That(result.EndingWorkdayType, Is.EqualTo(Calendar.EndingWorkdayType.PreviousDay));
        Assert.That(result.EndTime, Is.EqualTo(new TimeOnly(14, 0)));
    }

}