using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using lab_game.model;
using lab_game.infrastructure;
using lab_game.view;

namespace lab_game.infrastructure
{
    public static class GameEvents
    {
        public static void Notify(Board b, string com)
        {
            b.AddCom(com);
            GameLog.Log(com);
        }
    }
}
