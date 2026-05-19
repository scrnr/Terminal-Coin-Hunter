namespace TerminalCoinHunter.Source
{
    internal class InputHandler
    {
        public ConsoleKey ReadMenuKey()
        {
            return GetKey(
                ConsoleKey.UpArrow,
                ConsoleKey.DownArrow,
                ConsoleKey.M,
                ConsoleKey.Enter,
                ConsoleKey.Escape
                );
        }

        public ConsoleKey ReadGameplayKey()
        {
            return GetKey(
                ConsoleKey.UpArrow,
                ConsoleKey.DownArrow,
                ConsoleKey.LeftArrow,
                ConsoleKey.RightArrow,
                ConsoleKey.M,
                ConsoleKey.Escape
                );
        }

        public ConsoleKey ReadLoseKey() => GetKey(ConsoleKey.R, ConsoleKey.M, ConsoleKey.Escape);

        public ConsoleKey ReadWinKey() => GetKey(ConsoleKey.Enter, ConsoleKey.M, ConsoleKey.Escape);

        public ConsoleKey ReadPauseKey() => ReadWinKey();

        public ConsoleKey ReadErrorKey() => GetKey(ConsoleKey.Enter);

        private ConsoleKey GetKey(params ConsoleKey[] availableKeys)
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (availableKeys.Contains(key))
                    return key;
            }
        }
    }
}
