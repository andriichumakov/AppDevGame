using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class MainMenu : MenuWindow
    {
        public MainMenu(int width, int height, Texture2D background) : base(width, height, background)
        {
        }

        public override void Setup()
        {
            base.Setup();
            // Add buttons to the main menu
            AddButton(new Button(new Rectangle(100, 100, 200, 50), Color.Green, Color.White, "Start", null));
            AddButton(new Button(new Rectangle(100, 200, 200, 50), Color.Green, Color.White, "Quit", new QuitCommand()));
        }
    }
}
