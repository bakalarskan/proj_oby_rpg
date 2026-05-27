using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    internal class EnemyFactory
    {
        private Random _rnd = new Random();
        private readonly IDungeonTheme _theme;
        public EnemyFactory() : this(new DefaultDungeonTheme())
        {
        }
        public EnemyFactory(IDungeonTheme theme)
        {
            _theme = theme;
        }
        public Enemy CreateRandomEnemy(Base.Position pos)
        {
            return _theme.CreateRandomEnemy(pos, _rnd);
        }
    }
}
