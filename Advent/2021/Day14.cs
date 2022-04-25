using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day14
    {

        public string template;
        public Dictionary<string, (string a, string b)> rules;

        public Day14()
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

        public Dictionary<string, (string a, string b)> BuildRules(IEnumerable<string> input)
        {
            var rules = new Dictionary<string, (string a, string b)>();
            foreach (var line in input)
            {
                // Destructure the rule into the two pairs it will create
                var parts = line.Split(" -> ");
                var inLeft = parts[0][0];
                var inRight = parts[0][1];

                var outA = inLeft + parts[1];
                var outB = parts[1] + inRight;

                rules.Add(parts[0], (outA, outB));
            }
            return rules;
        }

        public void Solve()
        {
            Console.WriteLine(template);

            var (letters, pairs) = CountFreqs(template);
            foreach (var turn in Enumerable.Range(1, 40))
            {
                (letters, pairs) = ApplyRules(letters, pairs);
                Console.WriteLine($"After {turn} turns: {Score(letters)}");
            }
        }

        public (Dictionary<char, long> letters, Dictionary<string, long> pairs) CountFreqs(string input)
        {
            var letters = new Dictionary<char, long>();
            var pairs = new Dictionary<string, long>();
            foreach (var index in Enumerable.Range(0, input.Length - 1))
            {
                var letter = input[index];
                var pair = input[index..(index + 2)];
                AddValues(letters, letter, 1);
                AddValues(pairs, pair, 1);
            }
            // add the end letter that doesn't start a pair
            var lastLetter = input.Last();
            AddValues(letters, lastLetter, 1);

            return (letters, pairs);
        }

        public (Dictionary<char, long> letters, Dictionary<string, long> pairs) ApplyRules(Dictionary<char, long> letters, Dictionary<string, long> pairs)
        {
            var newLetters = new Dictionary<char, long>(letters);
            var newPairs = new Dictionary<string, long>(pairs);
            foreach(var pair in pairs.Keys)
            {
                var occurences = pairs[pair];
                if (occurences == 0) continue;
                if (rules.ContainsKey(pair))
                {
                    // Pair has transformed into an a and a b
                    // Update the pair frequencies by removing the pairs and adding an equiv number of as and bs
                    (string a, string b) = rules[pair];
                    newPairs[pair] -= occurences;

                    AddValues(newPairs, a, occurences);
                    AddValues(newPairs, b, occurences);

                    // Update the letter frequencies by adding the letter that has been added to form those two new pairs
                    var newLetter = a[1]; // Equally could be b[0]
                    AddValues(newLetters, newLetter, occurences);
                }
            }
            return (newLetters, newPairs);
        }

        public long Score(Dictionary<char, long> letters)
        {
            var maxFreq = letters.Max(pair => pair.Value);
            var minFreq = letters.Min(pair => pair.Value);

            return maxFreq - minFreq;
        }

        public void AddValues(Dictionary<string, long> dict, string key, long value)
        {
            if (!dict.ContainsKey(key))
                dict[key] = 0;
            dict[key] += value;
        }

        public void AddValues(Dictionary<char, long> dict, char key, long value)
        {
            if (!dict.ContainsKey(key))
                dict[key] = 0;
            dict[key] += value;
        }
    }
}
