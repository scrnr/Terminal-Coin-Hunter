using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source.UI
{
    internal class Theme
    {
        public char PlayerSymbol { get; init; }
        public char DeathSymbol { get; init; }
        public char EnemySymbol { get; init; }
        public char MenuCursorSymbol { get; init; }

        public ConsoleColor PlayerColor { get; init; }
        public ConsoleColor MenuColor { get; init; }
        public ConsoleColor CoinNotCollectedColor { get; init; }

        public ConsoleColor EnemyColor
        {
            get => _difficulty switch
            {
                Difficulty.Hard => ConsoleColor.DarkRed,
                _ => ConsoleColor.Red
            };
        }

        public string CoinName
        {
            get => _difficulty switch
            {
                Difficulty.Hard => "Diamonds",
                _ => "Coins"
            };
        }

        public char CoinSymbol
        {
            get => _difficulty switch
            {
                Difficulty.Hard => '\u2666',
                _ => '\u25CB'
            };
        }

        public ConsoleColor CoinColor
        {
            get => _difficulty switch
            {
                Difficulty.Hard => ConsoleColor.Cyan,
                _ => ConsoleColor.DarkYellow
            };
        }

        public char WallSymbol
        {
            get => _difficulty switch
            {
                Difficulty.Hard => '\u2591',
                _ => '\u2593'
            };
        }

        public ConsoleColor WallColor
        {
            get => _difficulty switch
            {
                Difficulty.Hard => ConsoleColor.Gray,
                _ => ConsoleColor.White
            };
        }

        private readonly Difficulty _difficulty;

        public Theme(Difficulty difficulty)
        {
            _difficulty = difficulty;

            PlayerSymbol = '\u263a';
            EnemySymbol = '\u263b';
            DeathSymbol = '\u2020';
            MenuCursorSymbol = '\u2192';

            PlayerColor = ConsoleColor.Green;
            CoinNotCollectedColor = ConsoleColor.DarkGray;
            MenuColor = ConsoleColor.Magenta;
        }
    }
}
