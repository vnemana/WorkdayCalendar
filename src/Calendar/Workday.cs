using System.Runtime;

namespace Calendar;
public class Workday {
    private TimeOnly _startTime;
    private TimeOnly _stopTime;
    public bool SetStartTime(string hours) {
        return TimeOnly.TryParse(hours, out _startTime);
    }
    public bool SetStopTime(string hours) {
        return TimeOnly.TryParse(hours, out _stopTime);
    }

    public double GetMilliseconds() {
        return (_stopTime - _startTime).TotalMilliseconds;
    }

    public WorkdayEndTime GetEndTime(DateTime inputDate, double remainingTime)
    {
        Direction direction = remainingTime > 0 ? Direction.Forward : Direction.Backward;
        var startTimeData = GetStartTime(inputDate, direction);
        DateTime endDateTime = startTimeData.Item1.AddMilliseconds(remainingTime * GetMilliseconds());
        return GetEndTimeForDate(inputDate, endDateTime, direction, startTimeData.Item2);
    }

    private Tuple<DateTime, EndingWorkdayType> GetStartTime(DateTime inputDate, Direction direction) {
        TimeOnly inputStartTime = new TimeOnly(inputDate.Hour, inputDate.Minute, inputDate.Second);
        DateTime startDateTime = inputDate;
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
