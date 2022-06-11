using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventSixteen;

public class Day4
{
    public List<RoomCode> RoomCodes { get; set; }

    public Day4()
    {
        using (StreamReader sr = File.OpenText("2016/input4"))
        {
            var input = sr.ReadToEnd().Trim();
            /*
            var input = @"aaaaa-bbb-z-y-x-123[abxyz]
                            a-b-c-d-e-f-g-h-987[abcde]
                            not-a-real-room-404[oarel]
                            totally-real-room-200[decoy]";
            */
            RoomCodes = input.Split('\n').Select(s => s.Trim()).Select(s => new RoomCode(s)).ToList();
        }
    }

    public void Solve()
    {
        var validRooms = RoomCodes.Where(r => r.RealRoom());
        Console.WriteLine($"{validRooms.Count()} valid rooms");
        Console.WriteLine($"Sum of ids: {validRooms.Sum(r => r.SectorId)}");

        foreach(var room in validRooms)
        {
            var decrypted = room.Decrypt();
            if (decrypted.Contains("north"))
            {
                Console.WriteLine(decrypted);
                Console.WriteLine(room);
            }
        }
    }

    public class RoomCode
    {
        public string Name { get; set; }
        public int SectorId { get; set; }
        public List<char> Checksum { get; set; }
        public Dictionary<char, int>? Frequencies { get; set; }

        public RoomCode(string s)
        {
            var parts = s.Split('-');
            Name = "";
            foreach (var part in parts)
            {
                if (char.IsLetter(part[0]))
                {
                    Name += part + '-';
                } else
                {
                    // The last group
                    var subParts = part.Split('[');
                    SectorId = int.Parse(subParts[0]);
                    Checksum = subParts[1][..^1].ToCharArray().ToList();
                }
            }
            if(Checksum == null) throw new Exception("Bad RoomCode");
            Name = Name[..^1]; // We will have added an additional trailing dash
        }

        public bool RealRoom() 
        {
            var calculated = CalculateChecksum();
            for(int i = 0; i < Checksum.Count; i++)
            {
                if (Checksum[i] != calculated[i]) return false;
            }
            return true;
        }

        public List<char> CalculateChecksum()
        {
            var chars = Name.ToCharArray();
            var groups = chars.GroupBy(c => c).Where(group => group.First() != '-');
            // Now group by count so we can tiebreak intelligently
            var countGroups = groups.GroupBy(g => g.Count());
            var sorted = countGroups.OrderByDescending(group => group.Key);
            var frequencies = new List<char>();
            foreach(var count in sorted)
            {
                var sortedLetters = count.OrderBy(c => c.Key);
                foreach (var letter in sortedLetters) frequencies.Add(letter.Key);
            }
            return frequencies.Take(5).ToList();
        }
            
        public string Decrypt()
        {
            var offset = SectorId % 26;
            var result = "";
            foreach(char c in Name)
            {
                if (c == '-')
                {
                    result += ' ';
                } 
                else
                {
                    var charNum = (int)c;
                    charNum += offset;
                    if (charNum > (int)'z') charNum -= 26;
                    result += (char)charNum;
                }
            }
            return result;
        }

        public override string ToString()
        {
            return $"Name: {Name} | ID: {SectorId} | Checksum {new string(Checksum.ToArray())} | Calc: {new string(CalculateChecksum().ToArray())} | Valid: {RealRoom()}";
        }
    }
}

