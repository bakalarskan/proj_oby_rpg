using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public abstract class Species : IEnemySub
    {
        private readonly List<Enemy> _enemies = new List<Enemy>();
        public string Name { get; }
        protected Species(string name)
        {
            Name = name;
        }
        public void AddEnemy(Enemy e)
        {
            if (!_enemies.Contains(e))
            {
                _enemies.Add(e);
            }
        }
        public void RemoveEnemy(Enemy e)
        {
            _enemies.Remove(e);
        }
        public void OnDeath(Enemy e)
        {
            RemoveEnemy(e);
            foreach (var enemy in _enemies)
            {
                ApplyEffect(enemy);
            }
        }
        protected abstract void ApplyEffect(Enemy e);
    }
    public sealed class Cowardly : Species
    {
        public Cowardly(string name) : base(name) { }
        protected override void ApplyEffect(Enemy e)
        {
            e.AdjustStats(damageDiff: -1, defenseDiff: -1);
        }
    }
    public sealed class Aggressive : Species
    {
        public Aggressive(string name) : base(name) { }
        protected override void ApplyEffect(Enemy e)
        {
            e.AdjustStats(damageDiff: 2, defenseDiff: 0);
        }
    }
    public sealed class Cautious : Species
    {
        public Cautious(string name) : base(name) { }
        protected override void ApplyEffect(Enemy e)
        {
            e.AdjustStats(damageDiff: 0, defenseDiff: 2);
        }
    }
}
