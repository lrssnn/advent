using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day9
    {
        public List<List<int>> lines;

        public Day9()
        {
            using (StreamReader sr = File.OpenText("input9"))
            {

                lines = sr.ReadToEnd().Trim().Split('\n').Select(line =>
                    line.Trim().Select(c => int.Parse(c.ToString())).ToList()
                    ).ToList();

                /*
                lines = @"2199943210
3987894921
9856789892
8767896789
9899965678".Trim().Split('\n').Select(line => line.Trim().Select(c => int.Parse(c.ToString())).ToList()).ToList();
                */
            }
        }

        public void Solve()
        {
            var lowPoints = GetLowPoints();
            foreach(var lowPoint in lowPoints)
            {
                Console.WriteLine($"{lowPoint.X},{lowPoint.Y}: {lines[lowPoint.X][lowPoint.Y]}");
            }

            var basins = lowPoints.Select(lowPoint => FillBasin(lowPoint, new HashSet<Point>()));

            foreach(var basin in basins)
            {
                Console.Write($"{basin.Count()}: ");
                foreach (var point in basin)
                {
                    Console.Write($"({point.X}, {point.Y}), ");
                }
                Console.WriteLine();
            }

            Console.WriteLine(lowPoints.Sum(p => lines[p.X][p.Y] + 1));
            Console.WriteLine(basins
                .OrderBy(basin => basin.Count)
                .TakeLast(3)
                .Aggregate(1, (prod, basin) => prod * basin.Count())
            );
        }

        public List<Point> GetLowPoints()
        {
            var lowPoints = new List<Point>();
            for(int x = 0; x < lines.Count; x++)
            {
                for(int y = 0; y < lines[x].Count; y++)
                {
                    var point = new Point(x, y);
                    if (IsLowPoint(point))
                        lowPoints.Add(point);
                }
            }
            return lowPoints;
        }

        public HashSet<Point> FillBasin(Point p, HashSet<Point> basin)
        {
            var value = lines[p.X][p.Y];
            
            // This point is in the basin unless it is a 9
            if (value == 9) return basin;
            basin.Add(p);
            // Check the four directions
            if (p.X != 0 && value <= lines[p.X - 1][p.Y])
                FillBasin(new Point(p.X - 1, p.Y), basin);
            // Below
            if (p.X != lines.Count -1 && value <= lines[p.X + 1][p.Y]) 
                FillBasin(new Point(p.X + 1, p.Y), basin);
            // Left
            if (p.Y != 0 && value <= lines[p.X][p.Y - 1]) 
                FillBasin(new Point(p.X, p.Y - 1), basin);
            // Above
            if (p.Y != lines[p.X].Count -1 && value <= lines[p.X][p.Y + 1]) 
                FillBasin(new Point(p.X, p.Y + 1), basin);

            return basin;
        }

        public bool IsLowPoint(Point p)
        {
            var value = lines[p.X][p.Y];
            // Above
            if (p.X != 0 && value >= lines[p.X - 1][p.Y])
                return false;
            // Below
            if (p.X != lines.Count -1 && value >= lines[p.X + 1][p.Y]) 
                return false;
            // Left
            if (p.Y != 0 && value >= lines[p.X][p.Y - 1]) 
                return false;
            // Above
            if (p.Y != lines[p.X].Count -1 && value >= lines[p.X][p.Y + 1]) 
                return false;

            // No smaller number in any direciton
            return true;
        }
    }

    public record struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
