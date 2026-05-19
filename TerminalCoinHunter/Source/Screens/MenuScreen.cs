using TerminalCoinHunter.Source.Enums;
using TerminalCoinHunter.Source.UI;

namespace TerminalCoinHunter.Source.Screens
{
    internal class MenuScreen : Screen
    {
        public event Action? MenuSelected;
        public event Action? NavigationBlocked;
        public event Action? SelectionChanged;

        public Difficulty CurrentDifficulty { get; private set; }

        private readonly MenuRenderer _renderer;

        private Point _cursorPosition;
        private Point _cursorOldPosition;

        public MenuScreen(InputHandler inputHandler, string title) : base(inputHandler)
        {
            CurrentDifficulty = Difficulty.Normal;
            _cursorPosition = new Point(5, 9);
            _cursorOldPosition = _cursorPosition;
            _renderer = new MenuRenderer(title, 5, new Theme(CurrentDifficulty));
        }

        public override void Draw()
        {
            _renderer.ClearCursor(_cursorOldPosition);
            _renderer.DrawCursor(_cursorPosition);
        }

        public override void Init()
        {
            Difficulty[] difficulties = Enum.GetValues<Difficulty>();
            _renderer.DrawMenu(difficulties, _cursorPosition);
        }

        public override void Update()
        {
            ConsoleKey key = _inputHandler.ReadMenuKey();

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (TryChangeDifficulty((int)CurrentDifficulty - 1))
                    {
                        SetNewCursorPosition(-2);
                        OnSelectionChanged();
                    }
                    else
                        OnNavigationBlocked();

                    break;
                case ConsoleKey.DownArrow:
                    if (TryChangeDifficulty((int)CurrentDifficulty + 1))
                    {
                        SetNewCursorPosition(2);
                        OnSelectionChanged();
                    }
                    else
                        OnNavigationBlocked();

                    break;
                case ConsoleKey.Enter:
                    OnMenuSelected();
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

        private void OnMenuSelected() => MenuSelected?.Invoke();
        private void OnNavigationBlocked() => NavigationBlocked?.Invoke();
        private void OnSelectionChanged() => SelectionChanged?.Invoke();

        private bool TryChangeDifficulty(int newDifficultyIndex)
        {
            if (newDifficultyIndex < 0 || newDifficultyIndex >= Enum.GetValues<Difficulty>().Length)
                return false;

            CurrentDifficulty = (Difficulty)newDifficultyIndex;

            return true;
        }

        private void SetNewCursorPosition(int y)
        {
            _cursorOldPosition = _cursorPosition;
            _cursorPosition = _cursorPosition.Add(0, y);
        }
    }
}
