using Advent;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day21 : Day
    {

        public override string DayName => "21";
        public override string Answer1 => "605070";
        public override string Answer2 => "218433063958910";

        public static readonly Dictionary<int, int> ScoreWeights = new()
        {
            {3, 1}, {4, 3}, {5, 6}, {6, 7},
            {7, 6}, {8, 3}, {9, 1},
        };

        public static readonly Dictionary<(int hashCode, bool p1Turn), (long p1, long p2)> Cache = new();
        public static long CacheHits = 0;
        public static long CacheMiss = 0;

        public Ring Ring { get; set; }
        public GameStateNode Root { get; set; }

        public Day21() : base("2021/input21")
        {
                /*
                var input = @"Player 1 starting position: 4
                              Player 2 starting position: 8";
                */

            var lines = Input.Split("\n").Select(line => line.Trim()).ToList();

            int p1 = int.Parse(lines[0][^1].ToString());
            int p2 = int.Parse(lines[1][^1].ToString());
            Ring = new Ring(p1, p2, new DDice());
            Root = new GameStateNode(0, 0, p1, p2);
        }

        public override void Solve()
        {
            //Console.WriteLine(Ring.P1.Location);
            //Console.WriteLine(Ring.P2.Location);
            //Console.WriteLine(Ring.PlayGame(1000));
            Result1 = Ring.PlayGame(1000).ToString();

            //Console.WriteLine(CountWinners(Root, true));
            //Console.WriteLine((double)CacheHits / (double)(CacheHits + CacheMiss));
            Result2 = CountWinners(Root, true).p1.ToString();
        }

        public static (long p1, long p2) CountWinners(GameStateNode node, bool p1Turn)
        {
            // Base case
            if (node.HasWinner) return (node.P1Winner ? 1 : 0, node.P2Winner ? 1 : 0);

            // Look in the cache
            if (Cache.ContainsKey((node.GetHashCode(), p1Turn)))
            {
                CacheHits++;
                return Cache[(node.GetHashCode(), p1Turn)];
            }
            CacheMiss++;


            // Add up the scores for each possible outcome. ScoreWeights contains the number of 
            // different ways to get each score, we multiply by that to account for the more likely universes
            (long p1, long p2) scores = (0, 0);
            foreach(var pair in ScoreWeights)
            {
                var child = new GameStateNode(node, pair.Key, p1Turn);
                var childScores = CountWinners(child, !p1Turn);
                scores = (scores.p1 + (pair.Value * childScores.p1), scores.p2 + (pair.Value * childScores.p2));
            }

            // Populate Cache
            Cache[(node.GetHashCode(), p1Turn)] = scores;

            return scores;
        }
    }

    public class GameStateNode
    {
        const int TARGET_SCORE = 21;

        public int P1Score { get; set; }
        public int P2Score { get; set; }
        public int P1Loc { get; set; }
        public int P2Loc { get; set; }

        public List<GameStateNode> Children { get; set; }

        public GameStateNode(int p1s, int p2s, int p1l, int p2l)
        {
            P1Score = p1s;
            P2Score = p2s;
            P1Loc = p1l;
            P2Loc = p2l;

            Children = new List<GameStateNode>();
        }

        public GameStateNode(GameStateNode from, int roll, bool p1Turn)
        {
            if (p1Turn)
            {
                P1Loc = from.P1Loc + roll;
                P1Loc %= 10;
                if (P1Loc == 0) P1Loc = 10;
                P1Score = from.P1Score + P1Loc;

                P2Loc = from.P2Loc;
                P2Score = from.P2Score;
            }
            else
            {
                P1Loc = from.P1Loc;
                P1Score = from.P1Score;

                P2Loc = from.P2Loc + roll;
                P2Loc %= 10;
                if (P2Loc == 0) P2Loc = 10;
                P2Score = from.P2Score + P2Loc;
            }
            Children = new List<GameStateNode>();
        }

        public bool HasWinner => P1Score >= TARGET_SCORE || P2Score >= TARGET_SCORE;
        public bool P1Winner => P1Score >= TARGET_SCORE;
        public bool P2Winner => P2Score >= TARGET_SCORE;

        public override bool Equals(object? obj)
        {
            return obj is GameStateNode node &&
                   P1Score == node.P1Score &&
                   P2Score == node.P2Score &&
                   P1Loc == node.P1Loc &&
                   P2Loc == node.P2Loc;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(P1Score, P2Score, P1Loc, P2Loc);
        }
    }

    public class Ring
    {
        public Player P1 { get; set; }
        public Player P2 { get; set; }

        public DDice Die { get; set; }

        public Ring(int p1, int p2, DDice die)
        {
            P1 = new Player(p1);
            P2 = new Player(p2);

            Die = die;
        }

        public int PlayGame(int targetScore)
        {
            while(true)
            {
                P1.Location += Die.Roll() + Die.Roll() + Die.Roll();
                P1.Score += P1.Location;
                if (P1.Score >= targetScore) break;

                P2.Location += Die.Roll() + Die.Roll() + Die.Roll();
                P2.Score += P2.Location;
                if (P2.Score >= targetScore) break;
            }

            return (Math.Min(P1.Score, P2.Score)) * Die.Rolls;
        }
    }


    public class DDice
    {

        public int Next { get; set; }
        public int Rolls { get; set; }

        public DDice()
        {
            Next = 1;
            Rolls = 0;
        }

        public int Roll()
        {
            Rolls++;
            if (Next == 101) 
                Next = 1;
            return Next++; 
        }

        public override string ToString()
        {
            return $"{Next}: {Rolls}";
        }
    }

    public class Player
    {
        private int _location;
        public int Location 
        { 
            get => _location; 
            set 
            { 
                _location = value; 
                _location %= 10; 
                if(_location == 0) _location = 10;
            } 
        }
        public int Score { get; set; }

        public Player(int startingLocation)
        {
            _location = startingLocation;
            Score = 0;
        }

        public override string ToString()
        {
            return $"{Location}: {Score}";
        }
    }
}
