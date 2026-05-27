using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace lab_game.model
{
    public abstract class Tile
    {
        public abstract bool CanWalk { get; }
        public abstract char GetSymbol(bool IsPlayer);
        private readonly List<Item> _stuff = new List<Item>(); // lista itemów na tym polu
        public IReadOnlyList<Item> Stuff => _stuff;
        public void AddStuff(Item item)
        {
            _stuff.Add(item);
        }
        public bool RemoveStuff(Item item)
        {
            return _stuff.Remove(item);
        }

    }

    public class Wall : Tile
    {
        public override bool CanWalk => false;
        public override char GetSymbol(bool IsPlayer)
        {
            return '█';
        }
    }
    public class Floor : Tile
    {
        public override bool CanWalk => true;
        public override char GetSymbol(bool IsPlayer)
        {
            if (IsPlayer)
            {
                return '¶';
            }
            else if (Stuff.Count > 0)
            {
                return Stuff.Last().Symbol;
            }
            else
            {
                return ' ';
            }
        }
    }

    public class Board
    {
        private readonly Random _rnd = new Random();
        private readonly Events _events = new Events();
        public Events Events => _events;
        public int Height { get; }
        public int Width { get; }
        private readonly Tile[,] _tiles;
        public IReadOnlyList<Item> GetItems(int x, int y)
        {
            return GetTile(x, y).Stuff;
        }
        private readonly List<Enemy> _enemies = new List<Enemy>();
        public IReadOnlyList<Enemy> Enemies => _enemies;
        public Board()
        {
            Height = Base.MazeHeight;
            Width = Base.MazeWidth;
            _tiles = new Tile[Height, Width];
        }
        public Item? GetItemAt(int x, int y, int index)
        {
            var items = GetItems(x, y);
            return (index >= 0 && index < items.Count) ? items[index] : null;
        }
        public void AddItem(int x, int y, Item item)
        {
            GetTile(x, y).AddStuff(item);
        }
        public bool RemoveItem(int x, int y, Item item)
        {
            return GetTile(x,y).RemoveStuff(item);
        }
        public bool IsInside(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
        public Tile GetTile(int x, int y)
        {
            if (IsInside(x, y))
            {
                return _tiles[y, x];
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pozycja poza planszą");
            }
        }
        public void SetTile(int x, int y, Tile tile)
        {
            if (IsInside(x, y))
            {
                _tiles[y, x] = tile;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Pozycja poza planszą");
            }
        }
        public bool CanWalk(int x, int y)
        {
            if (IsInside(x, y))
            {
                return _tiles[y, x].CanWalk;
            }
            else
            {
                return false;
            }
        }
        public Enemy? GetEnemyAt(int x, int y)
        {
            return _enemies.FirstOrDefault(e => e.Pos.x == x && e.Pos.y == y && e.IsAlive);
        }
        public void AddEnemy(Enemy enemy)
        {
            if (!IsInside(enemy.Pos.x, enemy.Pos.y))
            {
                throw new ArgumentOutOfRangeException("Pozycja wroga poza planszą");
            }
            if (!CanWalk(enemy.Pos.x, enemy.Pos.y))
            {
                throw new InvalidOperationException("Nie można umieścić wroga na ścianie");
            }
            if (GetEnemyAt(enemy.Pos.x, enemy.Pos.y) != null)
            {
                throw new InvalidOperationException("Na tej pozycji już jest wróg");
            }
            _enemies.Add(enemy);
            Events.SoundPublisher.Subscribe(enemy);
        }
        public bool RemoveEnemy(Enemy enemy)
        {
            Events.SoundPublisher.Unsubscribe(enemy);
            return _enemies.Remove(enemy);
        }
        public bool RemoveEnemyAt(int x, int y)
        {
            var enemy = GetEnemyAt(x, y);
            if (enemy != null)
            {
                return RemoveEnemy(enemy);
            }
            return false;
        }
        public Queue<string> Communications { get; private set; } = new Queue<string>();

        public void AddCom(string msg)
        {
            Communications.Enqueue(msg);
            if (Communications.Count > Base.ComsLength)
            {
                Communications.Dequeue();
            }
        }
        public void ClearCom()
        {
            Communications.Clear();
        }
        public IReadOnlyDictionary<Base.Position, int> GetDistancesInRange(Base.Position source, int maxRange)
        {
            Dictionary<Base.Position, int> result = new Dictionary<Base.Position, int>();
            if (!IsInside(source.x, source.y))
            {
                return result;
            }

            int[,] dist = new int[Height, Width];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    dist[y, x] = -1;
                }
            }

            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
            dist[source.y, source.x] = 0;
            queue.Enqueue((source.x, source.y));
            result[new Base.Position(source.x, source.y)] = 0;

            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };

            while (queue.Count > 0)
            {
                var (cx, cy) = queue.Dequeue();
                int cd = dist[cy, cx];
                if (cd >= maxRange)
                {
                    continue;
                }

                for (int i = 0; i < 4; i++)
                {
                    int nx = cx + dx[i];
                    int ny = cy + dy[i];

                    if (!IsInside(nx, ny))
                    {
                        continue;
                    }
                    if (!GetTile(nx, ny).CanWalk)
                    {
                        continue;
                    }
                    if (dist[ny, nx] != -1)
                    {
                        continue;
                    }

                    dist[ny, nx] = cd + 1;
                    if (dist[ny, nx] <= maxRange)
                    {
                        result[new Base.Position(nx, ny)] = dist[ny, nx];
                        queue.Enqueue((nx, ny));
                    }
                }
            }

            return result;
        }
        public void MoveEnemies(Player p)
        {
            foreach (var e in _enemies.Where(e => e.IsAlive).ToList())
            {
                var directions = new List<(int dx, int dy)>
                {
                    (0, -1),
                    (1, 0), 
                    (0, 1),  
                    (-1, 0)  
                }.OrderBy(_ => _rnd.Next()).ToList();
                foreach (var (dx, dy) in directions)
                {
                    int nx = e.Pos.x + dx;
                    int ny = e.Pos.y + dy;
                    if (IsInside(nx, ny) && CanWalk(nx, ny) && GetEnemyAt(nx, ny) == null)
                    {
                        e.MoveTo(new Base.Position(nx, ny));
                        break;
                    }
                }
            }
        }
    }
}
