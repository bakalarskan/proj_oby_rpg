using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_game.model;
using lab_game.infrastructure;

namespace lab_game.infrastructure
{
    public sealed class Config
    {
        public string PlayerName { get; set; } = "Student";
        public string LogDirectory { get; init; } = "logs";
    }
}
