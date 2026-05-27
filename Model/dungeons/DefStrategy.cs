using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public sealed class DefaultGenerationStrategy : IDungeonGenerationStrategy
    {
        private const int MainRoomWidth = 5;
        private const int MainRoomHeight = 5;
        private const int RoomsCount = 5;

        public void Generate(IBuilder builder)
        {
            builder.FillWalls();
            builder.AddMainRoom(MainRoomWidth, MainRoomHeight);
            builder.AddRooms(RoomsCount);
            builder.AddCorridors();
        }
    }
}
