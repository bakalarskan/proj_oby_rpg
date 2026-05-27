using lab_game.model;

namespace lab_game.controller
{
    public sealed class LocalController : IGameController
    {
        private readonly KeyHandler _handler;

        public LocalController(GameModel model, GameKeys input)
        {
            var combatHandler = new CombatHandler(model);
            KeyHandler commandHandler = new CommandKeyHandler(input);
            combatHandler.SetNext(commandHandler);
            _handler = combatHandler;
        }

        public void HandleInput(ConsoleKey key, Player player, Board board, LocalModel local)
        {
            _handler.Handle(key, player, board, local);
        }
    }
}