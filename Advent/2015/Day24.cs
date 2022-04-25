using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day24
{

    public List<int> Weights { get; set; }
    public int Target { get; set; }
    

    public long BestQE { get; set; }
    public int ValidCandidates { get; set; }

    public Day24()
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
            Target = Weights.Sum() / 4;
            State._target = Target;
            ValidCandidates = 0;
            BestQE = long.MaxValue;
        }
    }

    public void Solve()
    {
        // We want to find the smallest group 1, so start at one and go upwards
        var candidates = Weights.Select(s => new State(s, s, Weights.Where(e => e != s).ToList()));
        State? validCandidate = null;
        foreach(var c in candidates) if(ValidCandidate(c)) validCandidate = c;
        var length = 1;
        while(validCandidate == null)
        {
            // From candidates, try adding another number to each one
            Console.WriteLine($"Generating length {length}");
            candidates = candidates.SelectMany(c => c.GetExtensions());
            Console.WriteLine($"Searching length {length}");
            foreach(var c in candidates) if(ValidCandidate(c)) validCandidate = c;
            length++;
        }

        Console.WriteLine($"Found {ValidCandidates} candidates");
        Console.WriteLine($"{validCandidate.Value.QE}");
    }

    public bool ValidCandidate(State candidate)
    {
        // Is the first group valid?
        if (candidate.Sum != Target) return false;

        // Is this better than the current target, if we have one?
        if (candidate.QE >= BestQE) return false;

        // Can the remaining numbers form a valid solution?
        if (!CanAddToFlat(candidate.Options)) return false;

        // We have a winner
        Console.WriteLine($"New best candidate {candidate.QE}");
        BestQE = candidate.QE;
        return true;
    }

    public bool CanAddToFlat(List<int> options)
    {
        var used = new bool[options.Count];
        while (UsedSum(used, options) != Target)
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

    public record struct State
    {
        public static int _target;

        public int Sum { get; set; }
        public long QE { get; set; }
        public List<int> Options { get; set; }

        public State(int sum, long qe, List<int> options) { Sum = sum; Options = options; QE = qe; }

        public List<State> GetExtensions()
        {
            var res = new List<State>();
            foreach(var n in Options)
            {
                var newSum = Sum + n;
                if(newSum <= _target)
                {
                    var newQe = QE * n;
                    res.Add(new State(newSum, newQe, Options.Where(e => e != n).ToList()));
                }
            }
            return res;
        }
    }
}
