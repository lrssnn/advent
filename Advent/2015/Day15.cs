using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day15
{

    List<Ingredient> Ingredients;

    public Day15()
    {
        using (StreamReader sr = File.OpenText("2015/input15"))
        {
            var input = sr.ReadToEnd();
            /*
            var input = @"Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
                        Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3";
            */

            Ingredients = input.Trim().Split("\n").Select(s => s.Trim()).Select(s => new Ingredient(s)).ToList();
        }
    }

    public void Solve()
    {
        var enumerations = EnumerateRecipes(Ingredients);

        Console.WriteLine("Searching for Part 1...");
        enumerations = enumerations.OrderByDescending(e => e.Value());
        var best = enumerations.FirstOrDefault();
        Console.WriteLine(best);
        Console.WriteLine(best.Value());

        Console.WriteLine("Searching for Part 2...");
        best = enumerations.First(e => e.IsValid2);
        Console.WriteLine(best);
        Console.WriteLine(best.Value());
    }

    public IEnumerable<Recipe> EnumerateRecipes(List<Ingredient> ingredients)
    {
        var percents = new List<Amount>();
        // Start every one on 0%
        foreach (var ingredient in ingredients) percents.Add(new Amount(ingredient, 0));

        // Start searching...
        bool done = false;
        while (!done)
        {
            var recipe = new Recipe(percents.ToList()); // Dictionary Copy
            if (recipe.IsValid) yield return recipe;
            // Permute
            var index = 0;
            var permuted = false;
            while (!permuted)
            {
                // If we can just add one to this digit, we're done
                if(percents[index].Percentage < 100)
                {
                    var item = percents[index];
                    item.Percentage += 1;
                    percents[index] = item;
                    permuted = true;
                } else
                {
                    // Overflow this one back to zero
                    var item = percents[index];
                    item.Percentage = 0;
                    percents[index] = item;
                    // We need to look at the next index
                    index++;
                    if(index >= ingredients.Count)
                    {
                        // Ran out of items to overflow to
                        yield break;
                    }
                }
            }

        }
    }

    public record struct Recipe
    {
        public List<Amount> Amounts { get; set; }

        public Recipe(List<Amount> amounts) { Amounts = amounts; }

        public bool IsValid => Amounts.Sum(pair => pair.Percentage) == 100;
        public bool IsValid2 => Amounts.Sum(item => item.Percentage * item.Ingredient.Calories) == 500;

        public long Value()
        {
            long Capacity = Math.Max(0, Amounts.Sum(a => a.Percentage * a.Ingredient.Capacity));
            long Durability = Math.Max(0, Amounts.Sum(a => a.Percentage * a.Ingredient.Durability));
            long Flavour = Math.Max(0, Amounts.Sum(a => a.Percentage * a.Ingredient.Flavour));
            long Texture = Math.Max(0, Amounts.Sum(a => a.Percentage * a.Ingredient.Texture));

            return Capacity * Durability * Flavour * Texture;
        }

        public override string ToString()
        {
            string str = "";
            foreach (var item in Amounts) str += $"({item.Ingredient.Name}: {item.Percentage}) ";
            return str;
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }

        public int Capacity { get; set; }
        public int Durability { get; set; }
        public int Flavour { get; set; }
        public int Texture { get; set; }
        public int Calories { get; set; }

        public Ingredient(string s)
        {
            var parts = s.Split(' ');

            Name = parts[0][..^1];

            Capacity   = int.Parse(parts[2][..^1]);
            Durability = int.Parse(parts[4][..^1]);
            Flavour    = int.Parse(parts[6][..^1]);
            Texture    = int.Parse(parts[8][..^1]);
            Calories   = int.Parse(parts[10]);
        }

        public override string ToString()
        {
            return $"{Name}: Capacity {Capacity}, Durability {Durability}, Flavour {Flavour}, Texture {Texture}, Calories {Calories}";
        }
    }

    public struct Amount
    {
        public Ingredient Ingredient { get; set; }
        public int Percentage { get; set; }

        public Amount (Ingredient ingredient, int percentage) { Ingredient = ingredient; Percentage = percentage; }
    }
}
