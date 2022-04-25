using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day25
    {


        public char[][] Lines;
        public static bool Changed = false;

        public Day25()
        {
            using (StreamReader sr = File.OpenText("input25"))
            {

                var input = sr.ReadToEnd().Trim();

                //var input = @"...>>>>>...";

                /*
                var input = @"..........
                              .>v....v..
                              .......>..
                              ..........";
                */
                /*
                var input = @"v...>>.vv>
                              .vv>>.vv..
                              >>.>v>...v
                              >>v>>.>.v.
                              v>v.vv.v..
                              >.>>..v...
                              .vv..>.>v.
                              v.v..>>v.v
                              ....v..v.>";
                */

                Lines = input.Split("\n").Select(line => line.Trim().ToCharArray()).ToArray();
            }
        }

        public void Solve()
        {
            var moved = true;
            int turns = 0;
            while (moved)
            {
                turns++;
                Changed = false;
                Lines = MoveEast(Lines);
                Lines = MoveSouth(Lines);
                //Console.WriteLine(turns);
                //foreach(var line in Lines)Console.WriteLine(line);
                moved = Changed;
            }
            Console.WriteLine(turns);
        }

        public char[][] MoveEast(char[][] lines)
        {
            var result = new char[lines.Length][];
            var changed = false;
            for (int i = 0; i < lines.Length; i++)
            {
                var resLine = new char[lines[i].Length];
                // First item might get the arrow from the last line
                resLine[0] = CorrectChar(lines[i][0], lines[i][^1], lines[i][1], true);
                for (int j = 1; j < lines[i].Length - 1; j++)
                {
                    resLine[j] = CorrectChar(lines[i][j], lines[i][j - 1], lines[i][j + 1], true);
                } 
                // Last item might move into the zeroth space
                resLine[^1] = CorrectChar(lines[i][^1], lines[i][^2], lines[i][0], true);

                result[i] = resLine;
            }
            return result;
        }

        public char[][] MoveSouth(char[][] lines)
        {
            if (lines.Length < 2) return lines;
            var result = new char[lines.Length][];
            for (var i = 0; i < lines.Length; i++) result[i] = new char[lines[i].Length];

            for (int j = 0; j < lines[0].Length; j++)
            {
                // First item might get the arrow from the last line
                result[0][j] = CorrectChar(lines[0][j], lines[^1][j], lines[1][j], false);
                for (int i = 1; i < lines.Length - 1; i++)
                {
                    result[i][j] = CorrectChar(lines[i][j], lines[i - 1][j], lines[i + 1][j], false);
                } 
                // Last item might move into the zeroth space
                result[^1][j] = CorrectChar(lines[^1][j], lines[^2][j], lines[0][j], false);
            }
            return result;
        }

        public static char CorrectChar(char me, char left, char right, bool goingRight)
        {
            var mover = goingRight ? '>' : 'v';
            if (me == '.')
            {
                // Maybe the left to me has moved into my space
                if (left == mover)
                {
                    Changed = true;
                    return mover;
                }
                else
                    return '.';
            }
            else if (me == mover)
            {
                // Maybe I have moved away to the right
                if (right == '.')
                {
                    Changed = true;
                    return '.';
                }
                else
                    return mover;
            }
            else
                return me;
        }
    }
}

