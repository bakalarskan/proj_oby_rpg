using System.Collections.Generic;
using lab_game.model;
using lab_game.infrastructure;

namespace lab_game.controller
{
    public interface ICommand
    {
        string Description { get; }
        void Execute(Player p, Board b, LocalModel local);
    }
    public class MoveCommand : ICommand
    {
        private int _dx, _dy;
        public string Description => "poruszanie się";

        public MoveCommand(int dx, int dy)
        {
            _dx = dx;
            _dy = dy;
        }

        public void Execute(Player p, Board b, LocalModel local)
        {
            if (!local.IsInventoryOpen)
            {
                int newX = p.Pos.x + _dx;
                int newY = p.Pos.y + _dy;

                if (newX < 0 || newX >= b.Width || newY < 0 || newY >= b.Height)
                {
                    GameEvents.Notify(b, "Nie można tam iść, jest ściana!");
                    return;
                }
                if (b.GetTile(newX, newY).CanWalk)
                {
                    p.MoveTo(new Base.Position(newX, newY));
                    b.MoveEnemies(p);
                }
                else
                {
                    GameEvents.Notify(b, "Nie można tam iść, jest ściana!");
                }
            }
        }
    }
    public class PickUpCommand : ICommand
    {
        public string Description => "podnieś przedmiot";

        public void Execute(Player p, Board b, LocalModel local)
        {
            if (!local.IsInventoryOpen && b.GetItems(p.Pos.x, p.Pos.y).Count > 0)
            {
                Item item = b.GetItems(p.Pos.x, p.Pos.y)[0];
                p.PickUpItem(item, b);
                b.RemoveItem(p.Pos.x, p.Pos.y, item);
            }
        }
    }
    public class ManageInventoryCommand : ICommand
    {
        public string Description => "otwórz/zamknij plecak";

        public void Execute(Player p, Board b, LocalModel local)
        {
            local.IsInventoryOpen = !local.IsInventoryOpen;
            GameEvents.Notify(b, local.IsInventoryOpen ? "Otwarto plecak" : "Zamknięto plecak");
        }
    }
    public class DropItemCommand : ICommand
    {
        public string Description => "wyrzuć przedmiot z plecaka";

        public void Execute(Player p, Board b, LocalModel local)
        {
            if (local.IsInventoryOpen && p.Inventory.Count > 0)
            {
                bool dropped = p.DropItemAt(local.InventoryIndex, b);
                if (dropped && local.InventoryIndex >= p.Inventory.Count && local.InventoryIndex > 0)
                {
                    local.InventoryIndex--;
                }
                if (p.Inventory.Count == 0)
                {
                    local.IsInventoryOpen = false;
                }
            }
        }
    }
    public class EquipCommand : ICommand
    {
        private char _hand;
        public string Description => _hand == 'R' ? "umieść do prawej ręki" : "umieść do lewej ręki";

        public EquipCommand(char hand)
        {
            _hand = hand;
        }

        public void Execute(Player p, Board b, LocalModel local)
        {
            if (local.IsInventoryOpen && p.Inventory.Count > 0)
            {
                Item itemToEquip = p.Inventory[local.InventoryIndex];
                p.EquipItem(itemToEquip, b, _hand);
            }
        }
    }
    public class SelectItemCommand : ICommand
    {
        private int _direction;
        public string Description => "wybierz przedmiot";

        public SelectItemCommand(int direction)
        {
            _direction = direction;
        }

        public void Execute(Player p, Board b, LocalModel local)
        {
            if (local.IsInventoryOpen && p.Inventory.Count > 0)
            {
                local.InventoryIndex += _direction;
                if (local.InventoryIndex < 0) local.InventoryIndex = p.Inventory.Count - 1;
                if (local.InventoryIndex >= p.Inventory.Count) local.InventoryIndex = 0;
            }
        }
    }
    public class ClearCommentsCommand : ICommand
    {
        public string Description => "wyczyść tablicę";

        public void Execute(Player p, Board b, LocalModel local)
        {
            b.ClearCom();
        }
    }
    public class ShowJournalCommand : ICommand
    {
        public string Description => "pokaż dziennik";
        public void Execute(Player p, Board b, LocalModel local)
        {
            local.ShowJournalRequested = true;
        }
    }
}