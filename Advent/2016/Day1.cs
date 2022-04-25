using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day1
{
    public List<Instruction> Instructions { get; set; }

    public Day1()
    {
        using (StreamReader sr = File.OpenText("2016/input1"))
        {

            var input = sr.ReadToEnd().Trim();
            var input1 = "R8, R4, R4, R8";
            Instructions = input.Split(", ").Select(s => s.Trim()).Select(s => new Instruction(s)).ToList();
        }
    }

    public void Solve()
    {
        var location = (0, 0);
        var facing = Direction.North;
        var visited = new HashSet<(int, int)>();

        (int, int)? winner = null;
        foreach(var instruction in Instructions)
        {
            facing = Turn(facing, instruction.Right);
            // Walk to the location
            var stepLocation = location;
            for(var step = 1; step <= instruction.Magnitude; step++)
            {
                stepLocation = Move(stepLocation, facing, 1);
                if (visited.Contains(stepLocation))
                {
                    Console.WriteLine($"!!!!!!Location: {stepLocation}");
                    if (winner == null) winner = stepLocation;
                } else
                {
                    //Console.WriteLine($"    New Location: {stepLocation}");
                    visited.Add(stepLocation);
                }
            }
            location = stepLocation;
            Console.WriteLine($"{instruction.D}{instruction.Magnitude}: { location}");
        }
        Console.WriteLine(Math.Abs(location.Item1) + Math.Abs(location.Item2));
        Console.WriteLine(Math.Abs(winner.Value.Item1) + Math.Abs(winner.Value.Item2));
    }

    public Direction Turn(Direction facing, bool right)
    {
        // This is disgusting
        if (right)
        {
            return facing switch
            {
                Direction.North => Direction.East,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                Direction.East => Direction.South,
            };
        }
        else
        {
            return facing switch
            {
                Direction.North => Direction.West,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                Direction.East => Direction.North,
            };
        }
    }

    public (int x, int y) Move((int x, int y) location, Direction facing, int mag)
    {
        return facing switch
        {
            Direction.North => (location.x, location.y + mag),
            Direction.East => (location.x + mag, location.y),
            Direction.South => (location.x, location.y - mag),
            Direction.West => (location.x - mag, location.y),
        };
    }

    public class Instruction
    {
        public bool Right;
        public int Magnitude;

        public string D => Right ? "R" : "L";

        public Instruction(string s)
        {
            Right = s[0] == 'R' ? true : false;
            Magnitude = int.Parse(s[1..]);
        }
    }

    public enum Direction { North, South, East,West }
}

