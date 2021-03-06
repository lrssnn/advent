using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day10: Day
    {
        public override string DayName => "10";
        public override string Answer1 => "266301";
        public override string Answer2 => "3404870164";

        public List<List<char>> lines;

        public Day10(): base("2021/input10")
        {
            /*
                Input = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";
            */

            lines = Input.Split('\n').Select(line =>
                line.Trim().ToList()
                ).ToList();
        }

        public override void Solve()
        {
            var corrupts = new List<char>();
            var corruptLines = new List<List<char>>();
            foreach (var line in lines)
            {
                var first = IsCorrupted(line);
                if (first != '_')
                {
                    corrupts.Add(first);
                    corruptLines.Add(line);
                }
            }

            Result1 = corrupts.Sum(ValueConverter).ToString();

            lines = lines.Where(l => !corruptLines.Contains(l)).ToList();

            var completions = lines.Select(l => GetCompletion(l));

            var scores = completions.Select(ValueCompletion).OrderBy(score => score);
            var middleIndex = scores.Count() / 2;
            Result2 = scores.ElementAt(middleIndex).ToString();
        }

        public List<char> GetCompletion(List<char> l)
        {
            var completion = new List<char>();
            var stack = new Stack<char>();
            foreach (char c in l)
            {
                if (IsOpener(c))
                {
                    stack.Push(c);
                }
                else
                {
                    var expected = GetExpected(stack.Pop());
                    if (c == expected) continue;
                    throw new Exception("This should be impossible");
                    // We've reached the end of the line, build the completion from the contents of the stack
                }
            }
            return stack.ToList().Select(GetExpected).ToList();
        }

        public char IsCorrupted(List<char> line)
        {
            var stack = new Stack<char>();
            foreach (char c in line)
            {
                if (IsOpener(c))
                {
                    stack.Push(c);
                }
                else
                {
                    // The line is corrupt if the next thing doesn't equal the corresponding bracket to the top of the stack
                    var expected = GetExpected(stack.Pop());
                    if (c != expected)
                        return c;
                }
            }

            return '_';
        }

        public char GetExpected(char c)
        {
            return c switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                '<' => '>',
                _ => ' ',
            };
        }

        public bool IsOpener(char c)
        {
            return c switch
            {
                '(' => true,
                '[' => true,
                '{' => true,
                '<' => true,
                _ => false,
            };
        }

        public static int ValueConverter(char c)
        {
            return c switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => 0,
            };
        }

        public static int ValueConverterLinear(char c)
        {
            return c switch
            {
                ')' => 1,
                ']' => 2,
                '}' => 3,
                '>' => 4,
                _ => 0,
            };
        }

        public static long ValueCompletion(List<char> completion)
        {
            long score = 0;
            foreach (char c in completion)
            {
                score *= 5;
                score += ValueConverterLinear(c);
            }
            return score;
        }
    }
}
