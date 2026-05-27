using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_game;
using lab_game.model;
using lab_game.infrastructure;
using lab_game.view;

namespace lab_game.controller
{
    public class GameKeys
    {
        public Dictionary<ConsoleKey, ICommand> KeyBindings { get; private set; }
        public Dictionary<ConsoleKey, string> CombatBindings { get; private set; }


        public GameKeys()
        {
            KeyBindings = new Dictionary<ConsoleKey, ICommand>
            {
                { ConsoleKey.W, new MoveCommand(0, -1) },
                { ConsoleKey.S, new MoveCommand(0, 1) },
                { ConsoleKey.A, new MoveCommand(-1, 0) },
                { ConsoleKey.D, new MoveCommand(1, 0) },

                { ConsoleKey.E, new PickUpCommand() },
                { ConsoleKey.P, new ManageInventoryCommand() },
                { ConsoleKey.Q, new DropItemCommand() },
                { ConsoleKey.C, new ClearCommentsCommand() },
                { ConsoleKey.J, new ShowJournalCommand() },

                { ConsoleKey.R, new EquipCommand('R') },
                { ConsoleKey.L, new EquipCommand('L') },

                { ConsoleKey.UpArrow, new SelectItemCommand(-1) },
                { ConsoleKey.DownArrow, new SelectItemCommand(1) }
            };
            CombatBindings = new Dictionary<ConsoleKey, string>
            {
                { ConsoleKey.F, "rozpocznij walkę" },
                { ConsoleKey.NumPad1, "atak zwykły" },
                { ConsoleKey.D1, "atak zwykły" },
                { ConsoleKey.D2, "atak skryty" },
                { ConsoleKey.NumPad2, "atak skryty" },
                { ConsoleKey.NumPad3, "atak magiczny" },
                { ConsoleKey.D3, "atak magiczny" }

            };
        }

        public string GetKeysForDescription(string description)
        {
            var commandMatches = KeyBindings
                .Where(k => k.Value.Description == description)
                .Select(k => k.Key switch
                {
                    ConsoleKey.UpArrow => "↑",
                    ConsoleKey.DownArrow => "↓",
                    _ => k.Key.ToString()
                });
            var combatMatches = CombatBindings
                .Where(k => k.Value == description)
                .Select(k => FormatKey(k.Key));

            string result = string.Join(",", commandMatches.Concat(combatMatches).Distinct());
            return string.IsNullOrEmpty(result) ? "???" : result;
        }
        private static string FormatKey(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.UpArrow => "↑",
                ConsoleKey.DownArrow => "↓",
                _ => key.ToString()
            };
        }
    }
}
