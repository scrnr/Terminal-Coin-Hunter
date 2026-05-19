namespace TerminalCoinHunter.Source.Entities
{
    internal class Coin
    {
        public bool IsCollected { get; private set; }

        public Coin()
        {
            IsCollected = false;
        }

        public bool TryCollect()
        {
            if (IsCollected)
                return false;

            IsCollected = true;

            return true;
        }

        public void Reset() => IsCollected = false;
    }
}
