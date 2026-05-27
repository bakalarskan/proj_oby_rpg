using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public sealed class Rj45Cable : LightWeapon
    {
        public override string Name => "Zaplątany kabel RJ45";
        public override char Symbol => 'r';
        public override int Damage => 14;
        public override bool IsTwoHanded => false;
    }

    public sealed class Pendrive : MagicWeapon
    {
        public override string Name => "Pendrive";
        public override char Symbol => 'p';
        public override int Damage => 20;
        public override bool IsTwoHanded => false;
    }

    public sealed class Glasses : Other
    {
        public override string Name => "Okulary";
        public override char Symbol => 'o';
        public override int IntelligenceBonus => 4;

        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
            => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
    }

    public sealed class LeakFreeScript : Artifact
    {
        public override string Name => "Skrypt wolny od wycieków pamięci";
        public override char Symbol => '§';
        public override int Damage => 45;
        public override bool IsTwoHanded => false;
    }
}
