namespace TerminalCoinHunter.Source.UI
{
    internal static class ControlsText
    {
        public readonly static string[] Menu = new string[]
        {
            "----------",
            "↑ / ↓ - Navigation",
            "Enter - Start",
            "M - Mute",
            "ESC - Exit"
        };

        public readonly static string[] Gameplay = new string[]
        {
            "----------",
            "↑ / ↓ / ← / → - Moving",
            "R - Restart Level",
            "M - Mute",
            "ESC - Exit"
        };

        public readonly static string[] Lose = new string[]
        {
            "----------",
            "R - Restart Level",
            "M - Mute",
            "ESC - Exit"
        };

        public static string[] Win(bool hasNextLevel)
        {
            List<string> text = new List<string>();
            text.Add("----------");

            if (hasNextLevel)
                text.Add("Enter - Next Level");
            else
                text.Add("Enter - Restart Game");

            text.Add("R - Restart Level");
            text.Add("M - Mute");
            text.Add("ESC - Exit");

            return text.ToArray();
        }
    }
}