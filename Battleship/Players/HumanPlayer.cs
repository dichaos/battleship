using Battleship.Enums;
using System;

namespace Battleship
{
    public class HumanPlayer : IPlayer
    {
        private readonly IBoardDrawer _boardDrawer;
        private readonly IBattleField _battleField;
        private readonly IShipPlacer _shipPlacer;

        public string Name => "Human";

        public bool Won => _battleField.ShipsLeft.Count == 0;

        public HumanPlayer(IBoardDrawer boardDrawer, IBattleField battleField, IShipPlacer shipPlacer)
        {
            _boardDrawer = boardDrawer ?? throw new ArgumentNullException(nameof(boardDrawer));
            _battleField = battleField ?? throw new ArgumentNullException(nameof(battleField));
            _shipPlacer = shipPlacer ?? throw new ArgumentNullException(nameof(shipPlacer));
        }

        private bool CheckInput(string strInput)
        {
            if (strInput == "board")
            {
                _boardDrawer.DrawBoard(true);
                return false;
            }

            if (strInput.Length > 2 || strInput.Length < 2)
            {
                Console.WriteLine("Wrong input");
                return false;
            }

            if (!Char.IsNumber(strInput[1]))
            {
                Console.WriteLine("Wrong input");
                return false;
            }

            if (strInput[0] == 'A' ||
                strInput[0] == 'B' ||
                strInput[0] == 'C' ||
                strInput[0] == 'D' ||
                strInput[0] == 'E' ||
                strInput[0] == 'F' ||
                strInput[0] == 'G' ||
                strInput[0] == 'H' ||
                strInput[0] == 'I' ||
                strInput[0] == 'J' ||
                strInput[0] == 'a' ||
                strInput[0] == 'b' ||
                strInput[0] == 'c' ||
                strInput[0] == 'd' ||
                strInput[0] == 'e' ||
                strInput[0] == 'f' ||
                strInput[0] == 'g' ||
                strInput[0] == 'h' ||
                strInput[0] == 'i' ||
                strInput[0] == 'j')
                return true;
            
            return false;
        }

        private int[] GetInput()
        {
            string strInput = Console.ReadLine();
            
            while (!CheckInput(strInput))
                strInput = Console.ReadLine();

            char col = strInput[0];

            string strRow = new String(new char[1] { strInput[1] });
            int intCol = 0;

            int.TryParse(strRow, out int row);

            if (strInput[0] == 'A' || strInput[0] == 'a')
                intCol = 0;

            if (strInput[0] == 'B' || strInput[0] == 'b')
                intCol = 1;

            if (strInput[0] == 'C' || strInput[0] == 'c')
                intCol = 2;

            if (strInput[0] == 'D' || strInput[0] == 'd')
                intCol = 3;

            if (strInput[0] == 'E' || strInput[0] == 'e')
                intCol = 4;

            if (strInput[0] == 'F' || strInput[0] == 'f')
                intCol = 5;

            if (strInput[0] == 'G' || strInput[0] == 'g')
                intCol = 6;

            if (strInput[0] == 'H' || strInput[0] == 'h')
                intCol = 7;

            if (strInput[0] == 'I' || strInput[0] == 'i')
                intCol = 8;

            if (strInput[0] == 'J' || strInput[0] == 'j')
                intCol = 9;

            return new int[2] { row, intCol };
        }

        public void Attack()
        {
            do
            {
                int[] input = GetInput();

                var toAttack = _battleField[input[0], input[1]];

                var condition = toAttack.HitCell();
                
                _boardDrawer.DrawBoard(true);

                if (Won)
                    return;

                if (_battleField.CellsLeft.Count == 0)
                    return;

                if (condition != CellCondition.ShipBombed && condition != CellCondition.ShunkShip)
                    return;
                
            } while (true);
        }

        public void SetBoard()
        {
            _shipPlacer.PlaceShips();
            _boardDrawer.DrawBoard(false);
        }
    }
}
