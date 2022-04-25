using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day13
{

    List<Opinion> Opinions { get; set; }

    public Day13()
    {
        using (StreamReader sr = File.OpenText("2015/input13"))
        {
            string input = sr.ReadToEnd();
            /*
            string input = @"Alice would gain 54 happiness units by sitting next to Bob.
                            Alice would lose 79 happiness units by sitting next to Carol.
                            Alice would lose 2 happiness units by sitting next to David.
                            Bob would gain 83 happiness units by sitting next to Alice.
                            Bob would lose 7 happiness units by sitting next to Carol.
                            Bob would lose 63 happiness units by sitting next to David.
                            Carol would lose 62 happiness units by sitting next to Alice.
                            Carol would gain 60 happiness units by sitting next to Bob.
                            Carol would gain 55 happiness units by sitting next to David.
                            David would gain 46 happiness units by sitting next to Alice.
                            David would lose 7 happiness units by sitting next to Bob.
                            David would gain 41 happiness units by sitting next to Carol.";
            */

            Opinions = input.Trim().Split('\n').Select(e => e.Trim()).Select(e => new Opinion(e)).ToList();
        }
    }

    public void Solve()
    {

        var pairings = CalculatePairings(Opinions);
        var people = Opinions.Select(o => o.From).ToHashSet().ToList();
        var tables = GenerateTables(people);
        var best = tables.OrderByDescending(t => EvaluateTable(t, pairings)).First();
        var answer1 = EvaluateTable(best, pairings);
        Console.WriteLine(answer1);

        // Part 2
        Opinions.AddRange(GenerateMyOpinions(people));
        people = Opinions.Select(o => o.From).ToHashSet().ToList();
        pairings = CalculatePairings(Opinions);
        tables = GenerateTables(people);
        best = tables.OrderByDescending(t => EvaluateTable(t, pairings)).First();
        var answer2 = EvaluateTable(best, pairings);
        Console.WriteLine(answer2);
        Console.WriteLine(answer2 - answer1);

        /*
        foreach(var opinion in Opinions)
        {
            Console.WriteLine(opinion);
        }

        Console.WriteLine("------------");

        foreach(var person in people)
        {
            Console.WriteLine(person);
        }

        Console.WriteLine("------------");

        foreach(var pair in pairings)
        {
            Console.WriteLine(pair);
        }

        Console.WriteLine("------------");


        foreach(var table in tables)
        {
            Console.WriteLine(table);
        }

        Console.WriteLine(best);
        */

    }

    public List<Opinion> GenerateMyOpinions(List<string> people)
    {
        var answer = new List<Opinion>();
        foreach (var p in people)
        {
            answer.Add(new Opinion("Me", p, 0));
            answer.Add(new Opinion(p, "Me", 0));
        }
        return answer;
    }

    public List<Pairing> CalculatePairings(List<Opinion> opinions)
    {
        var pairings = new List<Pairing>();
        var used = new bool[opinions.Count];

        for(int i = 0; i < opinions.Count; i++)
        {
            if (used[i]) continue;
            var me = opinions[i];
            // Search for the corresponding opinions
            Opinion match = default;
            bool foundMatch = false;
            for(var j = i; j < opinions.Count; j++)
            {
                if (used[j]) continue;
                var other = opinions[j];
                if (other.CorrespondsTo(me))
                {
                    match = opinions[j];
                    used[j] = true;
                    foundMatch = true;
                }
            }

            if (!foundMatch) throw new Exception("Missing Match!");

            pairings.Add(new Pairing(me, match));
        }

        return pairings;
    }

    public int EvaluateTable(Table table, List<Pairing> pairings)
    {
        int totalHappiness = 0;
        for(int i = 0; i < table.Seats.Count - 1; i++)
        {
            var me = table.Seats[i];
            var next = table.Seats[i + 1];
            var pairing = pairings.First(p => p.Matches(me, next));
            totalHappiness += pairing.Gain;
        }

        // Wrap around from last to first
        var first = table.Seats[0];
        var last = table.Seats[^1];
        var lastPair = pairings.First(p => p.Matches(first, last));
        totalHappiness += lastPair.Gain;

        return totalHappiness;
    }

    public List<Table> GenerateTables(List<string> people)
    {
        // Always start with the first person
        var baseTable = new Table(people.Count);
        baseTable.Add(people[0]);

        return GenerateTables(baseTable, people.Skip(1).ToList());
    }

    public List<Table> GenerateTables(Table baseTable, List<string> people)
    {
        if (baseTable.IsFull) return new List<Table> { baseTable };
        var result = new List<Table>();

        // For each remaining person, add them and call again
        foreach(var person in people)
        {
            var newTable = new Table(baseTable, person);
            var newPeople = people.Where(p => p != person).ToList();
            result.AddRange(GenerateTables(newTable, newPeople));
        }

        return result;
    }

    public record struct Opinion
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Gain { get; set; }

        public Opinion (string from, string to, int gain) { From = from; To = to; Gain = gain; }

        public Opinion (string init)
        {
            var parts = init.Split(' ');
            From = parts[0];
            To = parts[10][0..^1]; // Trim period

            var sign = parts[2] == "gain" ? 1 : -1;

            Gain = int.Parse(parts[3]) * sign;
        }

        public override string ToString()
        {
            return $"{From} -> {To}: {Gain}";
        }

        public bool CorrespondsTo(Opinion other) => other.From == To && other.To == From;
    }

    public record struct Pairing
    {
        public string A { get; set; }
        public string B { get; set; }
        public int Gain { get; set; }

        public Pairing (Opinion OpA, Opinion OpB)
        {
            A = OpA.From;
            B = OpA.To;
            Gain = OpA.Gain + OpB.Gain;
        }

        public override string ToString()
        {
            return $"{A} & {B}: {Gain}";
        }

        public bool Matches(string x, string y) => (x == A && y == B) || (x == B && y == A);
    }

    public record struct Table
    {
        public List<string> Seats { get; set; }
        public int Size { get; set; }

        public Table(int size)
        {
            Size = size;
            Seats = new List<string>(size);
        }

        public Table(Table other, string person)
        {
            Size = other.Size;
            Seats = other.Seats.ToList();
            Seats.Add(person);
        }

        public bool IsFull => Seats.Count == Size;
        public bool Contains(string person) => Seats.Contains(person);
        public void Add(string person) => Seats.Add(person);

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var item in Seats)
                str.Append($"{item} ");
            return str.ToString();
        }

    }

}
