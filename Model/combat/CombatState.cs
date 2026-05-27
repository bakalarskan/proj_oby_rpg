namespace lab_game.model
{
    public sealed class CombatState
    {
        public bool IsActive { get; private set; }
        public Player? Player { get; private set; }
        public Enemy? Enemy { get; private set; }

        public void Start(Player player, Enemy enemy)
        {
            Player = player;
            Enemy = enemy;
            IsActive = true;
        }

        public void End()
        {
            Player = null;
            Enemy = null;
            IsActive = false;
        }
    }
}