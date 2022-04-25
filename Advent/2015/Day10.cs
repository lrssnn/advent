using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day10
{

    public Day10()
    {
    }

    public void Solve()
    {
        /*
        Console.WriteLine(Read("1"));
        Console.WriteLine(Read("11"));
        Console.WriteLine(Read("21"));
        Console.WriteLine(Read("1211"));
        Console.WriteLine(Read("111221"));
        Console.WriteLine(Read("3113322113"));
        */
        var str = "3113322113";
        foreach(int i in Enumerable.Range(1, 50))
        {
            str = Read(str).ToString();
            Console.WriteLine($"Processed {i}");
        }
        Console.WriteLine(str.Length);
    }

    public Reading Read(string s)
    {
        var numbers = s.ToCharArray().Select(e => int.Parse(e.ToString())).ToArray();
        var reading = new Reading();
        var currentItem = new Item(numbers[0], 0);
        for(int i = 0; i < s.Length; i++)
        {
            if (numbers[i] == currentItem.Number)
            {
                currentItem.Count++;
            }
            else
            {
                reading.Items.Add(currentItem);
                currentItem = new Item(numbers[i], 1);
            }
        }
        reading.Items.Add(currentItem);
        return reading;
    }

    public record struct Reading
    {
        public List<Item> Items { get; set; }

        public Reading() { Items = new List<Item>(); }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var item in Items)
            {
                str.Append(item.Count);
                str.Append(item.Number);
            }
            return str.ToString();
        }
    }

    public record struct Item
    {
        public int Number { get; set; }
        public int Count { get; set; }

        public Item(int number, int count) { Number = number; Count = count; }
    }

}
