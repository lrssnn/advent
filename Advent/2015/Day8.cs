using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day8
{

    List<string> Input { get; set; }

    public Day8()
    {
        using (StreamReader sr = File.OpenText("2015/input8"))
        {
            var input = sr.ReadToEnd().Trim();

            /*
            var input = @"""""
                          ""abc""
                          ""aaa\""aaa""
                          ""\x27""";
            */

            Input = input.Split('\n').Select(l => l.Trim()).ToList();
        }
    }

    public void Solve()
    {
        // Trim the enclosing quotations from each string, we can account for those with a constant factor
        var noQuotes = Input.Select(x => x[1..^1]);

        var total = 0;
        foreach(var noQuote in noQuotes)
        {
            var count = Count(noQuote) + 2;
            Console.WriteLine($"|{noQuote}|: {count}");
            total += count;
        }
        Console.WriteLine(total);

    }

    public int Count(string s)
    {
        int count = 0;
        for (int i = 0; i < s.Length - 1; i++)
        {
            if (s[i..(i + 2)] == "\\\\")
            {
                count++;
                i++;
            }
            else if (s[i..(i + 2)] == "\\\"")
            {
                count++;
                i++;
            }
            else if (s[i..(i + 2)] == "\\x")
            {
                count += 3;
                i += 3;
            }
        }
        return count;
    }
}
