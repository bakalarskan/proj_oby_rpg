namespace lab_game.dto
{
    public sealed class CombatStateDto
    {
        public bool IsActive { get; set; }
        public string? PlayerName { get; set; }
        public string? EnemyName { get; set; }
    }
}