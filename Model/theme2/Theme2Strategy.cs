using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    public sealed class Theme2Strategy : IDungeonGenerationStrategy
    {
        public void Generate(IBuilder builder)
        {
            builder.FillWalls();
            builder.AddMainRoom(5, 5);

            builder.AddRooms(12);

            builder.AddCorridors();
        }
    }
}
