using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public sealed class Theme3Strategy : IDungeonGenerationStrategy
    {
        public void Generate(IBuilder builder)
        {
            builder.FillWalls();
            builder.AddMainRoom(24, 12);
            builder.AddRooms(3);
            builder.AddCorridors();
        }
    }
}
