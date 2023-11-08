namespace UnitTests;

public class CalendarUnitTests
{
    private Calendar.Calendar _calendar;
    [SetUp]
    public void Setup() {
        _calendar = new Calendar.Calendar();
    }

    [Test]
    public void Calendar_SetInvalidDateForRecurringHoliday_ReturnsFalse() {
        var result = _calendar.SetRecurringHoliday(04, 31);
        Assert.That(result, Is.False);
    }

    [Test]
    public void Calendar_SetValidDateForRecurringHoliday_ReturnsTrue() {
        Assert.That(_calendar.SetRecurringHoliday(03, 15), Is.True);
        Assert.That(_calendar.GetRecurringHolidays().Count(), Is.EqualTo(1));
    }

    [Test]
    public void Calendar_SetInvalidDateForHoliday_ReturnsFalse() {
        Assert.That(_calendar.SetHoliday(2023, 04, 31), Is.False);
    }

    [Test]
    public void Calendar_SetValidDateForHoliday_ReturnsTrue() {
        Assert.That(_calendar.SetHoliday(2023, 03, 14));
    }

    [Test]
    public void Calendar_IsValidWorkdayInRecurringHoliday_ReturnsFalse() {
        _calendar.SetRecurringHoliday(03,15);
        DateTime inputDate = new DateTime(2005, 03, 15);
        Assert.That(_calendar.IsValidWorkday(inputDate), Is.False);
    }

    [Test]
    public void Calendar_IsValidWorkdayInHoliday_ReturnsFalse() {
        _calendar.SetRecurringHoliday(04, 15);
        _calendar.SetHoliday(2005, 03, 15);
        DateTime inputDate = new DateTime(2005, 03, 15);
        Assert.That(_calendar.IsValidWorkday(inputDate), Is.False);
    }

    [Test]
    public void Calendar_IsValidWorkdayInWeekend_ReturnsFalse() {
        DateTime inputDate = new DateTime(2023, 10, 29);
        Assert.That(_calendar.IsValidWorkday(inputDate), Is.False);
    }

    [Test]
    public void Calendar_IsValidWorkdayInWeekday_ReturnsTrue() {
        _calendar.SetRecurringHoliday(04, 15);
        _calendar.SetHoliday(2005, 05, 15);
        DateTime inputDate = new DateTime(2005, 03, 15);
        Assert.That(_calendar.IsValidWorkday(inputDate), Is.True);
    }

    [Test]
    public void Calendar_GetPreviousWorkday_SkipsWeekend() {
        DateTime inputDate = new(2023, 11, 06);    //Monday
        var previousDay = _calendar.GetPreviousWorkday(inputDate);
        Assert.That(previousDay, Is.EqualTo(new DateOnly(2023, 11, 03)));
    }

    [Test]
    public void Calendar_GetPreviousWorkdayFromWeekend_ReturnsFriday() {
        DateTime inputDate = new(2023, 11, 05);
        var previousDay = _calendar.GetPreviousWorkday(inputDate);
        Assert.That(previousDay, Is.EqualTo(new DateOnly(2023, 11, 03)));
    }

    [Test]
    public void Calendar_GetPreviousWorkdayFromWeekend_SkipsRecurringAndRegularHoliday() {
        DateTime inputDate = new(2023, 11, 05);
        _calendar.SetRecurringHoliday(11, 03);
        _calendar.SetHoliday(2023, 11, 02);
        var previousDay = _calendar.GetPreviousWorkday(inputDate);
        Assert.That(previousDay, Is.EqualTo(new DateOnly(2023, 11, 01)));
    }

    [Test]
    public void Calendar_GetPreviousWorkday_SkipsRecurringHoliday() {
        DateTime inputDate = new(2023, 04, 06);
        _calendar.SetRecurringHoliday(04, 05);
        var previousDay = _calendar.GetPreviousWorkday(inputDate);
        Assert.That(previousDay, Is.EqualTo(new DateOnly(2023, 04, 04)));
    }

    [Test]
    public void Calendar_GetPreviousWorkday_SkipsHoliday() {
        DateTime inputDate = new(2023, 04, 06);
        _calendar.SetRecurringHoliday(04, 05);
        _calendar.SetHoliday(2023, 04, 04);
        var previousDay = _calendar.GetPreviousWorkday(inputDate);
        Assert.That(previousDay, Is.EqualTo(new DateOnly(2023, 04, 03)));
    }

    [Test]
    public void Calendar_GetPreviousWorkday_ReturnsPreviousDay() {
        DateTime inputDate = new(2023, 04, 07);
        _calendar.SetRecurringHoliday(04, 05);
        _calendar.SetHoliday(2023, 04, 04);
        var previousDay = _calendar.GetPreviousWorkday(inputDate);
        Assert.That(previousDay, Is.EqualTo(new DateOnly(2023, 04, 06)));
    }

    [Test]
    public void Calendar_GetNextWorkday_SkipsWeekend() {
        DateTime inputDate = new(2023, 11, 03);    //Friday
        var nextDay = _calendar.GetNextWorkday(inputDate);
        Assert.That(nextDay, Is.EqualTo(new DateOnly(2023, 11, 06)));
    }

    [Test]
    public void Calendar_GetNextWorkdayFromWeekend_ReturnsFriday() {
        DateTime inputDate = new(2023, 11, 05);
        var nextDay = _calendar.GetNextWorkday(inputDate);
        Assert.That(nextDay, Is.EqualTo(new DateOnly(2023, 11, 06)));
    }

    [Test]
    public void Calendar_GetNextWorkdayFromWeekend_SkipsRecurringAndRegularHoliday() {
        DateTime inputDate = new(2023, 11, 05);
        _calendar.SetRecurringHoliday(11, 06);
        _calendar.SetHoliday(2023, 11, 07);
        var nextDay = _calendar.GetNextWorkday(inputDate);
        Assert.That(nextDay, Is.EqualTo(new DateOnly(2023, 11, 08)));
    }

    [Test]
    public void Calendar_GetNextWorkday_SkipsRecurringHoliday() {
        DateTime inputDate = new(2023, 04, 04);
        _calendar.SetRecurringHoliday(04, 05);
        var nextDay = _calendar.GetNextWorkday(inputDate);
        Assert.That(nextDay, Is.EqualTo(new DateOnly(2023, 04, 06)));
    }

    [Test]
    public void Calendar_GetNextWorkday_SkipsHoliday() {
        DateTime inputDate = new(2023, 04, 03);
        _calendar.SetRecurringHoliday(04, 05);
        _calendar.SetHoliday(2023, 04, 04);
        var nextDay = _calendar.GetNextWorkday(inputDate);
        Assert.That(nextDay, Is.EqualTo(new DateOnly(2023, 04, 06)));
    }

    [Test]
    public void Calendar_GetNextWorkday_ReturnsNextDay() {
        DateTime inputDate = new(2023, 04, 10);
        _calendar.SetRecurringHoliday(04, 05);
        _calendar.SetHoliday(2023, 04, 04);
        var nextDay = _calendar.GetNextWorkday(inputDate);
        Assert.That(nextDay, Is.EqualTo(new DateOnly(2023, 04, 11)));
    }

}