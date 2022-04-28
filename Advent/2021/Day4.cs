using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day4 : Day
    {
        public override string DayName => "04";
        public override string Answer1 => "54275";
        public override string Answer2 => "13158";

        private List<Board> Boards { get; set; }
        private List<int> Numbers { get; set; }

        public Day4(string inputFileName) : base(inputFileName)
        {
            var lines = Input.Split('\n');
            Numbers = lines[0].Split(',').Select(s => int.Parse(s)).ToList();
            Boards = FindBoards(lines.TakeLast(lines.Length - 2));
        }

        public override void Solve()
        {
            var boards = Boards.ToList();

            foreach (var num in Numbers)
            {
                boards = boards.AsEnumerable().Select(board => MarkNumber(board, num)).ToList();
                var winners = boards.Where(board => IsWinner(board));
                if (winners.Any())
                {
                    var winner = winners.First();
                    var score = CalculateScore(winner, num);
                    Result1 = score.ToString();
                    break;
                }
            }

            boards = Boards;

            foreach (var num in Numbers)
            {
                boards = boards.AsEnumerable().Select(board => MarkNumber(board, num)).ToList();
                var winners = boards.Where(board => IsWinner(board)).ToList();
                if (winners.Any())
                {
                    // Eliminate the winners
                    foreach(var winner in winners)
                    {
                        if (boards.Count() == 1)
                        {
                            var score = CalculateScore(boards.Single(), num);
                            Result2 = score.ToString();
                            return;
                        }
                        else
                        {
                            boards.Remove(winner);
                        }
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
