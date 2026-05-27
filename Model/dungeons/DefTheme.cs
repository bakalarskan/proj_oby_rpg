using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_game;

namespace lab_game.model
{
    public sealed class DefaultDungeonTheme : IDungeonTheme
    {
        public string ThemeName => "Wydział MiNI";
        public string IntroMessage => "Zapach starych notatek i kawy unosi się w powietrzu.";

        public int StuffCount => 20;
        public int WeaponsCount => 10;
        public int CurrencyCount => 15;
        public int EnemiesCount => 5;
        private readonly Species _cowardly = new Cowardly("Tchórzliwy");
        private readonly Species _aggressive = new Aggressive("Agresywny");
        private readonly Species _cautious = new Cautious("Ostrożny");

        public IDungeonGenerationStrategy GenerationStrategy { get; } = new DefaultGenerationStrategy();

        public Item CreateRandomStuff(Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new RedBull(),
                1 => new Bilard(),
                _ => new Notebook()
            };
        }

        public Item CreateRandomWeapon(Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new Laptop(),
                1 => new Notes(),
                _ => new Student_ID()
            };
        }

        public Item CreateRandomCurrency(Random rnd)
        {
            return rnd.Next(0, 2) switch
            {
                0 => new Coin(),
                _ => new ECTS()
            };
        }

        public Item CreateArtifact()
        {
            return new Laptop();
        }

        public Enemy CreateRandomEnemy(Base.Position pos, Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new ThemedEnemy("Słaby Wróg", 's', pos, 50, 5, 0, _cowardly),
                1 => new ThemedEnemy("Średni Wróg", 'm', pos, 80, 12, 5, _cautious),
                _ => new ThemedEnemy("Silny Wróg", 'S', pos, 120, 25, 10, _aggressive)
            };
        }
    }
}
