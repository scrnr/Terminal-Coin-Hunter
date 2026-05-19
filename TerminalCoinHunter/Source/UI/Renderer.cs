namespace TerminalCoinHunter.Source.UI
{
    internal class Renderer
    {
        private readonly char _emptySymbol = '\u0020';

        public void ClearConsole() => Console.Clear();

        public void DrawText(Point position, string text)
        {
            SetCursorPosition(position);
            Console.WriteLine(text);
        }

        public void DrawText(Point position, string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            DrawText(position, text);
            Console.ResetColor();
        }

        protected void ClearLine(int top)
        {
            SetCursorPosition(new Point(0, top));
            Console.Write(new string(' ', Console.WindowWidth));
        }

        protected void DrawAnimationText(string text, int timeout)
        {
            foreach (char symbol in text)
            {
                Console.Write(symbol);
                Thread.Sleep(timeout);
            }
            DrawEmptyLine();
        }

        protected void DrawAnimationText(string text, int timeout, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            DrawAnimationText(text, timeout);
            Console.ResetColor();
        }

        protected void DrawEmptyLine() => Console.WriteLine();

        protected void DrawEmptySymbol(Point position) => DrawSymbol(position, _emptySymbol);

        protected void DrawSymbol(Point position, char symbol)
        {
            SetCursorPosition(position);
            Console.Write(symbol);
        }

        protected void DrawSymbol(Point position, char symbol, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            DrawSymbol(position, symbol);
            Console.ResetColor();
        }

        protected void SetCursorPosition(Point position) => Console.SetCursorPosition(position.X, position.Y);
    }
}
