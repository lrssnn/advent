using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day10
{
    public List<Instruction> Input;

    public Day10()
    {
        using StreamReader sr = File.OpenText("2016/input10");
        var input = sr.ReadToEnd().Trim();
        var input1 = @"value 5 goes to bot 2
                    bot 2 gives low to bot 1 and high to bot 0
                    value 3 goes to bot 1
                    bot 1 gives low to output 1 and high to bot 0
                    bot 0 gives low to output 2 and high to output 0
                    value 2 goes to bot 2";
        Input = input1.Split('\n').Select(s => s.Trim()).Select(s => new Instruction(s)).ToList();
    }

    public void Solve()
    {
        var bots = new List<Bot>();
        var setupInstructions = Input.Where(i => i.Type == InstructionType.Single);
        
        foreach (var instruction in Input)
        {
            Console.WriteLine(instruction);
        }

    }

    public class Bot
    {
        public int Id { get; set; }
        public int Low { get; set; }
        public int High { get; set; }

        public Bot(int id, int a, int b)
        {
            Id = id;
            Low = a;
            High = b;
        }
    }

    public class Instruction
    {
        public InstructionType Type { get; set; }
        public int Source { get; set; }
        public int DestLow { get; set; }
        public int DestHigh { get; set; }
        public int Value { get; set; }
        public bool LowOutput { get; set; }
        public bool HighOutput { get; set; }

        public Instruction(string s)
        {
            var parts = s.Split(' ');
            switch(parts[0])
            {
                case "value":
                    Type = InstructionType.Single;
                    Value = int.Parse(parts[1]);
                    DestLow = int.Parse(parts[5]);
                    break;

                case "bot":
                    Type = InstructionType.Double;
                    Source = int.Parse(parts[1]);
                    LowOutput = parts[5] == "output";
                    DestLow = int.Parse(parts[6]);
                    HighOutput = parts[10] == "output";
                    DestHigh = int.Parse(parts[11]);
                    break;
            }
        }

        public override string ToString()
        {
            return Type == InstructionType.Single ? ToStringSingle() : ToStringDouble();
        }

        private string ToStringSingle() => $"value {Value} goes to bot {DestLow} ";
        private string ToStringDouble() => $"bot {Source} gives low to {LowOutputString} {DestLow} and high to {HighOutputString} {DestHigh}";

        private string LowOutputString => LowOutput ? "Output" : "Bot";
        private string HighOutputString => HighOutput ? "Output" : "Bot";
    }

    public enum InstructionType { Single, Double }

}
