using System.Text;
using TerminalCoinHunter.Source.Enums;
using TerminalCoinHunter.Source.Screens;
using TerminalCoinHunter.Source.UI;

namespace TerminalCoinHunter.Source
{
    internal class Game
    {
        private const string Title = "Terminal Coin Hunter";

        private bool _isRunning;

        private readonly InputHandler _inputHandler;
        private readonly LevelLoader _levelLoader;
        private readonly Sound _sound;

        private Difficulty _difficulty;

        public Game()
        {
            _inputHandler = new InputHandler();
            _levelLoader = new LevelLoader();
            _sound = new Sound();
            _isRunning = true;
        }

        public void Init()
        {
            Console.Title = Title;
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.OutputEncoding = Encoding.Unicode;
        }

        public void Run()
        {
            while (_isRunning)
            {
                Menu();

                if (!_isRunning)
                    break;

                LoadLevels();

                if (!_isRunning)
                    break;

                Start();
            }
        }

        private void LoadLevels()
        {
            try
            {
                _levelLoader.LoadLevels(_difficulty);
            }
            catch (InvalidDataException ex)
            {
                ErrorScreen errorScreen = new ErrorScreen(_inputHandler, ex.Message);
                errorScreen.Init();
                errorScreen.Update();
                Stop();
            }
        }

        private void Menu()
        {
            MenuScreen menuScreen = new MenuScreen(_inputHandler, Title);
            menuScreen.Init();

            menuScreen.MuteToggled += _sound.HandleMuteToggled;
            menuScreen.SelectionChanged += _sound.HandleMenuNavigation;
            menuScreen.NavigationBlocked += _sound.HandlerError;
            menuScreen.MenuSelected += _sound.HandlerConfirm;

            Loop(menuScreen);

            _difficulty = menuScreen.CurrentDifficulty;

            menuScreen.MuteToggled -= _sound.HandleMuteToggled;
            menuScreen.SelectionChanged -= _sound.HandleMenuNavigation;
            menuScreen.NavigationBlocked -= _sound.HandlerError;
            menuScreen.MenuSelected -= _sound.HandlerConfirm;

            if (menuScreen.ExitRequested)
                Stop();
        }

        private void Loop(Screen screen)
        {
            while (!screen.IsComplete)
            {
                screen.Update();
                screen.Draw();
            }
        }

        private void Start()
        {
            Theme theme = new Theme(_difficulty);

            while (_levelLoader.HasNextLevel)
            {
                Level? level = _levelLoader.GetNextLevel();

                if (level != null)
                {
                    LevelScreen levelScreen = new LevelScreen(level, theme, _inputHandler, _levelLoader.HasNextLevel);
                    levelScreen.Init();

                    Subscribe(levelScreen);
                    Loop(levelScreen);
                    Unsubscribe(levelScreen);

                    if (levelScreen.ExitRequested)
                    {
                        Stop();

                        return;
                    }
                }
                else
                {
                    ErrorScreen errorScreen = new ErrorScreen(_inputHandler, "Next level is not found");
                    errorScreen.Init();
                    errorScreen.Update();
                    Stop();
                }
            }
        }

        private void Stop() => _isRunning = false;

        private void Subscribe(LevelScreen screen)
        {
            screen.MuteToggled += _sound.HandleMuteToggled;
            screen.GameContinued += _sound.HandlerConfirm;
            screen.NextLevelSelected += _sound.HandlerConfirm;
            screen.LevelRestarted += _sound.HandlerConfirm;
            screen.GameOver += _sound.HandlerError;
            screen.CoinCollected += _sound.HandleCoinCollected;
            screen.PlayerHitTheWall += _sound.HandlerError;
        }

        private void Unsubscribe(LevelScreen screen)
        {
            screen.MuteToggled -= _sound.HandleMuteToggled;
            screen.GameContinued -= _sound.HandlerConfirm;
            screen.NextLevelSelected -= _sound.HandlerConfirm;
            screen.LevelRestarted -= _sound.HandlerConfirm;
            screen.GameOver -= _sound.HandlerError;
            screen.CoinCollected -= _sound.HandleCoinCollected;
            screen.PlayerHitTheWall -= _sound.HandlerError;
        }
    }
}
