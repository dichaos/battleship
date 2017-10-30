using Battleship.Enums;
using System;

namespace Battleship
{
    public class Cell
    {
        public CellCondition Condition { get; private set; }
        public bool Available { get { return Condition == CellCondition.EmptySea; } }
        
        private Ship _ship;
        private int _size;
        private BattleField _battleField;

        public int Row { get; private set; }
        public int Col { get; private set; }
        public int Probability { get; set; }
        
        public Cell(int row, int col, int size, BattleField battlefield)
        {
            Condition = CellCondition.EmptySea;
            _ship = null;
            _battleField = battlefield;
            Row = row;
            Col = col;

            _size = size;
        }

        public CellCondition HitCell()
        {
            if (Condition == CellCondition.ShipLocation)
            {
                _ship.Hit();
                Probability = 0;
                Condition = CellCondition.ShipBombed;

                if (_ship.Shunk)
                {
                    _battleField.CellsLeft.Remove(this);
                    return CellCondition.ShunkShip;
                }
            }
            else if (Condition == CellCondition.EmptySea)
            {
                Probability = 0;
                Condition = CellCondition.EmptySeaBombed;
            }

            _battleField.CellsLeft.Remove(this);
            return Condition;
        }

        public void SetShipLocation(Ship ship)
        {
            _ship = ship;
            Condition = CellCondition.ShipLocation;
        }
        
        public bool Top { get { return Row == _size - 1; } }
        public bool Left { get { return Col == _size - 1;  } }

        public bool Bottom { get { return Row == 0; } }
        public bool Right { get { return Col == 0; } }

        public override string ToString()
        {
            switch (Condition)
            {
                case CellCondition.ShipBombed:
                    return "X";
                case CellCondition.EmptySeaBombed:
                    return "@"; 
                case CellCondition.EmptySea:
                    return "-";
                case CellCondition.ShipLocation:
                    return _ship.ToString()[0].ToString();
            }

            return "-";
        }
    }
}
