using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day25
{

    public int TargetRow { get; set; }
    public int TargetCol { get; set; }

    public Day25()
    {
        TargetRow = 2981;
        TargetCol = 3075;
    }

    public void Solve()
    {
        // I don't see any reason not to simply walk through the grid generating codes. Let's see

        var row = 2;
        var col = 1;

        long value = 20151125;

        while (true)
        {
            value = GetValue(value);

            if (row == TargetRow && col == TargetCol) break;

            (row, col) = GetRowCol(row, col);

            if (col == 1) Console.WriteLine($"Starting {row}th diagonal");
        }

        Console.WriteLine($"{value}");
    }

    public long GetValue(long value)
    {
        var product = value * 252533;
        return product % 33554393;
    }

    public (int r, int c) GetRowCol(int row, int col)
    {
        // If we can go up and right, we do, otherwise start new diag
        if (row != 1)
            return (row - 1, col + 1);
        else
            return (col + 1, 1);
    }

}
