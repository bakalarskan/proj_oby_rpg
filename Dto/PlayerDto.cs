using System.Collections.Generic;

namespace lab_game.dto
{
    public sealed class PlayerDto
    {
        public string Name { get; set; } = string.Empty;
        public char Symbol { get; set; }
        public PositionDto Position { get; set; } = new PositionDto();
        public int Health { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public int Aggression { get; set; }
        public int Intelligence { get; set; }
        public int Coins { get; set; }
        public int Ects { get; set; }
        public ItemDto? LeftHand { get; set; }
        public ItemDto? RightHand { get; set; }
        public List<ItemDto> Inventory { get; set; } = new List<ItemDto>();
    }
}