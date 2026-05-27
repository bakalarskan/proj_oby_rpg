namespace lab_game.dto
{
    public sealed class NetworkUpdateDto
    {
        public string Type { get; set; } = "state"; // "state" | "action"
        public GameModelDto? State { get; set; }
        public PlayerActionDto? Action { get; set; }
    }
}