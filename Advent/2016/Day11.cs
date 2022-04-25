using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day11
{
    public State Input { get; set; }

    public Day11()
    {
        /*
        var input = new State
        {
            Elevator = 0,
            Floors = new List<Floor>
            {
                new Floor
                {
                    Generators = new List<char> { 'S', 'P', },
                    Microchips = new List<char> { 'S', 'P', },
                },
                new Floor
                {
                    Generators = new List<char> { 'T', 'R', 'C' },
                    Microchips = new List<char> { 'R', 'C' },
                },
                new Floor
                {
                    Generators = new List<char>(),
                    Microchips = new List<char> { 'T' },
                },
                new Floor
                {
                    Generators = new List<char>(),
                    Microchips = new List<char>(),
                },
            }
        };
        var testInput = new State
        {
            Elevator = 0,
            Floors = new List<Floor>
            {
                new Floor
                {
                    Generators = new List<char>(),
                    Microchips = new List<char> { 'H', 'L', },
                },
                new Floor
                {
                    Generators = new List<char> { 'H' },
                    Microchips = new List<char>(),
                },
                new Floor
                {
                    Generators = new List<char> { 'L' },
                    Microchips = new List<char>(),
                },
                new Floor
                {
                    Generators = new List<char>(),
                    Microchips = new List<char>(),
                },
            }
        };
        Input = testInput;
        */
    }

    public void Solve()
    {
        Console.WriteLine(Input);
    }

    public static List<State> ValidMoves(State input)
    {
        // We can either move one or two objects.
        // We can either move up or down (maybe)
        // We need to choose which items to move (maybe)

        return new List<State>();
    }

    // Making this highly specialised because my brain is broken and I just can't with all this coding crap
    public record struct State
    {
        public int SG { get; set; }
        public int PG { get; set; }
        public int TG { get; set; }
        public int RG { get; set; }
        public int CG { get; set; }
        public int SM { get; set; }
        public int PM { get; set; }
        public int TM { get; set; }
        public int RM { get; set; }
        public int CM { get; set; }

        public int Elevator { get; set; }

        public State() 
        {
            Elevator = 0;
            SG = 1;
            SM = 1;
            PG = 1;
            PM = 1;

            TG = 2;
            RG = 2;
            RM = 2;
            CG = 2;
            CM = 2;

            TM = 3;
        }

        /*
        public override string ToString()
        {
            var result = new StringBuilder();
            var maxFloor = Floors.Count;
            for(var i = maxFloor - 1; i >= 0; i--)
            {
                result.Append($"F{i + 1} ");
                result.Append(Floors[i]);
                result.Append('\n');
            }
            return result.ToString();
        }
        */
    }

    public class Floor
    {
        public List<char> Generators { get; set; }
        public List<char> Microchips { get; set; }

        public Floor() { Generators = new List<char>(); Microchips = new List<char>(); }
        public Floor(Floor f) { Generators = f.Generators.ToList(); Microchips = f.Microchips.ToList(); }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach(var g in Generators) result.Append($"G{g} ");
            foreach(var m in Microchips) result.Append($"M{m} ");
            return result.ToString();
        }
    }


}
