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
    public interface IGameLogger
    {
        string LogFilePath { get; }
        void Log(string message);
        IReadOnlyList<string> GetRecent(int n);
        IReadOnlyList<string> GetAll();
    }
}
