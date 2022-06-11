using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day7
{
    public string[] Input;

    public Day7()
    {
        using StreamReader sr = File.OpenText("2016/input7");
        var input = sr.ReadToEnd().Trim();
        /*
        var input = @"abba[mnop]qrst
                            abcd[bddb]xyyx
                            aaaa[qwer]tyui
                            ioxxoj[asdfgh]zxcvbn";
        */
        Input = input.Split('\n').Select(s => s.Trim()).ToArray();
    }

    public void Solve()
    {
        Console.WriteLine(Input.Count(HasTLS));
    }

    public static bool HasTLS(string s)
    {
        bool inBrackets = false;
        bool valid = false;
        bool invalid = false;
        for(int i = 0; i <= s.Length - 4; i++)
        {
            switch (s[i])
            {
                case '[':
                    inBrackets = true; break;
                case ']':
                    inBrackets = false; break;
                default:
                    if(s[i] == s[i + 3] 
                        && s[i + 1] == s[i + 2]
                        && s[i] != s[i + 1])
                    {
                        if (inBrackets) invalid = true;
                        else valid = true;
                    }
                    break;
            }
        }
        return valid && !invalid;
    }
}
