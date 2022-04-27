using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent;
using FluentAssertions;

namespace AdventTwentyOne;

public class Day3 : Day
{
    public override string DayName => "03";
    public override string Answer1 => "3148794";
    public override string Answer2 => "2795310";

    private List<string> Data { get; set; }

    public Day3(string inputFileName) : base(inputFileName)
    {
        Data = Input.Trim()
            .Split("\n")
            .Select(s => s.Trim())
            .ToList();
    }

    public override void Solve()
    {
        var binary = "";
        var inverse = "";

        var threshold = Data.Count / 2;

        for(int i = 0; i < Data[0].Length; i++)
        {
            var most = MostFrequent(Data, i);
            binary += most;
            inverse += most == '1' ? '0' : '1';
        }

        var number = Convert.ToInt32(binary, 2);
        var inv = Convert.ToInt32(inverse, 2);
        Result1 = (number * inv).ToString();

        var oxy = Data.ToList();
        var index = 0;
        while(oxy.Count > 1)
        {
            var desired = MostFrequent(oxy, index);
            oxy = oxy.Where(x => x[index] == desired).ToList();
            index++;
        }

        var oxyN = Convert.ToInt32(oxy.Single(), 2);

        var c02 = Data.ToList();
        index = 0;
        while(c02.Count > 1)
        {
            var desired = MostFrequent(c02, index) == '1' ? '0' : '1';
            c02 = c02.Where(x => x[index] == desired).ToList();
            index++;
        }

        var c02N = Convert.ToInt32(c02.Single(), 2);

        Result2 = (oxyN * c02N).ToString();
    }

    private static char MostFrequent(List<string> items, int index)
    {
        var ones = 0;
        var zeroes = 0;
        foreach(var item in items)
        {
            if (item[index] == '1')
                ones++;
            else
                zeroes++;
        }

        return ones >= zeroes ? '1' : '0';
    }
}
