namespace TerminalCoinHunter.Source
{
    internal class Sound
    {
        private bool _isMute;
        private bool _isPlaying;

        public Sound()
        {
            _isPlaying = false;
            _isMute = false;
        }

        public void HandleCoinCollected() => Play(1200, 40);

        public void HandlerConfirm() => PlayConfirm();

        public void HandleMuteToggled()
        {
            _isMute = !_isMute;
            PlayConfirm();
        }

        public void HandleMenuNavigation() => Play(800, 40);

        public void HandlerError() => Play(200, 40);

        private void PlayConfirm() => Play(1100, 40);

        private void Play(int frequency, int duration)
        {
            if (!_isPlaying && !_isMute)
            {
                Task.Run(() =>
                {
                    _isPlaying = true;
                    Console.Beep(frequency, duration);
                    _isPlaying = false;
                });
            }
        }
    }
}
