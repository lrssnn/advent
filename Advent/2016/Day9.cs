using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day9
{
    public string Input;

    public Day9()
    {
        using StreamReader sr = File.OpenText("2016/input9");
        Input = sr.ReadToEnd().Trim();
    }

    public void Solve()
    {
        var tests = new List<string>()
        {
            "ADVENT",
            "A(1x5)BC",
            "(3x3)XYZ",
            "A(2x2)BCD(2x2)EFG",
            "(6x1)(1x3)A",
            "X(8x2)(3x3)ABCY",
            "(27x12)(20x12)(13x14)(7x10)(1x12)A",
            "(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN",
        };

        foreach(var test in tests)
        {
            var output = Decompress(test);
            Console.WriteLine($"{test} -> {output} ({output.Length} | {CalculateLength(test)})");
        }

        Console.WriteLine(Decompress(Input).Length);

        var lastLength = Input.Length;
        var res = Decompress(Input);
        while(res.Length != lastLength)
        {
            Console.WriteLine(res.Length);
            res = Decompress(res);
        }

        Console.WriteLine(res.Length);
    }

    public int CalculateLength(string input)
    {
        var output = 0;
        
        for(var i = 0; i < input.Length; i++)
        {
            if(input[i] == '(')
            {
                i++;
                var j = i;
                while (input[j] != 'x') j++;
                var length = int.Parse(input[i..j]);
                j++;
                var k = j;
                while (input[k] != ')') k++;
                var repeats = int.Parse(input[j..k]);
                k++;

                output += repeats * CalculateLength(input[k..(k+length)]);
                i = k + length - 1;
            } else
            {
                output++;
            }
        }
        return output;
    }

    public string Decompress(string input)
    {
        var output = new StringBuilder();
        
        for(var i = 0; i < input.Length; i++)
        {
            if(input[i] == '(')
            {
                i++;
                var j = i;
                while (input[j] != 'x') j++;
                var length = int.Parse(input[i..j]);
                j++;
                var k = j;
                while (input[k] != ')') k++;
                var repeats = int.Parse(input[j..k]);
                k++;

                foreach (var repeat in Enumerable.Range(0,repeats)) output.Append(input[k..(k + length)]);
                i = k + length - 1;
            } else
            {
                output.Append(input[i]);
            }
        }
        return output.ToString();
    }

}
