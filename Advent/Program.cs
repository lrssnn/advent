﻿using Advent;
using AdventTwentyOne;


var days = new Day[] { new Day1("2021/input1"), new Day2("2021/input2"), new Day3("2021/input3")};

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

