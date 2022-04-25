using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventTwentyOne
{
    public class Day15
    {

        List<Node> Nodes = new List<Node>();

        public Day15()
        {
            using (StreamReader sr = File.OpenText("input15"))
            {

                var input = sr.ReadToEnd().Trim();
                /*
                var input = @"1163751742
                              1381373672
                              2136511328
                              3694931569
                              7463417111
                              1319128137
                              1359912421
                              3125421639
                              1293138521
                              2311944581";
                */

                var lines = input.Split('\n').Select(e => e.Trim());

                Nodes = BuildNodes(lines.ToList());
            }
        }

        public List<Node> BuildNodes(List<string> input)
        {
            Node.MaxX = input[0].Length;
            Node.MaxY = input.Count;

            var output = new List<Node>();
            foreach (var y in Enumerable.Range(0, Node.MaxY))
            {
                var line = input[y];
                foreach (var x in Enumerable.Range(0, Node.MaxX))
                {
                    var node = new Node(x, y, int.Parse(line[x].ToString()));
                    output.Add(node);
                }
            }
            return output;
        }

        public void Solve()
        {
            var smallCost = GetPathCost(Nodes, Nodes.First(), Nodes.Last());
            Console.WriteLine($"{smallCost}");

            var bigNodes = Embiggen(Nodes);
            var bigCost = GetPathCost(bigNodes, bigNodes.First(), bigNodes.Last());
            Console.WriteLine(bigCost);
        }

        public int GetPathCost(List<Node> nodes, Node source, Node target)
        {
            // Dijkstra
            // dist[n] is the distance from source to Node n via the path we have currently found
            var dist = new List<int>();
            // prev[n] is the next step backwards on the path from Node n
            var prev = new List<Node>();
            // q is all the nodes we haven't checked yet
            var q = new HashSet<Node>();

            // UGLY - No null in record struct
            var NODE_UNDEFINED = new Node(-100, -100, -100);

            // Preload the list with default values
            foreach(Node n in nodes)
            {
                dist.Add(int.MaxValue);
                prev.Add(NODE_UNDEFINED);
                q.Add(n);
            }

            // The distance from source to source is always 0
            dist[source.Index] = 0;

            // Check each node
            while (q.Count > 0)
            {
                var nodesLeft = q.Count;
                if (nodesLeft % 100 == 0) Console.WriteLine($"{nodesLeft} nodes left");
                // The current end of the shortest path we know about
                var u = GetMinDist(q, dist);

                q.Remove(u);

                // We have the shortest path from source to target populated in prev.
                // We could continue to populate all the shortest paths to all nodes
                if (u.Index == target.Index)
                    break;

                foreach(Node n in NeighboursOf(nodes, u).Where(x => q.Contains(x)))
                {
                    // alt is the distance to of the current path plus the neighbour we are looking at
                    var alt = dist[u.Index] + n.Risk;
                    // If that's new new shortest way we know of to get to that neighbour, replace whatever current
                    // path we have to that neighbour with this one
                    if(alt < dist[n.Index])
                    {
                        dist[n.Index] = alt;
                        prev[n.Index] = u;
                    }
                }
            }

            // Build the path by looking back through prev from target
            var path = new List<Node>();    
            if(prev[target.Index] != NODE_UNDEFINED)
            {
                while (target != NODE_UNDEFINED)
                {
                    path.Add(target);
                    target = prev[target.Index];
                }
            }

            // print the path
            foreach(Node n in path)
            {
                Console.WriteLine($"({n.X},{n.Y}): {n.Risk}");
            }

            // - Source.Risk as the first node isn't counted
            return path.Sum(n => n.Risk) - source.Risk;
        }

        public List<Node> NeighboursOf(List<Node> nodes, Node n)
        {
            var result = new List<Node>();
            // Up
            if(n.Y > 0) result.Add(nodes[n.IndexUp]);
            // Down
            if(n.Y < Node.MaxY - 1) result.Add(nodes[n.IndexDown]);
            // Left
            if(n.X > 0) result.Add(nodes[n.IndexLeft]);
            // Right
            if(n.X < Node.MaxX - 1) result.Add(nodes[n.IndexRight]);
            return result;
        }

        public Node GetMinDist(HashSet<Node> nodes, List<int> distances)
        {
            return nodes.OrderBy(n => distances[n.Index]).First();
        }

        public List<Node> Embiggen(List<Node> input)
        {
            var oldMaxX = Node.MaxX;
            var oldMaxY = Node.MaxY;
            Node.MaxX *= 5;
            Node.MaxY *= 5;
            // Copy the list
            List<Node> bigNodes = new List<Node>();
            // For each tile, make an appropriate copy of the node
            foreach(int yTile in Enumerable.Range(0, 5))
            {
                foreach(int xTile in Enumerable.Range(0, 5))
                {
                    foreach(Node n in input)
                    {
                        var x = n.X + (xTile * oldMaxX);
                        var y = n.Y + (yTile * oldMaxY);
                        var risk = n.Risk + xTile + yTile;
                        if (risk >= 10) risk -= 9;
                        bigNodes.Add(new Node(x, y, risk));
                    }
                }
            }
            // Our list will be all jumbled, need to sort it so bigNodes[n.Index] = n
            return bigNodes.OrderBy(n => n.Index).ToList();
        }
    }

    public record struct Node
    {
        public static int MaxX = 0;
        public static int MaxY = 0;

        public int X { get; set; }
        public int Y { get; set; }
        public int Risk { get; set; }
        public int Index { get; set; }

        public Node (int x, int y, int risk)
        {
            X = x;
            Y = y;
            Risk = risk;
            Index = IndexOf(X, Y);
        }

        public static int IndexOf(int x, int y)
        {
            return (MaxX * y) + x;
        }

        public override string ToString()
        {
            return $"({X},{Y}): {Risk}";
        }

        public int IndexUp => IndexOf(X, Y - 1);
        public int IndexDown => IndexOf(X, Y + 1);
        public int IndexLeft => IndexOf(X - 1, Y);
        public int IndexRight => IndexOf(X + 1, Y);
    }
}
