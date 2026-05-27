using System.Collections.Generic;

namespace lab_game.dto
{
    public sealed class TileDto
    {
        public bool CanWalk { get; set; }
        public List<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}