using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source.Entities
{
    internal class Player : Entity
    {
        public int Coins { get; private set; }

        public Player(Point position, GameMap gameMap) : base(position, gameMap)
        {
            Coins = 0;
        }

        public MoveResult Move(Point position)
        {
            if (_gameMap.IsWall(position))
                return MoveResult.HitWall;

            MoveResult result = MoveResult.None;

            if (_gameMap.IsCoin(position))
            {
                if (_gameMap.TryCollectCoin(position))
                {
                    Coins++;
                    result = MoveResult.CoinCollected;
                }
            }

            OldPosition = Position;
            Position = position;
            UpdateDirection();

            return result;
        }

        public bool CheckEnemyCollision(Point enemyPosition) => enemyPosition == Position;

        public override void Reset()
        {
            base.Reset();
            Coins = 0;
        }
    }
}
