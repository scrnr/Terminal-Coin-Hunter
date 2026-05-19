using TerminalCoinHunter.Source.Entities;
using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source
{
    internal class GameMap
    {
        private readonly Level _level;

        public GameMap(Level level)
        {
            _level = level;
        }

        public bool IsWall(Point position) => _level.Grid[position.Y, position.X] == Tile.Wall;

        public bool IsCoin(Point position) => _level.Coins.ContainsKey(position);

        public bool TryCollectCoin(Point position)
        {
            if (IsCoin(position))
            {
                Coin coin = _level.Coins[position];

                if (coin.TryCollect())
                    return true;
            }

            return false;
        }
    }
}
