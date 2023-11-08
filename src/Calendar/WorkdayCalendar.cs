using System.Text.RegularExpressions;

namespace Calendar;

public class WorkdayCalendar
{
    private readonly Calendar _calendar;
    private readonly Workday _workday;
    public WorkdayCalendar()
    {
        _calendar = new Calendar();
        _workday = new Workday();

        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        _calendar.SetRecurringHoliday(5, 17);
        _calendar.SetHoliday(2004, 5, 27);
    }

    public DateTime GetEndTime(DateTime startDateTime, double workDays)
    {
        DateTime endDateTime = _calendar.GetClosestWorkday(startDateTime, workDays);
        DateOnly workday = new(endDateTime.Year, endDateTime.Month, endDateTime.Day);
        double remainingTimeSpan = workDays % 1;
        WorkdayEndTime workdayEndTime = _workday.GetEndTime(endDateTime, remainingTimeSpan);
        if (workdayEndTime.EndingWorkdayType == EndingWorkdayType.NextDay) {
            workday = _calendar.GetNextWorkday(endDateTime);
        }
        else if (workdayEndTime.EndingWorkdayType == EndingWorkdayType.PreviousDay) {
            workday = _calendar.GetPreviousWorkday(endDateTime);
        }

        return new DateTime(workday.Year, workday.Month, workday.Day, workdayEndTime.EndTime.Hour, workdayEndTime.EndTime.Minute, workdayEndTime.EndTime.Second);
    }
}