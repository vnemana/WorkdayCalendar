using Calendar;

// See https://aka.ms/new-console-template for more information
WorkdayCalendar workdayCalendar = new();
// var inputDate = new DateTime(2004, 5, 24, 18, 5, 0);
// Console.WriteLine(inputDate + " with the addition of -5.5 working days is " + workdayCalendar.GetEndTime(inputDate, -5.5));

var inputDate = new DateTime(2004, 5, 24, 18, 5, 0);
Console.WriteLine(inputDate + " with the addition of -5.5 working days is " + workdayCalendar.GetEndTime(inputDate, -5.5));
inputDate = new DateTime(2004, 05, 24, 19, 3, 0);
Console.WriteLine(inputDate + " with the addition of 44.723656 working days is " + workdayCalendar.GetEndTime(inputDate, 44.723656));
inputDate = new DateTime(2004, 05, 24, 18, 3, 0);
Console.WriteLine(inputDate + " with the addition of -6.7470217 working days is " + workdayCalendar.GetEndTime(inputDate, -6.7470217));
inputDate = new DateTime(2004, 05, 24, 8, 3, 0);
Console.WriteLine(inputDate + " with the addition of 12.782709 working days is " + workdayCalendar.GetEndTime(inputDate, 12.782709));
inputDate = new DateTime(2004, 05, 24, 7, 3, 0);
Console.WriteLine(inputDate + " with the addition of 8.276628 working days is " + workdayCalendar.GetEndTime(inputDate, 8.276628));
