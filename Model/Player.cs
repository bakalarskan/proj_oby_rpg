namespace lab_game.model
{
    public class Player
    {
        public bool isAlive => Health > 0;
        public string Name { get; private set; }
        public char Symbol { get; private set; }
        public Base.Position Pos { get; private set; }
        private int _startStrength { get; set; }
        public int Strength => _startStrength + (LeftHand?.StrengthBonus ?? 0) + (RightHand?.StrengthBonus ?? 0);
        private int _startAgility { get; set; }
        public int Agility => _startAgility + (LeftHand?.AgilityBonus ?? 0) + (RightHand?.AgilityBonus ?? 0);
        private int _startHealth { get; set; }
        public int Health { get; set; }
        private int _startLuck { get; set; }
        public int Luck => _startLuck + (LeftHand?.LuckBonus ?? 0) + (RightHand?.LuckBonus ?? 0);
        private int _startAggression { get; set; }
        public int Aggression => _startAggression + (LeftHand?.AggressionBonus ?? 0) + (RightHand?.AggressionBonus ?? 0);
        private int _startIntelligence { get; set; }
        public int Intelligence => _startIntelligence + (LeftHand?.IntelligenceBonus ?? 0) + (RightHand?.IntelligenceBonus ?? 0);
        public Item? LeftHand { get; private set; }
        public Item? RightHand { get; private set; }
        public List<Item> Inventory { get; private set; }
        public int Coins { get; private set; }
        public int ECTS_points { get; private set; }
        public Player() : this("Student")
        {
        }
        public Player(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Student" : name.Trim();
            Symbol = '¶';
            Pos = new Base.Position(Base.start_x, Base.start_y);
            _startStrength = 10;
            _startAgility = 10;
            _startHealth = 100;
            _startLuck = 10;
            _startAggression = 10;
            _startIntelligence = 20;
            LeftHand = null;
            RightHand = null;
            Inventory = new List<Item>();
            Coins = 0;
            ECTS_points = 0;
            Health = _startHealth;
        }

        public void SetSymbol(char symbol)
        {
            Symbol = symbol;
        }

        public void GetPoint(int amount)
        {
            ECTS_points += amount;
        }

        public void GetCoins(int amount)
        {
            Coins += amount;
        }
        public void MoveTo(Base.Position newPosition)
        {
            Pos = newPosition;
        }
        public void PickUpItem(Item item, Board b)
        {
            item.PickUp(this, b);
        }
        public void EquipRight(Item? w)
        {
            RightHand = w;
        }

        public void EquipLeft(Item? w)
        {
            LeftHand = w;
        }

        public bool DropItemAt(int index, Board b)
        {
            if (index < 0 || index >= Inventory.Count)
            {
                return false;
            }

            Item item = Inventory[index];
            Inventory.RemoveAt(index);

            item.Pos = new Base.Position(Pos.x, Pos.y);

            b.AddItem(Pos.x, Pos.y, item);

            b.AddCom($"Wyrzucono: {item.Name}");
            return true;
        }
        public void EquipItem(Item item, Board b, char hand)
        {
            if (hand == 'R')
            {
                if (LeftHand != null && LeftHand.IsTwoHanded)
                {
                    b.AddCom("Ręce zajęte!");
                    return;
                }
                if (item.IsTwoHanded && LeftHand != null)
                {
                    b.AddCom("Ręce zajęte!");
                    return;
                }
                if (RightHand != null)
                {
                    Inventory.Add(RightHand);
                }

                EquipRight(item);
                Inventory.Remove(item);
                EmitSound(item, b);
                b.AddCom($"Wyposażono: {item.Name}");
            }
            else if (hand == 'L')
            {
                if (RightHand != null && RightHand.IsTwoHanded)
                {
                    b.AddCom("Ręce zajęte!");
                    return;
                }
                if (item.IsTwoHanded && RightHand != null)
                {
                    b.AddCom("Ręce zajęte!");
                    return;
                }
                if (LeftHand != null)
                {
                    Inventory.Add(LeftHand);
                }

                EquipLeft(item);
                Inventory.Remove(item);
                EmitSound(item, b);
                b.AddCom($"Wyposażono: {item.Name}");
            }
        }
        public int TakeDamage(int damage)
        {
            int newDamage = Math.Max(0, damage);
            if (Health - newDamage < 0)
            {
                newDamage = Health;
            }
            Health -= newDamage;
            return newDamage;
        }
        private void EmitSound(Item item, Board b)
        {
            if (item.SoundRange > 0)
            {
                b.Events.SoundPublisher.Notify(Pos, item.SoundRange, item.SoundType, b);
            }
        }
    }
}