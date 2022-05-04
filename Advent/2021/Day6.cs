using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day6 : Day
    {
        public override string DayName => "06";
        public override string Answer1 => "352195";
        public override string Answer2 => "1600306001288";

        private Fishes Fishes { get; set; }

        public Day6() : base("2021/input6")
        {
            Fishes = new Fishes(Input);
        }

        public override void Solve()
        {
            foreach (var turn in Enumerable.Range(1, 256))
            {
                Fishes.Tick();
                if (turn == 80) Result1 = Fishes.States.Sum().ToString();
            }

            Result2 = Fishes.States.Sum().ToString();
        }
    }

    public class Fishes
    {
        // Represent the ecosystem with the counts of each possible state
        public List<long> States { get; set; }

        public Fishes(string init)
        {
            States = new List<long>();
            var nums = init.Split(',').Select(long.Parse);
            for (var i = 0; i <= 8; i++)
            {
                States.Add(nums.Count(x => x == i));
            }
        }

        public void Tick()
        {
            // Remember how many zeroes we had
            var reproducers = States[0];
            // Move every state down one
            for(int i = 0; i <= 7; i++)
            {
                States[i] = States[i + 1];
            }
            // Zeroes turn into sixes and 8s
            States[6] += reproducers;
            States[8] = reproducers;
        }
    }
}
