using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day20
    {

        public Algorithm Algorithm { get; set; }
        public Image Image { get; set; }

        public Day20()
        {
            using (StreamReader sr = File.OpenText("input20"))
            {

                var input = sr.ReadToEnd().Trim();

                /*
                var input = @"#.#.#..#####.#.#.#.###.##.....###.##.#..###.####..#####..#....#..#..##..###..######.###...####..#..#####..##..#.#####...##.#.#..#.##..#.#......#.###.######.###.####...#.##.##..#..#..#####.....#.#....###..#.##......#.....#..#..#..##..#...##.######.####.####.#.#...#.......#..#.#.#...####.##.#......#..#...##.#.##..#...##.#.##..###.#......#.#.......#.#.#.####.###.##...#.....####.#..#..#.##.#....##..#.####....##...##..#...#......#.#.......#.......##..####..#...#.#.#...##..#.#..###..#####........#..####......#...

                            #..#.
                            #....
                            ##..#
                            ..#..
                            ..###";
                */

                var lines = input.Split("\n").Select(line => line.Trim()).ToList();

                Algorithm = new Algorithm(lines[0]);
                Image = new Image(lines.Skip(2).ToList());
            }
        }

        public void Solve()
        {
            Console.WriteLine(Algorithm.Values.Count());
            Console.WriteLine(Image.Size);
            var first = new Image(Image, Algorithm);
            Console.WriteLine(first.Size);
            Console.WriteLine("----");
            var second = new Image(first, Algorithm);
            Console.WriteLine(second.Size);
            Console.WriteLine(second.LitCount);
            Console.WriteLine("----");
            Console.WriteLine(Image);
            Console.WriteLine("----");
            Console.WriteLine(first);
            Console.WriteLine("----");
            Console.WriteLine(second);
        }

    }

    public class Algorithm
    {
        public List<bool> Values { get; set; }

        public bool this[int i] => Values[i];     

        public Algorithm(string init)
        {
            Values = new List<bool>();

            foreach (var c in init)
            {
                Values.Add(c == '#');
            }
        }
    }


    public class Image
    {
        const int PADDING = 10;
        public bool Outside { get; set; }
        public int Size { get; set; }
        public List<List<bool>> Values { get ; set; }

        public bool this[int i, int j] 
        { get
            {
                if (i < 0 || i >= Size) return Outside;
                if (j < 0 || j >= Size) return Outside;
                return Values[i][j];
            } 
        }

        public int EvaluateWindowAround(int i, int j)
        {
            var binary = "";
            foreach (var x in Enumerable.Range(i - 1, 3))
                foreach(var y in Enumerable.Range(j - 1, 3))
                    binary += this[x, y] ? '1' : '0';

            return Convert.ToInt32(binary, 2);
        }

        public int LitCount => Values.Sum(r => r.Sum(c => c?1:0));

        public Image(List<string> init)
        {
            Size = init[0].Length + (PADDING * 2);
            Outside = false;
            Values = new List<List<bool>>();

            foreach(var i in Enumerable.Range(0, PADDING)) Values.Add(Enumerable.Repeat(false, Size).ToList());

            foreach(var line in init)
            {
                var valueLine = new List<bool>();
                foreach(var i in Enumerable.Range(0, PADDING)) valueLine.Add(false);
                foreach(var c in line)
                    valueLine.Add(c == '#');
                foreach(var i in Enumerable.Range(0, PADDING)) valueLine.Add(false);
                Values.Add(valueLine);
            }
            foreach(var i in Enumerable.Range(0, PADDING)) Values.Add(Enumerable.Repeat(false, Size).ToList());
        }

        public Image(Image from, Algorithm alg)
        {
            Size = from.Size;
            Values = new List<List<bool>>();
            Outside = !from.Outside;
            foreach(var row in Enumerable.Range(0,Size))
            {
                var rowVals = new List<bool>();
                foreach(var col in Enumerable.Range(0, Size))
                {
                    var windowValue = from.EvaluateWindowAround(row, col);
                    rowVals.Add(alg[windowValue]);
                }
                Values.Add(rowVals);
            }
        }

        public override string ToString()
        {
            string result = "";
            foreach (var line in Values)
            {
                foreach (var v in line)
                    result += v ? '#' : '.';
                result += '\n';
            }
            return result;
        }
    }
}
