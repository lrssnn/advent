using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day1
{
    public string Input { get; set; }

    public Day1()
    {
        using (StreamReader sr = File.OpenText("2015/input1"))
        {

            var input = sr.ReadToEnd().Trim();
            Input = input;
        }
    }

    public void Solve()
    {
        Console.WriteLine(CountBrackets(Input));
        Console.WriteLine(FirstBasement(Input));
    }

    public int CountBrackets(string input)
    {
        int count = 0;
        foreach (char c in input)
            count += c switch
            {
                '(' => 1,
                ')' => -1,
            };
        return count;
    }

    public int FirstBasement(string input)
    {
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            count += input[i] switch
            {
                '(' => 1,
                ')' => -1,
            };
            if(count == -1) return i;
        }
        return -1;
    }
}

