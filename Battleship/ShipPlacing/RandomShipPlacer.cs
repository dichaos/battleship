using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Enums;

namespace Battleship.ShipPlacing
{
    public class RandomShipPlacer : ShipPlacer, IShipPlacer
    {
        public RandomShipPlacer(ShipType[] ships, BattleField battlefield) : base(ships, battlefield)
        {
        }
        
        private void PlaceShipRandomly(Ship ship)
        {
            if (ship.Size > _battleField.Size)
                throw new Exception("Ship is longer than the board");

            Random random = new Random(Guid.NewGuid().GetHashCode());

            while (true)  // random placement, nothing fancy, possibility of infinite loop if ship doesn't fit
            {
                int row = random.Next(_battleField.Size - 1);
                int col = random.Next(_battleField.Size - 1);

                if (_battleField[row, col].Condition == CellCondition.EmptySea)
                {
                    Direction dir = (Direction)random.Next(0, 4);

                    if ((dir == Direction.East || dir == Direction.West) && (_lstSpacePerRow[row] < ship.Size))
                        continue;

                    if ((dir == Direction.South || dir == Direction.North) && (_lstSpacePerCol[col] < ship.Size))
                        continue;

                    if (TryPlaceShip(ship, row, col, dir))
                        break;
                }
            }
        }

        public void PlaceShips()
        {
            foreach (var ship in _ships)
            {
                PlaceShipRandomly(new Ship(ship));
            }
        }
    }
}
