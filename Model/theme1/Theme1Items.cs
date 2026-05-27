using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public sealed class Backpack : HeavyWeapon
    {
        public override string Name => "Ciężki plecak";
        public override char Symbol => 'B';
        public override int Damage => 35;
        public override bool IsTwoHanded => true;
    }

    public sealed class Note : MagicWeapon
    {
        public override string Name => "Znalezione notatki";
        public override char Symbol => 'n';
        public override int Damage => 24;
        public override bool IsTwoHanded => false;
    }

    public sealed class Coffee : Other
    {
        public override string Name => "Kawa z automatu";
        public override char Symbol => 'k';
        public override int AgilityBonus => 1;

        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
            => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);

        public override void PickUp(Player p, Board b)
        {
            int before = p.Health;
            p.Health = Math.Min(100, p.Health + 15);
            int healed = p.Health - before;
            b.AddCom($"Kawa: +{healed} HP");
        }
    }

    public sealed class Sudoku : Other
    {
        public override string Name => "Sudoku";
        public override char Symbol => 'u';
        public override int IntelligenceBonus => 3;

        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
            => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
    }

    public sealed class CorridorMap : Other
    {
        public override string Name => "Plan korytarzy";
        public override char Symbol => 'p';
        public override int LuckBonus => 2;

        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
            => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
    }

    public sealed class BilliardBall8 : Artifact
    {
        public override string Name => "Magiczna Bila nr 8";
        public override char Symbol => '8';
        public override int Damage => 40;
        public override bool IsTwoHanded => false;
    }
}
