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

        // these should be constructor parameters. Maybe even just require the calendar and the workday to be passed in to the constructor
        _workday.SetStartTime("08:00");
        _workday.SetStopTime("16:00");
        _calendar.SetRecurringHoliday(5, 17);
        _calendar.SetHoliday(2004, 5, 27);
    }

    // Same comment with Get in the method name
    public DateTime GetEndTime(DateTime startDateTime, double workDays)
    {
        // Idiomatic C#: use var
        DateTime endDateTime = _calendar.GetClosestWorkday(startDateTime, workDays);
        // Idiomatic C#: use var
        DateOnly workday = new(endDateTime.Year, endDateTime.Month, endDateTime.Day);
        // Idiomatic C#: use var
        double remainingTimeSpan = workDays % 1;
        // Idiomatic C#: use var
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