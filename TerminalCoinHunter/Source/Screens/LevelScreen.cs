using TerminalCoinHunter.Source.Entities;
using TerminalCoinHunter.Source.Enums;
using TerminalCoinHunter.Source.UI;

namespace TerminalCoinHunter.Source.Screens
{
    internal class LevelScreen : Screen
    {
        public event Action? GameContinued;
        public event Action? GameOver;
        public event Action? LevelRestarted;
        public event Action? NextLevelSelected;
        public event Action? CoinCollected;
        public event Action? PlayerHitTheWall;

        private const string LoseText = "=== You are dead ===";
        private const string PauseText = "=== PAUSE ===";
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
                case LevelState.Pause:
                    _renderer.DrawPause(PauseText, ControlsText.Pause);
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
                case LevelState.Pause:
                    UpdatePauseScreen();
                    break;
                case LevelState.Win:
                    UpdateWinScreen();
                    break;
                case LevelState.Lose:
                    UpdateLoseScreen();
                    break;
            }
        }

        private void OnGameContinued() => GameContinued?.Invoke();
        private void OnLevelRestarted() => LevelRestarted?.Invoke();
        private void OnNextLevelSelected() => NextLevelSelected?.Invoke();
        private void OnGameOver() => GameOver?.Invoke();
        private void OnCoinCollected() => CoinCollected?.Invoke();
        private void OnPlayerHitTheWall() => PlayerHitTheWall?.Invoke();

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
            switch (moveResult)
            {
                case MoveResult.CoinCollected:
                    OnCoinCollected();
                    break;
                case MoveResult.HitWall:
                    OnPlayerHitTheWall();
                    break;
            }
        }

        private bool HandleWinCondition()
        {
            if (_player.Coins == _level.Coins.Count)
            {
                _levelState = LevelState.Win;

                return true;
            }

            return false;
        }

        private void UpdateGameplay()
        {
            ConsoleKey key = _inputHandler.ReadGameplayKey();
            MoveResult moveResult = MoveResult.None;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    moveResult = _player.Move(_player.Position.Add(0, -1));
                    break;
                case ConsoleKey.DownArrow:
                    moveResult = _player.Move(_player.Position.Add(0, 1));
                    break;
                case ConsoleKey.LeftArrow:
                    moveResult = _player.Move(_player.Position.Add(-1, 0));
                    break;
                case ConsoleKey.RightArrow:
                    moveResult = _player.Move(_player.Position.Add(1, 0));
                    break;
                case ConsoleKey.M:
                    OnMuteToggled();
                    return;
                case ConsoleKey.Escape:
                    _levelState = LevelState.Pause;
                    return;
            }

            if (HandleEnemyCollision())
                return;

            if (HandleWinCondition() && moveResult == MoveResult.CoinCollected)
            {
                OnCoinCollected();

                return;
            }

            _enemy.Move(_player.Position);

            if (HandleEnemyCollision())
                return;

            HandleMoveResult(moveResult);
        }

        private void UpdateLoseScreen()
        {
            switch (_inputHandler.ReadLoseKey())
            {
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

        private void UpdatePauseScreen()
        {
            switch (_inputHandler.ReadPauseKey())
            {
                case ConsoleKey.Enter:
                    OnGameContinued();
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

        private void UpdateWinScreen()
        {
            switch (_inputHandler.ReadWinKey())
            {
                case ConsoleKey.Enter:
                    OnNextLevelSelected();
                    IsComplete = true;
                    break;
                case ConsoleKey.M:
                    OnMuteToggled();
                    break;
                case ConsoleKey.Escape:
                    ExitRequest();
                    break;
            }
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