using Battleship.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.ShipPlacing
{
    public class ShipPlacementOptions
    {
        public ShipType ShipType { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int ShipSize { get; set; }
        public Direction Direction { get; set; }
        public bool Random { get; set; }
    }
}
