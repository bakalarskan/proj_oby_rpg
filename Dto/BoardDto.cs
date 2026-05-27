using System.Collections.Generic;

namespace lab_game.dto
{
    public sealed class BoardDto
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<List<TileDto>> Tiles { get; set; } = new List<List<TileDto>>();
    }
}