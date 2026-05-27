using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab_game.model
{
    public sealed class CheatSheet : MagicWeapon
    {
        public override string Name => "Zmięta ściąga";
        public override char Symbol => '!';
        public override int Damage => 22;
        public override bool IsTwoHanded => false;
    }

    public sealed class Pen : LightWeapon
    {
        public override string Name => "Długopis";
        public override char Symbol => '/';
        public override int Damage => 8;
        public override bool IsTwoHanded => false;
    }

    public sealed class Calculator : HeavyWeapon
    {
        public override string Name => "Kalkulator";
        public override char Symbol => '=';
        public override int Damage => 26;
        public override bool IsTwoHanded => false;
    }

    public sealed class WaterBottle : Other
    {
        public override string Name => "Woda";
        public override char Symbol => 'w';
        public override int AggressionBonus => -1;

        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
            => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);

        public override void PickUp(Player p, Board b)
        {
            int before = p.Health;
            p.Health = Math.Min(100, p.Health + 12);
            int healed = p.Health - before;
            b.AddCom($"Woda: +{healed} HP");
        }
    }

    public sealed class LegendOfTheStudent : Other
    {
        public override string Name => "Legenda o studencie, który zdał 1. termin";
        public override char Symbol => 'L';
        public override int LuckBonus => 4;
        public override int IntelligenceBonus => 2;

        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
            => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
    }

    public sealed class Three : Artifact
    {
        public override string Name => "Trójka z Dyskretnej";
        public override char Symbol => '3';
        public override int Damage => 55;
        public override bool IsTwoHanded => false;
    }
}
