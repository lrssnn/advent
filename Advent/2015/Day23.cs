using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day23
{

    public Computer PC;

    public Day23()
    {
        using (StreamReader sr = File.OpenText("2015/input23"))
        {
            var input = sr.ReadToEnd();

            var instructions = input.Trim().Split("\n").Select(s => s.Trim()).Select(s => new Instruction(s)).ToArray();
            PC = new Computer(instructions);
        }
    }

    public void Solve()
    {
        /*
        foreach (var instruction in PC.Instructions)
            Console.WriteLine(instruction);
        */

        while (!PC.Execute())
        {
            //Console.WriteLine($"a: {PC.Registers[0]}, b: {PC.Registers[1]}, PC: {PC.IPointer}");
        }

        Console.WriteLine("=================");
        Console.WriteLine($"a: {PC.Registers[0]}, b: {PC.Registers[1]}, PC: {PC.IPointer}");

        // Reset for part 2
        PC.IPointer = 0;
        PC.Registers[0] = 1;
        PC.Registers[1] = 0;
        while (!PC.Execute())
        {
            //Console.WriteLine($"a: {PC.Registers[0]}, b: {PC.Registers[1]}, PC: {PC.IPointer}");
        }

        Console.WriteLine("=================");
        Console.WriteLine($"a: {PC.Registers[0]}, b: {PC.Registers[1]}, PC: {PC.IPointer}");
    }

    public record struct Computer
    {
        public uint[] Registers { get; set; }

        public Instruction[] Instructions { get; set; }
        public int IPointer { get; set; }

        public Computer(Instruction[] instructions) 
        {
            Registers = new uint[2];
            IPointer = 0;
            Instructions = instructions;
        }

        public bool Execute()
        {
            if (IPointer < 0 || IPointer >= Instructions.Length) return true;
            var i= Instructions[IPointer];

            switch (i.Op)
            {
                case OpCode.hlf:
                    ExecuteHalf(i.Register);
                    break;
                case OpCode.tpl:
                    ExecuteTriple(i.Register);
                    break;
                case OpCode.inc:
                    ExecuteIncrement(i.Register);
                    break;
                case OpCode.jmp:
                    ExecuteJump(i.Offset);
                    break;
                case OpCode.jie:
                    ExecuteJumpEven(i.Register, i.Offset);
                    break;
                case OpCode.jio:
                    ExecuteJumpOne(i.Register, i.Offset);
                    break;
            };

            return false;
        }

        private void ExecuteHalf(int register)
        {
            Registers[register] /= 2;
            IPointer++;
        }

        private void ExecuteTriple(int register)
        {
            Registers[register] *= 3;
            IPointer++;
        }

        private void ExecuteIncrement(int register)
        {
            Registers[register] += 1;
            IPointer++;
        }

        private void ExecuteJump(int offset)
        {
            IPointer += offset;
        }

        private void ExecuteJumpEven(int register, int offset)
        {
            if (Registers[register] % 2 == 0)
                IPointer += offset;
            else
                IPointer += 1;
        }

        private void ExecuteJumpOne(int register, int offset)
        {
            if (Registers[register] == 1)
                IPointer += offset;
            else
                IPointer += 1;
        }
    }

    public record struct Instruction
    {
        public OpCode Op { get; set; }
        public int Register { get; set; }
        public int Offset { get; set; }

        public Instruction(OpCode opCode, int register, int offset) { Op = opCode; Register = register; Offset = offset; }

        public Instruction(string init)
        {
            var parts = init.Split(' ');
            Op = (OpCode)Enum.Parse(typeof(OpCode), parts[0]);
            if (HasRegisterAndOffset(Op))
            {
                Register = parts[1][0] == 'a' ? 0 : 1;
                Offset = int.Parse(parts[2]);
            }
            else if (HasRegister(Op))
            {
                Register = parts[1][0] == 'a' ? 0 : 1;
                Offset = -1;
            }
            else if (HasOffset(Op))
            {
                Offset = int.Parse(parts[1]);
                Register = -1;
            } else { throw new ArgumentException(); }
        }

        public override string ToString() => $"{Op}: {RegisterString}: {Offset}";

        public string RegisterString => Register switch
        {
            0 => "a",
            1 => "b",
            _ => "",
        };
    }

    public static bool HasRegister(OpCode op) => op is OpCode.hlf or OpCode.tpl or OpCode.inc or OpCode.jie or OpCode.jio;
    public static bool HasOffset(OpCode op) => op is OpCode.jmp or OpCode.jie or OpCode.jio;
    public static bool HasRegisterAndOffset(OpCode op) => HasRegister(op) && HasOffset(op);

    public enum OpCode { hlf, tpl, inc, jmp, jie, jio }
}
