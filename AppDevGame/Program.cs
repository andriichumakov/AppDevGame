using System;
using Microsoft.Xna.Framework;

using AppDevGame;

namespace AppDevGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Starting the Game");
            using (var game = MainApp.GetInstance())
                game.Run();
        }
    }
}