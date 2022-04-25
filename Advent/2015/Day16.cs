using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day16
{

    List<Sue> Sues;
    Dictionary<string, int> Target;

    public Day16()
    {
        using (StreamReader sr = File.OpenText("2015/input16"))
        {
            var input = sr.ReadToEnd();
            /*
            var input = @"Sue 1: goldfish: 6, trees: 9, akitas: 0
                        Sue 2: goldfish: 7, trees: 1, akitas: 0";
            */

            var target = @"children: 3
                            cats: 7
                            samoyeds: 2
                            pomeranians: 3
                            akitas: 0
                            vizslas: 0
                            goldfish: 5
                            trees: 3
                            cars: 2
                            perfumes: 1";

            Sues = input.Trim().Split("\n").Select(s => s.Trim()).Select(s => new Sue(s)).ToList();
            Target = ReadTarget(target);
        }
    }

    public Dictionary<string, int> ReadTarget(string s)
    {
        var lines = s.Trim().Split("\n").Select(s => s.Trim());
        var result = new Dictionary<string, int>();
        foreach(var line in lines)
        {
            var parts = line.Split(": ");
            result.Add(parts[0], int.Parse(parts[1]));
        }
        return result;
    }

    public void Solve()
    {

        //foreach(var p in Target) Console.WriteLine($"{p.Key}:{p.Value}");
        //foreach(Sue s in Sues) Console.WriteLine(s);

        IEnumerable<Sue> part1Sues = Sues;
        foreach(var p in Target)
        {
            part1Sues = part1Sues.Where(s => s.Contains(p)).ToList();
            Console.WriteLine($"{part1Sues.Count()} Sues remain");
            if (part1Sues.Count() == 1) break;
        }

        Console.WriteLine($"The final sue has been found");
        Console.WriteLine(part1Sues.Single());

        IEnumerable<Sue> part2Sues = Sues;
        foreach(var p in Target)
        {
            part2Sues = part2Sues.Where(s => s.ContainsHard(p)).ToList();
            Console.WriteLine($"{part2Sues.Count()} Sues remain");
            if (part2Sues.Count() == 1) break;
        }

        Console.WriteLine($"The final sue has been found");
        Console.WriteLine(part2Sues.Single());
    }

    public record struct Sue
    {
        public int Number { get; set; }
        public Dictionary<string, int> Properties { get; set; }

        public Sue(string s)
        {
            var parts = s.Split(' ');
            Number = int.Parse(parts[1][..^1]);
            Properties = new Dictionary<string, int>();

            var index = 2;
            while (index < parts.Length)
            {
                var name = parts[index][..^1]; // Remove colon
                var value = parts[index + 1];

                // Maybe remove comma
                if (value[^1] == ',') value = value[..^1];

                Properties.Add(name, int.Parse(value));
                index += 2;
            }
        }

        public bool Contains(KeyValuePair<string, int> pair) => !Properties.ContainsKey(pair.Key) || Properties[pair.Key] == pair.Value;

        public bool ContainsHard(KeyValuePair<string, int> pair)
        {
            if (!Properties.ContainsKey(pair.Key)) return true; // Cannot eliminate based on this property

            return pair.Key switch
            {
                "cats" or "trees" => Properties[pair.Key] > pair.Value,
                "pomeranians" or "goldfish" => Properties[pair.Key] < pair.Value,
                _ => Properties[pair.Key] == pair.Value,
            };
        }

        public override string ToString()
        {
            var str = $"Sue {Number}:";
            foreach (var name in Properties.Keys)
            {
                str += $"  {name}: {Properties[name]}";
            }
            return str;
        }
    }
}
