using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_game;


namespace lab_game.model
{
    public interface IBuilder
    {
        int RoomCount { get; }
        void buildEmptyDungeon();
        void FillWalls();
        void AddCorridors();
        void AddRooms(int number);
        void AddMainRoom(int width, int height);
        void AddStuff(int number);
        void AddWeapons(int number);
        void AddCurrency(int number);
        void AddEnemies(int number);
        void AddArtifact(Item artifact);
        Board BuildDungeon();
    }

    public class Director
    {
        private const int MainRoomWidth = 5;
        private const int MainRoomHeight = 5;
        private const int RoomsCount = 5;
        private const int StuffCount = 20;
        private const int WeaponsCount = 10;
        private const int CurrencyCount = 15;
        private const int EnemiesCount = 5;
        public void Construct(IBuilder builder, IDungeonTheme theme)
        {
            theme.GenerationStrategy.Generate(builder);
            builder.AddStuff(theme.StuffCount);
            builder.AddWeapons(theme.WeaponsCount);
            builder.AddCurrency(theme.CurrencyCount);
            builder.AddEnemies(theme.EnemiesCount);
            builder.AddArtifact(theme.CreateArtifact());
        }

        public void ConstructEmptyDungeon(IBuilder builder, IDungeonTheme theme)
        {
            builder.buildEmptyDungeon();
            builder.AddWeapons(theme.WeaponsCount);
            builder.AddStuff(theme.StuffCount);
            builder.AddCurrency(theme.CurrencyCount);
            builder.AddEnemies(theme.EnemiesCount);
            builder.AddArtifact(theme.CreateArtifact());
        }
    }

    public class ConcreteBuilder : IBuilder
    {
        private Board _board;
        private Factory _itemFactory;
        private EnemyFactory _enemyFactory;
        private readonly IDungeonTheme _theme;
        private Random _rnd = new Random();
        private bool _isInitialized;

        private List<(int x, int y)> _roomCenters = new List<(int x, int y)>();
        public int RoomCount => _roomCenters.Count;

        public ConcreteBuilder(IDungeonTheme theme)
        {
            _theme = theme;
            _board = new Board();
            _itemFactory = new Factory(theme);
            _enemyFactory = new EnemyFactory(theme);
        }
        public void buildEmptyDungeon()
        {
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    _board.SetTile(x, y, new Floor());
                }
            }
            _roomCenters.Clear();
            _isInitialized = true;
        }
        public void FillWalls()
        {
            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    _board.SetTile(x, y, new Wall());
                }
            }
            _roomCenters.Clear();
            _isInitialized = true;
        }
        public void AddMainRoom(int width, int height)
        {
            IsInitialized();
            int startX = (_board.Width - width) / 2;
            int startY = (_board.Height - height) / 2;
            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    _board.SetTile(x, y, new Floor());
                }
            }
            _roomCenters.Add((startX + width / 2, startY + height / 2));
        }
        public void AddRooms(int number)
        {
            IsInitialized();
            for (int i = 0; i < number; i++)
            {
                int size = _rnd.Next(1, 5);

                int maxRandomX = Math.Max(2, _board.Width - size - 1);
                int maxRandomY = Math.Max(2, _board.Height - size - 1);

                int startX = _rnd.Next(1, maxRandomX);
                int startY = _rnd.Next(1, maxRandomY);

                int endX = Math.Min(_board.Width, startX + size);
                int endY = Math.Min(_board.Height, startY + size);

                for (int y = startY; y < endY; y++)
                {
                    for (int x = startX; x < endX; x++)
                    {
                        _board.SetTile(x, y, new Floor());
                    }
                }
                _roomCenters.Add((startX + size/ 2, startY + size / 2));
            }
        }
        public void AddCorridors()
        {
            IsInitialized();
            CarveMaze(0, 0);

            if (_roomCenters.Count > 0)
            {
                CarveCorridor(Base.start_x, Base.start_y, _roomCenters[0].x, _roomCenters[0].y);

                for (int i = 0; i < _roomCenters.Count - 1; i++)
                {
                    CarveCorridor(_roomCenters[i].x, _roomCenters[i].y, _roomCenters[i + 1].x, _roomCenters[i + 1].y);
                }
            }
        }

        private void CarveMaze(int x, int y)
        {
            _board.SetTile(x, y, new Floor());

            var directions = new List<(int dx, int dy)>
            {
                (0, -2), (2, 0), (0, 2), (-2, 0)
            };

            directions = directions.OrderBy(d => _rnd.Next()).ToList();

            foreach (var dir in directions)
            {
                int nx = x + dir.dx;
                int ny = y + dir.dy;

                if (nx >= 0 && nx < _board.Width && ny >= 0 && ny < _board.Height)
                {
                    if (!_board.GetTile(nx, ny).CanWalk)
                    {
                        _board.SetTile(x + dir.dx / 2, y + dir.dy / 2, new Floor());
                        CarveMaze(nx, ny);
                    }
                }
            }
        }

        private void CarveCorridor(int x1, int y1, int x2, int y2)
        {
            int minX = Math.Min(x1, x2);
            int maxX = Math.Max(x1, x2);
            for (int x = minX; x <= maxX; x++)
            {
                if (y1 >= 0 && y1 < _board.Height && x >= 0 && x < _board.Width)
                    _board.SetTile(x, y1, new Floor());
            }

            int minY = Math.Min(y1, y2);
            int maxY = Math.Max(y1, y2);
            for (int y = minY; y <= maxY; y++)
            {
                if (y >= 0 && y < _board.Height && x2 >= 0 && x2 < _board.Width)
                    _board.SetTile(x2, y, new Floor());
            }
        }
        public void AddStuff(int number)
        {
            IsInitialized();
            for (int i = 0; i < number; i++)
            {
                Item item = _itemFactory.CreateRandomStuff();
                PlaceR(item);
            }
        }
        public void AddWeapons(int number)
        {
            IsInitialized();
            for (int i = 0; i < number; i++)
            {
                Item weapon = _itemFactory.CreateRandomWeapon();
                PlaceR(weapon);
            }
        }

        public void AddCurrency(int number)
        {
            IsInitialized();
            for (int i = 0; i < number; i++)
            {
                Item currency = _itemFactory.CreateCurrency();
                PlaceR(currency);
            }
        }
        public void AddEnemies(int number)
        {
            IsInitialized();
            for (int i = 0; i < number; i++)
            {
                PlaceEnemyR();
            }
        }
        public void AddArtifact(Item artifact)
        {
            IsInitialized();
            bool placed = PlaceR(artifact);
            if (!placed)
                throw new InvalidOperationException("artefakt nie został umieszczony");
        }
        private bool PlaceEnemyR(int maxAttempts = 2000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                int x = _rnd.Next(0, _board.Width);
                int y = _rnd.Next(0, _board.Height);
                if (!_board.GetTile(x, y).CanWalk || _board.GetEnemyAt(x, y) != null || (x == Base.start_x && y == Base.start_y) || _board.GetItems(x,y).Count > 0)
                {
                    continue;
                }
                Enemy enemy = _enemyFactory.CreateRandomEnemy(new Base.Position(x, y));
                _board.AddEnemy(enemy);
                return true;
            }
            return false;
        }
        private bool PlaceR(Item item, int maxAttempts = 2000)
        {
            for (int i = 0; i < maxAttempts; i++)
            {
                int x = _rnd.Next(0, _board.Width);
                int y = _rnd.Next(0, _board.Height);

                if (_board.GetTile(x, y).CanWalk && _board.GetEnemyAt(x, y) == null)
                {
                    _board.AddItem(x, y, item);
                    item.Pos = new Base.Position(x, y);
                    return true;
                }
            }
            return false;
        }
        public Board BuildDungeon()
        {
            IsInitialized();
            _board.SetTile(Base.start_x, Base.start_y, new Floor());
            return _board;
        }
        private void IsInitialized()
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Najpierw wywołaj buildEmptyDungeon albo FillWalls");
        }
    }
}