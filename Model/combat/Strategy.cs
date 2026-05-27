using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public interface IAttackStrategy
    {
        string Name { get; }
        int Attack(HeavyWeapon w, Player p, int baseDamage);
        int Attack(LightWeapon w, Player p, int baseDamage);
        int Attack(MagicWeapon w, Player p, int baseDamage);
        int Attack(Item i, Player p, int baseDamage);
        int Defense(HeavyWeapon w, Player p);
        int Defense(LightWeapon w, Player p);
        int Defense(MagicWeapon w, Player p);
        int Defense(Item i, Player p);
    }
    public class Normal : IAttackStrategy
    {
        public string Name => "Zwykły";
        public int Attack(HeavyWeapon w, Player p, int damage) => damage + p.Strength + p.Aggression;
        public int Attack(LightWeapon w, Player p, int damage) => damage + p.Agility + p.Luck;
        public int Attack(MagicWeapon w, Player p, int damage) => 1;
        public int Attack(Item i, Player p, int damage) => 0;
        public int Defense(HeavyWeapon w, Player p) => p.Strength + p.Luck;
        public int Defense(LightWeapon w, Player p) => p.Agility + p.Luck;
        public int Defense(MagicWeapon w, Player p) => p.Agility + p.Luck;
        public int Defense(Item i, Player p) => p.Agility;
    }
    public class Sneaky : IAttackStrategy
    {
        public string Name => "Skryty";
        public int Attack(HeavyWeapon w, Player p, int damage) => (damage +p.Strength + p.Aggression) / 2;
        public int Attack(LightWeapon w, Player p, int damage) => (damage + p.Agility + p.Luck) * 2;
        public int Attack(MagicWeapon w, Player p, int damage) => 1;
        public int Attack(Item i, Player p, int damage) => 0;
        public int Defense(HeavyWeapon w, Player p) => p.Strength;
        public int Defense(LightWeapon w, Player p) => p.Agility;
        public int Defense(MagicWeapon w, Player p) => 0;
        public int Defense(Item i, Player p) => 0;
    }
    public class Magic : IAttackStrategy
    {
        public string Name => "Magiczny";
        public int Attack(HeavyWeapon w, Player p, int damage) => 1;
        public int Attack(LightWeapon w, Player p, int damage) => 1;
        public int Attack(MagicWeapon w, Player p, int damage) => damage + p.Intelligence;
        public int Attack(Item i, Player p, int damage) => 0;
        public int Defense(HeavyWeapon w, Player p) => p.Luck;
        public int Defense(LightWeapon w, Player p) => p.Luck;
        public int Defense(MagicWeapon w, Player p) => p.Intelligence * 2;
        public int Defense(Item i, Player p) => p.Luck;
    }
}
