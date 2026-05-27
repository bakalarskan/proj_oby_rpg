using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public sealed class SoundPublisher
    {
        private readonly HashSet<ISoundSub> _subscribers = new HashSet<ISoundSub>();
        public void Subscribe(ISoundSub sub)
        {
            _subscribers.Add(sub);
        }

        public void Unsubscribe(ISoundSub sub)
        {
            _subscribers.Remove(sub);
        }

        public void Notify(Base.Position src, int range, string soundType, Board b)
        {
            var distances = b.GetDistancesInRange(src, range);
            foreach (var sub in _subscribers)
            {
                if (distances.TryGetValue(sub.Pos, out int distance))
                {
                    sub.OnSound(src, distance, soundType, b);
                }
            }
        }
    }
}
