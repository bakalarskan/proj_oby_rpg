using System.Text;
using lab_game.model;
using lab_game.controller;
using lab_game.view;

namespace lab_game.infrastructure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ConfigPath = "gameconfig.json";
            Config config = Loader.Load(ConfigPath);
            Player student = new Player(config.PlayerName);
            Directory.CreateDirectory(config.LogDirectory);
            IGameLogger logger = LoggerFactory.CreateDefault(config.PlayerName, config.LogDirectory);
            GameLog.SetLogger(logger);
            string logPath = logger.LogFilePath;

            IGameView view = new ConsoleView();
            IDungeonTheme theme = ThemeFactory.CreateRandom();
            IBuilder mapBuilder = new ConcreteBuilder(theme);
            Director director = new Director();
            director.Construct(mapBuilder, theme);
            Board building = mapBuilder.BuildDungeon();

            GameModel model = new GameModel(building, student);
            Player localPlayer = model.LocalPlayer;
            Board board = model.Board;
            LocalModel localState = new LocalModel();

            GameKeys input = new GameKeys();
            Dictionary<string, string> keyMap = new Dictionary<string, string>
            {
                { "poruszanie się", input.GetKeysForDescription("poruszanie się") },
                { "otwórz/zamknij plecak", input.GetKeysForDescription("otwórz/zamknij plecak") },
                { "wyczyść tablicę", input.GetKeysForDescription("wyczyść tablicę") },
                { "podnieś przedmiot", input.GetKeysForDescription("podnieś przedmiot") },
                { "rozpocznij walkę", input.GetKeysForDescription("rozpocznij walkę") },
                { "umieść do prawej ręki", input.GetKeysForDescription("umieść do prawej ręki") },
                { "umieść do lewej ręki", input.GetKeysForDescription("umieść do lewej ręki") },
                { "wyrzuć przedmiot z plecaka", input.GetKeysForDescription("wyrzuć przedmiot z plecaka") },
                { "wybierz przedmiot", input.GetKeysForDescription("wybierz przedmiot") }
            };
            IGameController controller = new LocalController(model, input);

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(100, 38);
                Console.SetBufferSize(100, 38);
            }

            InstructionVisitor instructionVisitor = new InstructionVisitor(keyMap);
            for (int i = 0; i < board.Height; i++)
            {
                for (int j = 0; j < board.Width; j++)
                {
                    foreach (var item in board.GetItems(j, i))
                    {
                        item.Accept(instructionVisitor);
                    }
                }
            }

            view.PrintIntro(mapBuilder.RoomCount, instructionVisitor.WeaponCount, instructionVisitor.CurrencyCount, instructionVisitor.OtherCount);
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
                // pusta pętla, czekamy na wciśnięcie Enter
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine(theme.IntroMessage);
            Console.ResetColor();
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
                // pusta pętla, czekamy na wciśnięcie Enter
            }

            Console.Clear();
            while (true)
            {
                if (!localPlayer.isAlive)
                {
                    break;
                }

                InstructionVisitor visitor = new InstructionVisitor(keyMap);
                for (int i = 0; i < board.Height; i++)
                {
                    for (int j = 0; j < board.Width; j++)
                    {
                        foreach (var item in board.GetItems(j, i))
                        {
                            item.Accept(visitor);
                        }
                    }
                }

                List<string> currInstructions = visitor.GetInstructions(localState.IsInventoryOpen, board.Enemies.Count > 0);
                view.Render(model, localState, currInstructions);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                controller.HandleInput(keyInfo.Key, localPlayer, board, localState);
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }

            List<string> currInstructions_end = instructionVisitor.GetInstructions(localState.IsInventoryOpen, board.Enemies.Count > 0);
            view.Render(model, localState, currInstructions_end);

            view.PrintOutro(logPath);
        }
    }
}