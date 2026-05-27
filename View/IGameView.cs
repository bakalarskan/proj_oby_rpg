using lab_game.model;

namespace lab_game.view
{
    public interface IGameView
    {
        void PrintIntro(int roomCount, int weaponCount, int currencyCount, int otherCount);
        void Render(GameModel model, LocalModel local, IReadOnlyList<string> instructions);
        void PrintOutro(string logPath);
    }
}