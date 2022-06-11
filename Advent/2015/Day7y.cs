using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day7
{
    public List<Instruction> Instructions { get; set;}
    public Dictionary<string, ushort> Cache { get; set; }


    public Day7()
    {
        using (StreamReader sr = File.OpenText("2015/input7"))
        {
            var input = sr.ReadToEnd().Trim();

            /*
            var input = @"123 -> x
                        456 -> y
                        x AND y -> d
                        x OR y -> e
                        x LSHIFT 2 -> f
                        y RSHIFT 2 -> g
                        NOT x -> h
                        NOT y -> i";
            */

            Instructions = input.Split('\n').Select(line => line.Trim()).Select(l => new Instruction(l)).ToList();
            Cache = new Dictionary<string, ushort>();
        }
    }

    public void Solve()
    {
        var partOne = Evaluate(Instructions, "a");
        Console.WriteLine(partOne);
        DoctorInstructions("b", partOne);
        Cache.Clear();
        var partTwo = Evaluate(Instructions, "a");
        Console.WriteLine(partTwo);
    }

    public void DoctorInstructions(string targetLabel, ushort value)
    {
        var target = Instructions.Single(i => i.Destination == targetLabel);
        target.Op = Instruction.Operation.ASSIGN;
        target.OpA = value.ToString();
        target.OpB = null;
    }

    public ushort Evaluate(IEnumerable<Instruction> instructions, string targetLabel)
    {
        // Check the Cache
        if (Cache.ContainsKey(targetLabel))
        {
            Console.WriteLine($"Cache Hit on {targetLabel}");
            return Cache[targetLabel];
        }

        // We might be looking at an actual number already
        if (ushort.TryParse(targetLabel, out ushort res)) return res;
        
        // Not a number, so its a label, look up the rule for its expression
        var target = instructions.Single(i => i.Destination == targetLabel);

        // Evaluate each side of the equation
        var a = Evaluate(instructions, target.OpA);
        // B might be null if this is a not operator. The value won't matter but Evaluate can't handle it
        ushort b = 0;
        if (target.OpB != null)
            b = Evaluate(instructions, target.OpB);

        var result = ApplyOperation(target.Op, a, b);
        Console.WriteLine($"Caching {targetLabel} - {result}");
        Cache.Add(targetLabel, result);
        return result;
    }

    public ushort ApplyOperation(Instruction.Operation operation, ushort a, ushort b)
    {
        return operation switch
        {
            Instruction.Operation.AND    => (ushort)(a & b),
            Instruction.Operation.OR     => (ushort)(a | b),
            Instruction.Operation.NOT    => (ushort)(~a),
            Instruction.Operation.LSHIFT => (ushort)(a << b),
            Instruction.Operation.RSHIFT => (ushort)(a >> b),
            Instruction.Operation.ASSIGN => a,
            _ => throw new Exception("Unexpected input"),
        };
    }

    public class Instruction
    {
        public string OpA { get; set; }
        public string? OpB { get; set; }
        public string Destination { get; set; }
        public Operation Op { get; set; }

        public Instruction(string init)
        {
            var parts = init.Split(" -> ");
            var left = parts[0];
            var right = parts[1];

            var ops = left.Split(' ');
            if (ops.Length == 1)
            {
                OpA = ops[0];
                Op = Operation.ASSIGN;
            }
            else if (ops.Length == 2)
            {
                // Should be not
                Op = GetOperation(ops[0]);
                OpA = ops[1];
            }
            else
            {
                OpA = ops[0];
                Op = GetOperation(ops[1]);
                OpB = ops[2];
            }

            Destination = right;
        }

        public override string ToString()
        {
            return $"{OpA} {Op} {OpB} -> {Destination}";
        }

        private Operation GetOperation(string value)
        {
            return (Operation)Enum.Parse(typeof(Operation), value);
        }

        public enum Operation
        {
            AND,
            OR,
            NOT, 
            LSHIFT,
            RSHIFT,
            ASSIGN,
        }
    }
}
