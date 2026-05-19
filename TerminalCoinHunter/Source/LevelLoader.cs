using TerminalCoinHunter.Source.Entities;
using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source
{
    internal class LevelLoader
    {
        public bool HasNextLevel
        {
            get => _levelIndex + 1 < _levels.Count;
        }

        private List<Level> _levels;
        private int _levelIndex;

        public LevelLoader()
        {
            _levels = new List<Level>();
            _levelIndex = -1;
        }

        public Level? GetNextLevel()
        {
            if (HasNextLevel)
            {
                _levelIndex++;

                return _levels[_levelIndex];
            }

            return null;
        }

        public void LoadLevels(Difficulty difficulty)
        {
            Reset();

            string folder = $"Levels/{difficulty.ToString()}";

            if (!Directory.Exists(folder))
                throw new InvalidDataException($"Folder '{folder}' does not exist");

            string[] files = Directory.GetFiles(folder, "*.txt").Order().ToArray();

            if (files.Length == 0)
                throw new InvalidDataException($"No levels files were found in '{folder}'");

            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);
                string messageExceptionStart = $"Level file '{Path.GetFileName(file)}'";
                int width = lines[0].Length;

                if (lines.Any((line) => line.Length != width))
                    throw new InvalidDataException($"{messageExceptionStart} contains rows with different lengths");

                Tile[,] grid = new Tile[lines.Length, width];

                Point coinPosition = new Point();
                Point enemyPosition = new Point();
                Point playerPosition = new Point();

                Dictionary<Point, Coin> coins = new Dictionary<Point, Coin>();

                int playerCount = 0;
                int enemyCount = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    for (int j = 0; j < line.Length; j++)
                    {
                        switch (line[j])
                        {
                            case '#':
                                grid[i, j] = Tile.Wall;
                                continue;
                            case 'P':
                                playerPosition = new Point(j, i);
                                playerCount++;
                                break;
                            case 'E':
                                enemyPosition = new Point(j, i);
                                enemyCount++;
                                break;
                            case 'C':
                                coinPosition = new Point(j, i);
                                coins.Add(coinPosition, new Coin());
                                break;
                            case '.':
                                break;
                            default:
                                throw new InvalidDataException($"{messageExceptionStart} contains an unknown symbol");
                        }

                        grid[i, j] = Tile.Empty;
                    }
                }

                Validate(playerCount, enemyCount, coins.Count, messageExceptionStart);
                _levels.Add(new Level(playerPosition, enemyPosition, coins, grid));
            }
        }

        private void Validate(int playerCount, int enemyCount, int coinsCount, string messageExceptionStart)
        {
            if (playerCount == 0)
                throw new InvalidDataException($"{messageExceptionStart} does not contain a player");

            if (playerCount > 1)
                throw new InvalidDataException($"{messageExceptionStart} contains more than one player");

            if (enemyCount == 0)
                throw new InvalidDataException($"{messageExceptionStart} does not contain an enemy");

            if (enemyCount > 1)
                throw new InvalidDataException($"{messageExceptionStart} contains more than one enemy");

            if (coinsCount < 1)
                throw new InvalidDataException($"{messageExceptionStart} does not contain any coins");
        }

        private void Reset()
        {
            _levelIndex = -1;
            _levels.Clear();
        }
    }
}
