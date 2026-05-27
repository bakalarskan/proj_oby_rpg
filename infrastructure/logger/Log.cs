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
    public static class GameLog
    {
        private static IGameLogger _logger = new EmptyLogger();
        public static IGameLogger Logger => _logger;
        public static void SetLogger(IGameLogger logger)
        {
            _logger = logger ?? new EmptyLogger();
        }
        public static void Log(string message)
        {
            _logger.Log(message);
        }
        public static IReadOnlyList<string> GetRecent(int n)
        {
            return _logger.GetRecent(n);
        }
        public static IReadOnlyList<string> GetAll()
        {
            return _logger.GetAll();
        }
    }
}
