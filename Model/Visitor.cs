namespace lab_game.model
{
    public interface IVisitor
    {
        void Visit(Weapon weapon);
        void Visit(Currency currency);
        void Visit(Other other);
    }
}