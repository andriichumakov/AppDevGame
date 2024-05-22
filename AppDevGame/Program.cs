using System;

using AppDevGame;

namespace AppDevGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Starting the Game");
            using (var game = new MainApp())
                game.Run();
        }
    }
}