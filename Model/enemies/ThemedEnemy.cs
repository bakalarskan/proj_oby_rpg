using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public sealed class ThemedEnemy : Enemy
    {
        private readonly string _name;
        private readonly char _symbol;

        public override string Name => _name;
        public override char Symbol => _symbol;

        public ThemedEnemy(string name, char symbol, Base.Position pos, int maxHealth, int damage, int defense, Species species)
            : base(pos, maxHealth, damage, defense, species)
        {
            _name = name;
            _symbol = symbol;
        }
    }
}
