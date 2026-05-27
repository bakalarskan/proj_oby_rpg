using System.Collections.Generic;
using lab_game.model;

namespace lab_game.view
{
    public class InstructionVisitor : IVisitor
    {
        public int WeaponCount { get; private set; }
        public int CurrencyCount { get; private set; }
        public int OtherCount { get; private set; }
        private readonly IReadOnlyDictionary<string, string> _keys;

        public InstructionVisitor(IReadOnlyDictionary<string, string> keys)
        {
            _keys = keys;
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
                instructions.Add($"-> {GetKey("poruszanie się")} - poruszanie się");
                instructions.Add($"-> {GetKey("otwórz/zamknij plecak")} - otwórz plecak");
                instructions.Add($"-> {GetKey("wyczyść tablicę")} - wyczyść Tablicę Ogłoszeń");
                if (WeaponCount > 0 || CurrencyCount > 0)
                {
                    instructions.Add($"-> {GetKey("podnieś przedmiot")} - podnieś przedmiot");
                }
                if (hasEnemies)
                {
                    instructions.Add($"-> {GetKey("rozpocznij walkę")} - walcz");
                }
            }
            else
            {
                instructions.Add($"-> {GetKey("otwórz/zamknij plecak")} - zamknij plecak");
                if (WeaponCount > 0 || CurrencyCount > 0 || OtherCount > 0)
                {
                    string right = GetKey("umieść do prawej ręki");
                    string left = GetKey("umieść do lewej ręki");
                    instructions.Add($"-> {right}/{left} - umieść do prawej/lewej ręki");
                    instructions.Add($"-> {GetKey("wyrzuć przedmiot z plecaka")} - wyrzuć przedmiot");
                }
                instructions.Add($"-> {GetKey("wybierz przedmiot")} - wybierz przedmiot");
            }
            return instructions;
        }

        private string GetKey(string description)
        {
            return _keys.TryGetValue(description, out string key) ? key : "???";
        }
    }
}