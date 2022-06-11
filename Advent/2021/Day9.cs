using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day9 : Day
    {
        public override string DayName => "09";
        public override string Answer1 => "603";
        public override string Answer2 => "786780";

        private List<List<Point>> Grid;
        
        public Day9() : base("2021/input9")
        {

            /*
                Input = @"2199943210
                          3987894921
                          9856789892
                          8767896789
                          9899965678";
            */

            Grid = Input.Trim().Split('\n').Select((line, y) => 
                line.Trim().Select((c, x) => 
                    new Point(x, y, int.Parse(c.ToString()))
                ).ToList()
            ).ToList();
        }

        public override void Solve()
        {
            var lowPoints = GetLowPoints();
            var basins = GetBasins(lowPoints);
            
            Result1 = lowPoints.Sum(p => p.Value + 1).ToString();
            Result2 = basins.OrderByDescending(basin => basin.Value.Count)
                .Take(3)
                .Aggregate(1,(product, basin) => product * basin.Value.Count)
                .ToString();
        }

        public List<Point> GetLowPoints()
        {
            var lowPoints = new List<Point>();
            foreach(var row in Grid)
            {
                foreach(var point in row)
                {
                    if (GetLowNeighbour(point) == point && point.Value < 9)
                    {
                        point.BasinSink = point;
                        lowPoints.Add(point);
                    }
                }
            }
            return lowPoints;
        }
        
        public Dictionary<Point, List<Point>> GetBasins(List<Point> lowPoints) {
            // Each low point is certainly in its own basin
            var basins = new Dictionary<Point, List<Point>>();
            foreach(var point in lowPoints){
                basins[point] = new List<Point> { point };
            }
            
            // For each point on the grid, follow its 'flow' until we come across
            // Somewhere that we know where it leads
            foreach(var row in Grid) 
            {
                foreach(var point in row) 
                {
                    // Don't add the sinks to their own basins
                    if(basins.ContainsKey(point)) continue;
                    
                    // 9s don't go into a basin
                    if(point.Value == 9) continue;
                    
                    // Follow the neighbours until one takes us to a basin
                    var neighbour = point;
                    var path = new List<Point>();
                    while(neighbour.BasinSink == null){
                        // Remember this point so we can mark its basin once we find it
                        path.Add(neighbour);
                        neighbour = GetLowNeighbour(neighbour);
                    }
                    
                    // Mark all the ones we found that lead here
                    foreach(var p in path) p.BasinSink = neighbour.BasinSink;
                    
                    // Officially put this point in the right basin
                    basins[neighbour.BasinSink].Add(point);
                }
            }
                
            return basins;
        }

        public Point GetLowNeighbour(Point p) {
            var value = p.Value;
            
            Point candidate = p;
            // Above
            if (p.Y != 0 && Grid[p.Y - 1][p.X].Value < candidate.Value)
                candidate = Grid[p.Y - 1][p.X];
            // Below
            if (p.Y != Grid.Count -1 && Grid[p.Y + 1][p.X].Value < candidate.Value) 
                candidate = Grid[p.Y + 1][p.X];
            // Left
            if (p.X != 0 && Grid[p.Y][p.X - 1].Value < candidate.Value) 
                candidate = Grid[p.Y][p.X - 1];
            // Right
            if (p.X != Grid[p.Y].Count -1 && Grid[p.Y][p.X + 1].Value < candidate.Value) 
                candidate = Grid[p.Y][p.X + 1];

            return candidate;
        }
    }

    public class Point
    {
        public int X;
        public int Y;
        public int Value;
        public Point? BasinSink;

        public Point(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}
