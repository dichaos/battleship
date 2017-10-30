using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship
{
    public class Game
    {
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;

        public Game(IPlayer player1, IPlayer player2)
        {
            _player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
            _player2 = player2 ?? throw new ArgumentNullException(nameof(player2));
        }

        public void Play()
        {
            _player1.SetBoard();
            _player2.SetBoard();

            while(true)
            {
                _player1.Attack();

                if(_player1.Won)
                {
                    Console.WriteLine($"{_player1.Name} WON!!!!");
                    Console.ReadLine();
                    return;
                }

                _player2.Attack();

                if (_player2.Won)
                {
                    Console.WriteLine($"{_player2.Name} WON!!!!");
                    Console.ReadLine();
                    return;
                }
            }
        }
    }
}
