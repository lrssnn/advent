using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day17
{
    public List<Container> Containers;

    public Day17()
    {
        using (StreamReader sr = File.OpenText("2015/input17"))
        {
            var input = sr.ReadToEnd();
            /*
            var input = @"20
                        15
                        10
                        5
                        5";
            */


            Containers = input.Trim().Split("\n").Select(s => s.Trim()).Select(s => new Container(s)).ToList();
        }
    }

    public void Solve()
    {
        foreach(Container container in Containers) Console.WriteLine(container.ToString());
        var solutions = GetCombinations(new List<Container>(), Containers, 150);

        /*
        foreach (var solution in solutions) 
        {
            Console.WriteLine("---");
            foreach(Container c in solution) Console.Write($"{c}, ");
            Console.WriteLine();
        }
        */

        Console.WriteLine($"Total Solutions: {solutions.Count()}");

        var minimumContainers = solutions.Min(s => s.Count());
        var minumumMatches = solutions.Where(s => s.Count() == minimumContainers);

        Console.WriteLine($"Shortest: {minimumContainers}");
        Console.WriteLine($"Min Matches: {minumumMatches.Count()}");
    }

    public static IEnumerable<IEnumerable<Container>> GetCombinations(IEnumerable<Container> partial, IEnumerable<Container> containers, int target)
    {
        if(partial.Sum(c => c.Size) == target) return new List<List<Container>> { partial.ToList() };
        if(partial.Sum(c => c.Size) > target) return new List<List<Container>>();
        if(!containers.Any()) return new List<List<Container>>();

        // Either we use this one, or we don't
        var candidate = partial.Append(containers.First());
        var remaining = containers.Skip(1);

        var result = GetCombinations(candidate, remaining, target);
        return result.Concat(GetCombinations(partial, remaining, target));
    }

    public record struct Container
    {
        public int Id { get; set; }
        public int Size { get; set; }

        private static int _id = 0;

        public Container(string s)
        {
            Id = _id++;
            Size = int.Parse(s);
        }

        public override string ToString() => $"{Id}: {Size}";
    }
}
