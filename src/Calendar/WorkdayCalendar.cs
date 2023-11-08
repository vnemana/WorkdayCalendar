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
        Direction direction = workDays > 0 ? Direction.Forward : Direction.Backward;
        int incrementalDay = direction == Direction.Forward ? 1 : -1;
        var remainingTimeSpan = TimeSpan.FromDays(workDays).TotalDays;
        var endDateTime = startDateTime;
        while (!IsCurrentWorkday(remainingTimeSpan, direction))
        {
            endDateTime = endDateTime.AddDays(incrementalDay);
            while (!_calendar.IsValidWorkday(endDateTime))
            {
                endDateTime = endDateTime.AddDays(incrementalDay);
            }
            remainingTimeSpan += -1 * incrementalDay;
        }
        var workday = new DateOnly(endDateTime.Year, endDateTime.Month, endDateTime.Day);
        var resultWorkDay = _workday.GetEndTime(endDateTime, remainingTimeSpan);
        if (resultWorkDay.EndingWorkdayType == EndingWorkdayType.NextDay)
        {
            workday = _calendar.GetNextWorkday(endDateTime);
        }
        else if (resultWorkDay.EndingWorkdayType == EndingWorkdayType.PreviousDay)
        {
            workday = _calendar.GetPreviousWorkday(endDateTime);
        }

        return new DateTime(workday.Year, workday.Month, workday.Day, resultWorkDay.EndTime.Hour, resultWorkDay.EndTime.Minute, resultWorkDay.EndTime.Second);
    }

    private static bool IsCurrentWorkday(double remainingTimeSpan, Direction direction)
    {
        if (direction == Direction.Forward && remainingTimeSpan < 1)
        {
            return true;
        }
        if (direction == Direction.Backward && remainingTimeSpan > -1)
        {
            return true;
        }
        return false;
    }
}