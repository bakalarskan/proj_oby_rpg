using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_game;

namespace lab_game.model
{
    public sealed class Theme1 : IDungeonTheme
    {
        public string ThemeName => "Korytarze Wydziału MiNI";
        public string IntroMessage => "Błąkasz się po niekończących się korytarzach MiNI. Gdzieś w oddali słychać uderzenia bil...";

        public int StuffCount => 20;
        public int WeaponsCount => 10;
        public int CurrencyCount => 10;
        public int EnemiesCount => 6;
        private readonly Species _cowardly = new Cowardly("Tchórzliwy");
        private readonly Species _aggressive = new Aggressive("Agresywny");
        private readonly Species _cautious = new Cautious("Ostrożny");

        public IDungeonGenerationStrategy GenerationStrategy { get; } = new Theme1Strategy();

        public Item CreateRandomStuff(Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new Coffee(),
                1 => new Sudoku(),
                _ => new CorridorMap()
            };
        }

        public Item CreateRandomWeapon(Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new Student_ID(), 
                1 => new Backpack(),
                _ => new Note()
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
            return new BilliardBall8();
        }

        public Enemy CreateRandomEnemy(Base.Position pos, Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new ThemedEnemy("Zagubiony Pierwszak", 'z', pos, 50, 5, 0, _cowardly),
                1 => new ThemedEnemy("Zepsuty Automat", 'a', pos, 80, 12, 5, _cautious),
                _ => new ThemedEnemy("Kolejka do Dziekanatu", 'K', pos, 120, 25, 10, _aggressive)
            };
        }
    }
}
