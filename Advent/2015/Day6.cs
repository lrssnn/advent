using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day6
{
    public List<Instruction> Instructions { get; set;}

    public List<Rect> OnRects { get; set;}

    public Day6()
    {
        OnRects = new List<Rect>();

        using (StreamReader sr = File.OpenText("2015/input6"))
        {
            var input = sr.ReadToEnd().Trim();
            //var input = @"turn on 0,0 through 999,999
            //              turn off 499,499 through 500,500";
            //var input = @"turn on 0,0 through 100,100
                          //turn on 0,50 through 150,100";
                          //toggle 50,50 through 150,150";

            Instructions = input.Split('\n').Select(line => line.Trim()).Select(l => new Instruction(l)).ToList();
        }
    }

    public void Solve()
    {
        var instructionNum = 0;
        foreach(Instruction i in Instructions)
        {
            Console.WriteLine($"Instruction {instructionNum++}: {i}");
            ProcessInstruction(i);
        }
        Console.WriteLine(OnRects.Sum(r => r.Area));
    }

    public void ProcessInstruction(Instruction i)
    {
        switch (i.Op)
        {
            case Instruction.Operation.On:
                ProcessOn(i);
                break;
            case Instruction.Operation.Off:
                ProcessOff(i);
                break;
            case Instruction.Operation.Toggle:
                ProcessToggle(i);
                break;
        }
    }

    public void ProcessOn(Instruction i)
    {
        var newAreas = new List<Rect>() { i.Area };
        foreach(var onRect in OnRects)
        {
            var newNewAreas = new List<Rect>();
            foreach(var newArea in newAreas)
            {
                if (newArea.CollidesWith(onRect))
                {
                    Console.WriteLine($"{newArea} collides with {onRect}");
                    newNewAreas.AddRange(newArea.Subtract(newArea.CollisionArea(onRect)));
                } else
                {
                    newNewAreas.Add(newArea);
                }
            }
            newAreas = newNewAreas;
        }
        OnRects.AddRange(newAreas);
    }

    public void ProcessOff(Instruction i)
    {
        var newOnRects = new List<Rect>();
        foreach(var onRect in OnRects)
        {
            if (i.Area.CollidesWith(onRect))
            {
                Console.WriteLine($"{i.Area} collides with {onRect}");
                newOnRects.AddRange(onRect.Subtract(onRect.CollisionArea(i.Area)));
            } else
            {
                newOnRects.Add(onRect);
            }
        }
        OnRects = newOnRects;
    }

    public void ProcessToggle(Instruction i)
    {
        // Any collision, remove from the onRects, and also remove from the toggling area
        var newOnRects = new List<Rect>();
        //var toggleAreas = new List<Rect> { i.Area };
        var toggleArea = i.Area;
        var toggledOffAreas = new List<Rect>();
        foreach(var onRect in OnRects)
        {
            if(onRect.CollidesWith(toggleArea))
            {
                Console.WriteLine($"{toggleArea} collides with {onRect}");
                var collisionArea = onRect.CollisionArea(toggleArea);
                newOnRects.AddRange(onRect.Subtract(collisionArea));
                toggledOffAreas.Add(onRect);
            }
            else
            {
                newOnRects.Add(toggleArea);
            }
        }
        OnRects = newOnRects;

        // Now add the remainder areas
        var remainders = new List<Rect> { toggleArea };
        foreach(var toggleOffArea in toggledOffAreas)
        {
        }
        //OnRects.AddRange();
    }

    public class Instruction
    {
        public Operation Op { get; set; }
        public Rect Area { get; set; }

        public Instruction(string init)
        {
            var parts = init.Split(' ');
            if(parts[0] == "toggle")
            {
                Op = Operation.Toggle;
                parts = parts[1..];
            } else
            {
                Op = parts[1] switch
                {
                    "on" => Operation.On,
                    "off" => Operation.Off,
                    _ => throw new Exception("Unexpected input"),
                };
                parts = parts[2..];
            }

            var a = parts[0].Split(',');
            var xa = int.Parse(a[0]);
            var ya = int.Parse(a[1]);

            var b = parts[2].Split(',');
            var xb = int.Parse(b[0]) + 1; // +1 because the addresses are always 'top lefty', so (1, 1)->(2,2) addresses 4 squares. So we need to transform that for our maths to work
            var yb = int.Parse(b[1]) + 1;

            var x0 = Math.Min(xa, xb);
            var x1 = Math.Max(xa, xb);
            var y0 = Math.Min(ya, yb);
            var y1 = Math.Max(ya, yb);

            Area = new Rect(x0, x1, y0, y1);
        }

        public override string ToString()
        {
            return $"{Op} {Area}";
        }

        public enum Operation
        {
            On,
            Off,
            Toggle,
        }
    }

    public record struct Rect
    {
        public int X0 { get; set; }
        public int X1 { get; set; }
        public int Y0 { get; set; }
        public int Y1 { get; set; }

        public Rect(int x0, int x1, int y0, int y1) { X0 = x0; Y0 = y0; X1 = x1; Y1 = y1; }

        public int Width  => X1 - X0;
        public int Height => Y1 - Y0;
        public int Area => Width * Height;

        public bool CollidesWith(Rect other) => Intersects(other) || Contains(other) || other.Contains(this);
        public bool Intersects(Rect other) => X0 < other.X1 && X1 > other.X0 && Y0 < other.Y1 && Y1 > other.Y0;
        public bool Contains(Rect other) => X0 <= other.X0 && other.X1 <= X1 && Y0 <= other.Y0 && other.Y1 <= Y1;

        public List<Rect> CollidesWith(List<Rect> others)
        {
            var me = this; // gross
            return others.Where(e => me.CollidesWith(e)).ToList();
        }

        public Rect CollisionArea(Rect other)
        {
            var u0 = Math.Max(X0, other.X0);
            var u1 = Math.Min(X1, other.X1);
            var v0 = Math.Max(Y0, other.Y0);
            var v1 = Math.Min(Y1, other.Y1);
            return new Rect(u0, u1, v0, v1);
        }

        public List<Rect> Subtract(Rect other)
        {
            var result = new List<Rect>();
            result.Add(new Rect(X0,       other.X0, Y0,       other.Y0));
            result.Add(new Rect(other.X0, other.X1, Y0,       other.Y0));
            result.Add(new Rect(other.X1, X1,       Y0,       other.Y0));
            result.Add(new Rect(X0,       other.X0, other.Y0, other.Y1));
            result.Add(new Rect(other.X1, X1,       other.Y0, other.Y1));
            result.Add(new Rect(X0,       other.X0, other.Y1, Y1));
            result.Add(new Rect(other.X0, other.X1, other.Y1, Y1));
            result.Add(new Rect(other.X1, X1,       other.Y1, Y1));

            result = result.Where(r => r.Area > 0).ToList();
            Console.WriteLine($"Subtract {other} from {this}");
            foreach (var r in result) Console.WriteLine($"    {r}");
            return result;
        }

        public List<Rect> SubtractMany(List<Rect> others)
        {
            var result = new List<Rect>();
            foreach(var other in others)
            {
                result.AddRange(Subtract(other));
            }
            return result;
        }

        public override string ToString()
        {
            return $"({X0},{Y0}),({X1},{Y1})";
        }
    }
}
