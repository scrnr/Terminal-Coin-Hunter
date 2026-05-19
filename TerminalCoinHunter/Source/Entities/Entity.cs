using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source.Entities
{
    internal class Entity
    {
        public Point Position { get; protected set; }
        public Point OldPosition { get; protected set; }

        public char Symbol
        {
            get => _direction switch
            {
                Direction.Up => '\u25B2',
                Direction.Down => '\u25BC',
                Direction.Left => '\u25C0',
                _ => '\u25B6'
            };
        }

        protected readonly GameMap _gameMap;

        private readonly Point _startPosition;

        private Direction _direction;

        protected Entity(Point position, GameMap gameMap)
        {
            Position = position;
            OldPosition = position;
            _gameMap = gameMap;
            _startPosition = position;
            _direction = Direction.Right;
        }

        public virtual void Reset()
        {
            OldPosition = _startPosition;
            Position = _startPosition;
        }

        protected void UpdateDirection()
        {
            Point diff = Position.Subtract(OldPosition);

            if (diff.X == 0)
                _direction = diff.Y > 0 ? Direction.Down : Direction.Up;
            else
                _direction = diff.X > 0 ? Direction.Right : Direction.Left;
        }
    }
}
