using System.Collections.Generic;

namespace lab_game.model
{
    public abstract class ItemDecorator : Item
    {
        protected readonly Item _item;
        public Item Inner => _item;
        protected ItemDecorator(Item item)
        {
            _item = item;
        }
        public override int StrengthBonus => _item.StrengthBonus;
        public override int AgilityBonus => _item.AgilityBonus;
        public override int IntelligenceBonus => _item.IntelligenceBonus;
        public override int LuckBonus => _item.LuckBonus;
        public override int AggressionBonus => _item.AggressionBonus;
        public override int Damage => _item.Damage;
        public override bool IsTwoHanded => _item.IsTwoHanded;
        public override bool IsUsable => _item.IsUsable;
        public override char Symbol => _item.Symbol;
        public override Base.Position Pos
        {
            get => _item.Pos;
            set => _item.Pos = value;
        }
        public override string Stat => _item.Stat;
        public override int SoundRange => _item.SoundRange;
        public override string SoundType => _item.SoundType;
        public override void PickUp(Player p, Board b)
        {
            p.Inventory.Add(this);
            b.AddCom($"Zdobyto: {this.Name}");
            if (SoundRange > 0)
            {
                b.Events.SoundPublisher.Notify(p.Pos, SoundRange, SoundType, b);
            }
        }
        public override void Accept(IVisitor visitor)
        {
            _item.Accept(visitor);
        }
        public override int GetAttack(IAttackStrategy s, Player p, int baseDamage)
        {
            return _item.GetAttack(s, p, this.Damage);
        }

        public override int GetDefense(IAttackStrategy s, Player p)
        {
            return _item.GetDefense(s, p);
        }
    }
    public class Unlucky : ItemDecorator
    {
        public Unlucky(Item item) : base(item) { }
        public override string Name => $"{_item.Name} (Pechowy)";
        public override int LuckBonus => _item.LuckBonus - 5;
    }
    public class Strong : ItemDecorator
    {
        private const int DamageBonus = 5;
        public Strong(Item item) : base(item) { }
        public override string Name => $"{_item.Name} (Silny)";
        public override int Damage => _item.Damage + 5;
        public override string Stat => $" (+{Damage})";
    }
}