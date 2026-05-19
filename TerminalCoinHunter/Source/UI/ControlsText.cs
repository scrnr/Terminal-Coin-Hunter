namespace TerminalCoinHunter.Source.UI
{
    internal static class ControlsText
    {
        public readonly static string[] Menu = new string[]
        {
            "----------",
            "↑/↓ - Navigation",
            "M - Mute",
            "Enter - Start",
            "ESC - Exit"
        };

        public readonly static string[] Gameplay = new string[]
        {
            "----------",
            "↑/↓/←/→ - Moving",
            "M - Mute",
            "ESC - Pause"
        };

        public readonly static string[] Pause = new string[]
        {
            "----------",
            "M - Mute",
            "Enter - Continue",
            "ESC - Exit"
        };

        public readonly static string[] Lose = new string[]
        {
            "----------",
            "M - Mute",
            "R - Retry",
            "ESC - Exit"
        };

        public static string[] Win(bool hasNextLevel)
        {
            List<string> text = new List<string>();
            text.Add("----------");
            text.Add("M - Mute");

            if (hasNextLevel)
                text.Add("Enter - Next Level");
            else
                text.Add("Enter - Restart Game");

            text.Add("ESC - Exit");

            return text.ToArray();
        }
    }
}