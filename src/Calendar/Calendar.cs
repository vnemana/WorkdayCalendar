
namespace Calendar;
public class Calendar {
    private readonly List<MonthDay> _recurringHolidays;
    private readonly List<DateOnly> _holidays;
    
    public Calendar() {
        _recurringHolidays = new List<MonthDay>();
        _holidays = new List<DateOnly>();
    }

    public IEnumerable<MonthDay> GetRecurringHolidays() {
        return _recurringHolidays;
    }

    public bool SetRecurringHoliday(int month, int day) {
        try {
            _recurringHolidays.Add(new MonthDay(month, day));
            return true;
        } catch (ArgumentOutOfRangeException e) {
            Console.WriteLine("Invalid inputs for recurring holiday - month: " + month + ", day: " 
            + day + " Exception: " + e.Message);
            return false;
        }
    }

    public bool SetHoliday(int year, int month, int day) {
        try {
            _holidays.Add(new DateOnly(year, month, day));
            return true;
        } catch (Exception e) {
            Console.WriteLine("Invalid date for holiday " + e.Message);
            return false;
        }
    }

    public bool IsValidWorkday(DateTime date) {
        MonthDay monthDay = new MonthDay(date.Month, date.Day);
        DateOnly dateOnly = new DateOnly(date.Year, date.Month, date.Day);
        if (_recurringHolidays.Any(rh => rh.Month().Equals(date.Month) && rh.Day().Equals(date.Day))) {
            return false;
        }
        if (_holidays.Any(h => h.Year.Equals(dateOnly.Year) && h.Month.Equals(dateOnly.Month) && h.Day.Equals(dateOnly.Day))) {
            return false;
        }
        if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday) {
            return false;
        }
        return true;
    }

    public DateOnly GetPreviousWorkday(DateTime date) {
        DateTime previousDay = date.AddDays(-1);
        while (!IsValidWorkday(previousDay)) {
            previousDay = previousDay.AddDays(-1);
        }
        return new DateOnly(previousDay.Year, previousDay.Month, previousDay.Day);
    }

    public DateOnly GetNextWorkday(DateTime date) {
        DateTime nextDay = date.AddDays(1);
        while (!IsValidWorkday(nextDay)) {
            nextDay = nextDay.AddDays(1);
        }
        return new DateOnly(nextDay.Year, nextDay.Month, nextDay.Day);
    }

    public DateTime GetClosestWorkday(DateTime inputDate, double workDays) {
        Direction direction = workDays > 0 ? Direction.Forward : Direction.Backward;
        var remainingTimeSpan = TimeSpan.FromDays(workDays).TotalDays;
        int incrementalDay = direction == Direction.Forward ? 1 : -1;
        DateTime endDateTime = inputDate;
        while (!IsCurrentWorkday(remainingTimeSpan, direction)) {
            endDateTime = endDateTime.AddDays(incrementalDay);
            while (!IsValidWorkday(endDateTime)) {
                endDateTime = endDateTime.AddDays(incrementalDay);
            }
            remainingTimeSpan += -1 * incrementalDay;
        }
        return endDateTime;
    }

    private static bool IsCurrentWorkday(double remainingTimeSpan, Direction direction) {
        if (direction == Direction.Forward && remainingTimeSpan < 1) {
            return true;
        }
        if (direction == Direction.Backward && remainingTimeSpan > -1) {
            return true;
        }
        return false;
    }

}
