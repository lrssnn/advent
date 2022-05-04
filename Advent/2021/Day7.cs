using Advent;

namespace AdventTwentyOne
{
    public class Day7 : Day
    {
        public override string DayName => "07";
        public override string Answer1 => "328262";
        public override string Answer2 => "90040997";

        private List<Crab> Crabs { get; set; }

        public Day7() : base("2021/input7")
        {
            Crabs = Input.Split(',').Select(s => new Crab(int.Parse(s))).ToList();
        }

        public override void Solve()
        {
            Result1 = Optimise(Crabs, Crab.Distance).cost.ToString();
            Result2 = Optimise(Crabs, Crab.DistanceScaled).cost.ToString();
        }

        public static (int target, int cost) Optimise(IEnumerable<Crab> crabs, Func<Crab, int, int> costFunction)
        {
            var maxValue = crabs.Max(c => c.Position);
            var minValue = int.MaxValue;
            var minTarget = -1;

            foreach (var target in Enumerable.Range(0, maxValue))
            {
                var distance = crabs.Sum(c => costFunction(c, target));
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
