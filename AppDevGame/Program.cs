using System;

using AppDevGame;

namespace AppDevGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("something here");
            using (var game = new Game1())
                game.Run();
             Console.WriteLine("we still alive question");
        }
    }
}
