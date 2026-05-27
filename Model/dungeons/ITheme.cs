using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public interface IDungeonTheme
    {
        string ThemeName { get; }
        string IntroMessage { get; }

        int StuffCount { get; }
        int WeaponsCount { get; }
        int CurrencyCount { get; }
        int EnemiesCount { get; }

        IDungeonGenerationStrategy GenerationStrategy { get; }

        Item CreateRandomStuff(Random rnd);
        Item CreateRandomWeapon(Random rnd);
        Item CreateRandomCurrency(Random rnd);
        Item CreateArtifact();
        Enemy CreateRandomEnemy(Base.Position pos, Random rnd);
    }
}
