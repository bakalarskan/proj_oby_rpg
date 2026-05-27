using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public class CombatRes
    {
        public bool EnemyHit { get; init; }
        public int EnemyDamage { get; init; }
        public bool EnemyKilled { get; init; }
        public bool PlayerHit { get; init; }
        public int PlayerDamage { get; init; }
        public bool PlayerKilled { get; init; }
    }
}
