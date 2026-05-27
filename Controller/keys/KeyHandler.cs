using lab_game.model;
using lab_game.infrastructure;

namespace lab_game.controller
{
    public abstract class KeyHandler
    {
        protected KeyHandler? _nextHandler;

        public void SetNext(KeyHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public virtual void Handle(ConsoleKey key, Player p, Board b, LocalModel local)
        {
            _nextHandler?.Handle(key, p, b, local);
        }
    }
    public class CombatHandler : KeyHandler
    {
        private readonly GameModel _model;
        private readonly Combat _combat = new Combat();
        private readonly IAttackStrategy _normal = new Normal();
        private readonly IAttackStrategy _sneaky = new Sneaky();
        private readonly IAttackStrategy _magic = new Magic();

        private bool _isChoosing = false;
        private const ConsoleKey FightKey = ConsoleKey.F;

        public CombatHandler(GameModel model)
        {
            _model = model;
        }

        public override void Handle(ConsoleKey key, Player p, Board b, LocalModel local)
        {
            if (_isChoosing)
            {
                IAttackStrategy? s = TryGetStrategyFromKey(key);
                if (s != null)
                {
                    CombatRes? res = _combat.isEnemy(p, b, s);
                    if (res != null && !res.EnemyKilled && !res.PlayerKilled)
                    {
                        GameEvents.Notify(b, "Wybierz kolejny atak (1/2/3)");
                    }
                    else
                    {
                        _isChoosing = false;
                        _model.Combat.End();
                    }
                }
                else
                {
                    GameEvents.Notify(b, "Anulowano wybór");
                    _isChoosing = false;
                    _model.Combat.End();
                }
                return;
            }
            if (key == FightKey)
            {
                Enemy? e = b.GetEnemyAt(p.Pos.x, p.Pos.y);
                if (e == null)
                {
                    GameEvents.Notify(b, "Nie ma tu wroga");
                    return;
                }
                GameEvents.Notify(b, $"Walka z: {e.Name}");
                GameEvents.Notify(b, "Wybierz atak:");
                GameEvents.Notify(b, "1 - zwykły");
                GameEvents.Notify(b, "2 - skryty");
                GameEvents.Notify(b, "3 - magiczny");
                _model.Combat.Start(p, e);
                _isChoosing = true;
                return;
            }
            base.Handle(key, p, b, local);
        }
        private IAttackStrategy? TryGetStrategyFromKey(ConsoleKey key)
        {
            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
            {
                return _normal;
            }
            else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
            {
                return _sneaky;
            }
            else if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3)
            {
                return _magic;
            }
            else
            {
                return null;
            }
        }
    }

    public class CommandKeyHandler : KeyHandler
    {
        private GameKeys _input;

        public CommandKeyHandler(GameKeys input)
        {
            _input = input;
        }

        public override void Handle(ConsoleKey key, Player p, Board b, LocalModel local)
        {
            if (_input.KeyBindings.ContainsKey(key))
            {
                _input.KeyBindings[key].Execute(p, b, local);
            }
            else
            {
                GameEvents.Notify(b, "Nieznany klawisz!");
                base.Handle(key, p, b, local);
            }
        }
    }
}