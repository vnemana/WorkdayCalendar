namespace Calendar;
// Nitpick: There are no constants in the constants.cs file

// Idiomatic C#: Put these in separate files that are named by the type name. EndingWorkdayType.cs WorkdayEndTime.cs Direction.cs
public enum EndingWorkdayType {
    CurrentDay,
    NextDay,
    PreviousDay
};

// I'd consider making some of these internal instead of public. If they are only used internal to this assembly for bookkeeping they don't need to be public if you arent asserting on them
public record WorkdayEndTime {
    public EndingWorkdayType EndingWorkdayType {get; set;}
    public TimeOnly EndTime {get; set;}
}

public enum Direction {
    Forward,
    Backward
}
