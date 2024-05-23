using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppDevGame
{
    public class MainMenu : MenuWindow
    {
        private WindowManager _windowManager;
        private BaseWindow _settingsWindow;

        public MainMenu(int width, int height, Texture2D background, WindowManager windowManager, BaseWindow settingsWindow) 
            : base(width, height, background)
        {
            _windowManager = windowManager;
            _settingsWindow = settingsWindow;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;
            // Add buttons to the main menu
            Vector2 buttonPos = CalcButtonPosition(3, buttonWidth, buttonHeight, buttonSpacing); // Update to 3 buttons
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddButton(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Start", null));
            AddButton(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, "Change Settings", new LoadWindowCommand(_windowManager, _settingsWindow)));
            AddButton(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Quit", new QuitCommand()));
        }
    }

    public class SettingsMenu : MenuWindow
    {
        private WindowManager _windowManager;

        public SettingsMenu(int width, int height, Texture2D background, WindowManager windowManager) : base(width, height, background)
        {
            _windowManager = windowManager;
        }

        public override void Setup()
        {
            base.Setup();
            int buttonWidth = 200;
            int buttonHeight = 50;
            int buttonSpacing = 10;

            Vector2 buttonPos = CalcButtonPosition(5, buttonWidth, buttonHeight, buttonSpacing);
            int x = (int)buttonPos.X;
            int y = (int)buttonPos.Y;

            AddButton(new Button(new Rectangle(x, y, buttonWidth, buttonHeight), Color.Green, Color.White, "Language Menu", null));
            AddButton(new Button(new Rectangle(x, (y + buttonHeight + buttonSpacing), buttonWidth, buttonHeight), Color.Green, Color.White, "Sound Menu", null));
            AddButton(new Button(new Rectangle(x, (y + 2 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Mod Menu", null));
            AddButton(new Button(new Rectangle(x, (y + 3 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Apply", null));
            AddButton(new Button(new Rectangle(x, (y + 4 * (buttonHeight + buttonSpacing)), buttonWidth, buttonHeight), Color.Green, Color.White, "Cancel", new LoadWindowCommand(_windowManager, MainApp.GetInstance().MainMenu)));
        }
    }
}