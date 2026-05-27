using System;

namespace lab_game.model
{
    public abstract class Enemy : ISoundSub
    {
        public abstract string Name { get; }
        public abstract char Symbol { get; }
        public Base.Position Pos { get; private set; }
        public int MaxHealth { get; }
        public int Health { get; private set; }
        private readonly EnemyPublisher _enemyPublisher = new EnemyPublisher();
        private readonly Species _species;
        private readonly int _baseDamage;
        private readonly int _baseDefense;
        private int _bonusDamage;
        private int _bonusDefense;
        public int Damage => Math.Max(0, _baseDamage + _bonusDamage);
        public int Defense => Math.Max(0, _baseDefense + _bonusDefense);
        public bool IsAlive => Health > 0;
        protected Enemy(Base.Position pos, int maxHealth, int damage, int defense, Species species)
        {
            Pos = pos;
            MaxHealth = maxHealth;
            Health = maxHealth;
            _baseDamage = damage;
            _baseDefense = defense;
            _species = species;
            _species.AddEnemy(this);
            _enemyPublisher.Subscribe(_species);
        }
        public void MoveTo(Base.Position newPos)
        {
            Pos = newPos;
        }
        public int TakeDamage(int damage)
        {
            int newDamage = Math.Max(0, damage - Defense);
            if (Health - newDamage < 0)
            {
                newDamage = Health;
            }
            Health -= newDamage;
            return newDamage;
        }
        public virtual int Attack()
        {
            return Damage;
        }
        public virtual void OnDeath()
        {
            NotifyDeath();
        }
        public virtual void OnSound(Base.Position src, int distance, string soundType, Board b)
        {
            b.AddCom($"Hałas ({soundType}) usłyszany przez {Name} na ({Pos.x}, {Pos.y})" +
                $"źródło: ({src.x}, {src.y}), dystans: {distance}");
        }
        public void AdjustStats(int damageDiff, int defenseDiff)
        {
            _bonusDamage += damageDiff;
            _bonusDefense += defenseDiff;
            if (_baseDamage + _bonusDamage < 0)
            {
                _bonusDamage = -_baseDamage;
            }
            if (_baseDefense + _bonusDefense < 0)
            {
                _bonusDefense = -_baseDefense;
            }
        }
        public void NotifyDeath()
        {
            _enemyPublisher.Notify(this);
            _enemyPublisher.Unsubscribe(_species);
        }
    }
    public class WeakEnemy : Enemy
    {
        public override string Name => "Wejściówka";
        public override char Symbol => 'w';
        public WeakEnemy(Base.Position pos, Species species) : base(pos, maxHealth: 50, damage: 5, defense: 0, species)
        {
        }
    }
    public class StrongEnemy : Enemy
    {
        public override string Name => "SOPy";
        public override char Symbol => 's';
        public StrongEnemy(Base.Position pos, Species species) : base(pos, maxHealth: 120, damage: 25, defense: 10, species)
        {
        }
    }
    public class MediumEnemy : Enemy
    {
        public override string Name => "Egzamin z dyskretnej";
        public override char Symbol => 'e';
        public MediumEnemy(Base.Position pos, Species species) : base(pos, maxHealth: 80, damage: 12, defense: 5, species)
        {
        }
    }
}