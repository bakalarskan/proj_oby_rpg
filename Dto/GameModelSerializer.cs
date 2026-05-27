using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using lab_game.model;

namespace lab_game.dto
{
    public static class GameModelSerializer
    {
        public static string Serialize(GameModel model)
        {
            GameModelDto dto = ToDto(model);
            return JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
        }

        public static GameModelDto Deserialize(string json)
        {
            return JsonSerializer.Deserialize<GameModelDto>(json) ?? new GameModelDto();
        }

        public static GameModelDto ToDto(GameModel model)
        {
            return new GameModelDto
            {
                Board = ToDto(model.Board),
                Players = model.Players.Select(ToDto).ToList(),
                Enemies = model.Enemies.Select(ToDto).ToList(),
                EventLog = model.EventLog.ToList(),
                Combat = new CombatStateDto
                {
                    IsActive = model.Combat.IsActive,
                    PlayerName = model.Combat.Player?.Name,
                    EnemyName = model.Combat.Enemy?.Name
                }
            };
        }

        private static BoardDto ToDto(Board board)
        {
            BoardDto dto = new BoardDto
            {
                Width = board.Width,
                Height = board.Height
            };

            for (int y = 0; y < board.Height; y++)
            {
                List<TileDto> row = new List<TileDto>();
                for (int x = 0; x < board.Width; x++)
                {
                    Tile tile = board.GetTile(x, y);
                    TileDto tileDto = new TileDto
                    {
                        CanWalk = tile.CanWalk,
                        Items = board.GetItems(x, y).Select(ToDto).ToList()
                    };
                    row.Add(tileDto);
                }
                dto.Tiles.Add(row);
            }

            return dto;
        }

        private static PlayerDto ToDto(Player p)
        {
            return new PlayerDto
            {
                Name = p.Name,
                Symbol = p.Symbol,
                Position = ToDto(p.Pos),
                Health = p.Health,
                Strength = p.Strength,
                Agility = p.Agility,
                Luck = p.Luck,
                Aggression = p.Aggression,
                Intelligence = p.Intelligence,
                Coins = p.Coins,
                Ects = p.ECTS_points,
                LeftHand = p.LeftHand != null ? ToDto(p.LeftHand) : null,
                RightHand = p.RightHand != null ? ToDto(p.RightHand) : null,
                Inventory = p.Inventory.Select(ToDto).ToList()
            };
        }

        private static EnemyDto ToDto(Enemy e)
        {
            return new EnemyDto
            {
                Name = e.Name,
                Symbol = e.Symbol,
                Position = ToDto(e.Pos),
                Health = e.Health,
                MaxHealth = e.MaxHealth,
                Damage = e.Damage,
                Defense = e.Defense
            };
        }

        private static ItemDto ToDto(Item item)
        {
            Item baseItem = item.GetBaseItem();
            return new ItemDto
            {
                Name = baseItem.Name,
                DisplayName = item.Name,
                Category = item.Category,
                Damage = item.Damage,
                IsTwoHanded = item.IsTwoHanded,
                Symbol = item.Symbol,
                Position = ToDto(item.Pos),
                Effects = item.GetEffectNames().ToList(),
                SoundRange = item.SoundRange,
                SoundType = item.SoundType
            };
        }

        private static PositionDto ToDto(Base.Position pos)
        {
            return new PositionDto { X = pos.x, Y = pos.y };
        }
    }
}