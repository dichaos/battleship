using Battleship.Enums;
using System;
using System.Collections.Generic;

namespace Battleship
{

    public class BattleField : IBattleField
    {
        private Cell[,] _board;
        private List<Ship> _ships;
        private HashSet<ShipType> _shipsLeft;
        private HashSet<Cell> _cellsLeft;
        public Cell this[int x, int y]
        {
            get
            {
                return _board[x,y];
            }
        }
        public int Size { get; private set; }
        
        public HashSet<ShipType> ShipsLeft { get { return _shipsLeft; } }
        public HashSet<Cell> CellsLeft { get { return _cellsLeft; } }

        public BattleField(int _size)
        {
            Size = _size;
            _shipsLeft = new HashSet<ShipType>();
            _cellsLeft = new HashSet<Cell>();
            _board = new Cell[Size, Size];

            Size = _size;

            for (int j = 0; j < Size; j++)
            {
                for (int k = 0; k < Size; k++)
                {
                    _board[j, k] = new Cell(j, k, Size, this);
                    _cellsLeft.Add(_board[j, k]);
                }
            }
            
            this._ships = new List<Ship>();
        }
        
        internal void AddShip(Ship ship)
        {
            _ships.Add(ship);
            _shipsLeft.Add(ship.Type);
        }
    }
}
