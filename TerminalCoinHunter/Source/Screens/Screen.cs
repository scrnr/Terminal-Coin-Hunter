namespace TerminalCoinHunter.Source.Screens
{
    internal abstract class Screen
    {
        public bool IsComplete { get; protected set; }

        public bool ExitRequested { get; protected set; }

        public event Action? MuteToggled;

        protected readonly InputHandler _inputHandler;

        protected Screen(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            IsComplete = false;
            ExitRequested = false;
        }

        public abstract void Draw();
        public abstract void Init();
        public abstract void Update();

        protected void OnMuteToggled() => MuteToggled?.Invoke();

        protected void ExitRequest()
        {
            IsComplete = true;
            ExitRequested = true;
        }
    }
}
