using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_game.model;
using lab_game.infrastructure;
using lab_game.view;

namespace lab_game.infrastructure
{
    public static class LoggerFactory
    {
        public static IGameLogger CreateDefault(string playerName, string logDirectory)
        {
            return new MFLogger(playerName, logDirectory);
        }
    }
}
