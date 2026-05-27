using System.Collections.Generic;

namespace lab_game.dto
{
    public sealed class ItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Damage { get; set; }
        public bool IsTwoHanded { get; set; }
        public char Symbol { get; set; }
        public PositionDto Position { get; set; } = new PositionDto();
        public List<string> Effects { get; set; } = new List<string>();
        public int SoundRange { get; set; }
        public string SoundType { get; set; } = string.Empty;
    }
}