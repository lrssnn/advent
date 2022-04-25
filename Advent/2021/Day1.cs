using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace AdventTwentyOne;

public class Day1
{
    public string Answer1 => "1393";
    public string Answer2 => null;

    public string Result1 { get; set; }
    public string Result2 { get; set; }

    public List<int> Input { get; set; }

    public Day1()
    {
        using StreamReader sr = File.OpenText("2021/input1");
        var input = sr.ReadToEnd();

        Input = input.Trim()
            .Split("\n")
            .Select(s => s.Trim())
            .Select(s => int.Parse(s))
            .ToList();
    }

    public (string, string) Solve()
    {
        var last = int.MaxValue;
        var increases = 0;
        foreach (var depth in Input)
        {
            if(depth > last)
            {
                increases++;
            }
            last = depth;
        }
        Result1 = increases.ToString();
        return (increases.ToString(), "");
            
    }

    public override string ToString() => $"Part 1: {Result1} | Part 2: {Result2}";

    public void Verify()
    {
        Result1.Should().Be(Answer1);
        Result2.Should().Be(Answer2);
    }
}
