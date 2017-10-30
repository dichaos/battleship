using Battleship.Enums;
using Battleship.ShipPlacing;
using System;
using System.Collections.Generic;

namespace Battleship
{
    public class RobotPlayer : IPlayer
    {
        private IBattleField _battleField;
        private IShipPlacer _shipPlacer;
        private IBoardDrawer _boardDrawer;
        private AttackMode _currentAttackMode;

        public bool Won => _battleField.ShipsLeft.Count == 0;

        private Cell _firstOfTheAfterHitPhase;

        public string Name => "Computer";

        public RobotPlayer(IBoardDrawer boardDrawer, IBattleField battleField, IShipPlacer shipPlacer)
        {
            _battleField = battleField;
            _shipPlacer = shipPlacer;
            _boardDrawer = boardDrawer;
        }

        public void Attack()
        {
            SetProbabilities();
            
            switch (_currentAttackMode)
            {
                case AttackMode.Exploratory:
                    if(AttackExploratory())
                        AttackAfterHit();
                    break;
                case AttackMode.AfterHit:
                    AttackAfterHit();
                    break;
            }

            _boardDrawer.DrawBoard(false);
        }

        public void SetBoard()
        {
            _shipPlacer.PlaceShips();
            _boardDrawer.DrawBoard(false);
        }

        #region Exploratory
        private bool AttackExploratory()
        {
            var cell = BestCell();
            
            switch(cell.HitCell())
            {
                case CellCondition.ShipBombed:
                    _currentAttackMode = AttackMode.AfterHit;
                    _firstOfTheAfterHitPhase = cell;
                    return true;
                case CellCondition.ShunkShip:
                    _currentAttackMode = AttackMode.Exploratory;
                    return true;
                default:
                    return false;
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

                        if (_shipPlacer.CanPlace(new ShipPlacementOptions{ShipSize = shipSize, Row = row, Col = col, Direction = Direction.West}, _battleField.CellsLeft))
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
                    if(_battleField[row,col].Probability > bestProb)
                    {
                        bestProb = _battleField[row, col].Probability;
                        bestCell = _battleField[row, col];
                    }
                }
            }

            return bestCell;
        }
        #endregion

        #region AfterHit
        private void AttackAfterHit()
        {
            ExecAttackMode(_firstOfTheAfterHitPhase, PickDirection(_firstOfTheAfterHitPhase));
        }
        private Direction PickDirection(Cell cell)
        {
            double northDirection = 0, southDirection = 0, eastDirection = 0, westDirection = 0;

            if(cell.Top && cell.Left)
            {
                if (_battleField[cell.Row - 1, cell.Col].Probability > _battleField[cell.Row, cell.Col - 1].Probability)
                    return Direction.East;
                else
                    return Direction.South;
            }

            if(cell.Top && cell.Right)
            {
                if (_battleField[cell.Row, cell.Col - 1].Probability > _battleField[cell.Row - 1, cell.Col].Probability)
                    return Direction.West;
                else
                    return Direction.South;
            }

            if(cell.Bottom && cell.Right)
            {
                if (_battleField[cell.Row + 1, cell.Col].Probability > _battleField[cell.Row, cell.Col + 1].Probability)
                    return Direction.West;
                else
                    return Direction.North;
            }

            if(cell.Bottom && cell.Left)
            {
                if (_battleField[cell.Row, cell.Col - 1].Probability > _battleField[cell.Row - 1, cell.Col].Probability)
                    return Direction.East;
                else
                    return Direction.North;
            }

            if (!cell.Top && _battleField[cell.Row, cell.Col - 1].Available) northDirection = _battleField[cell.Row, cell.Col - 1].Probability;
            if (!cell.Bottom && _battleField[cell.Row, cell.Col + 1].Available) southDirection = _battleField[cell.Row, cell.Col + 1].Probability;
            if (!cell.Left && _battleField[cell.Row - 1, cell.Col].Available) eastDirection = _battleField[cell.Row - 1, cell.Col].Probability;
            if (!cell.Right && _battleField[cell.Row + 1, cell.Col].Available) westDirection = _battleField[cell.Row + 1, cell.Col].Probability;

            if (northDirection >= southDirection && northDirection >= eastDirection && northDirection >= westDirection)
                return Direction.North;

            if (southDirection >= northDirection && southDirection >= eastDirection && southDirection >= westDirection)
                return Direction.South;

            if (eastDirection >= northDirection && eastDirection >= southDirection && eastDirection >= westDirection)
                return Direction.East;

            if (westDirection >= northDirection && westDirection >= southDirection && westDirection >= eastDirection)
                return Direction.West;

            return Direction.All;
        }
        private void ExecAttackMode(Cell cell, Direction dir)
        {
            var nextCoordinates = GetNextCoordinates(cell, dir);
            
            int rowToHit = nextCoordinates.Item1;
            int colToHit = nextCoordinates.Item2;
            dir = nextCoordinates.Item3;

            _boardDrawer.DrawBoard(false);
            
            switch (_battleField[rowToHit, colToHit].HitCell())
            {
                case CellCondition.ShipBombed:
                    SetProbabilities();
                    _boardDrawer.DrawProbabilityBoard();
                    ExecAttackMode(_battleField[rowToHit, colToHit], dir);
                    break;
                case CellCondition.ShunkShip:
                    if (!Won)
                    {
                        _currentAttackMode = AttackMode.Exploratory;
                        Attack();
                    }
                    break;
                default:
                    Console.WriteLine("");
                    break;
            }
        }

        private Tuple<int,int, Direction> GetNextCoordinates(Cell cell, Direction dir)
        {
            switch (dir)
            {
                case Direction.East:
                    if (cell.Left || !_battleField[cell.Row, cell.Col - 1].Available)
                        return new Tuple<int, int, Direction>(_firstOfTheAfterHitPhase.Row, _firstOfTheAfterHitPhase.Col + 1, Direction.West);
                    return new Tuple<int, int, Direction>(cell.Row, cell.Col - 1, dir);
                case Direction.West:
                    if(cell.Right || !_battleField[cell.Row, cell.Col + 1].Available) return new Tuple<int, int, Direction>(_firstOfTheAfterHitPhase.Row, _firstOfTheAfterHitPhase.Col - 1, Direction.East);
                    return new Tuple<int, int, Direction>(cell.Row, cell.Col + 1, dir);
                case Direction.North:
                    if (cell.Top || !_battleField[cell.Row - 1, cell.Col].Available) return new Tuple<int, int, Direction>(_firstOfTheAfterHitPhase.Row + 1, _firstOfTheAfterHitPhase.Col, Direction.South);
                    return new Tuple<int, int, Direction>(cell.Row - 1, cell.Col, Direction.South);
                case Direction.South:
                    if (cell.Bottom || !_battleField[cell.Row + 1, cell.Col].Available) return new Tuple<int, int, Direction>(_firstOfTheAfterHitPhase.Row - 1, _firstOfTheAfterHitPhase.Col, Direction.North);
                    return new Tuple<int, int, Direction>(cell.Row + 1, cell.Col, Direction.South);
                default:
                    throw new Exception("What happened here ?");
            }


        }
        #endregion
    }
}
