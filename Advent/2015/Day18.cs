using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day18
{
    public char[][] Grid;

    public Day18()
    {
        using (StreamReader sr = File.OpenText("2015/input18"))
        {
            var input = sr.ReadToEnd();
            /*
            var input = @".#.#.#
                        ...##.
                        #....#
                        ..#...
                        #.#..#
                        ####..";
            */

            Grid = input.Trim().Split("\n").Select(s => s.Trim().ToCharArray()).ToArray();
        }
    }

    public void Solve()
    {
        var board = Grid;
        var stuckBoard = Grid.Select(c => c.ToArray()).ToArray();
        stuckBoard[0][0] = '#';
        stuckBoard[0][^1] = '#';
        stuckBoard[^1][0] = '#';
        stuckBoard[^1][^1] = '#';
        //foreach(var l in stuckBoard)Console.WriteLine(l);
        foreach(var turn in Enumerable.Range(1, 100))
        {
            Console.WriteLine(turn);
            board = Simulate(board, false);
            stuckBoard = Simulate(stuckBoard, true);

            //foreach(var l in stuckBoard)Console.WriteLine(l);
        }
        var score = board.Sum(l => l.Count(c => c == '#'));
        var stuckScore = stuckBoard.Sum(l => l.Count(c => c == '#'));
        Console.WriteLine($"Result: {score} | {stuckScore}");
    }

    public char[][] Simulate(char[][] input, bool stuckCorners)
    {
        var output = new char[input.Length][];
        for(int i = 0; i < output.Length; i++)
        {
            output[i] = new char[input[i].Length];
            for(int j = 0; j < input[i].Length; j++)
            {
                output[i][j] = SimulateCell(input, i, j, stuckCorners);
            }
        }
        return output;
    }

    public char SimulateCell(char[][] input, int i, int j, bool stuckCorners)
    {
        if (stuckCorners && (i == 0 || i == input.Length - 1) && (j == 0 || j == input[i].Length - 1)) return '#';

        var on = input[i][j] == '#';

        var neighbours = UpLeft(input, i, j) + Up(input, i, j) + UpRight(input, i, j)
            + Left(input, i, j) + Right(input, i, j)
            + DownLeft(input, i, j) + Down(input, i, j) + DownRight(input, i, j);

        if (on)
            return neighbours == 2 || neighbours == 3 ? '#' : '.';
        else
            return neighbours == 3 ? '#' : '.';
    }

    public int UpLeft(char[][] input, int i, int j)
    {
        if (i == 0) return 0;
        if (j == 0) return 0;
        return input[i - 1][j - 1] == '#' ? 1 : 0;
    }

    public int Up(char[][] input, int i, int j)
    {
        if (i == 0) return 0;
        return input[i - 1][j] == '#' ? 1 : 0;
    }

    public int UpRight(char[][] input, int i, int j)
    {
        if (i == 0) return 0;
        if (j == input[i].Length - 1) return 0;
        return input[i - 1][j + 1] == '#' ? 1 : 0;
    }
    
    public int Left(char[][] input, int i, int j)
    {
        if (j == 0) return 0;
        return input[i][j - 1] == '#' ? 1 : 0;
    }

    public int Right(char[][] input, int i, int j)
    {
        if (j == input[i].Length - 1) return 0;
        return input[i][j + 1] == '#' ? 1 : 0;
    }

    public int DownLeft(char[][] input, int i, int j)
    {
        if (i == input.Length - 1) return 0;
        if (j == 0) return 0;
        return input[i + 1][j - 1] == '#' ? 1 : 0;
    }

    public int Down(char[][] input, int i, int j)
    {
        if (i == input.Length - 1) return 0;
        return input[i + 1][j] == '#' ? 1 : 0;
    }

    public int DownRight(char[][] input, int i, int j)
    {
        if (i == input.Length - 1) return 0;
        if (j == input[i].Length - 1) return 0;
        return input[i + 1][j + 1] == '#' ? 1 : 0;
    }
}
