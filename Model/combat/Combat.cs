using System;

namespace lab_game.model
{
    public class Combat
    {
        public CombatRes Turn(Player p, Board b, Enemy e, IAttackStrategy s)
        {
            int leftAttack = p.LeftHand?.GetAttack(s, p, p.LeftHand.Damage) ?? 0;
            int rightAttack = p.RightHand?.GetAttack(s, p, p.RightHand.Damage) ?? 0;
            int totalAttack = leftAttack + rightAttack;
            int damageToEnemy = e.TakeDamage(totalAttack);
            bool enemyKilled = !e.IsAlive;
            b.AddCom($"{s.Name}: zadajesz {damageToEnemy} obrażen {e.Name} ({e.Name})");
            if (enemyKilled)
            {
                e.OnDeath();
                b.RemoveEnemy(e);
                b.AddCom($"{e.Name} pokonany!");
                return new CombatRes()
                {
                    EnemyHit = true,
                    EnemyDamage = damageToEnemy,
                    EnemyKilled = true,
                    PlayerHit = false,
                    PlayerDamage = 0,
                    PlayerKilled = false
                };
            }
            int leftDefense = p.LeftHand?.GetDefense(s, p) ?? 0;
            int rightDefense = p.RightHand?.GetDefense(s, p) ?? 0;
            int totalDefense = leftDefense + rightDefense;
            int damageToPlayer = Math.Max(0, e.Attack() - totalDefense);
            int playerDamageTaken = p.TakeDamage(damageToPlayer);
            bool playerKilled = !p.isAlive;
            b.AddCom($"{e.Name} kontratakuje (-{playerDamageTaken} HP)");
            if (playerKilled)
            {
                b.AddCom("Zostałaś pokonana");
            }
            return new CombatRes()
            {
                EnemyHit = true,
                EnemyDamage = damageToEnemy,
                EnemyKilled = enemyKilled,
                PlayerHit = true,
                PlayerDamage = playerDamageTaken,
                PlayerKilled = playerKilled
            };
        }
        public CombatRes? isEnemy(Player p, Board b, IAttackStrategy s)
        {
            Enemy? e = b.GetEnemyAt(p.Pos.x, p.Pos.y);
            if (e == null)
            {
                b.AddCom("Nie ma tu wroga!");
                return null;
            }
            return Turn(p, b, e, s);
        }
    }
}