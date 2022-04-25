using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day4
{
    public string Input;

    public Day4()
    {
        Input = "yzbqklnj";
    }

    public void Solve()
    {
        //Console.WriteLine(Lowest5DigitHashNumber("abcdef"));
        Console.WriteLine(Lowest5DigitHashNumber(Input));
        Console.WriteLine(Lowest6DigitHashNumber(Input));
    }

    public int Lowest5DigitHashNumber(string input)
    {
        var number = 0;
        var hasher = MD5.Create();
        while (true)
        {
            string value = $"{input}{number}";
            var hash = hasher.ComputeHash(Encoding.ASCII.GetBytes(value));
            if (hash.Take(2).All(x => x == 0) && (hash[2] < 8)) return number;
            number++;
        }
    }

    public int Lowest6DigitHashNumber(string input)
    {
        var number = 0;
        var hasher = MD5.Create();
        while (true)
        {
            string value = $"{input}{number}";
            var hash = hasher.ComputeHash(Encoding.ASCII.GetBytes(value));
            if (hash.Take(3).All(x => x == 0)) return number;
            number++;
        }
    }
}
