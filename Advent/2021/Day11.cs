using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day11 : Day
    {
        public override string DayName => "11";
        public override string Answer1 => "1739";
        public override string Answer2 => "324";

        private List<List<Octo>> octos;

        public Day11() : base("2021/input11")
        {
                /*
                lines = sr.ReadToEnd().Trim().Split('\n').Select(line =>
                    line.Trim().ToList()
                    ).ToList();
                */

                /*
                octos = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526".Trim()
                */
            octos = Input
                .Split('\n')
                .Select((line, i) => 
                    line.Trim()
                    .Select((c, j) => 
                        new Octo(i, j, c.ToString())
                    )
                    .ToList()
                 ).ToList();

            Octo.Octos = octos;
        }

        public override void Solve()
        {
            var hundredScore = -1;
            var allFlashedTurn = -1;
            var turn = 0;
            do
            {
                turn += 1;

                foreach (var row in octos)
                    foreach (var octo in row)
                        octo.Tick();

                if (Octo.AllFlashing)
                    allFlashedTurn = turn;

                foreach (var row in octos)
                    foreach (var octo in row)
                        octo.Reset();

                if (turn == 100)
                {
                    hundredScore = Octo.Flashes;
                }
            } while (hundredScore == -1 || allFlashedTurn == -1);

            Result1 = hundredScore.ToString();
            Result2 = allFlashedTurn.ToString();
        }

        public List<Octo> UpdateNeighbours(int i, int j)
        {
            var flashers = new List<Octo>();
            // Up
            if (i > 0)
            {
                var flashed = octos[i - 1][j].Tick();
                if (flashed)
                {
                    flashers.Add(octos[i - 1][j]);
                    //flashers = flashers.Concat(UpdateNeighbours(i - 1, j)).ToList();
                }
            }
               
            // Down
            if (i < octos.Count - 1)
            { 
                var flashed = octos[i + 1][j].Tick();
                if (flashed)
                {
                    flashers.Add(octos[i + 1][j]);
                    //flashers = flashers.Concat(UpdateNeighbours(i + 1, j)).ToList();
                }
            } 

            // Left
            if (j != 0)
            { 
                var flashed = octos[i][j - 1].Tick();
                if (flashed)
                {
                    flashers.Add(octos[i][j - 1]);
                    //flashers = flashers.Concat(UpdateNeighbours(i, j - 1)).ToList();
                }
            } 

            // Right
            if (j < octos[i].Count - 1)
            { 
                var flashed = octos[i][j + 1].Tick();
                if (flashed)
                {
                    flashers.Add(octos[i][j + 1]);
                    //flashers = flashers.Concat(UpdateNeighbours(i, j + 1)).ToList();
                }
            }
            return flashers;
        }
    }

    public class Octo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Energy { get; set; }

        public static int Flashes = 0;
        public static List<List<Octo>> Octos = new List<List<Octo>>();

        public Octo(int x, int y, string init)
        {
            X = x;
            Y = y;
            Energy = int.Parse(init);
        }

        public bool Tick()
        {
            Energy += 1;
            if(Energy == 10)
            {
                Flashes += 1;
                TickNeighbours();
                return true;
            }
            return false;
        }

        public void Reset()
        {
            if(Energy >= 10)
                Energy = 0;
        }

        public void TickNeighbours()
        {
            foreach (int i in Enumerable.Range(-1, 3))
            {
                foreach (int j in Enumerable.Range(-1, 3))
                {
                    if (i == 0 && j == 0) continue;

                    var x = X + i;
                    var y = Y + j;

                    // Check edges
                    if (x < 0) continue;
                    if (x > Octos.Count - 1) continue;
                    if (y < 0) continue;
                    if (y > Octos[x].Count - 1) continue;
                    Octos[x][y].Tick();
                }
            }
        }

        public static bool AllFlashing => Octos.All(line => line.All(o => o.Energy >= 10));

        public override string ToString()
        {
            //return $"{Id}:{Energy} ";
            return $"{Energy}";
        }
    }
}
