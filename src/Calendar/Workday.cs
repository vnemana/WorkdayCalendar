using System.Runtime;

namespace Calendar;
public class Workday {
    private TimeOnly _startTime;
    private TimeOnly _stopTime;
    
    // I'd separate the parsing from the object and make these an auto-property with an init only setter for construction time
    // public TimeOnly StartTime { get; init; }
    // public TimeOnly StopTime { get; init; }
    public bool SetStartTime(string hours) {
        return TimeOnly.TryParse(hours, out _startTime);
    }
    public bool SetStopTime(string hours) {
        return TimeOnly.TryParse(hours, out _stopTime);
    }

    // Idiomatic C#: This should be a property and since it is the duration, I'd use the same "Total" in the name
    // public double TotalMilliseconds => (_stopTime - _startTime).TotalMilliseconds; 
    public double GetMilliseconds() {
        return (_stopTime - _startTime).TotalMilliseconds;
    }

    // Same comment about using Get in a method name, I'd name this more about what the method is doing: CalculateEndTime
    public WorkdayEndTime GetEndTime(DateTime inputDate, double remainingTime)
    {
        // idiomatic C#: use var
        Direction direction = remainingTime > 0 ? Direction.Forward : Direction.Backward;
        var startTimeData = GetStartTime(inputDate, direction);
        // idiomatic C#: use var
        DateTime endDateTime = startTimeData.Item1.AddMilliseconds(remainingTime * GetMilliseconds());
        return GetEndTimeForDate(inputDate, endDateTime, direction, startTimeData.Item2);
    }
    
    // This function and the end function has some duplicated complexity that I think can be simplified to improve readability.
    // I think it might be a result of your approach, but if you look they both have this conditional on behaving one way or another based on direction
    // Given your approach, I would probably split this and use the strategy pattern, one behavior for forward, and one behavior for backwards
    
    private Tuple<DateTime, EndingWorkdayType> GetStartTime(DateTime inputDate, Direction direction) {
        // idiomatic C#: use var
        TimeOnly inputStartTime = new TimeOnly(inputDate.Hour, inputDate.Minute, inputDate.Second);
        // idiomatic C#: use var
        DateTime startDateTime = inputDate;
        // idiomatic C#: use var
        EndingWorkdayType endingWorkdayType = EndingWorkdayType.CurrentDay;
        if (direction == Direction.Forward) {
            if (inputStartTime < _startTime) {
                startDateTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, _startTime.Hour, _startTime.Minute, _startTime.Second);
            }
            if (inputStartTime > _stopTime) {
                startDateTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, _startTime.Hour, _startTime.Minute, _startTime.Second);
                endingWorkdayType = EndingWorkdayType.NextDay;
            }
        }
        else if (direction == Direction.Backward) {
            if (inputStartTime > _stopTime) {
                startDateTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, _stopTime.Hour, _stopTime.Minute, _stopTime.Second);
            }
            if (inputStartTime < _startTime) {
                startDateTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, _stopTime.Hour, _stopTime.Minute, _stopTime.Second);
                endingWorkdayType = EndingWorkdayType.PreviousDay;
            }
        }
        return new Tuple<DateTime, EndingWorkdayType> (startDateTime, endingWorkdayType);
    }

    // Same comment about using Get in a method name
    private WorkdayEndTime GetEndTimeForDate(DateTime inputDate, DateTime endDateTime, Direction direction, EndingWorkdayType endingWorkdayType) {
        WorkdayEndTime result = new() {
            EndingWorkdayType = endingWorkdayType
        };
        DateTime endTime = endDateTime;
        if (direction == Direction.Forward) {
            if (endDateTime.TimeOfDay > _stopTime.ToTimeSpan()) {
                var nextDayTime = endDateTime.TimeOfDay - _stopTime.ToTimeSpan();
                var nextDayStartTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, _startTime.Hour, _startTime.Minute, _startTime.Second);
                endTime = nextDayStartTime.AddMilliseconds(nextDayTime.TotalMilliseconds);
                result.EndingWorkdayType = EndingWorkdayType.NextDay;
            }
        }
        else if (direction == Direction.Backward) {
            if (endDateTime.TimeOfDay < _startTime.ToTimeSpan()) {
                var nextDayTime = endDateTime.TimeOfDay - _startTime.ToTimeSpan();
                var nextDayEndTime = new DateTime(inputDate.Year, inputDate.Month, inputDate.Day, _stopTime.Hour, _stopTime.Minute, _stopTime.Second);
                endTime = nextDayEndTime.AddMilliseconds(nextDayTime.TotalMilliseconds);
                result.EndingWorkdayType = EndingWorkdayType.PreviousDay;
            }
        }
        result.EndTime = new TimeOnly(endTime.Hour, endTime.Minute, endTime.Second);
        return result;
    }
}
