using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day18
    {

        List<SnailNumber> Numbers;

        public Day18()
        {
            using (StreamReader sr = File.OpenText("input18"))
            {

                //var input = sr.ReadToEnd().Trim();
                /*
                var input = @"[1,2]
                              [[1,2],3]
                              [9,[8,7]]
                              [[1,9],[8,5]]
                              [[[[1,2],[3,4]],[[5,6],[7,8]]],9]
                              [[[9,[3,8]],[[0,9],6]],[[[3,7],[4,9]],3]]
                              [[[[1,3],[5,3]],[[1,3],[8,7]]],[[[4,9],[6,9]],[[8,2],[7,3]]]]";
                */

                var input = @"[[[[[9,8],1],2],3],4]
                              [7,[6,[5,[4,[3,2]]]]]
                              [[6,[5,[4,[3,2]]]],1]
                              [[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]
                              [[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]";


                var lines = input.Split("\n").Select(line => line.Trim());

                Numbers = lines.Select(l => new SnailNumber(l)).ToList();
            }
        }

        public void Solve()
        {
            foreach(var number in Numbers)
            {
                Console.WriteLine(number);
            }
        }

    }

    public class SnailNumber
    {
        public List<Value> Values { get; set; }

        public SnailNumber (string init)
        {
            Values = new List<Value>();
            var depth = 0;

            foreach(var c in init)
            {
                if (c == '[') depth++;
                else if (c == ']') depth--;
                else if (c == ',') continue;
                else Values.Add(new Value(int.Parse(c.ToString()), depth));
            }
        }

        public override string ToString()
        {
            var result = "";
            var depth = 0;
            foreach(var v in Values)
            {
                if(v.Depth > depth)
                {
                    foreach (char c in Enumerable.Repeat('[', v.Depth - depth))
                        result += c;
                }
                result += v.Val;
                if(v.Depth < depth)
                {
                    foreach (char c in Enumerable.Repeat(']', depth - v.Depth))
                        result += c;
                }
                result += ',';
                depth = v.Depth;
            }
            result = result[..^1];
            foreach (char c in Enumerable.Repeat(']', depth))
                result += c;
            return result;
        }
    }

    public class Value
    {
        public int Val { get; set; }
        public int Depth { get; set; }

        public Value(int value, int depth)
        {
            Val = value;
            Depth = depth;
        }
    }
}
