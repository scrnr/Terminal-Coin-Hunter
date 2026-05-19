namespace TerminalCoinHunter.Source.Entities
{
    internal class Entity
    {
        public Point Position { get; protected set; }
        public Point OldPosition { get; protected set; }

        protected readonly GameMap _gameMap;

        private readonly Point _startPosition;

        protected Entity(Point position, GameMap gameMap)
        {
            Position = position;
            OldPosition = position;
            _gameMap = gameMap;
            _startPosition = position;
        }

        public virtual void Reset()
        {
            OldPosition = _startPosition;
            Position = _startPosition;
        }
    }
}
