using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day24Slow
{

    public List<int> Weights { get; set; }
    public int Target { get; set; }

    public Day24Slow()
    {
        using (StreamReader sr = File.OpenText("2015/input24"))
        {
            var input = sr.ReadToEnd();

            var input2 = @"1
2
3
4
5
7
8
9
10
11";

            Weights = input.Trim().Split("\n").Select(s => s.Trim()).Select(s => int.Parse(s)).ToList();
            Target = Weights.Sum() / 3;
        }
    }

    public void Solve()
    {
        // We want to find the smallest group 1, so start at one and go upwards
        var candidates = Weights.Select(s => (IEnumerable<int>)new List<int> { s });
        var validCandidates = candidates.Where(ValidCandidate);
        while(!validCandidates.Any())
        {
            // From candidates, try adding another number to each one
            Console.WriteLine($"Generating length {candidates.First().Count() + 1}");
            candidates = GenerateCandidates(candidates);
            Console.WriteLine($"Searching length {candidates.First().Count()}");
            validCandidates = candidates.Where(ValidCandidate);
        }

        foreach(var c in validCandidates)
        {
            Console.WriteLine($"Candidate: {c.Aggregate(1, (prod, elem) => prod * elem)}");
            foreach(var n in c)
            {
                Console.WriteLine($"  {n}");
            }
        }
    }

    public IEnumerable<IEnumerable<int>> GenerateCandidates(IEnumerable<IEnumerable<int>> candidates)
    {
        var newCandidates = new List<IEnumerable<int>>();
        foreach(var candidate in candidates)
        {
            foreach(var weight in Weights)
            {
                if (!candidate.Contains(weight))
                {
                    newCandidates.Add(candidate.Append(weight));
                }
            }
        }
        return newCandidates;
    }

    public bool ValidCandidate(IEnumerable<int> candidate)
    {
        // Is the first group valid?
        if (candidate.Sum() != Target) return false;

        var remaining = Weights.Where(s => !candidate.Contains(s));

        // Only need to find one subset that adds to target (I think)
        return CanAddToFlat(remaining.ToList());
    }

    public bool CanAddToFlat(List<int> options)
    {
        var used = new bool[options.Count];
        var sum = UsedSum(used, options);
        while (sum != Target)
        {
            // We're effectively binary counting
            var index = 0;
            var done = false;
            while (!done)
            {
                used[index] = !used[index];
                if (used[index]) done = true;
                index++;
                if (index >= used.Length) return false;
            }
        }

        return true;
    }

    public int UsedSum(bool[] used, List<int> options)
    {
        var sum = 0;
        for(int i = 0; i < used.Length; i++)
        {
            if (used[i]) sum += options[i];
        }
        return sum;
    }

    public bool CanAddTo(int sum, IEnumerable<int> options)
    {
        // Try adding each of the remaining options
        foreach(var number in options)
        {
            var newSum = sum + number;
            if (newSum == Target) return true;
            if (newSum > Target) return false;
            if (CanAddTo(newSum, options.Where(e => e != number))) return true;
        }
        return false;
    }

    public static void Print(string s) => Console.WriteLine(s);
}
