using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day2
{
    public List<Box> Boxes { get; set; }

    public Day2()
    {
        using (StreamReader sr = File.OpenText("2015/input2"))
        {

            var input = sr.ReadToEnd().Trim();
            var lines = input.Split("\n").Select(line => line.Trim()).ToArray();
            Boxes = lines.Select(line => new Box(line)).ToList();
        }
    }

    public void Solve()
    {
        Console.WriteLine(Boxes.Sum(b => b.TotalWrap()));
        Console.WriteLine(Boxes.Sum(b => b.TotalRibbon()));
    }

}

public class Box
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Box(string init)
    {
        var parts = init.Split('x');
        X = int.Parse(parts[0]);
        Y = int.Parse(parts[1]);
        Z = int.Parse(parts[2]);
    }

    public int SurfaceArea() => (2 * X * Y) + (2 * Y * Z) + (2 * Z * X);

    public int SmallestFaceArea()
    {
        var A = X * Y;
        var B = Y * Z;
        var C = Z * X;
        return Math.Min(Math.Min(A, B), C);
    }

    public int SmallestFacePerimiter()
    {
        var A = 2*X + 2*Y;
        var B = 2*Y + 2*Z;
        var C = 2*Z + 2*X;
        return Math.Min(Math.Min(A, B), C);
    }

    public int Volume() => X * Y * Z;

    public int TotalWrap() => SurfaceArea() + SmallestFaceArea();

    public int TotalRibbon() => SmallestFacePerimiter() + Volume();
}

