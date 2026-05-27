using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public interface ISoundSub
    {
        Base.Position Pos { get; }
        void OnSound(Base.Position src, int distance, string soundType, Board b);
    }
}
