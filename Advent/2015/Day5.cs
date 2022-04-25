using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day5
{
    public List<string> Lines;

    public static readonly char[] Vowels = {'a', 'e', 'i', 'o', 'u'};

    public Day5()
    {
        using (StreamReader sr = File.OpenText("2015/input5"))
        {
            var input = sr.ReadToEnd().Trim();
            Lines = input.Split('\n').Select(line => line.Trim()).ToList();
        }
    }

    public void Solve()
    {
        Console.WriteLine(IsVeryNice("qjhvhtzxzqqjkmpb"));
        Console.WriteLine(IsVeryNice("xxyxx"));
        Console.WriteLine(IsVeryNice("uurcxstgmygtbstg"));
        Console.WriteLine(IsVeryNice("ieodomkazucvgmuy"));
        Console.WriteLine(Lines.Count(IsNice));
        Console.WriteLine(Lines.Count(IsVeryNice));
    }

    public bool IsNice(string input)
    {
        return HasThreeVowels(input) &&
            HasDoubleLetter(input) &&
            NoIllegalSubstrings(input);
    }

    public bool IsVeryNice(string input)
    {
        return HasDoublePair(input) && HasSeparatedRepeater(input);
    }

    public bool HasDoublePair(string input)
    {
        for(int i = 0; i < input.Length - 1; i++)
        {
            var pair = input[i..(i + 2)];
            // Look forward for the pair
            for(int j = i + 2; j < input.Length - 1; j++)
            {
                var candidate = input[j..(j + 2)];
                if (pair == candidate) return true;
            }
        }

        return false;
    }
    
    public bool HasSeparatedRepeater(string input)
    {
        for (int i = 0; i < input.Length - 2; i++)
        {
            var window = input[i..(i + 3)];
            if (window[0] == window[^1]) return true;
        }
        return false;
    }

    public bool HasThreeVowels(string input) => 
        input.Count(c => Vowels.Contains(c)) >= 3;

    public bool HasDoubleLetter(string input)
    {
        for (int i = 0; i < input.Length - 1; i++)
            if (input[i] == input[i + 1]) return true;
        return false;
    }

    public bool NoIllegalSubstrings(string input) =>
        !(input.Contains("ab") || input.Contains("cd") || input.Contains("pq") || input.Contains("xy"));
}
