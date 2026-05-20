using TerminalCoinHunter.Source.Entities;
using TerminalCoinHunter.Source.Enums;
using TerminalCoinHunter.Source.UI;

namespace TerminalCoinHunter.Source.Screens
{
    internal class LevelScreen : Screen
    {
        public event Action? GameOver;
        public event Action? LevelRestarted;
        public event Action? NextLevelSelected;
        public event Action? CoinCollected;
        public event Action? PlayerHitTheWall;

        private const string LoseText = "=== You are dead ===";
        private const string WinText = "=== You win ===";
        private const string GameCompleteText = "=== You have successfully completed the game ===";

        private readonly Level _level;

        private readonly Enemy _enemy;
        private readonly Player _player;

        private readonly LevelRenderer _renderer;

        private readonly bool _hasNextLevel;

        private LevelState _levelState;

        public LevelScreen(Level level, Theme theme, InputHandler inputHandler, bool hasNextLevel) : base(inputHandler)
        {
            _level = level;
            _hasNextLevel = hasNextLevel;
            _renderer = new LevelRenderer(theme, new Point(0, 4));
            _levelState = LevelState.Init;

            GameMap gameMap = new GameMap(level);
            _player = new Player(level.PlayerStartPosition, gameMap);
            _enemy = new Enemy(level.EnemyStartPosition, gameMap);
        }

        public override void Draw()
        {
            switch (_levelState)
            {
                case LevelState.Playing:
                    _renderer.DrawGameplay(_player, _enemy, _level.Coins, ControlsText.Gameplay);
                    break;
                case LevelState.Lose:
                    _renderer.DrawLose(LoseText, _player, _enemy, _level.Coins, ControlsText.Lose);
                    break;
                case LevelState.Win:
                    string overlayText = _hasNextLevel ? WinText : GameCompleteText;
                    _renderer.DrawWin(overlayText, _player, ControlsText.Win(_hasNextLevel));
                    break;
            }
        }

        public override void Init()
        {
            _renderer.Init(_level, _player.Symbol, _enemy.Symbol, ControlsText.Gameplay);
            _levelState = LevelState.Playing;
        }

        public override void Update()
        {
            switch (_levelState)
            {
                case LevelState.Playing:
                    UpdateGameplay();
                    break;
                case LevelState.Win:
                    HandleScreenInput(_inputHandler.ReadWinKey());
                    break;
                case LevelState.Lose:
                    HandleScreenInput(_inputHandler.ReadLoseKey());
                    break;
            }
        }

        private void OnLevelRestarted() => LevelRestarted?.Invoke();
        private void OnNextLevelSelected() => NextLevelSelected?.Invoke();
        private void OnGameOver() => GameOver?.Invoke();
        private void OnCoinCollected() => CoinCollected?.Invoke();
        private void OnPlayerHitTheWall() => PlayerHitTheWall?.Invoke();

        private bool CheckWinCondition()
        {
            if (_player.Coins == _level.Coins.Count)
            {
                _levelState = LevelState.Win;

                return true;
            }

            return false;
        }

        private bool HandleEnemyCollision()
        {
            if (_player.CheckEnemyCollision(_enemy.Position))
            {
                OnGameOver();
                _levelState = LevelState.Lose;

                return true;
            }

            return false;
        }

        private void HandleMoveResult(MoveResult moveResult)
        {
            if (moveResult == MoveResult.CoinCollected)
                OnCoinCollected();

            if (moveResult == MoveResult.HitWall)
                OnPlayerHitTheWall();
        }

        private void HandleScreenInput(ConsoleKey pressedKey)
        {
            switch (pressedKey)
            {
                case ConsoleKey.Enter:
                    OnNextLevelSelected();
                    IsComplete = true;
                    break;
                case ConsoleKey.R:
                    OnLevelRestarted();
                    Reset();
                    _levelState = LevelState.Playing;
                    break;
                case ConsoleKey.M:
                    OnMuteToggled();
                    break;
                case ConsoleKey.Escape:
                    ExitRequest();
                    break;
            }
        }

        private void UpdateGameplay()
        {
            MoveResult moveResult = ProcessPlayerMove();

            if (HandleEnemyCollision())
                return;

            if (CheckWinCondition())
            {
                OnCoinCollected();

                return;
            }

            _enemy.Move(_player.Position);

            if (HandleEnemyCollision())
                return;

            HandleMoveResult(moveResult);
        }

        private MoveResult ProcessPlayerMove()
        {
            ConsoleKey key = _inputHandler.ReadGameplayKey();

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    return _player.Move(_player.Position.Add(0, -1));
                case ConsoleKey.DownArrow:
                    return _player.Move(_player.Position.Add(0, 1));
                case ConsoleKey.LeftArrow:
                    return _player.Move(_player.Position.Add(-1, 0));
                case ConsoleKey.RightArrow:
                    return _player.Move(_player.Position.Add(1, 0));
            }

            HandleScreenInput(key);

            return MoveResult.None;
        }

        private void Reset()
        {
            _level.Reset();
            _player.Reset();
            _enemy.Reset();
            _renderer.Init(_level, _player.Symbol, _enemy.Symbol, ControlsText.Gameplay);
        }
    }
}