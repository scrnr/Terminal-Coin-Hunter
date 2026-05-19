using TerminalCoinHunter.Source.Entities;
using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source
{
    internal class Level
    {
        public Point PlayerStartPosition { get; init; }
        public Point EnemyStartPosition { get; init; }

        public Dictionary<Point, Coin> Coins { get; init; }
        public Tile[,] Grid { get; init; }

        public Level(Point playerStartPosition, Point enemyStartPosition, Dictionary<Point, Coin> coins, Tile[,] grid)
        {
            PlayerStartPosition = playerStartPosition;
            EnemyStartPosition = enemyStartPosition;
            Coins = coins;
            Grid = grid;
        }

        public void Reset()
        {
            foreach (Coin coin in Coins.Values)
                coin.Reset();
        }
    }
}
