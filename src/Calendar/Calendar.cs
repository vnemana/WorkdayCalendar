
namespace Calendar;
public class Calendar {
    // Idiomatic C#: use the Least common interface you need for the class, in this case IList<T> would be good
    private readonly List<MonthDay> _recurringHolidays;
    private readonly List<DateOnly> _holidays;
    
    // The constructor isn't doing anything, you can remove it and let it use the default constructor and just initialize the fields like this:
    // private readonly List<MonthDay> _recurringHolidays = new List<MonthDay>();
    // private readonly List<DateOnly> _holidays = new List<DateOnly>();
    // or more modern C#:
    // private readonly List<MonthDay> _recurringHolidays = new();
    // private readonly List<DateOnly> _holidays = new();
    
    public Calendar() {
        _recurringHolidays = new List<MonthDay>();
        _holidays = new List<DateOnly>();
    }

    // Idiomatic C#: This should be a readonly property
    // Best practice: It should return an immutable version of the list like this:
    // public IReadOnlyList<MonthDay> RecurringHolidays => _recurringHolidays.AsReadOnly();
    public IEnumerable<MonthDay> GetRecurringHolidays() {
        return _recurringHolidays;
    }

    // The naming here is weird. Set implies it is mutating the object, which in C# would be a property, but it is really adding a month and day to a collection
    // I'd rename this to AddRecurringHoliday. Also, 2 ints are indistinguishable from each other, which means its very easy to use the wrong one in the wrong place
    // in production code. This could be improved by passing in a value object or enums or something to prevent that.
    // Something like AddRecurringHoliday(RecurringHoliday holiday) or AddRecurringHoliday(Month month, Day day)
    // Could even just use a DateTime object here
    public bool SetRecurringHoliday(int month, int day) {
        // Bad code: Don't use exceptions for control flow. Do validation as conditional code not as exceptions. Exceptions should only ever be exceptional scenarios
        try {
            _recurringHolidays.Add(new MonthDay(month, day));
            return true;
        } catch (ArgumentOutOfRangeException e) {
            Console.WriteLine("Invalid inputs for recurring holiday - month: " + month + ", day: " 
            + day + " Exception: " + e.Message);
            return false;
        }
    }

    // Same as the other comment. Could even just use a DateTime object here too
    public bool SetHoliday(int year, int month, int day) {
        // Bad code: Don't use exceptions for control flow. Do validation as conditional code not as exceptions. Exceptions should only ever be exceptional scenarios
        try {
            _holidays.Add(new DateOnly(year, month, day));
            return true;
        } catch (Exception e) {
            Console.WriteLine("Invalid date for holiday " + e.Message);
            return false;
        }
    }

    public bool IsValidWorkday(DateTime date) {
        // This variable is unused and should be deleted.
        MonthDay monthDay = new MonthDay(date.Month, date.Day);
        // Idiomatic C#: replace with var
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

    // This comment isn't that important, just a thought.
    // Because of the get/set properties in C#, whenever I am tempted to name a method in C# with "Get" or "Set" in it I take a pause and ask myself what
    // I'm trying to do exactly and see if there is a better name, or maybe a different design that would better fit.
    public DateOnly GetPreviousWorkday(DateTime date) {
        // Idiomatic C#: replace with var
        DateTime previousDay = date.AddDays(-1);
        // My code probably has the same issue now that I think about it, but reading yours I see that you might want to add a bottom out on this loop.
        // What happens if you created every day as a holiday or defined the workday with 0 hours.
        // Eventually it would hit DateTime.MinValue, but I'm not sure what happens with the loop here when that happens. Maybe it throws an exception?
        // Obviously an edge case and not something to be overly worried about in your code. I wouldn't hold it against you in a review and given how
        // the reviews I did went, I don't think anyone would have said anything about it if I submitted this code
        while (!IsValidWorkday(previousDay)) {
            previousDay = previousDay.AddDays(-1);
        }
        return new DateOnly(previousDay.Year, previousDay.Month, previousDay.Day);
    }

    public DateOnly GetNextWorkday(DateTime date) {
        // Idiomatic C#: replace with var
        DateTime nextDay = date.AddDays(1);
        // Similar to the last one, this one has potential to continue on forever, but again, super edge case and no one would've said anything.
        // I probably have the same issue in my code.
        while (!IsValidWorkday(nextDay)) {
            nextDay = nextDay.AddDays(1);
        }
        return new DateOnly(nextDay.Year, nextDay.Month, nextDay.Day);
    }

    public DateTime GetClosestWorkday(DateTime inputDate, double workDays) {
        // Idiomatic C#: replace with var
        Direction direction = workDays > 0 ? Direction.Forward : Direction.Backward;
        var remainingTimeSpan = TimeSpan.FromDays(workDays).TotalDays;
        // Idiomatic C#: replace with var
        int incrementalDay = direction == Direction.Forward ? 1 : -1;
        // Idiomatic C#: replace with var
        DateTime endDateTime = inputDate;
        
        // This was my initial algorithm too, but I changed mine slightly because I had some unit tests that were not passing like this. I can't remember the specific test case though
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
        // This could be inlined like this:
        // return (direction == Direction.Forward && remainingTimeSpan < 1) ||
        //        (direction == Direction.Backward && remainingTimeSpan > -1);
        // Or changed to a switch case with default fall through like this:
        // switch (direction)
        // {
        //     case Direction.Forward when remainingTimeSpan < 1:
        //     case Direction.Backward when remainingTimeSpan > -1:
        //         return true;
        //     default:
        //         return false;
        // }

        if (direction == Direction.Forward && remainingTimeSpan < 1) {
            return true;
        }
        if (direction == Direction.Backward && remainingTimeSpan > -1) {
            return true;
        }
        return false;
    }

}
