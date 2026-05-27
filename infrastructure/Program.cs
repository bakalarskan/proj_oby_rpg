using System.Text;
using lab_game.controller;
using lab_game.dto;
using lab_game.model;
using lab_game.network;
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

            ConsoleView consoleView = new ConsoleView();
            IGameView view = consoleView;
            IDungeonTheme theme = ThemeFactory.CreateRandom();
            IBuilder mapBuilder = new ConcreteBuilder(theme);
            Director director = new Director();
            director.Construct(mapBuilder, theme);
            Board building = mapBuilder.BuildDungeon();

            GameModel model = new GameModel(building, student);
            Player localPlayer = model.LocalPlayer;
            Board board = model.Board;
            LocalModel localState = new LocalModel();

            Dictionary<string, LocalModel> playerLocalStates = new Dictionary<string, LocalModel>
            {
                [localPlayer.Name] = localState
            };

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

            StartupMode mode = GetMode(args, out string host, out int port);

            ClientHost? client = null;
            GameModelDto? latestState = null;
            if (mode == StartupMode.Client)
            {
                client = new ClientHost(host, port);
                latestState = client.ConnectAndReceiveInitialState();
                consoleView.Render(latestState);

                client.StartReceiveLoop(update =>
                {
                    if (update.State != null)
                    {
                        latestState = update.State;
                        consoleView.Render(update.State);
                    }
                });
            }

            ServerHost? server = null;
            if (mode == StartupMode.Server)
            {
                server = new ServerHost(model, port);
                server.Start();
            }

            IGameController controller = mode == StartupMode.Client
                ? new NetworkController(key =>
                {
                    if (client == null)
                    {
                        return;
                    }

                    PlayerActionDto action = new PlayerActionDto
                    {
                        PlayerName = config.PlayerName,
                        Key = key.ToString()
                    };
                    client.SendAction(action);
                })
                : new LocalController(model, input);

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(100, 38);
                Console.SetBufferSize(100, 38);
            }

            InstructionVisitor? instructionVisitor = null;
            if (mode != StartupMode.Client)
            {
                instructionVisitor = new InstructionVisitor(keyMap);
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
            }

            while (true)
            {
                if (mode == StartupMode.Client)
                {
                    if (latestState != null)
                    {
                        consoleView.Render(latestState);
                    }

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    controller.HandleInput(keyInfo.Key, localPlayer, board, localState);

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        break;
                    }

                    continue;
                }

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

                ConsoleKeyInfo keyInfoLocal = Console.ReadKey(true);
                controller.HandleInput(keyInfoLocal.Key, localPlayer, board, localState);

                if (server != null)
                {
                    while (server.TryDequeueAction(out PlayerActionDto remoteAction))
                    {
                        if (!Enum.TryParse(remoteAction.Key, out ConsoleKey remoteKey))
                        {
                            continue;
                        }

                        Player remotePlayer = model.Players.FirstOrDefault(p => p.Name == remoteAction.PlayerName)
                            ?? new Player(remoteAction.PlayerName);

                        if (!model.Players.Contains(remotePlayer))
                        {
                            model.AddPlayer(remotePlayer);
                        }

                        if (!playerLocalStates.TryGetValue(remotePlayer.Name, out LocalModel remoteLocal))
                        {
                            remoteLocal = new LocalModel();
                            playerLocalStates[remotePlayer.Name] = remoteLocal;
                        }

                        controller.HandleInput(remoteKey, remotePlayer, board, remoteLocal);
                        server.BroadcastAction(remoteAction);
                        server.BroadcastState();
                    }
                }

                if (server != null)
                {
                    PlayerActionDto action = new PlayerActionDto
                    {
                        PlayerName = localPlayer.Name,
                        Key = keyInfoLocal.Key.ToString()
                    };
                    server.BroadcastAction(action);
                    server.BroadcastState();
                }

                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }

            if (mode == StartupMode.Client)
            {
                client?.Disconnect();
                return;
            }

            if (instructionVisitor != null)
            {
                List<string> currInstructions_end = instructionVisitor.GetInstructions(localState.IsInventoryOpen, board.Enemies.Count > 0);
                view.Render(model, localState, currInstructions_end);
            }

            view.PrintOutro(logPath);

            server?.Stop();
        }

        private static StartupMode GetMode(string[] args, out string host, out int port)
        {
            host = "127.0.0.1";
            port = 5555;

            if (args.Length >= 1 && args[0].Equals("-server", StringComparison.OrdinalIgnoreCase))
            {
                if (args.Length >= 2 && int.TryParse(args[1], out int parsed))
                {
                    port = parsed;
                }
                return StartupMode.Server;
            }

            if (args.Length >= 1 && args[0].Equals("-client", StringComparison.OrdinalIgnoreCase))
            {
                if (args.Length >= 2)
                {
                    string[] parts = args[1].Split(':');
                    if (parts.Length == 2)
                    {
                        host = parts[0];
                        int.TryParse(parts[1], out port);
                    }
                }
                return StartupMode.Client;
            }

            Console.WriteLine("Uruchomić jako (S)erwer czy (K)lient?");
            ConsoleKey key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.K)
            {
                return StartupMode.Client;
            }
            return StartupMode.Server;
        }

        private enum StartupMode
        {
            Server,
            Client
        }
    }
}