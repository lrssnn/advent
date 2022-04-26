using FluentAssertions;

namespace Advent
{
    public abstract class Day
    {
        public abstract string DayName { get; }
        public abstract string Answer1 { get; }
        public abstract string Answer2 { get; }

        public string Result1 { get; set; }
        public string Result2 { get; set; }

        public string Input { get; set; }

        public abstract void Solve();

        public Day(string inputFileName)
        {
            using StreamReader sr = File.OpenText(inputFileName);
            Input = sr.ReadToEnd();
        }
        
        public bool IsValid()
        {
            if (Result1 != Answer1) return false;
            if (Result2 != Answer2) return false;
            return true;
        }
    }
}
