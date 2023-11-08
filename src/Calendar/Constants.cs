namespace Calendar;
public enum EndingWorkdayType {
    CurrentDay,
    NextDay,
    PreviousDay
};

public record WorkdayEndTime {
    public EndingWorkdayType EndingWorkdayType {get; set;}
    public TimeOnly EndTime {get; set;}
}

public enum Direction {
    Forward,
    Backward
}