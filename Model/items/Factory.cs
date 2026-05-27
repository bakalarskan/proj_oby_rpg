using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab_game.model
{
    internal class Factory
    {
        private Random _rnd = new Random();
        private readonly IDungeonTheme _theme;
        public Factory() : this(new DefaultDungeonTheme())
        {
        }
        public Factory(IDungeonTheme theme)
        {
            _theme = theme;
        }
        public Item CreateRandomStuff()
        {
            Item item = _theme.CreateRandomStuff(_rnd);
            return ApplyRandomDecs(item);
            
        }
        public Item CreateRandomWeapon()
        {
            Item item = _theme.CreateRandomWeapon(_rnd);
            return ApplyRandomDecsW(item);
        }
        public Item CreateCurrency()
        {
            return _theme.CreateRandomCurrency(_rnd);
        }
        private Item ApplyRandomDecsW(Item item)
        {
            int isUnlucky = _rnd.Next(0, 2) ;
            int isStrong = _rnd.Next(0,2);
            if (isUnlucky == 0)
            {
                item = new Unlucky(item);
            }
            if (isStrong == 0)
            {
                item = new Strong(item);
            }
            return item;
        }
        private Item ApplyRandomDecs(Item item)
        {
            int isUnlucky = _rnd.Next(0, 2);
            if (isUnlucky == 0)
            {
                item = new Unlucky(item);
            }
            return item;
        }
    }
}
