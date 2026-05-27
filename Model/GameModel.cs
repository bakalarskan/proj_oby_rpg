using System;
using System.Collections.Generic;

namespace lab_game.model
{
    public sealed class GameModel
    {
        private readonly List<Player> _players = new List<Player>();
        public Board Board { get; }
        public IReadOnlyList<Player> Players => _players;
        public Player LocalPlayer { get; private set; }
        public IReadOnlyList<Enemy> Enemies => Board.Enemies;
        public IReadOnlyCollection<string> EventLog => Board.Communications;
        public CombatState Combat { get; } = new CombatState();

        public GameModel(Board board, Player localPlayer)
        {
            Board = board ?? throw new ArgumentNullException(nameof(board));
            LocalPlayer = localPlayer ?? throw new ArgumentNullException(nameof(localPlayer));
            _players.Add(localPlayer);
        }

        public void AddPlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            if (!_players.Contains(player))
            {
                _players.Add(player);
            }
        }

        public bool RemovePlayer(Player player)
        {
            return _players.Remove(player);
        }

        public IEnumerable<Item> GetAllItems()
        {
            for (int y = 0; y < Board.Height; y++)
            {
                for (int x = 0; x < Board.Width; x++)
                {
                    foreach (Item item in Board.GetItems(x, y))
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}