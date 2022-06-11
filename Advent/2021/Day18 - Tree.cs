using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day18Tree
    {

        List<SnailNumberTree> Numbers;

        public Day18Tree()
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

                Numbers = lines.Select(line => new SnailNumberTree(line, 0, null, null)).ToList();
            }
        }

        public void Solve()
        {
            foreach(var number in Numbers)
            {
                Console.WriteLine(number);
                number.Explode();
                Console.WriteLine(number);
            }
        }
    }

    public class SnailNumberTree
    {
        public SnailNumberTree? LeftNumber { get; set; }
        public SnailNumberTree? RightNumber { get; set; }
        public int? Value { get; set; }

        public int Depth { get; set; }
        public SnailNumberTree? Parent { get; set; }
        public bool? IsLeft { get;set; }

        public SnailNumberTree(string init, int depth, SnailNumberTree? parent, bool? isLeft)
        {
            var middleIndex = IndexOfMyComma(init);

            Depth = depth;
            Parent = parent;
            IsLeft = isLeft;

            if(middleIndex != -1)
            {
                LeftNumber = new SnailNumberTree(init[1..middleIndex], depth + 1, this, true);
                RightNumber = new SnailNumberTree(init[(middleIndex + 1)..^1], depth + 1, this, false);
            } else
            {
                Value = int.Parse(init);
            }
        }

        public static int IndexOfMyComma(string init)
        {
            // Count the open and close brackets, finding the comma which occurs when we only know one open bracket.
            // That is the comma which separates the two parts of the top level number
            var opens = 0;
            for (int i = 0; i < init.Length; i++)
            {
                if (init[i] == '[')
                    opens++;
                else if (init[i] == ']')
                    opens--;
                else if (init[i] == ',' && opens == 1)
                    return i;
            }
            return -1;
        }

        public bool Explode()
        {
            var exploder = GetExplodeCandidate();
            if (exploder == null) return false;

            var changed = exploder.ExplodeLeft();
            changed = exploder.ExplodeRight() || changed;

            exploder.LeftNumber = null;
            exploder.RightNumber = null;
            exploder.Value = 0;

            return changed;
        }

        public bool ExplodeLeft()
        {
            // We know we should only explode pairs of values
            var leftVal = LeftNumber!.Value;

            // Find the first of our parents which is a Right, that means it has a Left sibling
            var firstRightParent = FirstRightParent();

            if (firstRightParent == null)
                return false; // Nothing to our left

            var leftSibling = firstRightParent.LeftNumber;

            // Dig down the right side of that left sibling's children to find the first value
            var adjacentValue = leftSibling!.FindValueChildRight();

            adjacentValue.Value += leftVal;
            return true;
        }

        public bool ExplodeRight()
        {
            // We know we should only explode pairs of values
            var rightVal = RightNumber!.Value;

            // Find the first of our parents which is a Right, that means it has a Left sibling
            var firstLeftParent = FirstLeftParent();

            if (firstLeftParent == null)
                return false; // Nothing to our left

            var rightSibling = firstLeftParent.FindRightSibling();

            // Dig down the right side of that left sibling's children to find the first value
            var adjacentValue = rightSibling.FindValueChildLeft();

            adjacentValue.Value += rightVal;
            return true;
        }

        public SnailNumberTree FindRightSibling()
        {
            // Looking for the lowest level parent who has a right child? 
            // What do we do if we hit the root?
            if (RightNumber != null) return RightNumber;
            if (Parent?.RightNumber != null) return Parent.RightNumber;
            throw new Exception("Bad Situation?");
        }

        public SnailNumberTree FirstRightParent() 
        {
            var candidate = Parent;
            if(candidate == null) throw new Exception("Bad Situation");
            while (candidate.IsLeft ?? true)
            {
                if (candidate.Parent == null) return candidate;
                candidate = candidate.Parent;
            }
            return candidate;
        }

        public SnailNumberTree FirstLeftParent() 
        {
            var candidate = Parent;
            if(candidate == null) throw new Exception("Bad Situation");
            while (!candidate.IsLeft ?? true)
            {
                if (candidate.Parent == null) return candidate;
                candidate = candidate.Parent;
            }
            return candidate;
        }


        public SnailNumberTree FindValueChildRight()
        {
            var candidate = this;
            while(candidate.RightNumber != null)
            {
                candidate = candidate.RightNumber;
            }
            return candidate;
        }

        public SnailNumberTree FindValueChildLeft()
        {
            var candidate = this;
            while(candidate.LeftNumber != null)
            {
                candidate = candidate.LeftNumber;
            }
            return candidate;
        }

        public SnailNumberTree? GetExplodeCandidate()
        {
            if (LeftNumber != null)
            {
                var candidate = LeftNumber.GetExplodeCandidate();
                if (candidate != null) return candidate;
            }
            if (RightNumber != null)
            {
                var candidate = RightNumber.GetExplodeCandidate();
                if (candidate != null) return candidate;
            }
            if (Depth >= 4 && Value == null)
            {
                return this;
            }
            return null;
        }


        public override string ToString()
        {
            if (Value != null)
                return Value.Value.ToString();

            string result = "[";
            
            if (LeftNumber != null)
                result += LeftNumber.ToString();

            result += ",";

            if (RightNumber != null)
                result += RightNumber.ToString();

            result += $"]";

            return result;
        }
    }
}
