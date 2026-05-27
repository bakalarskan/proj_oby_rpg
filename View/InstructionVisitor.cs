using System.Collections.Generic;
using lab_game.controller;
using lab_game.model;

namespace lab_game.view
{
    public class InstructionVisitor : IVisitor
    {
        public int WeaponCount { get; private set; } = 0;
        public int CurrencyCount { get; private set; } = 0;
        public int OtherCount { get; private set; } = 0;
        private GameKeys _gameKeys;

        public InstructionVisitor(GameKeys gameKeys)
        {
            _gameKeys = gameKeys;
        }

        public void Visit(Weapon weapon)
        {
            WeaponCount++;
        }
        public void Visit(Currency currency)
        {
            CurrencyCount++;
        }
        public void Visit(Other other)
        {
            OtherCount++;
        }

        public List<string> GetInstructions(bool isInventoryOpen, bool hasEnemies)
        {
            List<string> instructions = new List<string>();
            instructions.Add("INSTRUKCJA:");
            if (!isInventoryOpen)
            {
                instructions.Add($"-> {_gameKeys.GetKeysForDescription("poruszanie się")} - poruszanie się");
                instructions.Add($"-> {_gameKeys.GetKeysForDescription("otwórz/zamknij plecak")} - otwórz plecak");
                instructions.Add($"-> {_gameKeys.GetKeysForDescription("wyczyść tablicę")} - wyczyść Tablicę Ogłoszeń");
                if (WeaponCount > 0 || CurrencyCount > 0)
                {
                    instructions.Add($"-> {_gameKeys.GetKeysForDescription("podnieś przedmiot")} - podnieś przedmiot");
                }
                if (hasEnemies)
                {
                    instructions.Add($"-> {_gameKeys.GetKeysForDescription("rozpocznij walkę")} - walcz");
                }
            }
            else
            {
                instructions.Add($"-> {_gameKeys.GetKeysForDescription("otwórz/zamknij plecak")} - zamknij plecak");
                if (WeaponCount > 0 || CurrencyCount > 0 || OtherCount > 0)
                {
                    string right = _gameKeys.GetKeysForDescription("umieść do prawej ręki");
                    string left = _gameKeys.GetKeysForDescription("umieść do lewej ręki");
                    instructions.Add($"-> {right}/{left} - umieść do prawej/lewej ręki");
                    instructions.Add($"-> {_gameKeys.GetKeysForDescription("wyrzuć przedmiot z plecaka")} - wyrzuć przedmiot");
                }
                instructions.Add($"-> {_gameKeys.GetKeysForDescription("wybierz przedmiot")} - wybierz przedmiot");
            }
            return instructions;
        }
    }
}