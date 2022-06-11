using Advent;
using AdventTwentyOne;

/*
var day = new Day10();
day.Solve();
Console.WriteLine($"{day.Valid1} | {day.Valid2}");
*/

FullTest();


void FullTest()
{
    var constructionStart = DateTime.Now;
    var days = new Day[] {
        new Day1(), new Day2(), new Day3(), new Day4(), new Day5(), new Day6(), new Day7(), new Day8(), new Day9(), new Day10(), new Day11(),
        new Day12(), new Day13(), new Day14(), 
        // Disabled because SUPREMELY slow new Day15(), new Day17(),
        new Day16(), new Day21() };
    var constructionend = DateTime.Now;
    var constructionTime = constructionend - constructionStart;
    Console.WriteLine($"Total construction time {constructionTime.TotalMilliseconds:0.00}ms");

    Console.WriteLine($"+-----+---+---+-----------------+");
    Console.WriteLine($"| Day | 1 | 2 | Solve Time (ms) |");
    Console.WriteLine($"+-----+---+---+-----------------+");
    var totalMillis = 0.0;
    foreach(Day day in days)
    {
        var start = DateTime.Now;
        day.Solve();
        var end = DateTime.Now;
        var solveTime = end - start;
        totalMillis += solveTime.TotalMilliseconds;
        Console.WriteLine($"|  {day.DayName} | {day.Valid1} | {day.Valid2} | {solveTime.TotalMilliseconds:       00000.00} |");
    }
    Console.WriteLine($"+-----+----------+--------------+");
    Console.WriteLine($"  Total solve time {totalMillis:0.00} ms");
}
