using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day4
    {
        public static void Solve1()
        {
            using (StreamReader sr = File.OpenText("input4"))
            {
                var lines = sr.ReadToEnd().Split('\n');
                var numbers = lines[0].Split(',').Select(s => int.Parse(s));
                var boards = FindBoards(lines.TakeLast(lines.Length - 2));
                var turn = 0;

                foreach (var num in numbers)
                {
                    turn++;
                    Console.WriteLine($"{turn}: {num}");
                    boards = boards.AsEnumerable().Select(board => MarkNumber(board, num)).ToList();
                    var winners = boards.Where(board => IsWinner(board));
                    if (winners.Any())
                    {
                        var winner = winners.First();
                        PrintBoard(winner);
                        Console.WriteLine($"Winning score is {CalculateScore(winner, num)}");
                    }
                }
            }
        }

        public static void Solve2()
        {
            using (StreamReader sr = File.OpenText("input4"))
            {
                var lines = sr.ReadToEnd().Split('\n');
                var numbers = lines[0].Split(',').Select(s => int.Parse(s));
                var boards = FindBoards(lines.TakeLast(lines.Length - 2));
                var turn = 0;

                foreach (var num in numbers)
                {
                    turn++;
                    Console.WriteLine($"{turn}: {num}");
                    boards = boards.AsEnumerable().Select(board => MarkNumber(board, num)).ToList();
                    var winners = boards.Where(board => IsWinner(board)).ToList();
                    if (winners.Any())
                    {
                        // Eliminate the winners
                        foreach(var winner in winners)
                        {
                            if (boards.Count() == 1)
                            {
                                Console.WriteLine($"Final Winner Found! Losingest Score = {CalculateScore(boards.Single(), num)}");
                            }
                            else
                            {
                                boards.Remove(winner);
                            }
                        }
                        Console.WriteLine($"Removed {winners.Count()} boards, {boards.Count()} remain...");
                    }
                }
            }
        }

        public static List<Board> FindBoards(IEnumerable<string> lines)
        {
            var id = 0;
            var boards = new List<Board>();
            var board = new Board() { Id = ++id };
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    boards.Add(board);
                    PrintBoard(board);
                    board = new Board() { Id = ++id };
                } else
                {
                    board.Numbers.Add(line.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e)).Select(s => int.Parse(s)).ToList());
                }
            }
            return boards;
        }

        public static Board MarkNumber(Board board, int target)
        {
            for(int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    board.Numbers[i][j] = board.Numbers[i][j] == target ? -1 : board.Numbers[i][j];
                }
            }
            return board;
            //return new Board(board.Id, board.Numbers.Select(row => row.Select(num => num == target ? -1 : num).ToList()).ToList());
        }

        public static bool IsWinner(Board board)
        {
            // Horizontal
            var winningRow = board.Numbers.Any(row => row.All(num => num == -1));
            // Vertical
            var winningCol = Enumerable.Range(0, 5)
                .Select(col => board.Numbers.All(row => row[col] == -1))
                .Any(e => e);

            return winningRow || winningCol;
        }

        public static int CalculateScore(Board board, int lastNum)
        {
            var sum = board.Numbers.Select(row => row.Where(n => n != -1).Sum()).Sum();
            return sum * lastNum;
        }

        public static void PrintBoard(Board board)
        {
            Console.WriteLine($"Board {board.Id}");
            foreach (var row in board.Numbers)
            {
                foreach (var num in row)
                {
                    Console.Write($"{num:00;-0} ");
                }
                Console.WriteLine();
            }
        }
    }

    public class Board
    {
        public List<List<int>> Numbers { get; set; }
        public int Id { get; set; }

        public Board()
        {
            Numbers = new List<List<int>>();
        }

        public Board(int id, List<List<int>> nums)
        {
            Id = id;
            Numbers = nums;
        }
    }
}
