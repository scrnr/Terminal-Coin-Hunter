using TerminalCoinHunter.Source.Enums;

namespace TerminalCoinHunter.Source.UI
{
    internal class MenuRenderer : Renderer
    {
        private const string ASCIITitle = """
            ████████╗ ██████╗██╗  ██╗
            ╚══██╔══╝██╔════╝██║  ██║
               ██║   ██║     ███████║
               ██║   ██║     ██╔══██║
               ██║   ╚██████╗██║  ██║
               ╚═╝    ╚═════╝╚═╝  ╚═╝
            """;

        private readonly string _title;

        private readonly Theme _theme;
        private readonly Point _textPosition;
        private readonly int _animationTimeout;

        public MenuRenderer(string title, int animationTimeout, Theme theme)
        {
            _title = title;
            _theme = theme;
            _textPosition = new Point(5, 0);
            _animationTimeout = animationTimeout;
        }

        public void ClearCursor(Point position) => DrawEmptySymbol(position);

        public void DrawCursor(Point position) => DrawSymbol(position, _theme.MenuCursorSymbol, _theme.MenuColor);

        public void DrawMenu(Difficulty[] difficulties, Point cursorPosition)
        {
            ClearConsole();
            SetCursorPosition(new Point(0, 0));

            DrawAnimationText(ASCIITitle, _animationTimeout, _theme.MenuColor);
            DrawEmptyLine();

            SetCursorPosition(_textPosition with { Y = Console.CursorTop });
            DrawAnimationText(_title, _animationTimeout);
            DrawEmptyLine();

            DrawCursor(cursorPosition);
            DrawDifficulties(difficulties, cursorPosition.Add(3, 0));
            DrawControls();
        }

        private void DrawControls()
        {
            foreach (string text in ControlsText.Menu)
            {
                SetCursorPosition(_textPosition with { Y = Console.CursorTop });
                DrawAnimationText(text, _animationTimeout);
                DrawEmptyLine();
            }
        }

        private void DrawDifficulties(Difficulty[] difficulties, Point position)
        {
            foreach (Difficulty difficulty in difficulties)
            {
                SetCursorPosition(position with { Y = Console.CursorTop });
                DrawAnimationText(difficulty.ToString(), _animationTimeout);
                DrawEmptyLine();
            }
        }
    }
}
