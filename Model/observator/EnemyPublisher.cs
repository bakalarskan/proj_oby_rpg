using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public sealed class EnemyPublisher
    {
        private readonly HashSet<IEnemySub> _subscribers = new HashSet<IEnemySub>();
        public EnemyPublisher()
        {
        }
        public void Subscribe(IEnemySub sub)
        {
            _subscribers.Add(sub);
        }

        public void Unsubscribe(IEnemySub sub)
        {
            _subscribers.Remove(sub);
        }

        public void Notify(Enemy e)
        {
            foreach (var sub in _subscribers)
            {
                sub.OnDeath(e);
            }
        }
    }
}
