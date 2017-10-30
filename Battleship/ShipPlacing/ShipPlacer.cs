using Battleship.Enums;
using Battleship.ShipPlacing;
using System.Collections.Generic;

namespace Battleship
{
    public abstract class ShipPlacer
    {
        protected readonly ShipType[] _ships;
        protected readonly IBattleField _battleField;

        protected List<int> _lstSpacePerRow;
        protected List<int> _lstSpacePerCol;

        public ShipPlacer(ShipType[] ships, IBattleField battlefield)
        {
            _ships = ships;
            _battleField = battlefield;

            _lstSpacePerRow = new List<int>();
            _lstSpacePerCol = new List<int>();

            for (int i = 0; i < _battleField.Size; i++)
            {
                _lstSpacePerRow.Add(_battleField.Size);
                _lstSpacePerCol.Add(_battleField.Size);
            }
        }

        public bool CanPlace(ShipPlacementOptions options, HashSet<Cell> cellsLeft = null)
        {
            switch (options.Direction)
            {
                case Direction.North:
                    if (options.Row - (options.ShipSize - 1) < 0)
                        return false;
                    else
                    {
                        for (int i = options.Row; i >= options.Row - (options.ShipSize - 1); i--)
                        {
                            if ((!_battleField[i, options.Col].AvailableToPlaceShip && cellsLeft == null) || (cellsLeft != null && !cellsLeft.Contains(_battleField[i, options.Col])))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                case Direction.East:
                    if (options.Col + (options.ShipSize - 1) >= _battleField.Size)
                        return false;
                    else
                    {
                        for (int i = options.Col; i <= options.Col + (options.ShipSize - 1); i++)
                        {
                            if ((!_battleField[options.Row, i].AvailableToPlaceShip && cellsLeft == null) || (cellsLeft != null && !cellsLeft.Contains(_battleField[options.Row, i])))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                case Direction.South:
                    if (options.Row + (options.ShipSize - 1) >= _battleField.Size)
                        return false;
                    else
                    {
                        for (int i = options.Row; i <= options.Row + (options.ShipSize - 1); i++)
                        {
                            if ((!_battleField[i, options.Col].AvailableToPlaceShip && cellsLeft == null) || (cellsLeft != null && !cellsLeft.Contains(_battleField[i, options.Col])))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                case Direction.West:
                    if (options.Col - (options.ShipSize - 1) < 0)
                        return false;
                    else
                    {
                        for (int i = options.Col; i >= options.Col - (options.ShipSize - 1); i--)
                        {
                            if ((!_battleField[options.Row, i].AvailableToPlaceShip && cellsLeft == null) || (cellsLeft != null && !cellsLeft.Contains(_battleField[options.Row, i])))
                            {
                                return false;
                            }
                        }
                    }
                    return true;
            }

            return false;
        }

        protected bool TryPlaceShip(Ship ship, int row, int col, Direction dir)
        {
            if (CanPlace(new ShipPlacementOptions()
                            {
                                ShipSize = ship.Size,
                                Row = row,
                                Col = col,
                                Direction = dir
                            }))
            {
                PlaceShip(ship, row, col, dir);
                _battleField.AddShip(ship);
                return true;
            }

            return false;

        }
        protected void PlaceShip(Ship ship, int row, int col, Direction dir)
        {
            switch (dir)
            {
                case Direction.West:
                    for (int i = col; i >= col - (ship.Size - 1); i--)
                    {
                        _battleField[row, i].SetShipLocation(ship);
                        ship.Cells.Add(_battleField[row, i]);
                        _lstSpacePerCol[i]--;
                    }
                    _lstSpacePerRow[row] = _lstSpacePerRow[row] - ship.Size;
                    break;
                case Direction.East:
                    for (int i = col; i <= col + (ship.Size - 1); i++)
                    {
                        _battleField[row, i].SetShipLocation(ship);
                        ship.Cells.Add(_battleField[row, i]);
                        _lstSpacePerCol[i]--;
                    }

                    _lstSpacePerRow[row] = _lstSpacePerRow[row] - ship.Size;
                    break;
                case Direction.North:
                    for (int i = row; i >= row - (ship.Size - 1); i--)
                    {
                        _battleField[i, col].SetShipLocation(ship);
                        ship.Cells.Add(_battleField[i, col]);
                        _lstSpacePerRow[i]--;
                    }

                    _lstSpacePerCol[col] = _lstSpacePerCol[col] - ship.Size;
                    break;
                case Direction.South:
                    for (int i = row; i <= row + (ship.Size - 1); i++)
                    {
                        _battleField[i, col].SetShipLocation(ship);
                        ship.Cells.Add(_battleField[i, col]);
                        _lstSpacePerRow[i]--;
                    }

                    _lstSpacePerCol[col] = _lstSpacePerCol[col] - ship.Size;
                    break;

            }
        }
    }
}
