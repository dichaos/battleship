using Battleship.Enums;
using Battleship.Players.AttackModes;

namespace Battleship
{
    public class RobotPlayer : IPlayer
    {
        private readonly IBattleField _battleField;
        private readonly IShipPlacer _shipPlacer;
        private readonly IBoardDrawer _boardDrawer;
        
        private readonly ExploratoryAttack _exploratoryAttack;
        private readonly AfterHitAttack _afterHitAttack;

        private AttackMode _currentAttackMode;
        private HitResult _lastHit;

        public bool Won => _battleField.ShipsLeft.Count == 0;
        
        public string Name => "Computer";

        public RobotPlayer(IBoardDrawer boardDrawer, 
                           IBattleField battleField, 
                           IShipPlacer shipPlacer)
        {
            _battleField = battleField;
            _shipPlacer = shipPlacer;
            _boardDrawer = boardDrawer;

            _exploratoryAttack = new ExploratoryAttack(battleField, shipPlacer);
            _afterHitAttack = new AfterHitAttack(battleField);

            _currentAttackMode = AttackMode.Exploratory;
        }

        public void Attack()
        {
            switch (_currentAttackMode)
            {
                case AttackMode.Exploratory:
                    var possibleHit = _exploratoryAttack.Attack();
                    _currentAttackMode = possibleHit.AttackMode;

                    if (possibleHit.Hit)
                    {
                        _lastHit = possibleHit;
                        _afterHitAttack.Attack(possibleHit);
                    }
                    break;
                case AttackMode.AfterHit:
                    _lastHit = _afterHitAttack.Attack(_lastHit);
                    _currentAttackMode = _lastHit.AttackMode;

                    if (_lastHit.Hit)
                        Attack();
                    break;
            }

            _boardDrawer.DrawBoard(false);
        }

        public void SetBoard()
        {
            _shipPlacer.PlaceShips();
            _boardDrawer.DrawBoard(false);
        }
    }
}
