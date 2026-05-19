using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source.Entities
{
    internal class Enemy : Entity
    {
        public Enemy(Point position, GameMap gameMap) : base(position, gameMap)
        {
        }

        public void Move(Point playerPosition)
        {
            Point nextPosition = Position;

            Point diff = Position.Subtract(playerPosition);

            if (Math.Abs(diff.X) > Math.Abs(diff.Y))
                nextPosition = GetPreferredPosition(Axis.X, playerPosition);
            else
                nextPosition = GetPreferredPosition(Axis.Y, playerPosition);

            if (nextPosition == Position)
                return;

            OldPosition = Position;
            Position = nextPosition;
            UpdateDirection();
        }

        private int GetDiff(int current, int target)
        {
            if (current < target)
                return 1;
            else if (current > target)
                return -1;
            else
                return 0;
        }

        private Point GetPotentialPosition(Axis axis, Point target)
        {
            if (axis == Axis.X)
            {
                int diff = GetDiff(Position.X, target.X);

                return Position.Add(diff, 0);
            }
            else
            {
                int diff = GetDiff(Position.Y, target.Y);

                return Position.Add(0, diff);
            }
        }

        private Point GetPreferredPosition(Axis axis, Point playerPosition)
        {
            Axis firstAttempt = axis;
            Axis secondAttempt = axis == Axis.X ? Axis.Y : Axis.X;

            Point nextPosition = GetPotentialPosition(firstAttempt, playerPosition);

            if (_gameMap.IsWall(nextPosition))
            {
                nextPosition = GetPotentialPosition(secondAttempt, playerPosition);

                if (_gameMap.IsWall(nextPosition))
                    return Position;
            }

            return nextPosition;
        }
    }
}
