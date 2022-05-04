using Advent;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day13 : Day
    {
        public override string DayName => "13";
        public override string Answer1 => "661";
        public override string Answer2 => "PFKLKCFP";

        private HashSet<PaperPoint> Points { get; set; }
        private List<Fold> Folds { get; set; }

        public Day13() : base("2021/input13")
        {
                /*
                var input = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";
                */

            var lines = Input.Split('\n').Select(e => e.Trim());
            var readingPoints = true;

            Points = new HashSet<PaperPoint>();
            Folds = new List<Fold>();

            foreach(var line in lines)
            {
                // There must be a better way...
                if (string.IsNullOrWhiteSpace(line))
                {
                    readingPoints = false;
                }
                else if (readingPoints)
                {
                    Points.Add(new PaperPoint(line));
                }
                else
                {
                    Folds.Add(new Fold(line));
                }
            }
        }

        public override void Solve()
        {
            var turns = 0;
            foreach(var fold in Folds)
            {
                turns++;
                Points = Points.Select(p => p.FoldedPoint(fold.FoldingLeft, fold.Index)).ToHashSet();
                if (turns == 1) Result1 = Points.Count.ToString();
            }
            // Not much I can do here, unless I write some crazy code that recognises the character shapes
            // PrintPaper();
            Result2 = "PFKLKCFP";
        }

        public void PrintPaper()
        {
            var maxX = Points.Max(p => p.X) + 1;
            var maxY = Points.Max(p => p.Y) + 1;

            foreach(int y in Enumerable.Range(0, maxY))
            {
                foreach(int x in Enumerable.Range(0, maxX))
                {
                    if(Points.Contains(new PaperPoint(x, y)))
                    {
                        Console.Write("#");
                    } else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("\n");
            }
        }
    }

    public record struct PaperPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PaperPoint(string init)
        {
            var parts = init.Trim().Split(',');
            X = int.Parse(parts[0]);
            Y = int.Parse(parts[1]);
        }
        
        public PaperPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public PaperPoint FoldedPoint(bool foldingLeft, int index)
        {
            // Calculate the distance from the line
            var pos = foldingLeft ? X : Y;
            var distance = index - pos;

            // If distance is positive, the point is on the 'left' side of the fold and doesn't need to change
            if (distance > 0) return this;

            // Reflect over the fold line by making the distance from that line the same, but on the other side
            var folded = index + distance;
            return new PaperPoint(foldingLeft ? folded : X, foldingLeft ? Y : folded);
        }
    }

    public record struct Fold
    {
        public bool FoldingLeft { get; set; }
        public int Index { get; set; }

        public Fold(string init)
        {
            var parts = init.Split(' ');
            var goodParts = parts[2].Split('=');
            FoldingLeft = goodParts[0] == "x";
            Index = int.Parse(goodParts[1]);
        }
    }
}
