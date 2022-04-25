using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day19
{

    List<Replacement> Replacements;
    string StartingString;

    public Day19()
    {
        using (StreamReader sr = File.OpenText("2015/input19"))
        {
            var input = sr.ReadToEnd();

            /*
            var input = @"H => HO
                        H => OH
                        O => HH

                        HOHOHO";
            */

            /*
            var input = @"e => H
                        e => O
                        H => HO
                        H => OH
                        O => HH

                        HOH";
            */

            Replacements = input.Trim().Split("\n")[..^2].Select(s => s.Trim()).Select(s => new Replacement(s)).ToList();
            StartingString = input.Trim().Split('\n')[^1].Trim();
        }
    }

    public void Solve()
    {
        Console.WriteLine("Part 1...");
        var possibles = PossibleResults(new HashSet<string> { StartingString }, Replacements);
        foreach(var possible in possibles) Console.WriteLine(possible.ToString());
        Console.WriteLine($"Found {possibles.Count} possible molecules.");

        Console.WriteLine("Part 2...");
        var done = false;
        var steps = 0;
        var molecules = new HashSet<string> { "e" };
        Console.WriteLine($"TargetLength = {StartingString.Length}");
        while (!done)
        {
            molecules = PossibleResults(molecules, Replacements);
            steps++;
            var avgLength = molecules.Average(s => s.Length);
            Console.WriteLine($"{molecules.Count} mols after {steps} steps (average length {avgLength})");
            if (molecules.Any(s => s == StartingString)) done = true;
            // We know that the string will never get shorter, so we can trim anything that has reached that length
            molecules = molecules.Where(s => s.Length < StartingString.Length).ToHashSet();
        }
        Console.WriteLine($"Found target in {steps} steps");
    }

    public static HashSet<string> PossibleResults(HashSet<string> inputs, List<Replacement> rs)
    {
        var result = new HashSet<string>();
        foreach (var s in inputs)
        {
            var res = PossibleResults(s, rs);
            foreach (var r in res) result.Add(r);
        }
        return result;
    }

    public static HashSet<string> PossibleResults(string s, List<Replacement> rs)
    {
        var result = new HashSet<string>();
        foreach (var replacement in rs)
        {
            var res = PossibleResults(s, replacement);
            foreach (var r in res) result.Add(r);
        }
        return result;
    }

    public static HashSet<string> PossibleResults(string s, Replacement r)
    {
        var result = new HashSet<string>();
        var windowSize = r.From.Length;
        for(int i = 0; i <= s.Length - windowSize; i++)
        {
            if(s[i..(i + windowSize)] == r.From)
            {
                var before = s[0..i];
                var after = s[(i + windowSize)..];
                result.Add(before + r.To + after);
            }
        }
        return result;
    }

    public record struct Replacement
    {
        public string From { get; set; }
        public string To { get; set; }

        public Replacement(string s)
        {
            var parts = s.Split(" => ");
            From = parts[0];
            To = parts[1];
        }

        public override string ToString() => $"{From} => {To}";
    }

}
