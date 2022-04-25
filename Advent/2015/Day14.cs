using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day14
{
    public List<Reindeer> Reindeers { get; set; }

    public Day14()
    {
        Reindeers = new List<Reindeer>();
        Reindeers.Add(new Reindeer(14, 10, 127));
        Reindeers.Add(new Reindeer(16, 11, 162));
    }

    public void Solve()
    {
        for (int i = 1; i < 2503 + 1; i++)
        {
            foreach (var r in Reindeers) r.Simulate();
            foreach(var r in GetLeaders(Reindeers)) r.Score++;
            if (i == 1) foreach (var r in Reindeers) Console.WriteLine(r);
            if (i == 10) foreach (var r in Reindeers) Console.WriteLine(r);
            if (i == 1000) foreach (var r in Reindeers) Console.WriteLine(r);
        }

        foreach (var r in Reindeers) Console.WriteLine(r);
    }

    public List<Reindeer> GetLeaders(List<Reindeer> reindeers)
    {
        var ordered = reindeers.OrderByDescending(r => r.Location);
        var first = ordered.First().Location;
        return ordered.Where(e => e.Location == first).ToList();
    }


    public class Reindeer
    {
        public int Speed { get; set; } // Km/s
        public bool Flying { get; set; }
        public int TimeLeft { get; set; }
        public int Location { get; set; }
        public int Score { get; set; }

        public int FlyTimeReset { get; set; }
        public int RestTimeReset { get; set; }

        public Reindeer(int speed, int flyTimeReset, int restTimeReset)
        {
            Speed = speed;
            FlyTimeReset = flyTimeReset;
            RestTimeReset = restTimeReset;

            Flying = true;
            TimeLeft = FlyTimeReset;
            Score = 0;
        }

        public void Simulate()
        {
            TimeLeft--;
            if (Flying) Location += Speed;
            if (TimeLeft == 0) ChangeState();
        }

        private void ChangeState()
        {
            Flying = !Flying;
            if (Flying)
                TimeLeft = FlyTimeReset;
            else
                TimeLeft = RestTimeReset;
        }

        public string FlyingString => Flying ? "Flying" : "Resting";

        public override string ToString()
        {
            return $"{Location} ({FlyingString}) - {Score} points";
        }
    }
}
