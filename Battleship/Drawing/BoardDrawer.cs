using Battleship.Enums;
using System;

namespace Battleship
{
    public class BoardDrawer : IBoardDrawer
    {
        private IBattleField battleField;
        private string name;
        public BoardDrawer(IBattleField _battleField, string _name)
        {
            battleField = _battleField;
            name = _name;
        }

        public void DrawBoard(bool hideLocations)
        {
            Console.WriteLine();
            Console.WriteLine(name);
            Console.WriteLine("  ABCDEFGHIJ");

            for (int j = 0; j < battleField.Size; j++)
            {
                string row = j + " " + string.Empty;

                for (int k = 0; k < battleField.Size; k++)
                {
                    switch (battleField[j, k].Condition)
                    {
                        case CellCondition.ShipLocation:
                            if (hideLocations)
                                row = row + "-";
                            else
                                row = row + battleField[j, k];
                            break;
                        default:
                            row = row + battleField[j, k];
                            break;
                    }
                }

                Console.WriteLine(row);
            }

            Console.WriteLine();
        }

        public void DrawProbabilityBoard()
        {
            Console.WriteLine();

            for (int j = 0; j < battleField.Size; j++)
            {
                string row = string.Empty;

                for (int k = 0; k < battleField.Size; k++)
                {
                    if (string.IsNullOrEmpty(row))
                    {
                        if (battleField[j, k].Probability < 10)
                            row = battleField[j, k].Probability + " ,";
                        else
                            row = battleField[j, k].Probability + ",";
                    }
                    else if (k + 1 < battleField.Size)
                    {
                        if (battleField[j, k].Probability < 10)
                            row = row + battleField[j, k].Probability + " ,";
                        else
                            row = row + battleField[j, k].Probability + ",";
                    }
                    else
                        row = row + battleField[j, k].Probability;
                }

                Console.WriteLine(row);
            }
        }
    }
}
