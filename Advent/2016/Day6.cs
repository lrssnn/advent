using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day6
{
    public string[] Input;

    public Day6()
    {
        using (StreamReader sr = File.OpenText("2016/input6"))
        {
            var input = sr.ReadToEnd().Trim();
            /*
            var input = @"eedadn
                            drvtee
                            eandsr
                            raavrd
                            atevrs
                            tsrnev
                            sdttsa
                            rasrtv
                            nssdts
                            ntnada
                            svetve
                            tesnvt
                            vntsnd
                            vrdear
                            dvrsen
                            enarar";
            */
            Input = input.Split('\n').Select(s => s.Trim()).ToArray();
        }
    }

    public void Solve()
    {
        var frequencies = new List<Dictionary<char, int>>();
        foreach(var c in Input[0]) frequencies.Add(new Dictionary<char, int>());

        foreach(var s in Input)
        {
            for(int i = 0; i < s.Length; i++)
            {
                Register(frequencies[i], s[i]);
            }
        }

        var result1 = frequencies.Select(d => d.OrderByDescending(kvp => kvp.Value).First().Key).ToArray();
        var result2 = frequencies.Select(d => d.OrderBy(kvp => kvp.Value).First().Key).ToArray();
        Console.WriteLine(result1);
        Console.WriteLine(result2);

    }

    public void Register(Dictionary<char, int> dict, char c)
    {
        if (dict.ContainsKey(c)) dict[c] += 1;
        else dict.Add(c, 1);
    }
}
