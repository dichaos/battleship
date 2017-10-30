using Battleship.Enums;
using System.Collections.Generic;

namespace Battleship
{
    public interface IBattleField
    {
        Cell this[int x, int y] { get; }

        int Size { get; }
        HashSet<ShipType> ShipsLeft { get; }
        HashSet<Cell> CellsLeft { get; }
    }
}