using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day5
    {
        public static void Solve1()
        {
            using (StreamReader sr = File.OpenText("input5"))
            {
                var textLines = sr.ReadToEnd().Split('\n');
                var lines = FindLines(textLines);
                var turn = 0;

                foreach (var line in lines)
                {
                    turn++;
                    Console.WriteLine($"{turn}: {line}");
                    line.PrintCovered();
                }

                var score = lines
                    .SelectMany(line => line.Covered()) // A flat list of all the covered coordinates, with duplicates
                    .GroupBy(coord => coord) // Duplicates Grouped
                    .Where(group => group.Count() > 1) // Only the groups with more than one coverage
                    .Count() // The score
                    ;
                Console.WriteLine(score);
            }
        }

        public static List<Line> FindLines(IEnumerable<string> lines)
        {
            var result = new List<Line>();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(" -> ");
                var start = parts[0].Split(',');
                var end = parts[1].Split(',');

                var created = new Line(
                        new Coord(int.Parse(start[0]), int.Parse(start[1])),
                        new Coord(int.Parse(end[0]), int.Parse(end[1])));

                Console.WriteLine($"{line} : {created}");
                result.Add(created);
            }
            return result;
        }
    }

    public class Line
    {

        public Coord Start { get; set; }
        public Coord End { get; set; }

        public Line(Coord start, Coord end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return $"{Start} -> {End}";
        }

        public List<Coord> Covered()
        {
            if (IsHorizontal)
                return HorizontalCovered();
            else if (IsVertical)
                return VerticalCovered();
            else 
                return DiagonalCovered();
        }

        public List<Coord> HorizontalCovered()
        {
            var result = new List<Coord>();
            var positive = Start.X < End.X;
            var start = positive ? Start.X : End.X;
            var end = positive ? End.X : Start.X;
            var y = Start.Y;
            for (int x = start; x <= end; x += 1)
            {
                result.Add(new Coord(x, y));
            }
            return result;
        }

        public List<Coord> VerticalCovered()
        {
            var result = new List<Coord>();
            var positive = Start.Y < End.Y;
            var start = positive ? Start.Y : End.Y;
            var end = positive ? End.Y : Start.Y;
            var x = Start.X;
            for (int y = start; y <= end; y += 1)
            {
                result.Add(new Coord(x, y));
            }
            return result;
        }

        public List<Coord> DiagonalCovered()
        {
            var result = new List<Coord>();
            var positive = (End.Y - Start.Y) / (End.X - Start.X) > 0;
            var left = Start.X < End.X ? Start : End;
            var right = Start.X < End.X ? End : Start;

            var length = right.X - left.X; // Rise and run are always the same
            // We always walk from left to right, so X always increases
            var xInc = 1;
            var yInc = positive ? 1 : -1;

            for (int offset = 0; offset <= length; offset++)
            {
                result.Add(new Coord(left.X + xInc * offset, left.Y + yInc * offset));
            }

            return result;
        }

        public void PrintCovered()
        {
            foreach(var coord in Covered())
            {
                Console.WriteLine($"    {coord}");
            }
        }

        public bool IsHorizontal => Start.Y == End.Y;
        public bool IsVertical   => Start.X == End.X;
    }

    public record struct Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
