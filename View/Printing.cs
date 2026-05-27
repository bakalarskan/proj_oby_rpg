using System;
using System.Collections.Generic;
using System.Text;
using lab_game.dto;
using lab_game.model;

namespace lab_game.view
{
    public class ConsoleView : IGameView
    {
        private int _lastFrameLineCount = 0;

        public void PrintIntro(int roomCount, int weaponCount, int currencyCount, int otherCount)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=====================================================");
            Console.WriteLine("                  WYDZIAŁ MiNI                     ");
            Console.WriteLine("=====================================================");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("Witaj Studencie! Jest koniec semestru, a Ty");
            Console.WriteLine("zostałeś uwięziony na wydziale po godzinach...");
            Console.WriteLine("Musisz zrobić wszystko aby zdać sesję.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("RAPORT Z DZIEKANATU:");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"- Wykryte otwarte sale: {roomCount}");
            Console.WriteLine($"- Porzucony sprzęt do obrony: {weaponCount}");
            Console.WriteLine($"- Przydatne przedmioty: {otherCount}");
            Console.WriteLine($"- Rozsypana waluta (ECTS i Kasa): {currencyCount}");
            Console.WriteLine();
            Console.WriteLine("Twoim celem jest przetrwać, zebrać punkty ECTS, pieniądze ");
            Console.WriteLine("na ewentualne warunki i nie dać się wyrzucić!");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Naciśnij [ENTER], aby rozpocząć koszmar...");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void Render(GameModel model, LocalModel local, IReadOnlyList<string> instructions)
        {
            if (local.ShowJournalRequested)
            {
                RenderJournal(model.EventLog);
                local.ShowJournalRequested = false;
                return;
            }

            Player p = model.LocalPlayer;
            Board b = model.Board;

            StringBuilder ui = UI(p, local, b);
            StringBuilder maze = Maze_title_com(b, p, instructions.ToList());

            printGame(ui, maze);
        }

        public void Render(GameModelDto dto)
        {
            StringBuilder ui = BuildDtoUi(dto);
            StringBuilder maze = BuildDtoMaze(dto);
            printGame(ui, maze);
        }
        private StringBuilder BuildDtoUi(GameModelDto dto)
        {
            StringBuilder ui = new StringBuilder();
            ui.AppendLine("┌" + new string('─', (Base.UIWidth - 6) / 2) + "GRACZE" + new string('─', Base.UIWidth - 6 - (Base.UIWidth - 6) / 2) + "┐");

            foreach (var p in dto.Players.Take(Base.InventoryLength))
            {
                string line = $"{p.Symbol} {p.Name} HP:{p.Health}";
                ui.AppendLine($"│{SafePad(line, Base.UIWidth)}│");
            }

            while (ui.ToString().Split('\n').Length < Base.InventoryLength + 2)
            {
                ui.AppendLine($"│{SafePad(" ", Base.UIWidth)}│");
            }

            ui.AppendLine("└" + new string('─', Base.UIWidth) + "┘");

            ui.AppendLine("┌" + new string('─', (Base.UIWidth - 16) / 2) + "TABLICA OGŁOSZEŃ" + new string('─', Base.UIWidth - 16 - (Base.UIWidth - 16) / 2) + "┐");

            int count = 0;
            foreach (var line in dto.EventLog.Take(Base.ComsLength))
            {
                ui.AppendLine($"│{SafePad(line, Base.UIWidth)}│");
                count++;
            }
            for (int i = count; i < Base.ComsLength; i++)
            {
                ui.AppendLine($"│{SafePad(" ", Base.UIWidth)}│");
            }

            ui.AppendLine("└" + new string('─', Base.UIWidth) + "┘");
            return ui;
        }
        private StringBuilder BuildDtoMaze(GameModelDto dto)
        {
            StringBuilder maze = new StringBuilder();
            maze.AppendLine("┌─────────────WYDZIAŁ MiNI───────────────┐");

            for (int y = 0; y < dto.Board.Height; y++)
            {
                maze.Append("│");
                for (int x = 0; x < dto.Board.Width; x++)
                {
                    char symbol = GetDtoSymbol(dto, x, y);
                    maze.Append(symbol);
                }
                maze.Append("│");
                maze.AppendLine();
            }

            maze.AppendLine("└────────────────────────────────────────┘");
            return maze;
        }
        private static char GetDtoSymbol(GameModelDto dto, int x, int y)
        {
            var player = dto.Players.FirstOrDefault(p => p.Position.X == x && p.Position.Y == y);
            if (player != null) return player.Symbol;

            var enemy = dto.Enemies.FirstOrDefault(e => e.Position.X == x && e.Position.Y == y);
            if (enemy != null) return enemy.Symbol;

            var tile = dto.Board.Tiles[y][x];
            if (tile.Items.Count > 0) return tile.Items[^1].Symbol;
            return tile.CanWalk ? ' ' : '█';
        }

        private void RenderJournal(IReadOnlyCollection<string> log)
        {
            List<string> coms = log.ToList();
            int pageSize = Math.Max(1, Console.WindowHeight - 5);
            int pageCount = Math.Max(1, (int)Math.Ceiling((double)coms.Count / pageSize));
            int currentPage = pageCount - 1;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Tablica Ogłoszeń");
                Console.ResetColor();

                if (coms.Count == 0)
                {
                    Console.WriteLine("Brak nowych ogłoszeń");
                }
                else
                {
                    int start = currentPage * pageSize;
                    int end = Math.Min(start + pageSize, coms.Count);
                    for (int i = start; i < end; i++)
                    {
                        Console.WriteLine(coms[i]);
                    }
                }

                Console.WriteLine();
                Console.WriteLine($"Strona {currentPage + 1}/{pageCount}");
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow && currentPage > 0)
                {
                    currentPage--;
                }
                else if (key == ConsoleKey.RightArrow && currentPage < pageCount - 1)
                {
                    currentPage++;
                }
                else if (key == ConsoleKey.J || key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }

        public void PrintOutro(string logPath)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Tablica Ogłoszeń zapisana w: {Path.GetFullPath(logPath)}");
            Console.ResetColor();
            Console.WriteLine("KONIEC GRY - naciśnij dowolny klawisz, aby zakończyć.");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        private string SafePad(string text, int width)
        {
            if (text.Length > width) return text.Substring(0, width);
            return text.PadRight(width);
        }

        private StringBuilder UI(Player p, LocalModel local, Board b)
        {
            StringBuilder ui = new StringBuilder();
            ui.AppendLine("┌" + new string('─', (Base.UIWidth - 5) / 2) + "STATS" + new string('─', Base.UIWidth - 5 - (Base.UIWidth - 5) / 2) + "┐");
            ui.AppendLine($"│{SafePad($"Zdrowie : {p.Health}", Base.UIWidth)}│");
            ui.AppendLine($"│{SafePad($"Inteligencja: {p.Intelligence}", Base.UIWidth)}│");
            ui.AppendLine($"│{SafePad($"Siła: {p.Strength}", Base.UIWidth)}│");
            ui.AppendLine($"│{SafePad($"Zwinność: {p.Agility}", Base.UIWidth)}│");
            ui.AppendLine($"│{SafePad($"Fart: {p.Luck}", Base.UIWidth)}│");
            ui.AppendLine($"│{SafePad($"Frustracja: {p.Aggression}", Base.UIWidth)}│");
            ui.AppendLine("└" + new string('─', Base.UIWidth) + "┘");

            ui.AppendLine("┌" + new string('─', (Base.UIWidth - 6) / 2) + "WALUTY" + new string('─', Base.UIWidth - 6 - (Base.UIWidth - 6) / 2) + "┐");
            ui.AppendLine($"│{SafePad($"Kasa na warunek: {p.Coins}", Base.UIWidth)}│");
            ui.AppendLine($"│{SafePad($"Punkty ECTS: {p.ECTS_points}", Base.UIWidth)}│");
            ui.AppendLine("└" + new string('─', Base.UIWidth) + "┘");

            ui.AppendLine("┌" + new string('─', (Base.UIWidth - 6) / 2) + "PLECAK" + new string('─', Base.UIWidth - 6 - (Base.UIWidth - 6) / 2) + "┐");

            string leftName = p.LeftHand != null ? $"{p.LeftHand.Name} ({p.LeftHand.Damage})" : "Brak";
            if (p.RightHand != null && p.RightHand.IsTwoHanded && p.LeftHand == null) leftName = "Zablokowane (Oburęczna)";
            ui.AppendLine($"│{SafePad($"L: {leftName}", Base.UIWidth)}│");

            string rightName = p.RightHand != null ? $"{p.RightHand.Name} ({p.RightHand.Damage})" : "Brak";
            if (p.LeftHand != null && p.LeftHand.IsTwoHanded && p.RightHand == null) rightName = "Zablokowane (Oburęczna)";
            ui.AppendLine($"│{SafePad($"P: {rightName}", Base.UIWidth)}│");
            ui.AppendLine($"│{SafePad(new string('-', Base.UIWidth), Base.UIWidth)}│");

            int start = local.InventoryIndex >= Base.InventoryLength ? local.InventoryIndex - (Base.InventoryLength - 1) : 0;
            for (int i = 0; i < Base.InventoryLength; i++)
            {
                int currIndex = start + i;
                if (currIndex < p.Inventory.Count)
                {
                    Item item = p.Inventory[currIndex];
                    string pointer = (local.IsInventoryOpen && currIndex == local.InventoryIndex) ? "> " : "- ";
                    string itemStr = $"{pointer}{item.Name}{item.Stat}";
                    ui.AppendLine($"│{SafePad(itemStr, Base.UIWidth)}│");
                }
                else
                {
                    ui.AppendLine($"│{SafePad(" ", Base.UIWidth)}│");
                }
            }
            ui.AppendLine("└" + new string('─', Base.UIWidth) + "┘");

            ui.AppendLine("┌" + new string('─', (Base.UIWidth - 16) / 2) + "TABLICA OGŁOSZEŃ" + new string('─', Base.UIWidth - 16 - (Base.UIWidth - 16) / 2) + "┐");
            Enemy? enemyOnTile = b.GetEnemyAt(p.Pos.x, p.Pos.y);
            Tile currTile = b.GetTile(p.Pos.x, p.Pos.y);
            string lyingItem = "";
            if (enemyOnTile != null)
            {
                lyingItem = $"UWAGA! Wróg: {enemyOnTile.Name}";
            }
            if (currTile.Stuff.Count > 0)
            {
                lyingItem = currTile.Stuff.Count == 1 ? $"Widzisz tu: {currTile.Stuff[0].Name}" : $"Widzisz tu {currTile.Stuff.Count} przedmioty";
            }
            ui.AppendLine($"│{SafePad(lyingItem, Base.UIWidth)}│");

            var com = b.Communications.ToArray();
            for (int i = 0; i < Base.ComsLength; i++)
            {
                if (i < com.Length) ui.AppendLine($"│{SafePad(com[i], Base.UIWidth)}│");
                else ui.AppendLine($"│{SafePad(" ", Base.UIWidth)}│");
            }
            ui.AppendLine("└" + new string('─', Base.UIWidth) + "┘");

            return ui;
        }

        private StringBuilder Maze_title_com(Board b, Player p, List<string> instructions)
        {
            StringBuilder maze = new StringBuilder();
            maze.AppendLine("┌─────────────WYDZIAŁ MiNI───────────────┐");
            for (int i = 0; i < b.Height; i++)
            {
                maze.Append("│");
                for (int j = 0; j < b.Width; j++)
                {
                    bool isPlayer = (p.Pos.x == j && p.Pos.y == i);
                    Enemy? enemy = b.GetEnemyAt(j, i);

                    if (isPlayer)
                    {
                        maze.Append(b.GetTile(j, i).GetSymbol(true));
                    }
                    else if (enemy != null)
                    {
                        maze.Append(enemy.Symbol);
                    }
                    else
                    {
                        maze.Append(b.GetTile(j, i).GetSymbol(false));
                    }
                }
                maze.Append("│");
                maze.AppendLine();
            }
            maze.AppendLine("└────────────────────────────────────────┘");

            maze.AppendLine("┌─────────────INSTRUKCJA─────────────────┐");
            int maxInstructionLines = 6;
            int linesAdded = 0;

            foreach (string line in instructions)
            {
                if (line == "INSTRUKCJA:") continue;
                maze.AppendLine($"│{SafePad(line, Base.MazeWidth)}│");
                linesAdded++;
            }

            while (linesAdded < maxInstructionLines)
            {
                maze.AppendLine($"│{SafePad(" ", Base.MazeWidth)}│");
                linesAdded++;
            }
            maze.AppendLine("└────────────────────────────────────────┘");
            return maze;
        }

        private void printGame(StringBuilder ui, StringBuilder maze)
        {
            Console.SetCursorPosition(0, 0);

            string[] UI = ui.ToString().TrimEnd('\r', '\n').Replace("\r", "").Split('\n');
            string[] Maze = maze.ToString().TrimEnd('\r', '\n').Replace("\r", "").Split('\n');

            int maxLines = Math.Max(UI.Length, Maze.Length);

            for (int i = 0; i < maxLines; i++)
            {
                string mazeLine = i < Maze.Length ? Maze[i] : new string(' ', Base.MazeWidth + 2);
                string uiLine = i < UI.Length ? UI[i] : string.Empty;

                PrintMazeColored(mazeLine);
                Console.Write("  ");
                Console.Write(uiLine);
                Console.Write("          ");

                if (i < maxLines - 1)
                {
                    Console.WriteLine();
                }
            }

            for (int i = maxLines; i < _lastFrameLineCount; i++)
            {
                Console.Write(new string(' ', 80));
                Console.WriteLine();
            }

            _lastFrameLineCount = maxLines;
        }

        private void PrintMazeColored(string line)
        {
            ConsoleColor currColor = Console.ForegroundColor;

            foreach (char c in line)
            {
                ConsoleColor nextColor = ConsoleColor.Gray;
                switch (c)
                {
                    case '█':
                        nextColor = ConsoleColor.DarkGray;
                        break;
                    case '¶':
                        nextColor = ConsoleColor.Magenta;
                        break;
                    case '$':
                        nextColor = ConsoleColor.Green;
                        break;
                    case '★':
                        nextColor = ConsoleColor.Yellow;
                        break;
                    case '◘':
                        nextColor = ConsoleColor.Cyan;
                        break;
                    case '∑':
                        nextColor = ConsoleColor.Gray;
                        break;
                    case '○':
                        nextColor = ConsoleColor.DarkYellow;
                        break;
                    case 'Φ':
                        nextColor = ConsoleColor.Red;
                        break;
                    case '<':
                        nextColor = ConsoleColor.Blue;
                        break;
                    case '■':
                        nextColor = ConsoleColor.DarkCyan;
                        break;
                }

                if (nextColor != currColor)
                {
                    Console.ForegroundColor = nextColor;
                    currColor = nextColor;
                }
                Console.Write(c);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}