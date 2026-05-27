using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab_game.model
{
    public sealed class Theme3 : IDungeonTheme
    {
        public string ThemeName => "Sala 107";
        public string IntroMessage => "Przekraczasz próg sali 107. \nRzędy krzeseł przypominają o nadchodzącej zagładzie, a w powietrzu unosi się zapach czystego stresu.";

        public int StuffCount => 16;
        public int WeaponsCount => 12;
        public int CurrencyCount => 8;
        public int EnemiesCount => 6;
        private readonly Species _cowardly = new Cowardly("Tchórzliwy");
        private readonly Species _aggressive = new Aggressive("Agresywny");
        private readonly Species _cautious = new Cautious("Ostrożny");

        public IDungeonGenerationStrategy GenerationStrategy { get; } = new Theme3Strategy();

        public Item CreateRandomStuff(Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new WaterBottle(),
                1 => new LegendOfTheStudent(),
                _ => new Sudoku()
            };
        }

        public Item CreateRandomWeapon(Random rnd)
        {
            return rnd.Next(0, 5) switch
            {
                0 => new CheatSheet(),
                1 => new Pen(),
                2 => new Calculator(),
                3 => new Student_ID(),
                _ => new Notes()
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
            return new Three();
        }

        public Enemy CreateRandomEnemy(Base.Position pos, Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new ThemedEnemy("Wejściówka", 'w', pos, 50, 6, 0, _cowardly),
                1 => new ThemedEnemy("Kolokwium", 'k', pos, 90, 14, 6, _cautious),
                _ => new ThemedEnemy("Egzamin z Dyskretnej", 'E', pos, 145, 30, 12, _aggressive)
            };
        }
    }
}
