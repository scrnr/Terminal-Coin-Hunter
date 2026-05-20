using TerminalCoinHunter.Source.Entities;
using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source.UI
{
    internal class LevelRenderer : Renderer
    {
        private readonly Point _mapStartPosition;
        private readonly Theme _theme;

        private Point _coinAmountPosition;
        private Point _controlsPosition;

        public LevelRenderer(Theme theme, Point mapStartPosition)
        {
            _controlsPosition = new Point();
            _mapStartPosition = mapStartPosition;
            _theme = theme;
        }

        public void DrawGameplay(Player player, Enemy enemy, Dictionary<Point, Coin> coins, string[] controls)
        {
            ClearOverlay();
            ClearEntity(enemy.OldPosition);
            DrawCoins(coins);
            DrawPlayer(player.Position, player.OldPosition, player.Symbol);
            DrawEnemy(enemy.Position, enemy.Symbol);
            DrawCoinAmount(player.Coins, coins.Count);
            DrawControls(controls);
        }

        public void DrawLose(string overlayText, Player player, Enemy enemy, Dictionary<Point, Coin> coins, string[] controls)
        {
            ClearOverlay();
            DrawOverlay(overlayText, ConsoleColor.Red);
            ClearEntity(player.OldPosition);
            ClearEntity(enemy.OldPosition);
            DrawSymbol(player.Position.Add(_mapStartPosition), _theme.DeathSymbol);
            DrawCoins(coins);
            DrawControls(controls);
        }

        public void DrawPause(string overlayText, string[] controls)
        {
            ClearOverlay();
            DrawOverlay(overlayText, ConsoleColor.DarkGray);
            DrawControls(controls);
        }

        public void DrawWin(string overlayText, Player player, string[] controls)
        {
            ClearOverlay();
            DrawOverlay(overlayText, ConsoleColor.Green);
            DrawCoinAmount(player.Coins, player.Coins);
            DrawPlayer(player.Position, player.OldPosition, player.Symbol);
            DrawControls(controls);
        }

        public void Init(Level level, char playerSymbol, char enemySymbol, string[] controls)
        {
            ClearConsole();

            DrawText(new Point(), level.Name);
            DrawText(new Point(level.Name.Length, 0), $" | {_theme.CoinName}: ");

            int coinAmountLeftPosition = level.Name.Length + _theme.CoinName.Length + 5;
            _coinAmountPosition = new Point(coinAmountLeftPosition, 0);
            DrawCoinAmount(0, level.Coins.Count);

            DrawMap(level.Grid);

            _controlsPosition = new Point(1, Console.CursorTop + 1);
            DrawControls(controls);

            DrawPlayer(level.PlayerStartPosition, playerSymbol);
            DrawEnemy(level.EnemyStartPosition, enemySymbol);
            DrawCoins(level.Coins);
        }

        private void ClearEntity(Point position) => DrawEmptySymbol(position.Add(_mapStartPosition));

        private void ClearOverlay() => ClearLine(2);

        private void DrawOverlay(string text, ConsoleColor color) => DrawText(new Point(0, 2), text, color);

        private void DrawCoinAmount(int collectedCoins, int coins)
        {
            int notCollectedCoins = coins - collectedCoins;
            Point position = _coinAmountPosition;

            for (int i = 0; i < collectedCoins; i++)
                DrawSymbol(position.Add(i, 0), _theme.CoinSymbol, _theme.CoinColor);

            position = position.Add(collectedCoins, 0);

            for (int i = 0; i < notCollectedCoins; i++)
                DrawSymbol(position.Add(i, 0), _theme.CoinSymbol, _theme.CoinNotCollectedColor);
        }

        private void DrawCoins(Dictionary<Point, Coin> coins)
        {
            foreach (var (point, coin) in coins)
            {
                if (!coin.IsCollected)
                    DrawSymbol(point.Add(_mapStartPosition), _theme.CoinSymbol, _theme.CoinColor);
            }
        }

        private void DrawControls(string[] text)
        {
            Point position = _controlsPosition;

            foreach (string line in text)
            {
                ClearLine(position.Y);
                DrawText(position, line);
                position = position.Add(0, 1);
            }
        }

        private void DrawEnemy(Point position, char symbol)
        {
            DrawSymbol(position.Add(_mapStartPosition), symbol, _theme.EnemyColor);
        }

        private void DrawPlayer(Point position, char symbol)
        {
            DrawSymbol(position.Add(_mapStartPosition), symbol, _theme.PlayerColor);
        }

        private void DrawPlayer(Point position, Point oldPosition, char symbol)
        {
            if (position != oldPosition)
                ClearEntity(oldPosition);

            DrawPlayer(position, symbol);
        }

        private void DrawMap(Tile[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Point position = _mapStartPosition.Add(j, i);

                    if (map[i, j] == Tile.Wall)
                        DrawSymbol(position, _theme.WallSymbol, _theme.WallColor);
                    else
                        DrawEmptySymbol(position);
                }

                DrawEmptyLine();
            }
        }
    }
}
