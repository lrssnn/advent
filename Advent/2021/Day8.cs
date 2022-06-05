using Advent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day8 : Day
    {
        public override string DayName => "08";
        public override string Answer1 => "303";
        public override string Answer2 => "961734";

        private List<Record> Records { get; set; }

        public Day8(): base("2021/input8")
        {
            //Records = new List<Record> { new Record("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf") };
            Records = Input.Trim().Split('\n').Select(line => new Record(line)).ToList();
        }

        public override void Solve()
        {
            var easyCount = Records.Sum(r =>
                r.Outputs.Count(o =>
                    o.Length == 2 ||
                    o.Length == 3 ||
                    o.Length == 4 ||
                    o.Length == 7
                    ));
            Result1 = easyCount.ToString();

            var total = 0;
            foreach(var record in Records)
            {
                var mappings = GenerateMappings(record.Inputs);
                var resultString = string.Concat(record.Outputs.Select(e => mappings[e]));
                var result = int.Parse(resultString);
                total += result;
            }
            Result2 = total.ToString();
        }

        static Dictionary<string, char> GenerateMappings(List<string> inputs)
        {
            // First, find the known mappings, based on those characters with unique numbers of inputs
            var one = inputs.Single(e => e.Length == 2);
            var four = inputs.Single(e => e.Length == 4);
            var seven = inputs.Single(e => e.Length == 3);
            var eight = inputs.Single(e => e.Length == 7);

            // We can identify 'f' as well, it is only missing from one of the inputs
            // This seems like a crazy way to calculate this but I like it
            var f = "abcdefg".Select(e => (e, inputs.Count(input => !input.Contains(e)))).Single(pair => pair.Item2 == 1).e;

            // 'c' is the other character in '1'
            var c = one.Single(e => e != f);

            // Identify 2, its the one missing f
            var two = inputs.Single(e => !e.Contains(f));

            var fiveLongs = inputs.Where(e => e.Length == 5);
            var three = fiveLongs.Single(e => e.Contains(c) && e != two);
            var five = fiveLongs.Single(e => !e.Contains(c));

            var e = "abcdefg".Single(e => two.Contains(e) && !three.Contains(e) && !five.Contains(e));

            var sixLongs = inputs.Where(input => input.Length == 6);
            var six = sixLongs.Single(input => !input.Contains(c));
            var nine = sixLongs.Single(input => !input.Contains(e));

            var mappings = new Dictionary<string, char>
            {
                {one, '1'},
                {two, '2'},
                {three, '3'},
                {four, '4'},
                {five, '5'},
                {six, '6'},
                {seven, '7'},
                {eight, '8'},
                {nine, '9'},
            };

            var zero = inputs.Single(e => !mappings.ContainsKey(e));
            
            mappings[zero] = '0';
            return mappings;
        }
    }

    public class Record
    {
        public List<string> Inputs { get; set; }
        public List<string> Outputs { get; set; }

        public IEnumerable<string> All => Inputs.Concat(Outputs);

        public Record(string init)
        {
            var parts = init.Split(" | ");
            Inputs = parts[0].Split(' ').ToList();
            Outputs = parts[1].Split(' ').ToList();

            // Sort the lists so we have consistency
            Inputs = Inputs.Select(e => string.Concat(e.OrderBy(e => e))).ToList();
            Outputs = Outputs.Select(e => string.Concat(e.OrderBy(e => e))).ToList();
        }

        public override string ToString()
        {
            var str = "";
            foreach (var input in Inputs)
            {
                str += input;
                str += ' ';
            }
            str += "| ";
            foreach (var output in Outputs)
            {
                str += output;
                str += ' ';
            }
            return str;
        }
    }
}
