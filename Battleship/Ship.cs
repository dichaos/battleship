using Battleship.Enums;
using System.Collections.Generic;

namespace Battleship
{
    public class Ship
    {
        public bool Shunk { get { return HitsTaken == Size; } }

        private int HitsTaken;
        public Direction Direct { get; private set; }
        public ShipType Type { get; private set; }
        
        public List<Cell> Cells { get; private set; }
        public int Size { get { return (int)this.Type; } }
        
        public void Hit()
        {
            if(!Shunk)
                HitsTaken++;
        }

        public Ship(ShipType _type)
        {
            this.Type = _type;
            
            Cells = new List<Cell>();
            HitsTaken = 0;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
