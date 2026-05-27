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
    public sealed class EmptyLogger : IGameLogger
    {
        public string LogFilePath => "brak pliku logu";
        public void Log(string message)
        {
        }
        public IReadOnlyList<string> GetRecent(int n)
        {
            return Array.Empty<string>();
        }
        public IReadOnlyList<string> GetAll()
        {
            return Array.Empty<string>();
        }
    }
}
