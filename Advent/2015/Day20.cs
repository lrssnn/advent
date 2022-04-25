using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day20
{

    int Target;
    
    public Day20()
    {
        Target = 33100000;
    }

    public void Solve()
    {
        //Part1();
        Part2(776160);
    }

    public void Part1()
    {
        int elf = 1;
        var product = 1;
        var previousProduct = 0;
        while (true)
        {
            previousProduct = product;
            product *= elf;
            var score = 0;
            foreach (var i in Enumerable.Range(1, product))
                if (product % i == 0) score += i * 10;
            Console.WriteLine($"House {product} gets all elves up to {elf} | Score : {score}");
            if (score >= Target) break;
            elf++;
        }
        Console.WriteLine($"House {product} gets all elves up to {elf}");
        previousProduct = product / 5; // Magic number, dividing by less than 5 finds it immediately for this input
        Console.WriteLine($"Searching from {previousProduct} to {product} ({product - previousProduct} houses to check)");

        var answer = 0;
        var started = DateTime.Now;
        for (var house = previousProduct; house <= product; house++)
        {
            var score = 0;
            foreach (var i in Enumerable.Range(1, house))
                if (house % i == 0) score += i * 10;
            if (house % 1000 == 0)
            {
                var time = DateTime.Now - started;
                Console.WriteLine($"{house} | {score} ({Target - score}) | {time.TotalMilliseconds}ms");
                started = DateTime.Now;
            }
            if (score >= Target)
            {
                answer = house;
                break;
            }
        }
        Console.WriteLine($"Found house {answer}");
    }

    public void Part2(int start)
    {
        // Now each elf delivers 50
        // We know that we must be looking at a higher house than before, so we start where step one finished
        var upperLimit = 0;
        var started = DateTime.Now;

        for (var house = start; true; house += 10000)
        {
            var score = 0;
            var lowestElf = (int)Math.Ceiling(((double)house) / 50.0);
            for (var i = lowestElf; i <= house; i++)
                if (house % i == 0) score += i * 10;
            if (house % 100000 == 0)
            {
                var time = DateTime.Now - started;
                Console.WriteLine($"{house} | {score} ({Target - score}) | {time.TotalMilliseconds}ms");
                started = DateTime.Now;
            }
            if (score >= Target)
            {
                upperLimit = house;
                break;
            }
        }

        Console.WriteLine($"Found Upper Limit {upperLimit}");

        var answer = 0;
        for (var house = upperLimit - 10000; true; house++)
        {
            var score = 0;
            var lowestElf = (int)Math.Ceiling(((double)house) / 50.0);
            for (var i = lowestElf; i <= house; i++)
                if (house % i == 0) score += i * 10;
            if (house % 1000 == 0)
            {
                var time = DateTime.Now - started;
                Console.WriteLine($"{house} | {score} ({Target - score}) | {time.TotalMilliseconds}ms");
                started = DateTime.Now;
            }
            if (score >= Target)
            {
                answer = house;
                break;
            }
        }
        Console.WriteLine($"Found house {answer}");
        
    }

}
