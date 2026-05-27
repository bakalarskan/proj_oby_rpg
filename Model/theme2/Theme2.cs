using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace lab_game.model
{
    public sealed class Theme2 : IDungeonTheme
    {
        public string ThemeName => "Sala Labowa (Techniczny)";
        public string IntroMessage => "Wchodzisz do dusznej sali komputerowej pełnej informatyków. \nSłychać tylko szum przegrzanych procesorów i nerwowe uderzanie w klawiatury.";

        public int StuffCount => 18;
        public int WeaponsCount => 12;
        public int CurrencyCount => 8;
        public int EnemiesCount => 6;
        private readonly Species _cowardly = new Cowardly("Tchórzliwy");
        private readonly Species _aggressive = new Aggressive("Agresywny");
        private readonly Species _cautious = new Cautious("Ostrożny");

        public IDungeonGenerationStrategy GenerationStrategy { get; } = new Theme2Strategy();

        public Item CreateRandomStuff(Random rnd)
        {
            return rnd.Next(0, 2) switch
            {
                0 => new RedBull(),
                _ => new Glasses(),
            };
        }

        public Item CreateRandomWeapon(Random rnd)
        {
            return rnd.Next(0, 4) switch
            {
                0 => new Rj45Cable(),
                1 => new Pendrive(),
                2 => new Laptop(),
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
            return new LeakFreeScript();
        }

        public Enemy CreateRandomEnemy(Base.Position pos, Random rnd)
        {
            return rnd.Next(0, 3) switch
            {
                0 => new ThemedEnemy("Segmentation Fault", 'f', pos, 55, 8, 1, _cowardly),
                1 => new ThemedEnemy("Nieskończona Pętla", 'l', pos, 90, 14, 5, _cautious),
                _ => new ThemedEnemy("SOPy", 'S', pos, 140, 28, 12, _aggressive)
            };
        }
    }
}
