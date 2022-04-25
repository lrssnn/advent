using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day11
{

    public static readonly char[] Illegals = { 'i', 'o', 'l' };

    public Day11()
    {
    }

    public void Solve()
    {
        var pt1 = NextPassword("hxbxwxba");
        var pt2 = NextPassword(pt1);
        Console.WriteLine(pt1);
        Console.WriteLine(pt2);
    }

    public string NextPassword(string s)
    {
        string candidate = s;

        do
        {
            candidate = Increment(candidate);
            //Console.WriteLine(candidate);
        }
        while (!Valid(candidate));

        return candidate;
    }

    public string Increment(string s)
    {
        var index = 1;
        bool overflow;
        char[] result = s.ToCharArray();

        do
        {
            (overflow, char c) = IncrementChar(result[^index]);
            result[^index] = c;
            index++;
        } while (overflow);

        return new string(result);
    }

    public (bool overflow, char c) IncrementChar(char c)
    {
        if(c != 'z')
        {
            char result = (char)(Convert.ToUInt16(c) + 1);
            if(Illegals.Contains(result))
                result = (char)(Convert.ToUInt16(result) + 1);
            return (false, result);
        } else
        {
            return (true, 'a');
        }
    }

    public bool Valid(string s)
    {
        return HasStraight(s) && HasTwoDifferentPairs(s) && HasNoIllegals(s);
    }

    public bool HasStraight(string s)
    {
        var numericals = s.Select(c => Convert.ToUInt16(c)).ToArray();
        for(int i = 2; i < numericals.Length; i++)
        {
            var twoBefore = numericals[i - 2];
            var before = numericals[i - 1];
            var me = numericals[i];
            if (twoBefore == me - 2 && before == me - 1)
                return true;
        }
        return false;
    }

    public bool HasTwoDifferentPairs(string s)
    {
        char? firstPair = null;
        for(int i = 1; i < s.Length; i++)
        {
            if(s[i - 1] == s[i])
            {
                if(firstPair == null)
                {
                    firstPair = s[i];
                    i++;
                } else if(firstPair.Value != s[i])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool HasNoIllegals(string s)
    {
        foreach (char c in s)
        {
            if (Illegals.Contains(c)) return false;
        }
        return true;
    }
}
