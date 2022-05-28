using Advent;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day17 : Day
    {

        public override string DayName => "17";
        public override string Answer1 => "3160";
        public override string Answer2 => "1928";

        public Area Target { get; set; }

        public Day17() : base("2021/input17")
        {
                //var input = @"target area: x=20..30, y=-10..-5"; // Highest y 45 (vel 6,9)

            var parts = Input.Split(' ');
            var xrange = parts[2][2..^1].Split("..");
            var yrange = parts[3][2..].Split("..");

            var xMin = int.Parse(xrange[0]);
            var xMax = int.Parse(xrange[1]);
            var yMin = int.Parse(yrange[0]);
            var yMax = int.Parse(yrange[1]);

            Target = new Area(xMin, xMax, yMin, yMax);
        }

        public override void Solve()
        {
            // Define the search space
            var xMin = 1;
            var xSteps = 1000;
            var yMin = -100;
            var ySteps = 2000;

            // For each velocity in the search space, generate the path it would take
            var solutions = new List<List<PointVel>>();

            foreach(int xVel in Enumerable.Range(xMin, xSteps))
            {
                foreach(int yVel in Enumerable.Range(yMin, ySteps))
                {
                    // Check if its worth looking at this path: 
                    // Does it reach to the target columns?
                    var maxX = CalculateMax(xVel);
                    if (maxX < Target.XMin) continue;

                    var vel = new PointVel(0, 0, xVel, yVel);
                    var path = PathOf(vel);
                    if (path.Any())
                    {
                        solutions.Add(path);
                    }
                }
            }

            Result1 = solutions.Max(solution => solution.Max(point => point.Y)).ToString();
            Result2 = solutions.Count.ToString();
        }

        public List<PointVel> PathOf(PointVel vel)
        {
            const int STEPS = 1000;
            var path = new List<PointVel>();
            var location = new PointVel(vel);
            path.Add(location);
            foreach (int step in Enumerable.Range(1, STEPS))
            {
                location = ApplyPhysics(location);
                path.Add(location);
                if (Target.Contains(location))
                    return path;
            }
            return new List<PointVel>();
        }

        public static int CalculateMax(int initial)
        {
            return (initial * (initial + 1)) / 2;
        }

        public static PointVel ApplyPhysics(PointVel p)
        {
            var x = p.X + p.XVel;
            var y = p.Y + p.YVel;
            var sign = Math.Sign(p.XVel);
            var xVel = p.XVel - sign; // - 1 if positive, - -1 if negative, so always moves towards zero
            var yVel = p.YVel - 1;
            return new PointVel(x, y, xVel, yVel);
        }
    }

    public record struct Area
    {
        public int XMin { get; set; }
        public int XMax { get; set; }
        public int YMin { get; set; }
        public int YMax { get; set; }

        public Area(int xMin, int xMax, int yMin, int yMax)
        {
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
        }

        public bool Contains(PointVel p) =>
            XMin <= p.X && XMax >= p.X && YMin <= p.Y && YMax >= p.Y;
    }

    public record struct PointVel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int XVel { get; set; }
        public int YVel { get; set; }

        public PointVel(PointVel input)
        {
            X = input.X;
            Y = input.Y;
            XVel = input.XVel;
            YVel = input.YVel;
        }

        public PointVel(int x, int y, int xVel, int yVel)
        {
            X = x;
            Y = y;
            XVel = xVel;
            YVel = yVel;
        }
        
    }
}
