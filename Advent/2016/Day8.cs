using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day8
{
    public List<Instruction> Input;
    public int X;
    public int Y;

    public Day8()
    {
        using StreamReader sr = File.OpenText("2016/input8");
        var input = sr.ReadToEnd().Trim();
        var input1 = @"rect 3x2
                       rotate column x=1 by 1
                       rotate row y=0 by 4
                       rotate column x=1 by 1";
        X = 7;
        Y = 3;
        X = 50;
        Y = 6;
        Input = input.Split('\n').Select(s => s.Trim()).Select(s => new Instruction(s)).ToList();
    }

    public void Solve()
    {
        var board = new Board(X, Y);
        //Console.WriteLine(board);
        foreach(var instruction in Input)
        {
            //Console.WriteLine(instruction);
            board.ApplyInstruction(instruction);
            //Console.WriteLine(board);
        }
        Console.WriteLine(board);
        Console.WriteLine(board.Cells.Sum(r => r.Count(c => c)));
    }

    public class Instruction
    {
        public InstructionType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Magnitude { get; set; }

        public Instruction(string input)
        {
            var parts = input.Split(' ');
            switch (parts[0])
            {
                case "rect":
                    Type = InstructionType.Rect;
                    var xy = parts[1].Split('x');
                    X = int.Parse(xy[0]);
                    Y = int.Parse(xy[1]);
                    break;

                case "rotate":
                    switch (parts[1])
                    {
                        case "column":
                            Type = InstructionType.RotateCol;
                            var xStr = parts[2].Split('=')[1]; 
                            X = int.Parse(xStr);
                            Magnitude = int.Parse(parts[4]);
                            break;

                        case "row":
                            Type = InstructionType.RotateRow;
                            var yStr = parts[2].Split('=')[1]; 
                            Y = int.Parse(yStr);
                            Magnitude = int.Parse(parts[4]);
                            break;

                        default: throw new Exception("Parse error");
                    }
                    break;

                default: throw new Exception("Parse error");
            }
        }

        public override string ToString() => $"{Type} ({X}, {Y}) | {Magnitude}";
    }

    public class Board
    {
        public bool[][] Cells { get; set; }
        public int XSize { get; set; }
        public int YSize { get; set; }

        public Board(int xSize, int ySize)
        {
            Cells = new bool[ySize][];
            for(var i = 0; i < ySize; i++) Cells[i] = new bool[xSize];
            XSize = xSize;
            YSize = ySize;
        }

        public void ApplyInstruction(Instruction i)
        {
            switch (i.Type)
            {
                case InstructionType.Rect: ApplyRect(i.X, i.Y); break;
                case InstructionType.RotateRow: ApplyRotateRow(i.Y, i.Magnitude); break;
                case InstructionType.RotateCol: ApplyRotateCol(i.X, i.Magnitude); break;
            };
        }

        private void ApplyRect(int x, int y)
        {
            for(var u = 0; u < x; u++)
            for(var v = 0; v < y; v++)
                Cells[v][u] = true;
        }

        private void ApplyRotateRow(int y, int magnitude)
        {
            for(int round = 0; round < magnitude; round++)
            {
                var oldRow = Cells[y].ToArray();
                for(var x = 1; x < XSize; x++)
                {
                    Cells[y][x] = oldRow[x - 1];
                }
                Cells[y][0] = oldRow[XSize - 1];
            }
        }

        private void ApplyRotateCol(int x, int magnitude)
        {
            for(int round = 0; round < magnitude; round++)
            {
                var oldCells = Cells.Select(r => r.ToArray()).ToArray();
                for(var y = 1; y < YSize; y++)
                {
                    Cells[y][x] = oldCells[y - 1][x];
                }
                Cells[0][x] = oldCells[YSize - 1][x];
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach(var row in Cells)
            {
                foreach (var cell in row) str.Append(cell ? '#' : '.');
                str.Append('\n');
            }
            return str.ToString();
        }
    }

    public enum InstructionType { Rect, RotateCol, RotateRow }
}
