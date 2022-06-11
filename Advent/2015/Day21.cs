using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventFifteen;

public class Day21
{

    public Unit Boss;
    public Unit BasePlayer;
    public Shop TheShop;
    
    public Day21()
    {
        TheShop = new Shop();
        TheShop.Weapons.Add((new Weapon(4), 8));
        TheShop.Weapons.Add((new Weapon(5), 10));
        TheShop.Weapons.Add((new Weapon(6), 25));
        TheShop.Weapons.Add((new Weapon(7), 40));
        TheShop.Weapons.Add((new Weapon(8), 74));

        TheShop.Armor.Add((new Armor(1), 13));
        TheShop.Armor.Add((new Armor(2), 31));
        TheShop.Armor.Add((new Armor(3), 53));
        TheShop.Armor.Add((new Armor(4), 75));
        TheShop.Armor.Add((new Armor(5), 102));

        TheShop.Rings.Add((new Ring(1, 0), 25));
        TheShop.Rings.Add((new Ring(2, 0), 50));
        TheShop.Rings.Add((new Ring(3, 0), 100));
        TheShop.Rings.Add((new Ring(0, 1), 20));
        TheShop.Rings.Add((new Ring(0, 2), 40));
        TheShop.Rings.Add((new Ring(0, 3), 80));

        Boss = new Unit("Boss", 104, 8, 1, Shop.None);
        BasePlayer = new Unit("Player", 100, 0, 0, TheShop);
    }

    public void Solve()
    {
        IEnumerable<Unit> winners = new List<Unit>();
        IEnumerable<Unit> losers = new List<Unit>();
        IEnumerable<Unit> players = new List<Unit> { BasePlayer };
        while (true) {
            players = players.SelectMany(p => Buy(p));

            if(!players.Any()) break;

            var groups = players.GroupBy(p => PlayerWins(p, Boss));
            var theseWinners = groups.SingleOrDefault(g => g.Key);
            var theseLosers = groups.SingleOrDefault(g => !g.Key);
            if(theseWinners != null) winners = winners.Concat(theseWinners);
            if(theseLosers != null) losers = losers.Concat(theseLosers);
        }

        var winner = winners.OrderBy(p => p.GoldSpent).First();
        var loser = losers.OrderByDescending(p => p.GoldSpent).First();
        Console.WriteLine($"Winning guy spent {winner.GoldSpent}gp");
        Console.WriteLine($"Losing guy spent {loser.GoldSpent}gp");
    }

    public List<Unit> Buy(Unit player)
    {
        var result = new List<Unit>();

        if (player.CanEquipWeapon)
        {
            foreach(var weapon in player.Shop.Weapons)
            {
                var p = new Unit(player);
                p.EquipWeapon(weapon);
                p.Shop.Weapons.Remove(weapon);
                result.Add(p);
            }
        }

        if (player.CanEquipArmor)
        {
            foreach(var armor in player.Shop.Armor)
            {
                var p = new Unit(player);
                p.EquipArmor(armor);
                p.Shop.Armor.Remove(armor);
                result.Add(p);
            }
        }

        if (player.CanEquipRing)
        {
            foreach(var ring in player.Shop.Rings)
            {
                var p = new Unit(player);
                p.EquipRing(ring);
                p.Shop.Rings.Remove(ring);
                result.Add(p);
            }
        }

        return result;
    }

    public bool PlayerWins(Unit player, Unit boss)
    {
        //Console.WriteLine($"Player {player} fighting boss...");
        while (true)
        {
            boss.TakeDamage(player.ModAtk);
            if (boss.Health <= 0) return true;
        
            player.TakeDamage(boss.ModAtk);
            if (player.Health <= 0) return false;
        }
    }

    public record struct Unit
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int GoldSpent { get; set; }

        public Weapon? Weapon { get; set; }
        public Armor? Armor { get; set; }
        public Ring? Ring1 { get; set; }
        public Ring? Ring2 { get; set; }

        public Shop Shop { get; set; }

        public Unit(string name, int health, int attack, int defense, Shop shop) 
        { 
            Name = name; Health = health; Attack = attack; Defense = defense; GoldSpent = 0;
            Weapon = null; Armor = null; Ring1 = null; Ring2 = null; Shop = shop;
        }

        public Unit(Unit copy)
        {
            Name = copy.Name;
            Health = copy.Health;
            Attack = copy.Attack;
            Defense = copy.Defense;
            GoldSpent = copy.GoldSpent;

            Weapon = copy.Weapon;
            Armor = copy.Armor;
            Ring1 = copy.Ring1;
            Ring2 = copy.Ring2;

            Shop = new Shop(copy.Shop);
        }

        public bool CanEquipWeapon => Weapon == null;
        public bool CanEquipArmor => Armor == null;
        public bool CanEquipRing => Ring1 == null || Ring2 == null;

        public void EquipWeapon((Weapon, int) w)
        {
            Weapon = w.Item1;
            GoldSpent += w.Item2;
        }

        public void EquipArmor((Armor, int) a)
        {
            Armor = a.Item1;
            GoldSpent += a.Item2;
        }

        public void EquipRing((Ring, int) r)
        {
            if(Ring1 == null)
                Ring1 = r.Item1;
            else
                Ring2 = r.Item1;

            GoldSpent += r.Item2;
        }

        public int ModDef => Defense 
                + (Armor?.Armor ?? 0) 
                + (Ring1?.Armor ?? 0)
                + (Ring2?.Armor ?? 0);

        public int ModAtk => Attack 
                + (Weapon?.Damage ?? 0) 
                + (Ring1?.Damage ?? 0)
                + (Ring2?.Damage ?? 0);


        public int TakeDamage(int damage)
        {
            var taken = Math.Max(damage - ModDef, 1);
            Health -= taken;
            //Console.WriteLine($"{Name} Taking ({damage} - {ModDef} = {taken}) damage; Down to {Health}HP");
            return Health;
        }

        public override string ToString() => $"ATK: {ModAtk}, DEF: {ModDef}";
    }

    public class Item
    {
        public int Damage { get; set; }
        public int Armor { get; set; }

        public Item(int damage, int armor) { Damage = damage; Armor = armor; }
    }

    public class Weapon : Item { public Weapon(int damage)          : base(damage, 0) { } }
    public class Armor  : Item { public Armor(int armor)            : base(0, armor) { } }
    public class Ring   : Item { public Ring(int damage, int armor) : base(damage, armor) { } }

    public class Shop
    {
        public List<(Weapon, int)> Weapons { get; set; }
        public List<(Armor, int)> Armor { get; set; }
        public List<(Ring, int)> Rings { get; set; }

        public Shop()
        {
            Weapons = new List<(Weapon, int)>();
            Armor = new List<(Armor, int)>();
            Rings = new List<(Ring, int)>();
        }

        public Shop(Shop copy)
        {
            Weapons = copy.Weapons.ToList();
            Armor = copy.Armor.ToList();
            Rings = copy.Rings.ToList();
        }
        
        public static Shop None => new Shop();
    }
}
