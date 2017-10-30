using Battleship.Enums;
using Battleship.ShipPlacing;
using System;
using System.Linq;

namespace Battleship
{
    public class Program
    {
        static void Main(string[] args)
        {
            int battlefieldSize = 10;

            var humanBatlefield = new BattleField(battlefieldSize);
            var robotBatlefield = new BattleField(battlefieldSize);

            var humanDrawer = new BoardDrawer(humanBatlefield, "Human");
            var robotDrawer = new BoardDrawer(robotBatlefield, "Computer");

            var allShips = Enum.GetValues(typeof(ShipType)).Cast<ShipType>().ToArray();

            //var humanPlacer = new HumanShipPlacer(allShips, humanBatlefield);
            var humanPlacer = new RandomShipPlacer(allShips, humanBatlefield);
            var robotPlacer = new RandomShipPlacer(allShips, robotBatlefield);

            var humanPlayer = new HumanPlayer(robotDrawer, robotBatlefield, robotPlacer);
            var robotPlayer = new RobotPlayer(humanDrawer, humanBatlefield, humanPlacer);

            var game = new Game(humanPlayer, robotPlayer);

            game.Play();

        }
    }
}
