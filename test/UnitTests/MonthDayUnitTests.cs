using Calendar;
using Microsoft.VisualBasic;

namespace UnitTests;

public class MonthDayUnitTests {

    [Test]
    public void MonthDay_SetInvalidMonth_ThrowsException() {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonthDay(13, 20));
    }

    [Test]
    public void MonthDay_SetInvalidDay_ThrowsException() {
        Assert.Throws<ArgumentOutOfRangeException>(() => new MonthDay(12, 32));
    }

    [Test]
    public void MonthDay_SetValidLeapDay_IsSuccessful() {
        MonthDay monthDay = new(2, 29);
        Assert.Multiple(() =>
        {
            Assert.That(monthDay.Month(), Is.EqualTo(2));
            Assert.That(monthDay.Day(), Is.EqualTo(29));
        });
    }
}