using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day9
{

    HashSet<Node> Nodes { get; set; }
    HashSet<Edge> Edges { get; set; }

    public Day9()
    {
        using (StreamReader sr = File.OpenText("2015/input9"))
        {
            var input = sr.ReadToEnd().Trim();

            /*
            var input = @"London to Dublin = 464
                        London to Belfast = 518
                        Dublin to Belfast = 141";
            */

            (Nodes, Edges) = BuildGraph(input.Split('\n').Select(l => l.Trim()).ToList());
        }
    }

    public (HashSet<Node> Nodes, HashSet<Edge> Edges) BuildGraph(List<string> init)
    {
        HashSet<Node> nodes = new HashSet<Node>();
        HashSet<Edge> edges = new HashSet<Edge>();

        foreach (var line in init)
        {
            var parts = line.Split(" = ");
            var ends = parts[0].Split(" to ");

            var from = new Node(ends[0]);
            var to = new Node(ends[1]);
            var distance = int.Parse(parts[1]);

            nodes.Add(from);
            nodes.Add(to);
            edges.Add(new Edge(from, to, distance));
        }

        return (nodes, edges);
    }

    public void Solve()
    {
        Console.WriteLine("Nodes:");
        foreach (var node in Nodes)
            Console.WriteLine($"  {node}");

        Console.WriteLine("Edges:");
        foreach (var edge in Edges)
            Console.WriteLine($"  {edge}");

        var routes = BuildRoutes(Nodes, Edges);
        /*
        Console.WriteLine("Routes:");
        foreach(var route in routes)
        {
            foreach(var node in route.Nodes)
            {
                Console.Write($"{node}, ");
            }
            Console.WriteLine($"{route.TotalDistance}");
        }
        */
        Console.WriteLine($"Min Distance: {routes.Min(e => e.TotalDistance)}");
        Console.WriteLine($"Max Distance: {routes.Max(e => e.TotalDistance)}");
    }

    public List<Route> BuildRoutes(HashSet<Node> nodes, HashSet<Edge> edges)
    {
        // Call The recursive buildRoute starting from each node
        var routes = new List<Route>();
        foreach (var node in Nodes)
        {
            var start = new Route();
            start.Nodes.Add(node);
            var newNodes = nodes.ToHashSet();
            newNodes.Remove(node);
            routes.AddRange(BuildRoutes(newNodes, edges, start));
        }
        return routes;
    }

    public List<Route> BuildRoutes(HashSet<Node> nodes, HashSet<Edge> edges, Route route)
    {
        if (!nodes.Any())
        {
            // Complete route
            return new List<Route> { route };
        }

        var routes = new List<Route>();

        var lastNode = route.Nodes.Last();

        foreach(var node in nodes)
        {
            Edge edgeToNode;
            try
            {
                edgeToNode = edges.First(e => (e.From == lastNode && e.To == node) || (e.From == node && e.To == lastNode));
            }
            catch (Exception ex)
            {
                // There is no path to this node
                continue;
            }

            var newRoute = route.With(node, edgeToNode.Distance);

            // Maybe remove the node and edge from the collections for performance?
            var newNodes = nodes.ToHashSet();
            newNodes.Remove(node);
            var newEdges = edges.ToHashSet();
            newEdges.Remove(edgeToNode);
            routes.AddRange(BuildRoutes(newNodes, newEdges, newRoute));
        }
        return routes;
    }

    public record struct Node
    {
        public string Name;

        public Node(string name) { Name = name; }

        public override string ToString() { return Name; }
    }

    public record struct Edge
    {
        public Node From;
        public Node To;
        public int Distance;

        public Edge(Node from, Node to, int distance) { From = from; To = to; Distance = distance; }

        public override string ToString() { return $"{From} to {To}: {Distance}"; }
    }

    public record struct Route
    {
        public List<Node> Nodes { get; set; }
        public int TotalDistance { get; set; }

        public Route()
        {
            Nodes = new List<Node>();
            TotalDistance = 0;
        }

        public Route With(Node node, int distance)
        {
            var route = new Route();
            route.Nodes = Nodes.ToList();
            route.Nodes.Add(node);
            route.TotalDistance = TotalDistance + distance;
            return route;
        }
    }
}
