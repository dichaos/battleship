namespace Battleship
{
    public interface IBoardDrawer
    {
        void DrawBoard(bool hideLocations);
        void DrawProbabilityBoard();
    }
}