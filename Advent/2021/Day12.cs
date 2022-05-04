using Advent;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day12 : Day
    {
        public override string DayName => "12";
        public override string Answer1 => "4885";
        public override string Answer2 => "117095";

        private Graph graph { get; set; }

        public Day12() : base ("2021/input12")
        {
                /*
                var input = @"start-A
start - b
A - c
A - b
b - d
A - end
b - end";
                */

            var lines = Input.Split('\n').Select(e => e.Trim());
            graph = new Graph(lines);
            //graph.Draw();
        }

        public override void Solve()
        {

            var paths1 = graph.BuildPaths(false);
            var paths2 = graph.BuildPaths(true);
            Result1 = paths1.Count.ToString();
            Result2 = paths2.Count.ToString();
        }
    }

    public class Graph
    {
        public HashSet<Link> Links { get; set;}
        public HashSet<Cave> Caves { get; set;}

        public Graph(IEnumerable<string> init)
        {
            Links = new HashSet<Link>();
            Caves = new HashSet<Cave>();

            foreach(var line in init)
            {
                var parts = line.Split('-');
                var a = new Cave(parts[0].Trim());
                var b = new Cave(parts[1].Trim());
                Caves.Add(a);
                Caves.Add(b);
                Links.Add(new Link(a, b));
                Links.Add(new Link(b, a));
            }
        }

        public List<Path> BuildPaths(bool revisitAllowed)
        {
            var paths = new List<Path>();

            // All Paths start from start
            var start = Caves.First(c => c.Name == "start");

            // Each link out of start is the first step on a path
            foreach (var link in Links.Where(l => l.A == start))
            {
                var path = new Path(start);
                path.Add(link.B);
                paths.Add(path);
            }

            // For each of those paths, look for all paths that continue on from it
            var additions = new List<Path>();
            foreach (var path in paths)
            {
                additions.AddRange(BuildPaths(path, revisitAllowed));
            }
            paths.AddRange(additions);

            // Trim the paths that never got to the end
            paths.RemoveAll(p => !p.Ends);

            return paths;
        }

        public List<Path> BuildPaths(Path start, bool revisitAllowed)
        {
            var paths = new List<Path>();
            // No Paths can go to end and leave again
            if (start.Ends) return paths;

            // Look at each link that starts with the from the current last point
            foreach (var link in Links.Where(l => l.A == start.Last))
            {
                var path = new Path(start);
                // Can never return to start
                if (link.B.IsStart) continue;

                // Cannot return to small caves more than the allowed amount
                var revisitAllowedNext = revisitAllowed;
                if (!link.B.Big)
                {
                    var revisiting = path.Contains(link.B);

                    if (revisiting)
                    {
                        if (!revisitAllowed) continue;
                        revisitAllowedNext = false;
                    }
                }

                // Add the new path
                path.Add(link.B);
                paths.Add(path);

                // Look at all the paths that continue from this as well
                paths.AddRange(BuildPaths(path, revisitAllowedNext));
            }

            return paths;
        }

        public void Draw()
        {
            foreach (var link in Links)
            {
                Console.WriteLine(link);
            }
        }
    }

    public struct Link
    {
        public Cave A { get; set; }
        public Cave B { get; set; }

        public Link(Cave a, Cave b)
        {
            A = a; 
            B = b; 
        }

        public override string ToString()
        {
            return $"{A} -- {B}";
        }
    }

    public record struct Cave
    {
        public string Name { get; set; }
        public bool Big { get; set; }

        public Cave(string init) : this(init, init.ToUpper() == init) { }

        public Cave(string name, bool big)
        {
            Name = name;
            Big = big;
        }

        public bool IsStart => Name == "start";
        public bool IsEnd => Name == "end";

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public struct Path
    {
        public List<Cave> Caves { get; set; }

        public Path(Cave start)
        {
            Caves = new List<Cave> { start };
        }

        public Path(Path copyFrom)
        {
            Caves = new List<Cave>(copyFrom.Caves);
        }

        public void Add(Cave cave)
        {
            Caves.Add(cave);
        }

        public override string ToString()
        {
            var str = "";
            foreach (var cave in Caves)
            {
                str += $"{cave},";
            }
            return str[..^1];
        }

        public Cave Last => Caves.Last();
        public bool Ends => Last.IsEnd;

        public bool Contains(Cave c) => Caves.Contains(c);

        public bool Equals(Path p)
        {
            if(p.Caves.Count != Caves.Count) return false;
            for(int i = 0; i < Caves.Count; i++) {
                if (p.Caves[i] != Caves[i]) return false;
            }
            return true;
        }
    }
}
