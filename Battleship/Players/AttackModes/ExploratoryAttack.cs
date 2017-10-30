using Battleship.Enums;
using Battleship.ShipPlacing;
using System;
using System.Collections.Generic;

namespace Battleship.Players.AttackModes
{
    public class ExploratoryAttack
    {
        private readonly IBattleField _battleField;
        private readonly IShipPlacer _shipPlacer;

        public ExploratoryAttack(IBattleField battleField, IShipPlacer shipPlacer)
        {
            _battleField = battleField ?? throw new ArgumentNullException(nameof(battleField));
            _shipPlacer = shipPlacer ?? throw new ArgumentNullException(nameof(shipPlacer));
        }

        public HitResult Attack()
        {
            SetProbabilities();

            var cell = BestCell();
            
            switch (cell.HitCell())
            {
                case CellCondition.ShipBombed:
                    return new HitResult()
                    {
                        Hit = true,
                        AttackMode = AttackMode.AfterHit,
                        FirstAfterHitCell = cell,
                        NextAfterHitCell = cell,
                    };
                case CellCondition.ShunkShip:
                    return new HitResult()
                    {
                        Hit = true,
                        AttackMode = AttackMode.Exploratory,
                        FirstAfterHitCell = cell,
                        NextAfterHitCell = cell
                    };
                default:
                    return new HitResult()
                    {
                        AttackMode = AttackMode.Exploratory,
                        Hit = false,
                    };
            }
        }

        private void SetProbabilities()
        {
            for (int row = 0; row < _battleField.Size; row++)
                for (int col = 0; col < _battleField.Size; col++)
                    _battleField[row, col].Probability = 0;

            for (int row = 0; row < _battleField.Size; row++)
            {
                for (int col = 0; col < _battleField.Size; col++)
                {
                    if (_battleField[row, col].Condition == CellCondition.EmptySeaBombed || _battleField[row, col].Condition == CellCondition.ShipBombed)
                    {
                        _battleField[row, col].Probability = 0;
                        continue;
                    }

                    HashSet<ShipType> ships = _battleField.ShipsLeft;

                    foreach (ShipType s in ships)
                    {
                        int shipSize = (int)s;

                        if (_shipPlacer.CanPlace(new ShipPlacementOptions { ShipSize = shipSize, Row = row, Col = col, Direction = Direction.West }, _battleField.CellsLeft))
                            for (int i = col; i >= col - (shipSize - 1); i--)
                                _battleField[row, i].Probability++;

                        if (_shipPlacer.CanPlace(new ShipPlacementOptions { ShipSize = shipSize, Row = row, Col = col, Direction = Direction.East }, _battleField.CellsLeft))
                            for (int i = col; i <= col + (shipSize - 1); i++)
                                _battleField[row, i].Probability++;

                        if (_shipPlacer.CanPlace(new ShipPlacementOptions { ShipSize = shipSize, Row = row, Col = col, Direction = Direction.North }, _battleField.CellsLeft))
                            for (int i = row; i >= row - (shipSize - 1); i--)
                                _battleField[i, col].Probability++;

                        if (_shipPlacer.CanPlace(new ShipPlacementOptions { ShipSize = shipSize, Row = row, Col = col, Direction = Direction.South }, _battleField.CellsLeft))
                            for (int i = row; i <= row + (shipSize - 1); i++)
                                _battleField[i, col].Probability++;
                    }
                }
            }
        }

        private Cell BestCell()
        {
            Cell bestCell = _battleField[0, 0];
            double bestProb = bestCell.Probability;

            for (int row = 0; row < _battleField.Size; row++)
            {
                for (int col = 0; col < _battleField.Size; col++)
                {
                    if (_battleField[row, col].Probability > bestProb)
                    {
                        bestProb = _battleField[row, col].Probability;
                        bestCell = _battleField[row, col];
                    }
                }
            }

            return bestCell;
        }
    }
}
