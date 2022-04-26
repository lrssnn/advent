using Advent;
using AdventTwentyOne;


var days = new Day[] { new Day1("2021/input1"), new Day2("2021/input2") };

Console.WriteLine($"+-----+----------+-----------------+");
Console.WriteLine($"| Day | Is Valid | Solve Time (ms) |");
Console.WriteLine($"+-----+----------+-----------------+");
foreach(Day day in days)
{
    var start = DateTime.Now;
    day.Solve();
    var end = DateTime.Now;
    var solveTime = end - start;
    Console.WriteLine($"| {day.DayName}   | {day.IsValid()}     |       {solveTime.TotalMilliseconds} |");
}
Console.WriteLine($"+-----+----------+-----------------+");


