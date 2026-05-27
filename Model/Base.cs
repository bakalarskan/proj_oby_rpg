using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public class Base
    {
        public const int UIWidth =  55;
        public const int MazeWidth = 40;
        public const int MazeHeight = 20;
        public const int ComsLength = 7; // długość tablicy ogłoszeń
        public const int start_x = 0;
        public const int start_y = 0;
        public const int InventoryLength = 3; // ile itemów wyświetla się
        public readonly struct Position
        {
            public int x { get; }
            public int y { get; }
            public Position(int x_n, int y_n)
            {
                x = x_n;
                y = y_n;
            }
        }

    }

}
