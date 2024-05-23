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
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;
            // Add buttons to the main menu
            Vector2 buttonPos = CalcButtonPosition(2, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddButton(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Start", new LoadWindowCommand(new Level1(800, 600, 1200, 800, MainApp.GetInstance()._imageLoader.GetResource("BackgroundLvl1")))));
            AddButton(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, "Quit", new QuitCommand()));
        }
    }
}
