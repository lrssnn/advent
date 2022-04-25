using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day2
{
    public List<Code> Codes { get; set; }

    public Day2()
    {
        using (StreamReader sr = File.OpenText("2016/input2"))
        {

            var input = sr.ReadToEnd().Trim();
            var input1 = @"ULL
                            RRDDD
                            LURDL
                            UUUUD";
            Codes = input.Split("\n").Select(s => s.Trim()).Select(s => new Code(s)).ToList();
        }
    }

    public void Solve()
    {
        var answer1 = new List<string>();
        var answer2 = new List<string>();
        var keypad1 = new Keypad(true);
        var keypad2 = new Keypad(false);
        foreach(var code in Codes)
        {
            foreach (var d in code.Directions)
            {
                keypad1.MovePointer(d);
                keypad2.MovePointer(d);
            }
            answer1.Add(keypad1.Number);
            answer2.Add(keypad2.Number);
        }

        foreach (var n in answer1) Console.Write(n);
        Console.WriteLine();

        foreach (var n in answer2) Console.Write(n);
        Console.WriteLine();
    }

    public class Code
    {
        public List<Direction> Directions { get; set; } 

        public Code(string s)
        {
            Directions = new List<Direction>();
            foreach(var c in s)
            {
                Directions.Add(DirectionFrom(c));
            }
        }
    }

    public class Keypad
    {
        public string[][] Keys { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Keypad(bool partA)
        {
            if (partA)
            {
                Keys = new string[][]
                {
                new string[] { "X", "X", "X", "X", "X" },
                new string[] { "X", "1", "2", "3", "X" },
                new string[] { "X", "4", "5", "6", "X" },
                new string[] { "X", "7", "8", "9", "X" },
                new string[] { "X", "X", "X", "X", "X" },
                };
                X = 2;
                Y = 2;
            } else
            {
                Keys = new string[][]
                {
                new string[] { "X", "X", "X", "X", "X", "X", "X"},
                new string[] { "X", "X", "X", "1", "X", "X", "X"},
                new string[] { "X", "X", "2", "3", "4", "X", "X"},
                new string[] { "X", "5", "6", "7", "8", "9", "X"},
                new string[] { "X", "X", "A", "B", "C", "X", "X"},
                new string[] { "X", "X", "X", "D", "X", "X", "X"},
                new string[] { "X", "X", "X", "X", "X", "X", "X"},
                };
                X = 1;
                Y = 3;
            }
        }

        public string Number => Keys[Y][X];

        public void MovePointer(Direction d)
        {
            switch (d)
            {
                case Direction.Left:
                    GoLeft();
                    break;
                case Direction.Right:
                    GoRight();
                    break;
                case Direction.Up:
                    GoUp();
                    break;
                case Direction.Down:
                    GoDown();
                    break;
            }
        }

        public void GoLeft()
        {
            if (Keys[X - 1][Y] == "X") return;
            X -= 1;
        }

        public void GoRight()
        {
            if (Keys[X + 1][Y] == "X") return;
            X += 1;
        }

        public void GoUp()
        {
            if (Keys[X][Y - 1] == "X") return;
            Y -= 1;
        }

        public void GoDown()
        {
            if (Keys[X][Y + 1] == "X") return;
            Y += 1;
        }
    }

    public static Direction DirectionFrom(char c)
    {
        return c switch
        {
            'U' => Direction.Up,
            'R' => Direction.Right,
            'D' => Direction.Down,
            'L' => Direction.Left,
        };
    }

    public enum Direction { Up, Right, Down, Left }
}

