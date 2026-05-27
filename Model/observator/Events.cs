using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public sealed class Events
    {
        private readonly SoundPublisher _soundPublisher = new SoundPublisher();
        public SoundPublisher SoundPublisher => _soundPublisher;
    }
}
