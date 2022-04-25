using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day7
    {
        public static void Solve()
        {
            using (StreamReader sr = File.OpenText("input7"))
            {
                var text= sr.ReadToEnd().Trim();
                var crabs = text.Split(',').Select(s => new Crab(int.Parse(s)));

                var linear = Optimise(crabs, Crab.Distance);
                var scaled = Optimise(crabs, Crab.DistanceScaled);

                //Console.WriteLine(fishes.Count());
                Console.WriteLine($"Linear: {linear.target} ({linear.cost})");
                Console.WriteLine($"Scaled: {scaled.target} ({scaled.cost})");
            }
        }

        public static (int target, int cost) Optimise(IEnumerable<Crab> crabs, Func<Crab, int, int> costFunction)
        {
            var maxValue = crabs.Max(c => c.Position);
            var minValue = int.MaxValue;
            var minTarget = -1;

            foreach (var target in Enumerable.Range(0, maxValue))
            {
                var distance = crabs.Sum(c => costFunction(c, target));
                Console.WriteLine($"{target}: {distance}");
                if (distance < minValue)
                {
                    minTarget = target;
                    minValue = distance;
                }
            }
            return (minTarget, minValue);
        }
    }

    public class Crab
    {
        public int Position { get; set; }

        public Crab(int position)
        {
            Position = position;
        }

        public static int Distance(Crab c, int target)
        {
            return Math.Abs(c.Position - target);
        }

        public static int DistanceScaled(Crab c, int target)
        {
            var n = Distance(c, target);
            var cost = (n*(n+1))/2;
            return cost;
        }
    }
}
