namespace lab_game.dto
{
    public sealed class EnemyDto
    {
        public string Name { get; set; } = string.Empty;
        public char Symbol { get; set; }
        public PositionDto Position { get; set; } = new PositionDto();
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Damage { get; set; }
        public int Defense { get; set; }
    }
}