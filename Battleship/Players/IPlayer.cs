namespace Battleship
{
    public interface IPlayer
    {
        bool Won { get; }
        string Name { get; }
        void Attack();
        void SetBoard();
    }
}