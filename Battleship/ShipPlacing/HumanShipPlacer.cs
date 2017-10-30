using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battleship.Enums;

namespace Battleship.ShipPlacing
{
    public class HumanShipPlacer : ShipPlacer, IShipPlacer
    {
        public HumanShipPlacer(ShipType[] ships, BattleField battlefield) : base(ships, battlefield)
        {
        }

        public void PlaceShips()
        {
            foreach(var shipType in _ships)
            {
                while (true)
                {
                    Console.WriteLine($"Placing ship {shipType} of size { (int)shipType} Select Row, Col and Orientation (H for Horizontal, V for Vertical) for exampe (5,3)H or EXIT to exit");
                    var input = Console.ReadLine();

                    if (input == "EXIT")
                        throw new Exception("bye");

                    var direction = Direction.West;

                    if (input[input.Length - 1] == 'V' || input[input.Length - 1] == 'v')
                        direction = Direction.South;


                    if (!int.TryParse(input.Substring(input.IndexOf("("), input.IndexOf(",") - input.IndexOf("(")), out int row))
                    {
                        Console.WriteLine("Could not understad row");
                        continue;
                    }

                    if (!int.TryParse(input.Substring(input.IndexOf(","), input.IndexOf(")") - input.IndexOf(",")), out int col))
                    {
                        Console.WriteLine("Could not understad column");
                        continue;
                    }

                    if (!PlaceShip(new ShipPlacementOptions()
                    {
                        Direction = direction,
                        ShipType = shipType,
                        Row = row,
                        Col = col
                    }))
                    {
                        Console.WriteLine("Ship could not fit in given location");
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
        }
        
        public bool PlaceShip(ShipPlacementOptions options)
        {
            Ship ship = new Ship(options.ShipType);
            if (TryPlaceShip(ship, options.Row, options.Col, options.Direction))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
