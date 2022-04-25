using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day6
    {
        public static void Solve()
        {
            using (StreamReader sr = File.OpenText("input6"))
            {
                var text= sr.ReadToEnd().Trim();
                //var fishes = ReadFishes(text);
                var fishes = new Fishes(text);

                foreach (var turn in Enumerable.Range(1, 256))
                {
                    //PrintState(turn, fishes);
                    //fishes = fishes.SelectMany(fish => fish.Tick()).ToList();
                    fishes.Tick();
                    if (turn == 80) PrintState(turn, fishes);
                }

                //Console.WriteLine(fishes.Count());
                Console.WriteLine(fishes.States.Sum());
            }
        }

        public static void PrintState(int turn, Fishes fishes)
        {
            Console.WriteLine($"{turn}: {fishes.States.Sum()}");
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
