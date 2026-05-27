using System.Text.Json;
using lab_game.model;

namespace lab_game.dto
{
    public static class NetworkUpdateSerializer
    {
        public static string Serialize(NetworkUpdateDto update)
        {
            return JsonSerializer.Serialize(update, new JsonSerializerOptions { WriteIndented = false });
        }

        public static NetworkUpdateDto Deserialize(string json)
        {
            return JsonSerializer.Deserialize<NetworkUpdateDto>(json) ?? new NetworkUpdateDto();
        }

        public static NetworkUpdateDto CreateState(GameModel model)
        {
            return new NetworkUpdateDto
            {
                Type = "state",
                State = GameModelSerializer.ToDto(model)
            };
        }

        public static NetworkUpdateDto CreateAction(PlayerActionDto action)
        {
            return new NetworkUpdateDto
            {
                Type = "action",
                Action = action
            };
        }
    }
}