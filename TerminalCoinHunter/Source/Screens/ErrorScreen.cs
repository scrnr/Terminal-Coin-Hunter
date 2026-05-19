using TerminalCoinHunter.Source.UI;

namespace TerminalCoinHunter.Source.Screens
{
    internal class ErrorScreen : Screen
    {
        private readonly Renderer _renderer;

        private readonly string _message;

        public ErrorScreen(InputHandler inputHandler, string message) : base(inputHandler)
        {
            _renderer = new Renderer();
            _message = message;
        }

        public override void Draw()
        {
        }

        public override void Init()
        {
            _renderer.ClearConsole();

            Point position = new Point();

            _renderer.DrawText(position, _message, ConsoleColor.Red);
            _renderer.DrawText(position.Add(0, 2), "Press Enter to exit");
        }

        public override void Update() => _inputHandler.ReadErrorKey();
    }
}
