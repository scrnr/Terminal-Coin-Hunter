using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source.UI
{
    internal class Theme
    {
        public char DeathSymbol { get; init; }
        public char MenuCursorSymbol { get; init; }
        public char WallSymbol { get; init; }

        public ConsoleColor PlayerColor { get; init; }
        public ConsoleColor EnemyColor { get; init; }
        public ConsoleColor MenuColor { get; init; }

        public ConsoleColor WallColor { get; init; }
        public ConsoleColor CoinNotCollectedColor { get; init; }

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
                _ => '\u25C6'
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

        private readonly Difficulty _difficulty;

        public Theme(Difficulty difficulty)
        {
            _difficulty = difficulty;

            DeathSymbol = '\u2020';
            MenuCursorSymbol = '\u261E';
            WallSymbol = '\u2593';

            PlayerColor = ConsoleColor.Green;
            EnemyColor = ConsoleColor.Red;
            CoinNotCollectedColor = ConsoleColor.DarkGray;
            MenuColor = ConsoleColor.Magenta;
            WallColor = ConsoleColor.White;
        }
    }
}
