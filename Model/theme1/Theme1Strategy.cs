using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace lab_game.model
{
    public sealed class Theme1Strategy : IDungeonGenerationStrategy
    {
        public void Generate(IBuilder builder)
        {
            builder.FillWalls();
            builder.AddMainRoom(5, 5); 
            builder.AddCorridors(); 
        }
    }
}
