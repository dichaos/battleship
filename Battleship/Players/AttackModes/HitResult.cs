using Battleship.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Players.AttackModes
{
    public class HitResult
    {
        public Enums.AttackMode AttackMode { get; set; }
        public Cell FirstAfterHitCell { get; set; }

        public bool Hit { get; set; }
        public Cell NextAfterHitCell { get; set; }
        public HashSet<Direction> DirectionsToExploit { get; set; }

        public HitResult()
        {
            DirectionsToExploit = new HashSet<Direction>()
            {
                Direction.East,
                Direction.West,
                Direction.North,
                Direction.South
            };
        }
    }
}
