namespace TerminalCoinHunter.Source.Entities
{
    internal class Player : Entity
    {
        public event Action? CoinCollected;
        public event Action? WallHit;

        public int Coins { get; private set; }

        public Player(Point position, GameMap gameMap) : base(position, gameMap)
        {
            Coins = 0;
        }

        public void Move(Point position)
        {
            if (_gameMap.IsWall(position))
            {
                OnWallHit();
                return;
            }

            if (_gameMap.IsCoin(position))
            {
                if (_gameMap.TryCollectCoin(position))
                {
                    Coins++;
                    OnCoinCollected();
                }
            }

            OldPosition = Position;
            Position = position;
            UpdateDirection();
        }

        public bool CheckEnemyCollision(Point enemyPosition) => enemyPosition == Position;

        public override void Reset()
        {
            base.Reset();
            Coins = 0;
        }

        private void OnCoinCollected() => CoinCollected?.Invoke();
        private void OnWallHit() => WallHit?.Invoke();
    }
}
