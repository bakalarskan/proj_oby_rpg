using System;
using lab_game.model;

namespace lab_game.controller
{
    public sealed class NetworkController : IGameController
    {
        private readonly Action<ConsoleKey> _sendAction;

        public NetworkController(Action<ConsoleKey> sendAction)
        {
            _sendAction = sendAction ?? throw new ArgumentNullException(nameof(sendAction));
        }

        public void HandleInput(ConsoleKey key, Player player, Board board, LocalModel local)
        {
            _sendAction(key);
        }
    }
}