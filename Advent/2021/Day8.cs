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
        public override string Answer2 => "Unknown";

        private List<Record> Records { get; set; }

        public Day8(string inputFileName): base(inputFileName)
        {
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
            //Console.WriteLine($"{init} -> {this}");
        }

        public string MapString(string input)
        {
            var mapping = GetMapping();
            return string.Concat(input.Select(c => mapping.ContainsKey(c) ? mapping[c] : '?'));
        }

        public int DecodeString(string input)
        {
            return Standard.Mapping[string.Concat(input.OrderBy(c => c))];
        }

        public Dictionary<char, char> GetMapping()
        {
            var mapping = new Dictionary<char, char>();
            // 2 length tells us digit 1: cf
            var two = All.FirstOrDefault(code => code.Length == 2);
            if(two != null)
            {
                mapping[two[0]] = 'c';
                mapping[two[1]] = 'f';
            }
            // 3 length tells us digit 7: acf
            var three = All.FirstOrDefault(code => code.Length == 3);
            if(three != null)
            {
                mapping[three[0]] = 'a';
                mapping[three[1]] = 'c';
                mapping[three[2]] = 'f';
            }
            // 4 length tells us digit 4: bcdf
            var four = All.FirstOrDefault(code => code.Length == 4);
            if(four != null)
            {
                mapping[four[0]] = 'b';
                mapping[four[1]] = 'c';
                mapping[four[2]] = 'd';
                mapping[four[3]] = 'f';
            }
            // 7 length tells us digit 8: abcdefg
            var seven = All.FirstOrDefault(code => code.Length == 7);
            if(seven != null)
            {
                mapping[seven[0]] = 'a';
                mapping[seven[1]] = 'b';
                mapping[seven[2]] = 'c';
                mapping[seven[3]] = 'd';
                mapping[seven[4]] = 'e';
                mapping[seven[5]] = 'f';
                mapping[seven[6]] = 'g';
            }

            //PrintMapping(mapping);
            return mapping;
        }

        public string DecodedString()
        {
            var str = "";
            foreach (var input in Inputs.Select(MapString).Select(DecodeString))
            {
                str += input;
                str += ' ';
            }
            str += "| ";
            foreach (var output in Outputs.Select(MapString).Select(DecodeString))
            {
                str += output;
                str += ' ';
            }
            return str;
        }

        public static void PrintMapping<K, V>(Dictionary<K, V> mapping)
        {
            foreach(var pair in mapping)
            {
                Console.WriteLine($"{pair.Key} -> {pair.Value}");
            }
        }

        public string MappedString()
        {
            var str = "";
            foreach (var input in Inputs.Select(MapString))
            {
                str += input;
                str += ' ';
            }
            str += "| ";
            foreach (var output in Outputs.Select(MapString))
            {
                str += output;
                str += ' ';
            }
            return str;
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

    public static class Standard
    {
        public static readonly Dictionary<string, int> Mapping = new()
        {
            {"abcefg", 0 },
            {"cf", 1 },
            {"acdeg", 2 },
            {"acdfg", 3 },
            {"bcdf", 4 },
            {"abdfg", 5 },
            {"abdefg", 6 },
            {"acf", 7 },
            {"abcdefg", 8 },
            {"abcdfg", 9 },
        };
    }
}
