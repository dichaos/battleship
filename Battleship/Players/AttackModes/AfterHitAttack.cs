using Battleship.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Players.AttackModes
{
    public class AfterHitAttack
    {
        private readonly IBattleField _battleField;
        
        public AfterHitAttack(IBattleField battleField)
        {
            _battleField = battleField ?? throw new ArgumentNullException(nameof(battleField));
        }

        public HitResult Attack(HitResult hitResult, Direction direction = Direction.All)
        {
            if (direction == Direction.All)
                direction = PickDirection(hitResult);

            var nextCoordinates = GetNextCell(hitResult, direction);

            direction = nextCoordinates.Item2;
            var cell = nextCoordinates.Item1;
            
            switch (cell.HitCell())
            {
                case CellCondition.ShipBombed:
                    hitResult.NextAfterHitCell = cell;
                    RemoveOtherDirections(hitResult, direction);
                    hitResult.Hit = true;
                    return Attack(hitResult, direction);
                case CellCondition.ShunkShip:
                    hitResult.AttackMode = AttackMode.Exploratory;
                    hitResult.Hit = true;
                    return hitResult;
                case CellCondition.EmptySeaBombed:
                    hitResult.DirectionsToExploit.Remove(direction);
                    hitResult.NextAfterHitCell = hitResult.FirstAfterHitCell;
                    hitResult.Hit = false;
                    return hitResult;
                default:
                    throw new Exception("Something went wrong");
            }
        }

        private Direction PickDirection(HitResult hitResult)
        {
            double northDirection = 0, southDirection = 0, eastDirection = 0, westDirection = 0;
            var cell = hitResult.NextAfterHitCell;

            foreach (var Direction in AvailableDirections(hitResult))
            {
                switch (Direction)
                {
                    case Direction.East:
                        eastDirection = _battleField[cell.Row + 1, cell.Col].Probability;
                        break;
                    case Direction.West:
                        westDirection = _battleField[cell.Row - 1, cell.Col].Probability;
                        break;
                    case Direction.North:
                        northDirection = _battleField[cell.Row, cell.Col - 1].Probability;
                        break;
                    case Direction.South:
                        southDirection = _battleField[cell.Row, cell.Col + 1].Probability;
                        break;
                }
            }

            if (northDirection >= southDirection && northDirection >= eastDirection && northDirection >= westDirection)
                return Direction.North;
            
            if (southDirection >= northDirection && southDirection >= eastDirection && southDirection >= westDirection)
                return Direction.South;

            if (eastDirection >= northDirection && eastDirection >= southDirection && eastDirection >= westDirection)
                return Direction.East;

            if (westDirection >= northDirection && westDirection >= southDirection && westDirection >= eastDirection)
                return Direction.West;

            throw new Exception("Could not get direction");
        }

        private HashSet<Direction> AvailableDirections(HitResult hitResult)
        {
            var cell = hitResult.NextAfterHitCell;

            var toReturn = new HashSet<Direction>();
            if (!cell.Top && _battleField[cell.Row, cell.Col - 1].AvailableToBomb && hitResult.DirectionsToExploit.Contains(Direction.North)) toReturn.Add(Direction.North);
            if (!cell.Bottom && _battleField[cell.Row, cell.Col + 1].AvailableToBomb && hitResult.DirectionsToExploit.Contains(Direction.South)) toReturn.Add(Direction.South);
            if (!cell.Left && _battleField[cell.Row - 1, cell.Col].AvailableToBomb && hitResult.DirectionsToExploit.Contains(Direction.West)) toReturn.Add(Direction.West);
            if (!cell.Right && _battleField[cell.Row + 1, cell.Col].AvailableToBomb && hitResult.DirectionsToExploit.Contains(Direction.East)) toReturn.Add(Direction.East);

            return toReturn;
        }

        private void RemoveOtherDirections(HitResult hitResult, Direction direction)
        {
            switch(direction)
            {
                case Direction.East:
                    hitResult.DirectionsToExploit.Remove(Direction.East);
                    hitResult.DirectionsToExploit.Remove(Direction.North);
                    hitResult.DirectionsToExploit.Remove(Direction.South);
                    break;
                case Direction.West:
                    hitResult.DirectionsToExploit.Remove(Direction.West);
                    hitResult.DirectionsToExploit.Remove(Direction.North);
                    hitResult.DirectionsToExploit.Remove(Direction.South);
                    break;
                case Direction.North:
                    hitResult.DirectionsToExploit.Remove(Direction.North);
                    hitResult.DirectionsToExploit.Remove(Direction.East);
                    hitResult.DirectionsToExploit.Remove(Direction.West);
                    break;
                case Direction.South:
                    hitResult.DirectionsToExploit.Remove(Direction.South);
                    hitResult.DirectionsToExploit.Remove(Direction.East);
                    hitResult.DirectionsToExploit.Remove(Direction.West);
                    break;
            }
        }

        private bool CanIMove(HitResult hitResult, Direction dir, out Cell nextCell)
        {
            var cell = hitResult.NextAfterHitCell;
            nextCell = null;

            switch (dir)
            {
                case Direction.East:
                    if (cell.Left || !_battleField[cell.Row, cell.Col - 1].AvailableToBomb)
                        return false;

                    nextCell = _battleField[cell.Row, cell.Col - 1];
                    return true;
                case Direction.West:
                    if (cell.Right || !_battleField[cell.Row, cell.Col + 1].AvailableToBomb)
                        return false;

                    nextCell = _battleField[cell.Row, cell.Col + 1];
                    return true;
                case Direction.North:
                    if (cell.Top || !_battleField[cell.Row - 1, cell.Col].AvailableToBomb)
                        return false;

                    nextCell = _battleField[cell.Row - 1, cell.Col];
                    return true;
                case Direction.South:
                    if (cell.Bottom || !_battleField[cell.Row + 1, cell.Col].AvailableToBomb)
                        return false;

                    nextCell = _battleField[cell.Row + 1, cell.Col];
                    return true;
                default:
                    throw new Exception("What happened here? I cannot move anywhere");
            }
        }
        
        private Tuple<Cell, Direction> GetNextCell(HitResult hitResult, Direction dir)
        {
            if (CanIMove(hitResult, dir, out Cell toReturn))
            {
                return new Tuple<Cell, Direction>(toReturn, dir);
            }
            else
            {
                hitResult.NextAfterHitCell = hitResult.FirstAfterHitCell;
                dir = hitResult.DirectionsToExploit.ToArray()[0];
                hitResult.DirectionsToExploit.Remove(dir);

                return GetNextCell(hitResult, dir);
            }
        }
    }
}
