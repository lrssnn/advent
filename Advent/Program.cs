using AdventTwentyOne;

var start = DateTime.Now;
var day = new Day1();
var constructed = DateTime.Now;
Console.WriteLine($"Construction {(constructed - start).TotalMilliseconds}ms");
(var part1, var part2) = day.Solve();
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
day.Verify();
var end = DateTime.Now;
Console.WriteLine($"Solve {(end - constructed).TotalMilliseconds}ms");


