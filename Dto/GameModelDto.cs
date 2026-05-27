using System.Collections.Generic;

namespace lab_game.dto
{
    public sealed class GameModelDto
    {
        public BoardDto Board { get; set; } = new BoardDto();
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
        public List<EnemyDto> Enemies { get; set; } = new List<EnemyDto>();
        public List<string> EventLog { get; set; } = new List<string>();
        public CombatStateDto Combat { get; set; } = new CombatStateDto();
    }
}