namespace Calendar;
public class MonthDay {
    private int _month;
    private int _day;

    public MonthDay(int month, int day) {
        // You passed "Invalid month: ..." in the parameter name of the exception constructor instead of the message.
        // This should be new ArgumentOutOfRangeException(nameof(month), "Invalid month: " + month);
        if (month < 0 || month > 12) throw new ArgumentOutOfRangeException("Invalid month: " + month);
        if (day < 0 || day > DateTime.DaysInMonth(2000, month)) throw new ArgumentOutOfRangeException("Invalid day: " + day);
        _month = month;
        _day = day;
    }

    // Idiomatic C#: This should be a readonly property
    // public int Month => _month;
    public int Month() {
        return _month;
    }

    // Idiomatic C#: This should be a readonly property
    // public int Day => _day;
    public int Day() {
        return _day;
    }
}