using Battleship.Enums;
using Battleship.ShipPlacing;
using System.Collections.Generic;

namespace Battleship
{
    public interface IShipPlacer
    {
        void PlaceShips();
        bool CanPlace(ShipPlacementOptions options, HashSet<Cell> cellsLeft);
    }
}