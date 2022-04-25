using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day14Naive
    {

        public string template;
        public Dictionary<string, string> rules;

        public Day14Naive()
        {
            using (StreamReader sr = File.OpenText("input14"))
            {

                var input = sr.ReadToEnd().Trim();
                /*
                var input = @" NNCB

                                CH -> B
                                HH -> N
                                CB -> H
                                NH -> C
                                HB -> C
                                HC -> B
                                HN -> C
                                NN -> C
                                BH -> H
                                NC -> B
                                NB -> B
                                BN -> B
                                BB -> N
                                BC -> B
                                CC -> N
                                CN -> C";
                */

                var lines = input.Split('\n').Select(e => e.Trim());

                template = lines.First();
                rules = BuildRules(lines.Skip(2));
            }
        }

        public Dictionary<string, string> BuildRules(IEnumerable<string> input)
        {
            var rules = new Dictionary<string, string>();
            foreach (var line in input)
            {
                var parts = line.Split(" -> ");
                rules.Add(parts[0], parts[1]); 
            }
            return rules;
        }

        public void Solve()
        {
            Console.WriteLine(template);

            var result = template;
            foreach (var turn in Enumerable.Range(1, 40))
            {
                result = ApplyRules(result);
                Console.WriteLine($"After {turn} turns: {Score(result)} -> {result.Length}");
            }
        }

        public string ApplyRules(string input)
        {
            var result = "";
            foreach (var index in Enumerable.Range(0, input.Length - 1))
            {
                result += input[index];
                var pair = input[index..(index+2)];
                if (rules.ContainsKey(pair))
                    result += rules[pair];
            }
            result += input[input.Length-1];
            return result;
        }

        public int Score(string input)
        {
            var frequencies = input.GroupBy(c => c);
            var maxFreq = frequencies.Max(group => group.Count());
            var minFreq = frequencies.Min(group => group.Count());

            return maxFreq - minFreq;
        }
    }
}
