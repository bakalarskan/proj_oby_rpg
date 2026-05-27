using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace lab_game.model
{
    public abstract class Item
    {
        public virtual int StrengthBonus => 0;
        public virtual int HealthBonus => 0;
        public virtual int AgilityBonus => 0;
        public virtual int IntelligenceBonus => 0;
        public virtual int LuckBonus => 0;
        public virtual int AggressionBonus => 0;
        public abstract int Damage { get; }
        public abstract bool IsTwoHanded { get; }
        public abstract bool IsUsable { get; }
        public abstract string Name { get; }
        public abstract char Symbol { get; }
        public virtual Base.Position Pos { get; set; }
        public virtual string Stat => "";
        public abstract void PickUp(Player p, Board b);
        public abstract void Accept(IVisitor visitor);
        public abstract int GetAttack(IAttackStrategy s, Player p, int baseDamage);
        public abstract int GetDefense(IAttackStrategy s, Player p);
        public virtual int SoundRange => 0;
        public virtual string SoundType => "brak";
    }
    public abstract class Currency : Item
    {
        public override int Damage => 0;
        public override bool IsTwoHanded => false;
        public abstract int Value { get; }
        public override bool IsUsable => true;
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class Other : Item
    {
        public override bool IsTwoHanded => false;
        public override int Damage => 0;
        public override bool IsUsable => true;
        public override void PickUp(Player p, Board b)
        {
            p.Inventory.Add(this);
            b.AddCom($"Zdobyto: {this.Name}");
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class Weapon : Item
    {
        public override bool IsUsable => true;
        public override string Stat => $" (+{this.Damage})";
        public override void PickUp(Player p, Board b)
        {
            p.Inventory.Add(this);
            b.AddCom($"Zdobyto: {this.Name}, atak: {this.Damage}");
            if (SoundRange > 0)
            {
                b.Events.SoundPublisher.Notify(p.Pos, SoundRange, SoundType, b);
            }
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    public abstract class Artifact : HeavyWeapon
    {
        public sealed override bool IsUsable => true;
        public virtual string ArtifactType => "Artefakt";
        public override void PickUp(Player p, Board b)
        {
            p.Inventory.Add(this);
            b.AddCom($"Zdobyto artefakt: {this.Name}");
        }
    }

    public abstract class LightWeapon : Weapon
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override int SoundRange => 2;
        public override string SoundType => "lekki";
    }
    public abstract class HeavyWeapon : Weapon
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override void Accept(IVisitor visitor)
            {
                visitor.Visit(this);
        }
        public override int SoundRange => 6;
        public override string SoundType => "ciężki";

    }
    public abstract class MagicWeapon : Weapon
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        public override int SoundRange => 4;
        public override string SoundType => "magiczny";
    }

    public class ECTS : Currency
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override string Name => "Punkt ECTS";
        public override char Symbol => '★';
        public override int Value => 1;
        public override void PickUp(Player p, Board b)
        {
            b.AddCom($"Zdobyto: {this.Name} (+{this.Value})!");
            p.GetPoint(this.Value);
        }

    }
    public class Coin : Currency
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override string Name => "Kasa na warunki";
        public override char Symbol => '$';
        public override int Value => 10;
        public override void PickUp(Player p, Board b)
        {
            b.AddCom($"Zdobyto: {this.Name} (+{this.Value})!");
            p.GetCoins(this.Value);
        }

    }
    public class Laptop : HeavyWeapon
    {
        public override string Name => "Laptop z Linuxem";
        public override char Symbol => '■';
        public override int Damage => 50;
        public override bool IsTwoHanded => true;
        
    }
    public class Student_ID : LightWeapon
    {
        public override string Name => "Legitka";
        public override char Symbol => '◘';
        public override int Damage => 10;
        public override bool IsTwoHanded => false;
        
    }
    public class Notes : MagicWeapon
    {

        public override string Name => "Notatki z analizy";
        public override char Symbol => '∑';
        public override int Damage => 25;
        public override bool IsTwoHanded => false;
        
    }
    public class Bilard : Other
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override string Name => "Bilard";
        public override char Symbol => '○';

    }
    public class RedBull : Other
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override string Name => "RedBull";
        public override char Symbol => 'Φ';
    }
    public class Notebook : Other
    {
        public override int GetAttack(IAttackStrategy s, Player p, int currentDamage)
        => s.Attack(this, p, currentDamage);

        public override int GetDefense(IAttackStrategy s, Player p)
            => s.Defense(this, p);
        public override string Name => "Zeszyt";
        public override char Symbol => '<';
        
    }
}
