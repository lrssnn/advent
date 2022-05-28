using Advent;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day16 : Day
    {

        public override string DayName => "16";
        public override string Answer1 => "889";
        public override string Answer2 => "739303923668";


        public string Binary { get; set; }

        public Day16() : base("2021/input16")
        {
                //var input = @"D2FE28"; // Literal 2021 version 6
                //var input = @"38006F45291200"; // Operator Length Type
                //var input = @"EE00D40C823060"; // Operator count Type
                //var input = @"8A004A801A8002F478"; // Score 16

                
                //var input = @"C200B40A82"; // Evaluate 3

            Binary = ToBinary(Input);
        }

        public string ToBinary(string hex)
        {
            // Need to do this manually, converting numerically was losing leading zeroes
            // Do not look at me. Collapse the function. Do not look at me
            var binary = "";
            foreach(char c in hex)
            {
                binary += c switch
                {
                    '0' => "0000",
                    '1' => "0001",
                    '2' => "0010",
                    '3' => "0011",
                    '4' => "0100",
                    '5' => "0101",
                    '6' => "0110",
                    '7' => "0111",
                    '8' => "1000",
                    '9' => "1001",
                    'A' => "1010",
                    'B' => "1011",
                    'C' => "1100",
                    'D' => "1101",
                    'E' => "1110",
                    'F' => "1111",
                };
            }
            return binary;
        }

        public override void Solve()
        {
            //Console.WriteLine(Binary);
            (var packet, _) = Packet.ParsePacket(Binary);
            Result1 = packet.Score.ToString();
            Result2 = packet.Evaluate().ToString();
        }
    }

    public class Packet
    {
        public int Version { get; set; }
        public PacketType Type { get; set; }

        public long Value { get; set; }
        
        public List<Packet> SubPackets { get; set; }
        
        public Packet(int version, PacketType type, long value, List<Packet> subPackets)
        {
            Version = version;
            Type = type;
            Value = value;
            SubPackets = subPackets;
        }

        public int Score => Version + SubPackets.Sum(p => p.Score);

        public long Evaluate()
        {
            return Type switch 
            { 
                PacketType.Literal => Value,
                PacketType.Sum => SubPackets.Sum(p => p.Evaluate()),
                PacketType.Product => SubPackets.Aggregate((long)1, (product, p) => product * p.Evaluate()),
                PacketType.Min => SubPackets.Min(p => p.Evaluate()),
                PacketType.Max => SubPackets.Max(p => p.Evaluate()),
                PacketType.Greater => SubPackets[0].Evaluate() > SubPackets[1].Evaluate() ? 1 : 0,
                PacketType.Less => SubPackets[0].Evaluate() < SubPackets[1].Evaluate() ? 1 : 0,
                PacketType.Equal => SubPackets[0].Evaluate() == SubPackets[1].Evaluate() ? 1 : 0,
            };
        }

        public static (Packet p, string remaining) ParsePacket(string binary)
        {
            var version = Convert.ToInt32(binary[0..3], 2);

            var typeInt = Convert.ToInt32(binary[3..6], 2);
            var type = (PacketType)typeInt;

            long value = -1;
            var subPackets = new List<Packet>();
            var remaining = "";
            if (type == PacketType.Literal)
                (value, remaining) = ReadBinaryAsLiteral(binary[6..]);
            else
                (subPackets, remaining) = ReadBinaryAsSubPackets(binary[6..]);

            var p = new Packet(version, type, value, subPackets);
            return (p, remaining);
        }

        public static (long value, string remaining) ReadBinaryAsLiteral(string binary)
        {
            var valueBinary = "";
            var usedBits = 0;
            foreach(var group in binary.Chunk(5))
            {
                usedBits += group.Length;
                if (group.Length != 5) continue; // Should be the last group of all 0s, if any
                // Lop off the front bit and add the rest to the value
                valueBinary += String.Concat(group[1..5]);
                if (group[0] == '0') break; // Last group
            }

            return (Convert.ToInt64(valueBinary, 2), binary[usedBits..]);
        }

        public static (List<Packet> ps, string remaining) ReadBinaryAsSubPackets(string binary)
        {
            var lengthTypeID = binary[0];
            if (lengthTypeID == '0')
                return ReadSubPackets15Bit(binary[1..]);
            else
                return ReadSubPackets11Bit(binary[1..]);
        }

        public static (List<Packet> ps, string remaining) ReadSubPackets15Bit(string binary)
        {
            var length = Convert.ToInt32(binary[0..15], 2);
            var used = 0;
            var ps = new List<Packet>();
            var remaining = binary[15..];
            var originalLength = remaining.Length;
            while(used < length)
            {
                (var p, remaining) = ParsePacket(remaining);
                used = originalLength - remaining.Length;
                ps.Add(p);
            }
            return (ps, remaining);
        }

        public static (List<Packet> ps, string remaining) ReadSubPackets11Bit(string binary)
        {
            var count = Convert.ToInt32(binary[0..11], 2);
            var used = 0;
            var ps = new List<Packet>();
            var remaining = binary[11..];
            while(used < count)
            {
                (var p, remaining) = ParsePacket(remaining);
                used++;
                ps.Add(p);
            }
            return (ps, remaining);
        }
    }



    public enum PacketType
    {
        Sum = 0,
        Product = 1,
        Min = 2,
        Max = 3,
        Greater = 5,
        Less = 6,
        Equal = 7,
        Literal = 4,
    }
}
