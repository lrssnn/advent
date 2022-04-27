using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent;
using FluentAssertions;

namespace AdventTwentyOne;

public class Day2 : Day
{
    public override string DayName => "02";
    public override string Answer1 => "1855814";
    public override string Answer2 => "1845455714";

    private List<string> Data { get; set; }

    public Day2(string inputFileName) : base(inputFileName)
    {
        Data = Input.Trim()
            .Split("\n")
            .Select(s => s.Trim())
            .ToList();
    }

    public override void Solve()
    {
        var pos = 0;
        var depth = 0;

        foreach (var step in Data)
            (pos, depth) = ApplyStep(pos, depth, step);

        Result1 = (pos * depth).ToString();

        pos = 0;
        depth = 0;
        var aim = 0;

        foreach (var step in Data)
            (pos, depth, aim) = ApplyStep(pos, depth, aim, step);

        Result2 = (pos * depth).ToString();

    }

    private static (int pos, int depth) ApplyStep(int pos, int depth, string step)
    {
        var parts = step.Split(' ');
        var amt = int.Parse(parts[1]);
        return parts[0] switch
        {
            "forward" => (pos + amt, depth),
            "down"    => (pos, depth + amt),
            "up"      => (pos, depth - amt),
            _         => throw new Exception("Unrecognised Instruction"),
        };
    }

    private static (int pos, int depth, int aim) ApplyStep(int pos, int depth, int aim, string step)
    {
        var parts = step.Split(' ');
        var amt = int.Parse(parts[1]);
        return parts[0] switch
        {
            "forward" => (pos + amt, depth + (aim * amt), aim),
            "down"    => (pos, depth, aim + amt),
            "up"      => (pos, depth, aim - amt),
            _         => throw new Exception("Unrecognised Instruction"),
        };
    }
}
