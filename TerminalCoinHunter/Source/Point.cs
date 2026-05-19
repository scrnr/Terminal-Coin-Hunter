namespace TerminalCoinHunter.Source
{
    internal readonly record struct Point(int X, int Y)
    {
        public Point Add(Point other) => new Point(X + other.X, Y + other.Y);

        public Point Add(int x, int y) => new Point(X + x, Y + y);

        public Point Subtract(Point other) => new Point(X - other.X, Y - other.Y);
    }
}
