namespace Calendar;
public class MonthDay {
    private int _month;
    private int _day;

    public MonthDay(int month, int day) {
        if (month < 0 || month > 12) throw new ArgumentOutOfRangeException("Invalid month: " + month);
        if (day < 0 || day > DateTime.DaysInMonth(2000, month)) throw new ArgumentOutOfRangeException("Invalid day: " + day);
        _month = month;
        _day = day;
    }

    public int Month() {
        return _month;
    }

    public int Day() {
        return _day;
    }
}