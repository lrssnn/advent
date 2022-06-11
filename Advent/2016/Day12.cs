using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day12
{
    public List<Instruction> Input { get; set; }

    public Day12()
    {
        using (StreamReader sr = File.OpenText("2016/input12"))
        {
            var input = sr.ReadToEnd().Trim();
            /*
            var input = @"cpy 41 a
                            inc a
                            inc a
                            dec a
                            jnz a 2
                            dec a";
            */
            Input = input.Split('\n').Select(s => s.Trim()).Select(s => new Instruction(s)).ToList();
        }
    }

    public void Solve()
    {
        var computer = new Computer(Input, 0);
        computer.Process();

        var part2 = new Computer(Input, 1);
        part2.Process();
    }


    public class Computer
    {
        public int[] Registers { get; set; }

        public List<Instruction> Program { get; set; }

        public int PC { get; set; }
        public bool Halted { get; set; }

        public Computer(List<Instruction> program, int cValue)
        {
            Registers = new int[4];
            Registers[2] = cValue;
            PC = 0;
            Program = program;
            Halted = false;
        }

        public int Process()
        {
            var step = 0;
            while (!Halted)
            {
                Tick();
                step++;
            }
            Console.WriteLine($"{step} steps");
            Console.WriteLine(this);
            return Registers[0];
        }

        public void Tick()
        {
            var instruction = Program[PC];
            switch (instruction.Type)
            {
                case InstructionType.Cpy:
                    ProcessCpy(instruction);
                    break;
                case InstructionType.Inc:
                    ProcessInc(instruction);
                    break;
                case InstructionType.Dec:
                    ProcessDec(instruction);
                    break;
                case InstructionType.Jnz:
                    ProcessJnz(instruction);
                    break;
            }
            if (PC >= Program.Count)
                Halted = true;
        }

        private void ProcessCpy(Instruction instruction)
        {
            var value = instruction.SourceRegister.HasValue ? Registers[instruction.SourceRegister.Value] : instruction.SourceValue!.Value;
            Registers[instruction.DestRegister!.Value] = value;
            PC++;
        }

        private void ProcessInc(Instruction instruction)
        {
            Registers[instruction.DestRegister!.Value] += 1;
            PC++;
        }

        private void ProcessDec(Instruction instruction)
        {
            Registers[instruction.DestRegister!.Value] -= 1;
            PC++;
        }

        private void ProcessJnz(Instruction instruction)
        {
            var value = instruction.SourceRegister.HasValue ? Registers[instruction.SourceRegister.Value] : instruction.SourceValue!.Value;
            if (value != 0)
                PC += instruction.Offset!.Value;
            else
                PC++;
        }

        public override string ToString()
        {
            var result = new StringBuilder($"PC: {PC}: Registers [ ");
            foreach(var reg in Registers)
            {
                result.Append(reg.ToString());
                result.Append(' ');
            }
            result.Append(']');
            return result.ToString();
        }
    }

    public class Instruction
    {
        public InstructionType Type { get; set; }
        public int? SourceRegister { get; set; }
        public int? SourceValue { get; set; }
        public int? DestRegister { get; set; }
        public int? Offset { get; set; }

        public Instruction(string s)
        {
            var parts = s.Split(' ');
            Type = GetInstructionType(parts[0]);

            switch (Type)
            {
                case InstructionType.Cpy:
                    bool isValueSource = int.TryParse(parts[1], out var sourceValue);
                    if (isValueSource)
                        SourceValue = sourceValue;
                    else
                        SourceRegister = RegisterIndex(parts[1]);
                    DestRegister = RegisterIndex(parts[2]);
                    break;

                case InstructionType.Inc:
                case InstructionType.Dec:
                    DestRegister = RegisterIndex(parts[1]);
                    break;

                case InstructionType.Jnz:
                    isValueSource = int.TryParse(parts[1], out sourceValue);
                    if (isValueSource)
                        SourceValue = sourceValue;
                    else
                        SourceRegister = RegisterIndex(parts[1]);
                    Offset = int.Parse(parts[2]);
                    break;
            }
        }

        public override string ToString() =>
            Type switch
            {
                InstructionType.Cpy => $"cpy {SourceLabel} {DestLabel}",
                InstructionType.Inc => $"inc {DestLabel}",
                InstructionType.Dec => $"dec {DestLabel}",
                InstructionType.Jnz => $"jnz {SourceLabel} {Offset}",
                _ => throw new Exception("Unexpected input"),
            };


        private string SourceLabel => SourceRegister.HasValue ? RegisterLabel(SourceRegister.Value) : SourceValue.ToString() ?? "";
        private string DestLabel => RegisterLabel(DestRegister);

        private static string RegisterLabel(int? i) => i switch { 0 => "a" , 1 => "b", 2 => "c", 3 => "d", _ => ""};
        private static int RegisterIndex(string s) => s switch { "a" => 0, "b" => 1, "c" => 2, "d" => 3, _ => -1 };

        private static InstructionType GetInstructionType(string s) =>
            s switch
            {
                "cpy" => InstructionType.Cpy,
                "inc" => InstructionType.Inc,
                "dec" => InstructionType.Dec,
                "jnz" => InstructionType.Jnz,
                _ => throw new Exception("Unexpected input"),
            };
    }

    public enum InstructionType { Cpy, Inc, Dec, Jnz }

}
