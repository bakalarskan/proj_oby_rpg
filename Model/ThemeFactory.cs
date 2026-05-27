using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public static class ThemeFactory
    {
        private static readonly Random _rnd = new Random();

        public static IDungeonTheme CreateRandom()
        {
            return _rnd.Next(0, 3) switch
            {
                0 => new Theme1(),
                1 => new Theme2(),
                _ => new Theme3()
            };
        }

        public static IDungeonTheme CreateByName(string? name)
        {
            return (name ?? string.Empty).Trim().ToLowerInvariant() switch
            {
                "theme1" => new Theme1(),
                "theme2" => new Theme2(),
                "theme3" => new Theme3(),
                _ => CreateRandom()
            };
        }
    }
}
