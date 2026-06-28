namespace flextime_calculator.Models;

public record TimeCalculations(
    TimeSpan FeierabendWeek,
    TimeSpan FeierabendDay,
    TimeSpan[] DayDeltas,
    TimeSpan[] CumDeltas);
