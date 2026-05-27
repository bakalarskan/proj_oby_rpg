using lab_game.model;

namespace lab_game.controller
{
    public interface IGameController
    {
        void HandleInput(ConsoleKey key, Player player, Board board, LocalModel local);
    }
}