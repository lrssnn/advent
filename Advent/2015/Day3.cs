using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day3
{
    public string Input;

    public Day3()
    {
        using (StreamReader sr = File.OpenText("2015/input3"))
        {
            Input = sr.ReadToEnd().Trim();
        }
    }

    public void Solve()
    {
        Console.WriteLine(SantaSolve(Input));
        Console.WriteLine(RoboSolve(Input));
    }

    private int SantaSolve(string input)
    {
        var Houses = new HashSet<(int X, int Y)>();
        var x = 0;
        var y = 0;
        Houses.Add((x, y));
        foreach (char c in input)
        {
            (x, y) = Move(x, y, c);
            Houses.Add((x, y));
        }
        return Houses.Count;
    }

    private int RoboSolve(string input)
    {
        var Houses = new HashSet<(int X, int Y)>();
        var santaX = 0;
        var santaY = 0;
        var robotX = 0;
        var robotY = 0;
        Houses.Add((santaX, santaY));
        for (int i = 0; i < input.Length; i += 2)
        {
            var c1 = input[i];
            var c2 = input[i + 1];
            (santaX, santaY) = Move(santaX, santaY, c1);
            (robotX, robotY) = Move(robotX, robotY, c2);
            Houses.Add((santaX, santaY));
            Houses.Add((robotX, robotY));
        }
        return Houses.Count;
    }

    private (int x, int y) Move(int x, int y, char c)
    {
        return c switch
        {
            '>' => (x + 1, y),
            '<' => (x - 1, y),
            '^' => (x, y + 1),
            'v' => (x, y - 1),
        };
    }
}
