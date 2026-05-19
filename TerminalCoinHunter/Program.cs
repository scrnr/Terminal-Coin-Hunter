using TerminalCoinHunter.Source;

namespace TerminalCoinHunter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Init();
            game.Run();
        }
    }
}
