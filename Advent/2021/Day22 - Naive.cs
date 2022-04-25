using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day22Naive
    {

        public List<Instruction> Instructions { get; set; }

        public Day22Naive()
        {
            using (StreamReader sr = File.OpenText("2021/input22"))
            {

                var input = sr.ReadToEnd().Trim();

                /*
                var input = @"on x=10..12,y=10..12,z=10..12
                            on x=11..13,y=11..13,z=11..13
                            off x=9..11,y=9..11,z=9..11
                            on x=10..10,y=10..10,z=10..10";
                */

                /*
                var input = @"on x=-20..26,y=-36..17,z=-47..7
                            on x=-20..33,y=-21..23,z=-26..28
                            on x=-22..28,y=-29..23,z=-38..16
                            on x=-46..7,y=-6..46,z=-50..-1
                            on x=-49..1,y=-3..46,z=-24..28
                            on x=2..47,y=-22..22,z=-23..27
                            on x=-27..23,y=-28..26,z=-21..29
                            on x=-39..5,y=-6..47,z=-3..44
                            on x=-30..21,y=-8..43,z=-13..34
                            on x=-22..26,y=-27..20,z=-29..19
                            off x=-48..-32,y=26..41,z=-47..-37
                            on x=-12..35,y=6..50,z=-50..-2
                            off x=-48..-32,y=-32..-16,z=-15..-5
                            on x=-18..26,y=-33..15,z=-7..46
                            off x=-40..-22,y=-38..-28,z=23..41
                            on x=-16..35,y=-41..10,z=-47..6
                            off x=-32..-23,y=11..30,z=-14..3
                            on x=-49..-5,y=-3..45,z=-29..18
                            off x=18..30,y=-20..-8,z=-3..13
                            on x=-41..9,y=-7..43,z=-33..15
                            on x=-54112..-39298,y=-85059..-49293,z=-27449..7877
                            on x=967..23432,y=45373..81175,z=27513..53682";
                */

                var lines = input.Split("\n").Select(line => line.Trim()).ToList();

                Instructions = lines.Select(line => new Instruction(line)).ToList();
            }
        }

        public void Solve()
        {
            var OnCubes = new HashSet<Coord3>();
            var done = 0;
            foreach(var instruction in Instructions)
            {
                ProcessInstruction(OnCubes, instruction);
                done++;
                Console.WriteLine($"Processed {done}/{Instructions.Count}");
            }
            Console.WriteLine(OnCubes.Count);
        }

        public static void ProcessInstruction(HashSet<Coord3> onCubes, Instruction instruction)
        {
            if (instruction.On)
                ProcessOnInstruction(onCubes, instruction);
            else
                ProcessOffInstruction(onCubes, instruction);
        }

        public static void ProcessOnInstruction(HashSet<Coord3> onCubes, Instruction instruction)
        {
            foreach(var cube in instruction.Cubes())
            {
                onCubes.Add(cube);
            }
        }

        public static void ProcessOffInstruction(HashSet<Coord3> onCubes, Instruction instruction)
        {
            foreach(var cube in instruction.Cubes())
            {
                onCubes.Remove(cube);
            }
        }


    public class Instruction
    {
        public Range XRange { get; set; }
        public Range YRange { get; set; }
        public Range ZRange { get; set; }
        public bool On { get; set; }

        public Instruction(string init)
        {
            var parts = init.Split(' ');
            On = parts[0] == "on";

            var ranges = parts[1].Split(',');
            var xRange = ranges[0][2..].Split("..").Select(int.Parse).ToList();
            var yRange = ranges[1][2..].Split("..").Select(int.Parse).ToList();
            var zRange = ranges[2][2..].Split("..").Select(int.Parse).ToList();
            XRange = new Range(xRange[0], xRange[1]);
            YRange = new Range(yRange[0], yRange[1]);
            ZRange = new Range(zRange[0], zRange[1]);
        }

        public IEnumerable<Coord3> Cubes()
        {
            foreach (int x in XRange.Values())
            foreach (int y in YRange.Values())
            foreach (int z in ZRange.Values())
                yield return new Coord3(x, y, z);
        }

        public override string ToString()
        {
            return $"{(On ? "on" : "off")} x={XRange},y={YRange},z={ZRange}";
        }
    }

    public record struct Coord3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Coord3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z}";
        }
    }

    public class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public Range(int min, int max)
        {
            if (min < max)
            {
                Min = min;
                Max = max;
            }
            else
            {
                Min = max;
                Max = min;
            }
        }

        public IEnumerable<int> Values()
        {
            for (int i = Math.Max(Min, -50); i <= Math.Min(Max, 50); i++) yield return i;
            //for (int i = Min; i <= Max; i++) yield return i;
        }

        public override string ToString()
        {
            return $"{Min}..{Max}";
        }
    }
    }
}

