using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day3
{
    public List<Triangle> TrianglesOne { get; set; }
    public List<Triangle> TrianglesTwo { get; set; }

    public Day3()
    {
        using (StreamReader sr = File.OpenText("2016/input3"))
        {
            var input = sr.ReadToEnd().Trim();
            var flatNumbers = input.Split('\n').SelectMany(line => line.Split(' ').Where(p => p.Length > 0)).ToList();
            // Populate Part One
            TrianglesOne = new List<Triangle>();
            for(var group = 0; group < flatNumbers.Count; group += 3)
            {
                TrianglesOne.Add(new Triangle(flatNumbers[group], flatNumbers[group + 1], flatNumbers[group + 2]));
            }

            // Populate Part Two
            TrianglesTwo = new List<Triangle>();
            for(var group = 0; group < flatNumbers.Count; group += 9)
            {
                TrianglesTwo.Add(new Triangle(flatNumbers[group], flatNumbers[group + 3], flatNumbers[group + 6]));
                TrianglesTwo.Add(new Triangle(flatNumbers[group + 1], flatNumbers[group + 4], flatNumbers[group + 7]));
                TrianglesTwo.Add(new Triangle(flatNumbers[group + 2], flatNumbers[group + 5], flatNumbers[group + 8]));
            }
        }
    }

    public void Solve()
    {
        Console.WriteLine(TrianglesOne.Count());
        Console.WriteLine(TrianglesTwo.Count());
        Console.WriteLine(TrianglesOne.Count(c => c.Possible));
        Console.WriteLine(TrianglesTwo.Count(c => c.Possible));
    }

    public class Triangle
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public Triangle(string a, string b, string c)
        {
            A = int.Parse(a);
            B = int.Parse(b);
            C = int.Parse(c);
        }

        public bool Possible => A + B > C && A + C > B && B + C > A;

        public override string ToString() => $"{A}, {B}, {C}";
    }
}

